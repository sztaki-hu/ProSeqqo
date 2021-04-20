using SequencePlanner.GTSPTask.Result;
using SequencePlanner.Helper;
using SequencePlanner.OR_Tools;
using System;
using System.Collections.Generic;

namespace SequencePlanner.GeneralModels
{
    public class NewGeneralTask
    {
        public bool Validate { get; set; }
        public bool Cyclic { get; set; }
        public Motion StartDepot { get; set; }
        public Motion FinishDepot { get; set; }

        public List<Motion> Motions { get; set; }
        public List<Config> Configs { get; set; }
        public CostManager CostManager { get; set; }
        public Hierarchy Hierarchy { get; set; }
        public SolverSettings SolverSettings { get; set; }
        public List<ProcessPrecedence> Processrecedences { get; set; }
        public List<MotionPrecedence> MotionPrecedences { get; set; }
        public PCGTSPRepresentation PCGTSPRepresentation { get; set; }

        public NewGeneralTask()
        {
            Validate = true;
            Cyclic = true;
            StartDepot = null;
            FinishDepot = null;
            Motions = new List<Motion>();
            Configs = new List<Config>();
            CostManager = new CostManager();
            Hierarchy = new Hierarchy();
            SolverSettings = new SolverSettings();
            Processrecedences    = new List<ProcessPrecedence>();
            MotionPrecedences    = new List<MotionPrecedence>();
            PCGTSPRepresentation = new PCGTSPRepresentation();
        }


        public void Run()
        {
            ValidateTask();
            ChangeDepots();
            if (SolverSettings.UseMIPprecedenceSolver)
                CreateInitialSolution();

            if (SolverSettings.UseShortcutInAlternatives)
                CreateShortcuts();

            PCGTSPRepresentation.Build(Hierarchy, CostManager, Motions);
            GeneralTaskResult result = RunTask();
            result = ResolveSolution(result);

            if (SolverSettings.UseShortcutInAlternatives)
                ReverseShortcutsInSolution();

            if (SolverSettings.UseShortcutInAlternatives)
                result = ReverseShortcuts(result);

            result = ResolveDepots(result);
            ChangeBackDepots();
        }



        private void ValidateTask()
        {
            if (Validate)
                NewGeneralTaskValidator.Validate(this);
            else
                SeqLogger.Warning("Task validation turned off.");
        }

        private GeneralTaskResult RunTask()
        {
            var orToolsParam = new ORToolsTask()
            {
                TimeLimit = SolverSettings.TimeLimit,
                GTSPRepresentation = PCGTSPRepresentation.ToOldGTSPRepresentation(),
                LocalSearchStrategy = SolverSettings.Metaheuristics
            };
            var orTools = new ORToolsSequencerWrapper(orToolsParam);
            orTools.Build();
            return orTools.Solve();
        }

        private void CreateInitialSolution()
        {
            throw new NotImplementedException();
        }

        private void CreateShortcuts()
        {
            throw new NotImplementedException();
        }

        private GeneralTaskResult ReverseShortcuts(GeneralTaskResult result)
        {
            return result;
            throw new NotImplementedException();
        }

        private GeneralTaskResult ResolveSolution(GeneralTaskResult result)
        {
            throw new NotImplementedException();
        }

        private void ReverseShortcutsInSolution()
        {
            throw new NotImplementedException();
        }

        private void ChangeDepots() { }
        private void ChangeBackDepots() { }
        private GeneralTaskResult ResolveDepots(GeneralTaskResult result)
        {
            return result;
        }
    }
}
