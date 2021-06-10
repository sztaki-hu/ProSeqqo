using SequencePlanner.Helper;
using SequencePlanner.Model;
using SequencePlanner.Model.Hierarchy;
using SequencePlanner.OR_Tools;
using SequencePlanner.Task.Processors;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SequencePlanner.Task
{
    public class GeneralTask
    {
        public bool Validate { get; set; }
        public bool Cyclic { get; set; }
        public Config StartDepotConfig { get; set; }
        public Config FinishDepotConfig { get; set; }
        public Motion StartDepot { get; set; }
        public Motion FinishDepot { get; set; }
        public CostManager CostManager { get; set; }
        public Hierarchy Hierarchy { get; set; }
        public SolverSettings SolverSettings { get; set; }
        public List<ProcessPrecedence> ProcessPrecedences { get; set; }
        public List<MotionPrecedence> MotionPrecedences { get; set; }
        public GTSPRepresentation GTSPRepresentation { get; set; }

        private DepotMapper DepotMapper { get; set; }
        private ShortcutMapper ShortcutMapper { get; set; }
        private InitialSolver InitialSolver { get; set; }
        private Stopwatch Timer { get; set; }

        public GeneralTask()
        {
            Validate = true;
            Cyclic = true;
            StartDepotConfig = null;
            FinishDepotConfig = null;
            StartDepot = null;
            FinishDepot = null;
            CostManager = new CostManager();
            Hierarchy = new Hierarchy();
            SolverSettings = new SolverSettings();
            ProcessPrecedences = new List<ProcessPrecedence>();
            MotionPrecedences = new List<MotionPrecedence>();
            GTSPRepresentation = new GTSPRepresentation(this);
            DepotMapper = new DepotMapper(this);
            ShortcutMapper = new ShortcutMapper(this);
            InitialSolver = new InitialSolver(this);
            Timer = new Stopwatch();
        }

        public GeneralTaskResult Run()
        {
            Timer.Restart();
            ValidateTask();
            DepotMapper.Change();
            ShortcutMapper.Change();
            Hierarchy.Build();
            GTSPRepresentation.Build();
            GTSPRepresentation.ORToolsFixFinishDepot();
            InitialSolver.CreateInitialSolution();
            CheckSolution(GTSPRepresentation.DisjointSets, GTSPRepresentation.MotionPrecedences, ProcessPrecedences, GTSPRepresentation.InitialSolution);
            var result = RunTask();
            DepotMapper.ChangeBack();
            ShortcutMapper.ChangeBack();
            Timer.Stop();
            result.FullTime = Timer.Elapsed;
            result.PreSolverTime = InitialSolver.Time;
            return result;
        }

        private void ValidateTask()
        {
            if (Validate)
                TaskValidator.Validate(this);
            else
                SeqLogger.Warning("Task validation turned off.");
        }

        private GeneralTaskResult RunTask()
        {
            var orToolsParam = new ORToolsTask()
            {
                TimeLimit = SolverSettings.TimeLimit,
                GTSPRepresentation = GTSPRepresentation,
                LocalSearchStrategy = SolverSettings.Metaheuristics
            };
            var orTools = new ORToolsSequencerWrapper(orToolsParam);
            orTools.Build();
            var orToolsResult = orTools.Solve();
            var result = ResolveSolution(orToolsResult);
            CheckSolution(GTSPRepresentation.DisjointSets, GTSPRepresentation.MotionPrecedences, null, result.SolutionMotion);
            return result;
        }

        private GeneralTaskResult ResolveSolution(ORToolsResult result)
        {
            GeneralTaskResult taskResult = new GeneralTaskResult()
            {
                StatusCode = result.StatusCode,
                StatusMessage = result.StatusMessage,
                SolverTime = result.Time,
                PreSolverTime = InitialSolver.Time
            };

            if (result.StatusCode == 1)
            {
                foreach (var item in result.Solution)
                {
                    var motion = Hierarchy.GetMotionBySeqID(item);
                    if (motion != null)
                    {
                        taskResult.SolutionHierarchy.Add(Hierarchy.GetRecordByMotion(motion));
                        taskResult.SolutionMotionIDs.Add(motion.ID);
                        taskResult.SolutionMotion.Add(motion);
                        foreach (var config in motion.Configs)
                        {
                            taskResult.SolutionConfig.Add(config);
                            taskResult.SolutionConfigIDs.Add(config.ID);
                        }
                    }
                    else
                    {
                        throw new SeqException("OR-Tools solution seqID can not be resolved: " + item);
                    }
                }

                if (taskResult.SolutionMotion.Count > 0)
                    AddConfigCostsOfMotion(taskResult, taskResult.SolutionMotion[0]);
                for (int i = 1; i < taskResult.SolutionMotion.Count; i++)
                {
                    AddConfigCostsBetweenMotions(taskResult, taskResult.SolutionMotion[i-1], taskResult.SolutionMotion[i]);
                    AddConfigCostsOfMotion(taskResult, taskResult.SolutionMotion[i]);
                }
            }

            taskResult = DepotMapper.ResolveSolution(taskResult);
            taskResult = ShortcutMapper.ResolveSolution(taskResult);
            taskResult.CalculateSum();
            return taskResult;
        }

        private void AddConfigCostsOfMotion(GeneralTaskResult result, Motion motion)
        {
            for (int i = 0; i < motion.Configs.Count-1; i++)
            {
                var cost = CostManager.ComputeCostInMotion(motion.Configs[i], motion.Configs[i + 1]);
                result.CostsBetweenConfigs.Add(cost);
                result.ConfigCosts.Add(cost.FinalCost);
                result.FullConfigCost += cost.FinalCost;  
            }
        }
        private void AddConfigCostsBetweenMotions(GeneralTaskResult result, Motion from, Motion to)
        {
            var cost = CostManager.ComputeCostBetweenMotionConfigs(from.LastConfig, to.FirstConfig);
            result.CostsBetweenConfigs.Add(cost);
            result.ConfigCosts.Add(cost.FinalCost);
            result.FullConfigCost += cost.FinalCost;

            var motionCost = CostManager.ComputeCost(from, to);
            result.CostsBetweenMotions.Add(motionCost);
            result.MotionCosts.Add(motionCost.FinalCost);
            result.FullMotionCost += motionCost.FinalCost;
        }

        //VALIDATE RESULT
        public void CheckSolution(List<MotionDisjointSet> disjointConstraints, List<MotionPrecedence> motionPrecedence, List<ProcessPrecedence> processPrecedences, List<Motion> solution)
        {
            ValidateDisjoint(disjointConstraints, solution);
            ValidateMotionPrec(motionPrecedence, solution);
            //ValidateProcessPrec(processPrecedences, solution);
        }
        private void ValidateMotionPrec(List<MotionPrecedence> motionPrecedences, List<Motion> solution)
        {
            if (motionPrecedences is not null && solution.Count > 0 && motionPrecedences.Count > 0)
            {
                var findFirst = false;
                var first = -1;
                var findSecond = false;
                var second = -1;
                foreach (var prec in motionPrecedences)
                {
                    findFirst = false;
                    first = -1;
                    findSecond = false;
                    second = -1;
                    for (int i = 0; i < solution.Count - 1; i++)
                    {
                        if (solution[i].ID == prec.Before.ID)
                        {
                            findFirst = true;
                            first = i;
                        }
                        if (solution[i].ID == prec.After.ID)
                        {
                            findSecond = true;
                            second = i;
                        }
                    }

                    if (findSecond && findFirst && first > second)
                    {
                        SeqLogger.Critical(solution.ToIDListString());
                        throw new SeqException("Result violates motion precedence: " + prec);
                    }

                    //Check last result item, if not equal with the start depot
                    if (solution[0].GlobalID != solution[^1].GlobalID)
                    {
                        findFirst = false;
                        first = -1;
                        findSecond = false;
                        second = -1;
                        if (solution[^1].ID == prec.Before.ID)
                        {
                            findFirst = true;
                            first = solution.Count;
                        }
                        if (solution[^1].ID == prec.After.ID)
                        {
                            findSecond = true;
                            second = solution.Count;
                        }
                    }

                    if (findSecond && findFirst && first > second)
                        throw new SeqException("Result violates motion precedence: " + prec);
                }
            }
        }

        private void ValidateDisjoint(List<MotionDisjointSet> disjointConstraints, List<Motion> solution)
        {
            foreach (var disj in disjointConstraints)
            {
                foreach (var d in disj.IDs)
                {
                    var findOne = false;
                    for (int i = 0; i < solution.Count - 1; i++)
                    {
                        if (d == solution[i].ID)
                            if (findOne == true)
                                throw new SeqException("Result contains more than one element of disjoint set.");
                            else
                                findOne = true;
                    }
                }
            };
        }

        private void ValidateProcessPrec(List<ProcessPrecedence> processPrecedence, List<Motion> solution)
        {
            foreach (var prec in processPrecedence)
            {
                var findFirst = false;
                var first = -1;

                var findSecond = false;
                var second = -1;
                for (int i = 0; i < solution.Count; i++)
                {
                    if (solution[i].ID == prec.Before.ID)
                    {
                        findFirst = true;
                        first = i;
                    }
                    if (solution[i].ID == prec.After.ID)
                    {
                        findSecond = true;
                        second = i;
                    }
                }
                if (findSecond && findFirst && first > second)
                    throw new SeqException("Result violates motion precedence: " + prec);
            }
        }
    }
}