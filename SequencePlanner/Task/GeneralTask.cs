using SequencePlanner.Helper;
using SequencePlanner.Model;
using SequencePlanner.Model.Hierarchy;
using SequencePlanner.OR_Tools;
using SequencePlanner.Task.Processors;
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
        public PCGTSPRepresentation PCGTSPRepresentation { get; set; }

        private GeneraDepotMapper DepotMapper { get; set; }
        private GeneralShortcutMapper ShortcutMapper { get; set; }
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
            PCGTSPRepresentation = new PCGTSPRepresentation(this);
            DepotMapper = new GeneraDepotMapper(this);
            ShortcutMapper = new GeneralShortcutMapper(this);
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
            PCGTSPRepresentation.Build();
            InitialSolver.CreateInitialSolution();
            var result = RunTask();
            DepotMapper.ChangeBack();
            ShortcutMapper.ChangeBack();
            Timer.Stop();
            result.FullTime = Timer.Elapsed;
            return result;
        }

        private void ValidateTask()
        {
            if (Validate)
                GeneralTaskValidator.Validate(this);
            else
                SeqLogger.Warning("Task validation turned off.");
        }

        private GeneralTaskResult RunTask()
        {
            var orToolsParam = new ORToolsTask()
            {
                TimeLimit = SolverSettings.TimeLimit,
                GTSPRepresentation = PCGTSPRepresentation,
                LocalSearchStrategy = SolverSettings.Metaheuristics
            };
            var orTools = new ORToolsSequencerWrapper(orToolsParam);
            orTools.Build();
            var orToolsResult = orTools.Solve();
            var result = ResolveSolution(orToolsResult);
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
    }
}