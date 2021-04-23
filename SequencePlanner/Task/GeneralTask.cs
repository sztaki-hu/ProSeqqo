using System;
using System.Collections.Generic;
using System.Diagnostics;
using SequencePlanner.GeneralModels.Result;
using SequencePlanner.Helper;
using SequencePlanner.OR_Tools;

namespace SequencePlanner.GeneralModels
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
            ProcessPrecedences    = new List<ProcessPrecedence>();
            MotionPrecedences    = new List<MotionPrecedence>();
            PCGTSPRepresentation = new PCGTSPRepresentation(this);
            DepotMapper = new GeneraDepotMapper(this);
            ShortcutMapper = new GeneralShortcutMapper(this);
            InitialSolver = new InitialSolver(this);
            Timer = new Stopwatch();
        }


        public Result.GeneralTaskResult Run()
        {
            Timer.Restart();
            ValidateTask();
            DepotMapper.Change();
            ShortcutMapper.Change();
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
                        taskResult.SolutionHierarchy.Add(Hierarchy.GetRecordByMotionID(motion.ID));
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

                for (int i = 1; i < taskResult.SolutionConfig.Count; i++)
                {
                    DetailedConfigCost detailedConfigCost = null;
                    if (taskResult.SolutionConfig.Count > 1)
                    {
                        detailedConfigCost = CostManager.ComputeCost(taskResult.SolutionConfig[i - 1], taskResult.SolutionConfig[i]);
                        taskResult.CostsBetweenConfigs.Add(detailedConfigCost);
                        taskResult.ConfigCosts.Add(detailedConfigCost.FinalCost);
                    }
                }

                for (int i = 1; i < taskResult.SolutionMotion.Count; i++)
                {
                    DetailedMotionCost detailedMotionCost = null;
                    if (taskResult.SolutionMotion.Count > 1)
                    {
                        detailedMotionCost = CostManager.GetDetailedMotionCost(taskResult.SolutionMotion[i - 1], taskResult.SolutionMotion[i]);
                        taskResult.CostsBetweenMotions.Add(detailedMotionCost);
                        taskResult.MotionCosts.Add(detailedMotionCost.FinalCost);
                    }
                }
            }

            taskResult = DepotMapper.ResolveSolution(taskResult);
            taskResult = ShortcutMapper.ResolveSolution(taskResult);
            return taskResult;
        }
    }
}