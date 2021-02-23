using SequencePlanner.GTSP;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Task.Base;
using SequencePlanner.GTSPTask.Task.LineLike;
using SequencePlanner.GTSPTask.Task.PointLike.ShortCut;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using SequencePlanner.OR_Tools;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SequencePlanner.GTSPTask.Task.PointLike
{
    public class PointLikeTask : BaseTask
    {
        private int MAX_SEQUENCING_ID = 0;
        public List<Model.Task> Tasks { get; set; }
        public List<Alternative> Alternatives { get; set; }
        public List<Process> Processes { get; set; }
        public List<GTSPPrecedenceConstraint> PositionPrecedence { get; set; }
        public List<GTSPPrecedenceConstraint> ProcessPrecedence { get; set; }
        public List<GTSPDisjointConstraint> DisjointConstraints { get; set; }
        public PointLikeGTSPRepresentation GTSPRepresentation { get; set; }
        public ShortestPathProcessor ShortestPathProcessor { get; set; }
        public bool UseShortcutInAlternatives { get; set; }

        public PointLikeTask() : base()
        {
            Tasks = new List<Model.Task>();
            Alternatives = new List<Alternative>();
            Processes = new List<Process>();
            PositionPrecedence = new List<GTSPPrecedenceConstraint>();
            ProcessPrecedence = new List<GTSPPrecedenceConstraint>();
            ShortestPathProcessor = null;
        }

        public PointTaskResult RunModel()
        {
            Timer.Reset();
            Timer.Start();
            if (UseShortcutInAlternatives)
            {
                ShortestPathProcessor = new ShortestPathProcessor(this);
                ShortestPathProcessor.Change();
            }
            SeqLogger.Info("RunModel started!", nameof(PointLikeTask));
            SeqLogger.Indent++;
            GenerateModel();
            var orToolsParam = new ORToolsTask()
            {
                TimeLimit = TimeLimit,
                GTSPRepresentation = GTSPRepresentation,
                LocalSearchStrategie = LocalSearchStrategie
            };
            ToLog(LogLevel.Debug);
            var orTools = new ORToolsSequencerWrapper(orToolsParam);
            orTools.Build();
            PointTaskResult pointResult = new PointTaskResult(orTools.Solve());
            pointResult = ResolveSolution(pointResult);
            if (UseShortcutInAlternatives)
            {
                pointResult = ShortestPathProcessor.ResolveSolution(pointResult);
                ShortestPathProcessor.ChangeBack();
            }
            SeqLogger.Indent--;
            SeqLogger.Info("RunModel finished!", nameof(PointLikeTask));
            Timer.Stop();
            pointResult.FullTime = Timer.Elapsed;
            pointResult.ToLog(LogLevel.Info);
            //ToLog(LogLevel.Warning);
            return pointResult;
        }

        private void GenerateModel()
        {
            SeqLogger.Info("Model generation started!", nameof(PointLikeTask));
            SeqLogger.Indent++;
            GTSPRepresentation = new PointLikeGTSPRepresentation()
            {
                Matrix = CreateGTSPMatrix(),
                DisjointConstraints = CreateDisjointConstraints(),
                PrecedenceConstraints = CreatePrecedenceConstraints(),
                StartDepot = StartDepot.SequencingID
            };
            PositionMatrix.Matrix = GTSPRepresentation.Matrix;
            if (UseMIPprecedenceSolver)
                GTSPRepresentation.InitialRoutes = CreateInitialRout();
            GTSPRepresentation.RoundedMatrix = ScaleUpWeights(GTSPRepresentation.Matrix);
            SeqLogger.Indent--;
            SeqLogger.Info("Model generation finished!", nameof(PointLikeTask));
        }

        protected long[][] CreateInitialRout()
        {
            var ORPreSolver = new ORToolsPointLikePreSolverWrapper(new ORToolsPointLikePreSolverTask()
            {
                NumberOfNodes = PositionMatrix.Positions.Count,
                DisjointConstraints = GTSPRepresentation.DisjointConstraints,
                StrictOrderPrecedenceHierarchy = CreatePrecedenceHierarchiesForInitialSolution(),
                OrderPrecedenceConstraints = CreatePrecedenceConstraints(true),
                StartDepot = StartDepot.SequencingID,
                Processes = Processes
            });

            var result = ORPreSolver.Solve();
            MIPRunTime = ORPreSolver.RunTime;
            if (result.Count > 0)
            {
                long[][] initialSolution = new long[1][];
                initialSolution[0] = new long[result.Count];
                for (int i = 0; i < result.Count; i++)
                {
                    initialSolution[0][i] = result[i];
                }
                return initialSolution;
            }
            return null;
        }

        private List<GTSPPrecedenceConstraint> CreatePrecedenceConstraints(bool fullProcessPrecedence = false)
        {
            var precedences = new List<GTSPPrecedenceConstraint>();

            //Check circle in process precedences
            if (GTSPPrecedenceConstraint.isCyclic(ProcessPrecedence))
                throw new SeqException("Process precedences are cyclic.");

            if (PositionPrecedence != null)
            {
                foreach (var item in PositionPrecedence)
                {
                    if (item.Before != null && item.After != null)
                        precedences.Add(new GTSPPrecedenceConstraint(item.Before, item.After));
                }
            }

            if (ProcessPrecedence != null)
            {
                foreach (var precedence in ProcessPrecedence)
                {
                    if (fullProcessPrecedence)
                        precedences.AddRange(CreateProcessPrecedenceFull(precedence));
                    else
                        precedences.AddRange(CreateProcessPrecedence(precedence));
                }
            }
            return precedences;
        }

        private IEnumerable<GTSPPrecedenceConstraint> CreateProcessPrecedence(GTSPPrecedenceConstraint precedence)
        {
            List<GTSPPrecedenceConstraint> positionPrecedences = new List<GTSPPrecedenceConstraint>();
            foreach (var alterBefore in ((Process)precedence.Before).Alternatives)
            {
                foreach (var alterAfter in ((Process)precedence.After).Alternatives)
                {
                    if (alterBefore.Tasks.Count > 0 && alterAfter.Tasks.Count > 0)
                    {
                        foreach (var posBefore in alterBefore.Tasks[0].Positions)
                        {
                            foreach (var posAfter in alterAfter.Tasks[0].Positions)
                            {
                                positionPrecedences.Add(new GTSPPrecedenceConstraint(posBefore.In, posAfter.In));
                            }
                        }
                    }
                }
            }
            return positionPrecedences;
        }

        private IEnumerable<GTSPPrecedenceConstraint> CreateProcessPrecedenceFull(GTSPPrecedenceConstraint precedence)
        {
            List<GTSPPrecedenceConstraint> positionPrecedences = new List<GTSPPrecedenceConstraint>();
            foreach (var alterBefore in ((Process)precedence.Before).Alternatives)
            {
                foreach (var alterAfter in ((Process)precedence.After).Alternatives)
                {
                    if (alterBefore.Tasks.Count > 0 && alterAfter.Tasks.Count > 0)
                    {
                        for (int i = 0; i < alterBefore.Tasks.Count; i++)
                        {
                            for (int j = 0; j < alterAfter.Tasks.Count; j++)
                            {
                                foreach (var posBefore in alterBefore.Tasks[i].Positions)
                                {
                                    foreach (var posAfter in alterAfter.Tasks[j].Positions)
                                    {
                                        positionPrecedences.Add(new GTSPPrecedenceConstraint(posBefore.In, posAfter.In));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return positionPrecedences;
        }

        private List<GTSPDisjointConstraint> CreateDisjointConstraints()
        {
            DisjointConstraints = new List<GTSPDisjointConstraint>();
            foreach (var process in Processes)
            {
                if (process.Alternatives.Count > 0)//!
                {
                    int[] taskNumberOfAlternatives = new int[process.Alternatives.Count];
                    int maxTaskNumber = 0;
                    for (int i = 0; i < process.Alternatives.Count; i++)
                    {
                        taskNumberOfAlternatives[i] = process.Alternatives[i].Tasks.Count;
                        if (taskNumberOfAlternatives[i] > maxTaskNumber)
                            maxTaskNumber = taskNumberOfAlternatives[i];
                    }

                    //if (process.Alternatives.Count < 0)
                    //{
                    //    maxTaskNumber = process.Alternatives[0].Tasks.Count;
                    //}

                    for (int i = 0; i < maxTaskNumber; i++)
                    {
                        var constraint = new GTSPDisjointConstraint();
                        for (int j = 0; j < process.Alternatives.Count; j++)
                        {
                            if (taskNumberOfAlternatives[j] <= i)
                                //Add positions of positions of j. alternative last layer
                                foreach (var position in process.Alternatives[j].Tasks[taskNumberOfAlternatives[j] - 1].Positions)
                                    constraint.Add(position.Node);
                            else
                                //Add positions of positions of j. alternative i.layer
                                foreach (var position in process.Alternatives[j].Tasks[i].Positions)
                                    constraint.Add(position.Node);

                        }
                        DisjointConstraints.Add(constraint);
                    }
                }
            }
            return DisjointConstraints;
        }

        private double[,] CreateGTSPMatrix()
        {
            foreach (var position in PositionMatrix.Positions)
            {
                position.Node.SequencingID = MAX_SEQUENCING_ID++;
            }
            double[,] matrix = new double[MAX_SEQUENCING_ID, MAX_SEQUENCING_ID];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = int.MaxValue / (IGTSPRepresentation.WEIGHT_MULTIPLIER * 2);
                }
            }
            matrix = ConnectProcesses(Processes, matrix);
            matrix = ConnectInAlternatives(matrix);
            return matrix;
        }

        private double[,] ConnectProcesses(List<Process> processes, double[,] matrix)
        {
            foreach (var proc in Processes)
            {
                foreach (var proc2 in Processes)
                {
                    if (proc.GlobalID != proc2.GlobalID)
                    {
                        foreach (var alternative in proc.Alternatives)
                        {
                            if (alternative.Tasks.Count > 0)
                            {
                                foreach (var alternative2 in proc2.Alternatives)
                                {
                                    if (alternative.GlobalID != alternative2.GlobalID && alternative2.Tasks.Count > 0)
                                    {
                                        matrix = ConnectTasks(alternative.Tasks[alternative.Tasks.Count - 1], alternative2.Tasks[0], matrix);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return matrix;
        }

        private double[,] ConnectInAlternatives(double[,] matrix)
        {
            foreach (var alternative in Alternatives)
            {
                for (int i = 0; i < alternative.Tasks.Count - 1; i++)
                {
                    ConnectTasks(alternative.Tasks[i], alternative.Tasks[i + 1], matrix);
                }
            }
            return matrix;
        }

        private double[,] ConnectTasks(Model.Task a, Model.Task b, double[,] matrix)
        {
            foreach (var posA in a.Positions)
            {
                foreach (var posB in b.Positions)
                {
                    matrix[posA.Node.SequencingID, posB.Node.SequencingID] = CalculateWeight(posA.Out, posB.In);
                }
            }
            return matrix;
        }

        private double CalculateWeight(Position A, Position B)
        {
            if (A.Virtual || B.Virtual)
                return 0.0;
            double weight = PositionMatrix.DistanceFunction.ComputeDistance(A, B);
            weight = PositionMatrix.ResourceFunction.ComputeResourceCost(A, B, weight);
            return weight;
        }

        public System.Threading.Tasks.Task<PointTaskResult> RunModelAsync(int taskID, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override void ValidateModel()
        {
            var validator = new PointLikeTaskValidator();
            validator.Validate(this);
        }

        private PointTaskResult ResolveSolution(PointTaskResult result)
        {
            result.Log = SeqLogger.Backlog;
            result.PreSolverTime = MIPRunTime;
            var ORoutputRaw = result.SolutionRaw;
            result.SolutionRaw = new List<long>();
            foreach (var raw in ORoutputRaw)
            {
                var find = false;
                foreach (var position in PositionMatrix.Positions)
                {
                    if (position.Node.SequencingID == raw)
                    {
                        if (!position.Node.Virtual)
                        {
                            result.SolutionRaw.Add(position.Node.UserID);
                            result.PositionResult.Add(position);
                        }
                        else
                        {
                            //taskResult.ResultWithVirtual.Add(position.UserID);
                        }
                        find = true;
                        break;
                    }
                }
                if (!find)
                    throw new SeqException("Result of OR-Tools can not be resolved, no line found with the SequenceID: " + raw);
            }

            for (int i = 1; i < result.PositionResult.Count; i++)
            {
                result.CostsRaw.Add(GTSPRepresentation.Matrix[result.PositionResult[i - 1].Node.SequencingID, result.PositionResult[i].Node.SequencingID]);
                result.CostSum += result.CostsRaw[i - 1];
            }
            SeqLogger.Info("Solution resolved!", nameof(PointLikeTask));
            return result;
        }

        public List<GTSPPrecedenceConstraintList> CreatePrecedenceHierarchiesForInitialSolution()
        {
            var prec = new List<GTSPPrecedenceConstraintList>();
            foreach (var alternative in Alternatives)
            {
                if (alternative.Tasks.Count >= 2)
                {
                    GTSPPrecedenceConstraintList tmp = null;
                    for (int i = 0; i < alternative.Tasks.Count - 1; i++)
                    {
                        tmp = new GTSPPrecedenceConstraintList();
                        foreach (var item in alternative.Tasks[i].Positions)
                        {
                            tmp.Before.Add(item.Node);
                        }
                        foreach (var item in alternative.Tasks[i + 1].Positions)
                        {
                            tmp.After.Add(item.Node);
                        }
                        prec.Add(tmp);
                    }
                }
            }
            return prec;
        }

        public List<GTSPPrecedenceConstraint> CreateOrderPrecedenceForInitialSolution()
        {
            var prec = new List<GTSPPrecedenceConstraint>();
            foreach (var alternative in Alternatives)
            {
                if (alternative.Tasks.Count > 1)
                    for (int i = 0; i < alternative.Tasks.Count - 1; i++)
                    {
                        // for (int j = i+1; j < alternative.Tasks.Count; j++)
                        // {
                        foreach (var p in alternative.Tasks[i].Positions)
                        {
                            //foreach (var p2 in alternative.Tasks[j].Positions)
                            foreach (var p2 in alternative.Tasks[i + 1].Positions)
                            {
                                prec.Add(new GTSPPrecedenceConstraint(p.Node, p2.Node));
                            }
                        }
                        // }
                    }
            }
            return prec;
        }

        public void ToLog(LogLevel level)
        {
            base.ToLog(level);
            SeqLogger.WriteLog(level, "Hierarchy:");
            SeqLogger.Indent++;
            foreach (var process in Processes)
            {
                SeqLogger.WriteLog(level, process.ToString(), nameof(PointLikeTask));
                SeqLogger.Indent++;
                foreach (var alternative in process.Alternatives)
                {
                    SeqLogger.WriteLog(level, alternative.ToString(), nameof(PointLikeTask));
                    SeqLogger.Indent++;
                    foreach (var task in alternative.Tasks)
                    {
                        SeqLogger.WriteLog(level, task.ToString(), nameof(PointLikeTask));
                        SeqLogger.Indent++;
                        foreach (var position in task.Positions)
                        {
                            SeqLogger.WriteLog(level, position.ToString(), nameof(PointLikeTask));

                        }
                        SeqLogger.Indent--;
                    }
                    SeqLogger.Indent--;
                }
                SeqLogger.Indent--;
            }
            SeqLogger.Indent--;

            SeqLogger.WriteLog(level, "ProcessPrecedences:");
            SeqLogger.Indent++;
            foreach (var prec in ProcessPrecedence)
            {
                SeqLogger.WriteLog(level, prec.ToString(), nameof(PointLikeTask));
            }
            SeqLogger.Indent--;

            SeqLogger.WriteLog(level, "PositionPrecedences:");
            SeqLogger.Indent++;
            foreach (var prec in PositionPrecedence)
            {
                SeqLogger.WriteLog(level, prec.ToString(), nameof(PointLikeTask));
            }
            SeqLogger.Indent--;

            SeqLogger.WriteLog(level, "DisjointSets:");
            SeqLogger.Indent++;
            foreach (var prec in DisjointConstraints)
            {
                SeqLogger.WriteLog(level, prec.ToString(), nameof(PointLikeTask));
            }
            SeqLogger.Indent--;
        }
    }
}