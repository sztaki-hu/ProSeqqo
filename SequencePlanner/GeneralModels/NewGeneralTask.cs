﻿using System;
using System.Collections.Generic;
using SequencePlanner.GeneralModels.Result;
using SequencePlanner.Helper;
using SequencePlanner.OR_Tools;

namespace SequencePlanner.GeneralModels
{
    public class NewGeneralTask
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

        private NewGeneraDepotMapper DepotMapper { get; set; }
        private GeneralShortcutMapper ShortcutMapper { get; set; }
        private InitialSolver InitialSolver { get; set; }

        public NewGeneralTask()
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
            DepotMapper = new NewGeneraDepotMapper(this);
            ShortcutMapper = new GeneralShortcutMapper(this);
            InitialSolver = new InitialSolver(this);
        }


        public Result.TaskResult Run()
        {
            ValidateTask();
            DepotMapper.Change();
            ShortcutMapper.Change();
            PCGTSPRepresentation.Build();
            InitialSolver.CreateInitialSolution();
            var result = RunTask();
            DepotMapper.ChangeBack();
            ShortcutMapper.ChangeBack();
            return result;
        }

        private void ValidateTask()
        {
            if (Validate)
                NewGeneralTaskValidator.Validate(this);
            else
                SeqLogger.Warning("Task validation turned off.");
        }

        private TaskResult RunTask()
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

        private TaskResult ResolveSolution(ORToolsResult result)
        {

            TaskResult taskResult = new TaskResult()
            {
                StatusCode = result.StatusCode,
                StatusMessage = result.StatusMessage
            };
            if(result.StatusCode==1)
                foreach (var item in result.Solution)
                {
                    taskResult.Solution.Add(item);
                    var motion = Hierarchy.GetMotionBySeqID(item);
                    if (motion == null)
                        throw new SeqException("OR-Tools solution seqID can not be resolved: "+ item);
                    foreach (var config in motion.Configs)
                    {
                        taskResult.SolutionConfig.Add(config);
                    }
                    Console.WriteLine(motion);
                }
            taskResult = DepotMapper.ResolveSolution(taskResult);
            taskResult = ShortcutMapper.ResolveSolution(taskResult);
            return taskResult;
        }
    }
}