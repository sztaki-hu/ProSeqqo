using System;
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
            StartDepot = null;
            FinishDepot = null;
            CostManager = new CostManager();
            Hierarchy = new Hierarchy();
            SolverSettings = new SolverSettings();
            ProcessPrecedences    = new List<ProcessPrecedence>();
            MotionPrecedences    = new List<MotionPrecedence>();
            PCGTSPRepresentation = new PCGTSPRepresentation();
            DepotMapper = new NewGeneraDepotMapper(this);
            ShortcutMapper = new GeneralShortcutMapper(this);
            InitialSolver = new InitialSolver(this);
        }


        public Result.TaskResult Run()
        {
            ValidateTask();
            DepotMapper.Change();
            ShortcutMapper.Change();
            PCGTSPRepresentation.Build(Hierarchy, CostManager);
            InitialSolver.CreateInitialSolution();
            var result = RunTask();
            result = DepotMapper.ResolveSolution(result);
            result = ShortcutMapper.ResolveSolution(result);  
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
            return null;
            //var orToolsResult = orTools.Solve();
            //var result = ResolveSolution(orToolsResult);
            //return result;
        }

        private TaskResult ResolveSolution(TaskResult result)
        {
            throw new NotImplementedException();
        }
    }
}