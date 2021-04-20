using System.Collections.Generic;
using SequencePlanner.Model;
using SequencePlanner.Helper;
using SequencePlanner.OR_Tools;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Task.General.ShortCut;
using System;

namespace SequencePlanner.GTSPTask.Task.General
{
    public class GeneralTask
    {
        public TaskType TaskType { get { return TaskType.General; } }
        public bool Cyclic { get; set; }
        public int WeightMultipier { get; set; }
        public Position StartDepot { get; set; }
        public Position FinishDepot { get; set; }
        public PositionMatrix PositionMatrix { get; set; }
        public int TimeLimit { get; set; }
        public bool UseMIPprecedenceSolver { get; set; }
        public bool Validate { get; set; }
        public double IdlePenalty { get; set; }
        public LocalSearchStrategyEnum.Metaheuristics LocalSearchStrategy { get; set; }
        protected System.Diagnostics.Stopwatch Timer { get; set; }
        protected TimeSpan MIPRunTime { get; set; }
        protected IDepotMapper DepotMapper { get; set; }
        public List<Process> Processes { get; set; }
        public List<Alternative> Alternatives { get; set; }
        public List<Model.Task> Tasks { get; set; }
        public GeneralGTSPRepresentation GTSPRepresentation { get; set; }
        public List<GTSPPrecedenceConstraint> MotionPrecedence { get; set; }
        public List<GTSPPrecedenceConstraint> ProcessPrecedence { get; set; }
        public List<GTSPDisjointConstraint> DisjointConstraints { get; set; }
        public ShortestPathProcessor ShortestPathProcessor { get; set; }
        public bool UseShortcutInAlternatives { get; set; }
        public CalculateWeightDelegate CalculateWeightFunction { get; set; }
        public delegate double CalculateWeightDelegate(GTSPNode A, GTSPNode B);

        public GeneralTask()
        {
            Timer = new System.Diagnostics.Stopwatch();
            PositionMatrix = new PositionMatrix();
            LocalSearchStrategy = LocalSearchStrategyEnum.Metaheuristics.Automatic;
            WeightMultipier = 1000;
            Validate = false;
            Tasks = new List<Model.Task>();
            Alternatives = new List<Alternative>();
            Processes = new List<Process>();
            MotionPrecedence = new List<GTSPPrecedenceConstraint>();
            ProcessPrecedence = new List<GTSPPrecedenceConstraint>();
            ShortestPathProcessor = null;
            CalculateWeightFunction = PositionMatrix.CalculateWeight;
            DepotMapper = new GeneralDepotMapper();
        }


        public GeneralTaskResult RunModel()
        {
            SeqLogger.Debug("RunModel started!", nameof(GeneralTask));
            Timer.Reset();
            Timer.Start();
            AddBidirectionals();
            if (Validate)
                ValidateModel();
            DepotMapper.Map(this);
            if (UseShortcutInAlternatives)
            {
                ShortestPathProcessor = new ShortestPathProcessor(this);
                ShortestPathProcessor.Change();
            }
            SeqLogger.Indent++;
            CreateGTSPMatrix();
            GTSPRepresentation = new GeneralGTSPRepresentation()
            {
                Matrix = PositionMatrix.Matrix,
                //DisjointConstraints = CreateDisjointConstraints(),
                //PrecedenceConstraints = CreatePrecedenceConstraints(),
                StartDepot = DepotMapper.ORToolsStartDepotSequenceID,
                FinishDepot = DepotMapper.ORToolsFinishDepotSequenceID
            };
            
            if (UseMIPprecedenceSolver)
                GTSPRepresentation.InitialRoutes = CreateInitialRout();
            GTSPRepresentation.RoundedMatrix = ScaleUpWeights(GTSPRepresentation.Matrix);
            var orToolsParam = new ORToolsTask()
            {
                TimeLimit = TimeLimit,
                //GTSPRepresentation = GTSPRepresentation,
                LocalSearchStrategy = LocalSearchStrategy
            };
            ToLog(LogLevel.Debug);
            var orTools = new ORToolsSequencerWrapper(orToolsParam);
            orTools.Build();
            GeneralTaskResult pointResult = new GeneralTaskResult(orTools.Solve());
            pointResult = ResolveSolution(pointResult);
            if (UseShortcutInAlternatives)
            {
                pointResult = ShortestPathProcessor.ResolveSolution(pointResult, CalculateWeightFunction);
                ShortestPathProcessor.ChangeBack();
            }
            pointResult = (GeneralTaskResult)DepotMapper.ResolveSolution(pointResult);
            DepotMapper.ReverseMap(this);

            SeqLogger.Indent--;
            SeqLogger.Debug("RunModel finished!", nameof(GeneralTask));
            Timer.Stop();
            pointResult.FullTime = Timer.Elapsed;
            pointResult.ToLog(LogLevel.Info);
            //ToLog(LogLevel.Warning);
            pointResult.Validate(DisjointConstraints, MotionPrecedence);
            return pointResult;
        }
        public void ValidateModel()
        {
            GeneralTaskValidator.Validate(this);
        }
        protected long[][] CreateInitialRout()
        {
            var ORPreSolver = new ORToolsGeneralPreSolverWrapper(new ORToolsGeneralPreSolverTask()
            {
                NumberOfNodes = PositionMatrix.Positions.Count,
                DisjointConstraints = GTSPRepresentation.DisjointConstraints,
                //StrictOrderPrecedenceHierarchy = CreatePrecedenceHierarchiesForInitialSolution(),
                //OrderPrecedenceConstraints = CreatePrecedenceConstraints(true),
                StartDepot = DepotMapper.ORToolsStartDepotSequenceID,
                FinishDepot = DepotMapper.ORToolsFinishDepotSequenceID,
                Processes = Processes
            });

            var result = ORPreSolver.Solve();
            MIPRunTime = ORPreSolver.RunTime;
            if (result.Count > 0)
            {
                long[][] initialSolution = new long[1][];
                if (DepotMapper.ORToolsFinishDepot == null)
                {
                    initialSolution[0] = new long[result.Count];
                    for (int i = 0; i < result.Count; i++)
                    {
                        initialSolution[0][i] = result[i];
                    }
                }
                else
                {
                    initialSolution[0] = new long[result.Count - 1];
                    for (int i = 0; i < result.Count - 1; i++)
                    {
                        initialSolution[0][i] = result[i];
                    }
                }
                ResolveInitialSolution(initialSolution);
                return initialSolution;
            }
            return null;
        }

        public new void ToLog(LogLevel level)
        {
            SeqLogger.WriteLog(level, "Cyclic: " + Cyclic, nameof(GeneralTask));
            SeqLogger.WriteLog(level, "StartDepot: " + StartDepot, nameof(GeneralTask));
            SeqLogger.WriteLog(level, "FinishDepot: " + FinishDepot, nameof(GeneralTask));
            SeqLogger.WriteLog(level, "TimeLimit: " + TimeLimit, nameof(GeneralTask));
            SeqLogger.WriteLog(level, "UseMIPprecedenceSolver: " + UseMIPprecedenceSolver, nameof(GeneralTask));
            SeqLogger.WriteLog(level, "LocalSearchStrategy: " + LocalSearchStrategy, nameof(GeneralTask));
            PositionMatrix.ToLog(level);
            SeqLogger.WriteLog(level, "Hierarchy:");
            SeqLogger.Indent++;
            foreach (var process in Processes)
            {
                SeqLogger.WriteLog(level, process.ToString(), nameof(GeneralTask));
                SeqLogger.Indent++;
                foreach (var alternative in process.Alternatives)
                {
                    SeqLogger.WriteLog(level, alternative.ToString(), nameof(GeneralTask));
                    SeqLogger.Indent++;
                    foreach (var task in alternative.Tasks)
                    {
                        SeqLogger.WriteLog(level, task.ToString(), nameof(GeneralTask));
                        SeqLogger.Indent++;
                        foreach (var position in task.Positions)
                        {
                            SeqLogger.WriteLog(level, position.ToString(), nameof(GeneralTask));

                        }
                        SeqLogger.Indent--;
                    }
                    SeqLogger.Indent--;
                }
                SeqLogger.Indent--;
            }
            SeqLogger.Indent--;

            SeqLogger.WriteLog(level, "ProcessPrecedences:");
            if(ProcessPrecedence is not null)
            {
                SeqLogger.Indent++;
                foreach (var prec in ProcessPrecedence)
                {
                    SeqLogger.WriteLog(level, prec.ToString(), nameof(GeneralTask));
                }
                SeqLogger.Indent--;
            }

            SeqLogger.WriteLog(level, "MotionPrecedences:");
            if(MotionPrecedence is not null)
            {
                SeqLogger.Indent++;
                foreach (var prec in MotionPrecedence)
                {
                    SeqLogger.WriteLog(level, prec.ToString(), nameof(GeneralTask));
                }
                SeqLogger.Indent--;
            }

            SeqLogger.WriteLog(level, "DisjointSets:");
            if(DisjointConstraints is not null)
            {
                SeqLogger.Indent++;
                foreach (var prec in DisjointConstraints)
                {
                    SeqLogger.WriteLog(level, prec.ToString(), nameof(GeneralTask));
                }
                SeqLogger.Indent--;
            }
        }


        private void CreateGTSPMatrix()
        {
            PositionMatrix.Init();
            CalculateWeightFunction = PositionMatrix.CalculateWeight;
            ConnectProcesses(Processes);
            ConnectInAlternatives();
        }
        private List<GTSPPrecedenceConstraint> CreatePrecedenceConstraints(bool fullProcessPrecedence = false)
        {
            var precedences = new List<GTSPPrecedenceConstraint>();

            //Check circle in process precedences
            if (GTSPPrecedenceConstraint.IsCyclic(ProcessPrecedence))
                throw new SeqException("Process precedences are cyclic.");

            if (MotionPrecedence != null)
            {
                foreach (var item in MotionPrecedence)
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
        private static IEnumerable<GTSPPrecedenceConstraint> CreateProcessPrecedence(GTSPPrecedenceConstraint precedence)
        {
            List<GTSPPrecedenceConstraint> motionPrecedences = new List<GTSPPrecedenceConstraint>();
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
                                motionPrecedences.Add(new GTSPPrecedenceConstraint(posBefore.In, posAfter.In));
                            }
                        }
                    }
                }
            }
            return motionPrecedences;
        }
        private static IEnumerable<GTSPPrecedenceConstraint> CreateProcessPrecedenceFull(GTSPPrecedenceConstraint precedence)
        {
            List<GTSPPrecedenceConstraint> motionPrecedences = new List<GTSPPrecedenceConstraint>();
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
                                        motionPrecedences.Add(new GTSPPrecedenceConstraint(posBefore.In, posAfter.In));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return motionPrecedences;
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
        private List<GTSPPrecedenceConstraintList> CreatePrecedenceHierarchiesForInitialSolution()
        {
            var prec = new List<GTSPPrecedenceConstraintList>();
            foreach (var alternative in Alternatives)
            {
                if (alternative.Tasks.Count >= 2)
                {
                    for (int i = 0; i < alternative.Tasks.Count - 1; i++)
                    {
                        GTSPPrecedenceConstraintList tmp = new GTSPPrecedenceConstraintList();
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
        private List<GTSPPrecedenceConstraint> CreateOrderPrecedenceForInitialSolution()
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
        private void ResolveInitialSolution(long[][] initialSolution)
        {
            var InitialSolurionUserIDs = "";
            GeneralTaskResult pointTaskResult = new GeneralTaskResult();
            foreach (var solutionItem in initialSolution[0])
            {
                foreach (var pos in PositionMatrix.Positions)
                {
                    if (((int)solutionItem) == pos.Node.SequencingID)
                    {
                        InitialSolurionUserIDs += pos.Node.UserID + ", ";
                        pointTaskResult.PositionResult.Add(pos);
                    }
                }
            }
            SeqLogger.Trace("Initial solution UserIDs" + InitialSolurionUserIDs);
            SeqLogger.Trace("Initial solution validation started!");
            pointTaskResult.Validate(DisjointConstraints, MotionPrecedence);
            SeqLogger.Trace("Initial solution validation finished!");
        }
        private GeneralTaskResult ResolveSolution(GeneralTaskResult result)
        {
            //result.ToLog(LogLevel.Error);
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
                        result.SolutionRaw.Add(position.Node.UserID);
                        result.PositionResult.Add(position);
                        find = true;
                        break;
                    }
                }
                if (!find)
                    throw new SeqException("Result of OR-Tools can not be resolved, no line found with the SequenceID: " + raw);
            }
            for (int i = 0; i < result.ResolveHelper.Count; i++)
            {
                result.ResolveHelper[i].Node = result.PositionResult[i];
                result.ResolveHelper[i].GID = result.PositionResult[i].Node.GlobalID;
            }

            for (int i = 0; i < result.PositionResult.Count-1; i++)
            {
                result.CostsRaw.Add(GTSPRepresentation.Matrix[result.PositionResult[i].Node.SequencingID, result.PositionResult[i+1].Node.SequencingID]);
                result.ResolveHelper[i].Cost = GTSPRepresentation.Matrix[result.PositionResult[i].Node.SequencingID, result.PositionResult[i+1].Node.SequencingID];
                result.CostSum += result.CostsRaw[i];
            }
            SeqLogger.Debug("Solution resolved!", nameof(GeneralTask));
            return result;
        }
        private void ConnectProcesses(List<Process> processes)
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
                                        ConnectTasks(alternative.Tasks[^1], alternative2.Tasks[0]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void ConnectInAlternatives()
        {
            foreach (var alternative in Alternatives)
            {
                for (int i = 0; i < alternative.Tasks.Count - 1; i++)
                {
                    ConnectTasks(alternative.Tasks[i], alternative.Tasks[i + 1]);
                }
            }
        }
        private void ConnectTasks(Model.Task a, Model.Task b)
        {
            foreach (var posA in a.Positions)
            {
                foreach (var posB in b.Positions)
                {
                    PositionMatrix.CalculateWeight(posA, posB);
                }
            }
        }
        private void AddBidirectionals()
        {
            var addToTask = new List<Model.Task>();
            var nodeToAdd = new List<GTSPNode>();
            foreach (var t in Tasks)
            {
                foreach (var p in t.Positions)
                {
                    if (p.Bidirectional)
                    {
                        var reverse = p.GetBidirectional();
                        addToTask.Add(t);
                        nodeToAdd.Add(reverse);
                    }
                }
            }
            for (int i = 0; i < nodeToAdd.Count; i++)
            {
                addToTask[i].Positions.Add(nodeToAdd[i]);
                PositionMatrix.Positions.Add(nodeToAdd[i]);
            }
        }
        protected int[,] ScaleUpWeights(double[,] matrix)
        {
            int[,] roundedMatrix = new int[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    roundedMatrix[i, j] = Convert.ToInt32(WeightMultipier * matrix[i, j]);
                }
            }
            return roundedMatrix;
        }
    }
}