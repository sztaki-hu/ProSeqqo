using SequencePlanner.GTSP;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Task.Base;
using SequencePlanner.GTSPTask.Task.LineLike;
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
        //Alternative shortcut staff //TODO: Make it independent!
        private List<Position> DeletedPositionsOfShortcuts { get; set; }
        private List<GTSPPrecedenceConstraint> DeletedPositionPrecedencesOfShortcuts { get; set; }
        private List<GTSPPrecedenceConstraint> PositionPrecedencesOfShortcuts { get; set; }
        private List<AlternativeShortcut> AlternativeShortcuts { get; set; }
        public bool UseShortcutInAlternatives { get; set; }

        public PointLikeTask() : base()
        {
            Tasks = new List<Model.Task>();
            Alternatives = new List<Alternative>();
            Processes = new List<Process>();
            PositionPrecedence = new List<GTSPPrecedenceConstraint>();
            ProcessPrecedence = new List<GTSPPrecedenceConstraint>();
            DeletedPositionsOfShortcuts = new List<Position>();
            AlternativeShortcuts = new List<AlternativeShortcut>();
            DeletedPositionPrecedencesOfShortcuts = new List<GTSPPrecedenceConstraint>();
            PositionPrecedencesOfShortcuts = new List<GTSPPrecedenceConstraint>();
        }

        public PointTaskResult RunModel()
        {
            if (UseShortcutInAlternatives)
                CreateAlternativeShortcuts();
            SeqLogger.Info("RunModel started!", nameof(PointLikeTask));
            SeqLogger.Indent++;
            GenerateModel();
            var orToolsParam = new ORToolsTask()
            {
                TimeLimit = TimeLimit,
                GTSPRepresentation = GTSPRepresentation
            };
            var orTools = new ORToolsSequencerWrapper(orToolsParam);
            orTools.Build();
            PointTaskResult pointResult = new PointTaskResult(orTools.Solve());
            if (UseShortcutInAlternatives)
            {
                pointResult = ResolveSolutionWithAlternativeShortcuts(pointResult);
                ReverseAlternativeShortcuts();
            }
            pointResult = ResolveSolution(pointResult);
            SeqLogger.Indent--;
            SeqLogger.Info("RunModel finished!", nameof(PointLikeTask));
            pointResult.ToLog(LogLevel.Info);
            ToLog(LogLevel.Warning);
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

        private void CreateAlternativeShortcuts()
        {
            SeqLogger.Info("Alternative shortcut creation started!", nameof(PointLikeTask));
            SeqLogger.Indent++;
            AlternativeShortcuts.Clear();
            for (int i = 0; i < Alternatives.Count; i++)
            {
                if (Alternatives[i].Tasks.Count > 2)
                {
                    SeqLogger.Trace("Alternative " + Alternatives[i] + " in process!", nameof(PointLikeTask));
                    SeqLogger.Indent++;
                    var shortcut = new AlternativeShortcut();                                                           //Create alternative with shortcut for every alternative
                    shortcut.Assimilate(Alternatives[i]);                                                               //Init with the original, it is saved inside the new one.
                    shortcut.CreateShortcut(PositionMatrix.DistanceFunction, PositionMatrix.ResourceFunction);          //Create shortcuts between every position of first and last tasks.
                    for (int j = 1; j < Alternatives[i].Tasks.Count - 1; j++)
                    {
                        foreach (var position in Alternatives[i].Tasks[j].Positions)
                        {
                            PositionMatrix.Positions.Remove(position);
                            DeletedPositionsOfShortcuts.Add(position);
                            SeqLogger.Trace("Deleted position:" + position, nameof(PointLikeTask));
                        }
                    }
                    AlternativeShortcuts.Add(shortcut);
                    Alternatives[i] = shortcut;                                                                         //Change the alternatives for the new ones
                    foreach (var process in Processes)                                                                  //also in processes.
                    {
                        for (int j = 0; j < process.Alternatives.Count; j++)
                        {
                            if (process.Alternatives[j].GlobalID == shortcut.GlobalID)
                            {
                                process.Alternatives[j] = shortcut;
                            }
                        }
                    }
                    SeqLogger.Indent--;
                }
            }
            for (int i = 0; i < PositionPrecedence.Count; i++)                             //Check position precedences, if one of the positions part of shortcut,
            {                                                                              //it have to be override with the first position of the cut
                foreach (var shortcut in AlternativeShortcuts)
                {
                    var newPrec = shortcut.FindPrecedenceHeaderOfPositions(PositionPrecedence[i]);
                    if (newPrec != null && newPrec.Count > 0)
                    {
                        DeletedPositionPrecedencesOfShortcuts.Add(PositionPrecedence[i]);
                        PositionPrecedencesOfShortcuts.AddRange(newPrec);
                        PositionPrecedence.RemoveAt(i);
                        PositionPrecedence.AddRange(newPrec);
                    }

                }
            }
            SeqLogger.Indent--;
            SeqLogger.Info("Alternative shortcut creation finished!", nameof(PointLikeTask));
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

        private void CreateHierarchy()
        {

        }

        private List<GTSPPrecedenceConstraint> CreatePrecedenceConstraints(bool fullProcessPrecedence = false)
        {
            var precedences = new List<GTSPPrecedenceConstraint>();

            //Check circle in process precedences
            if (GTSPPrecedenceConstraint.isCyclic(ProcessPrecedence))
                throw new SequencerException("Process precedences is cyclic.");

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
                                positionPrecedences.Add(new GTSPPrecedenceConstraint(posBefore, posAfter));
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
                                        positionPrecedences.Add(new GTSPPrecedenceConstraint(posBefore, posAfter));
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
                                    constraint.Add(position);
                            else
                                //Add positions of positions of j. alternative i.layer
                                foreach (var position in process.Alternatives[j].Tasks[i].Positions)
                                    constraint.Add(position);

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
                position.SequencingID = MAX_SEQUENCING_ID++;
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
                    matrix[posA.SequencingID, posB.SequencingID] = CalculateWeight(posA, posB);
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
            var ORoutputRaw = result.SolutionRaw;
            result.SolutionRaw = new List<long>();
            foreach (var raw in ORoutputRaw)
            {
                var find = false;
                foreach (var position in PositionMatrix.Positions)
                {
                    if (position.SequencingID == raw)
                    {
                        if (!position.Virtual)
                        {
                            result.SolutionRaw.Add(position.UserID);
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
                    throw new SequencerException("Result of OR-Tools can not be resolved, no line found with the SequenceID: " + raw);
            }

            for (int i = 1; i < result.PositionResult.Count; i++)
            {
                result.CostsRaw.Add(GTSPRepresentation.Matrix[result.PositionResult[i - 1].SequencingID, result.PositionResult[i].SequencingID]);
                result.CostSum += result.CostsRaw[i - 1];
            }
            if (UseShortcutInAlternatives)
                result = ResolveSolutionWithAlternativeShortcuts(result);
            SeqLogger.Info("Solution resolved!", nameof(PointLikeTask));
            return result;
        }

        private PointTaskResult ResolveSolutionWithAlternativeShortcuts(TaskResult taskResult)
        {
            var solution = new PointTaskResult(taskResult);
            return solution;
        }

        private void ReverseAlternativeShortcuts()
        {
            foreach (var shortcut in AlternativeShortcuts)
            {
                for (int i = 0; i < Alternatives.Count; i++)                    //Change back alternatives in Alternatives collection to original.
                {
                    if (Alternatives[i].GlobalID == shortcut.GlobalID)
                        Alternatives[i] = shortcut.Original;
                }
                foreach (var process in Processes)                              //Change back alternatives in Processes collection to original.
                {
                    for (int i = 0; i < process.Alternatives.Count; i++)
                    {
                        if (process.Alternatives[i].GlobalID == shortcut.GlobalID)
                            process.Alternatives[i] = shortcut.Original;
                    }
                }
            }

            if (PositionMatrix != null && PositionMatrix.Positions != null)         //Add deleted positions of original alternatives to Positions collection.
                foreach (var position in DeletedPositionsOfShortcuts)
                {
                    PositionMatrix.Positions.Add(position);
                }
            PositionMatrix.DistanceFunction.StrictSystemEdgeWeights.DeleteAll();//Delete the edge weight ovverides of shortcuts.

            for (int i = 0; i < PositionPrecedence.Count; i++)                  //Remove precedences of shortcuts
            {
                for (int j = 0; j < PositionPrecedencesOfShortcuts.Count; j++)
                {
                    if (PositionPrecedence[i].Before.GlobalID == PositionPrecedencesOfShortcuts[j].Before.GlobalID && PositionPrecedence[i].After.GlobalID == PositionPrecedencesOfShortcuts[i].After.GlobalID)
                        PositionPrecedence.RemoveAt(i);
                }
            }
            foreach (var posPrec in DeletedPositionPrecedencesOfShortcuts)      //Add original precedences
            {
                PositionPrecedence.Add(posPrec);
            }

            AlternativeShortcuts.Clear();
            DeletedPositionsOfShortcuts.Clear();
            DeletedPositionPrecedencesOfShortcuts.Clear();
            PositionPrecedencesOfShortcuts.Clear();
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
                        tmp.Before.AddRange(alternative.Tasks[i].Positions);
                        tmp.After.AddRange(alternative.Tasks[i + 1].Positions);
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
                                prec.Add(new GTSPPrecedenceConstraint(p, p2));
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