using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProSeqqoLib.Task;

namespace SequencerTest.Helper
{
    public static class AssertGeneralTask
    {
        public static void AssertTasks(GeneralTask a, GeneralTask b)
        {
            AssertValidate(a,b);
            AssertCyclic(a,b);
            AssertStartDepot(a,b);
            AssertFinishDepot(a,b);
            AssertStartDepot(a, b);
            AssertFinishDepot(a, b);
            AssertConfigs(a, b);
            AssertHierarcy(a, b);
            AssertProcessPrecedences(a, b);
            AssertMotionPrecedences(a, b);
            AssertProcessPrecedences(a, b);
            AssertMotionPrecedences(a, b);
            AssertOverrideCosts(a, b);
            AssertDistanceFunction(a, b);
            AssertIdlePenalty(a, b);
            AssertBID(a, b);
            AssertInMotion(a, b);
            AssertResource(a, b);
            AssertHeur(a, b);
            AssertTimeLimit(a, b);
            AssertUSA(a, b);
            AssertMIP(a, b);
        }

        public static void AssertValidate(GeneralTask a, GeneralTask b)
        {
            Assert.AreEqual(a.Validate, b.Validate);
        }
        public static void AssertCyclic(GeneralTask a, GeneralTask b)
        {
            Assert.AreEqual(a.Cyclic, b.Cyclic);
        }

        public static void AssertStartDepot(GeneralTask a, GeneralTask b)
        {
            if(a.StartDepot is not null || b.StartDepot is not null)
                Assert.AreEqual(a.StartDepot.ID, b.StartDepot.ID);
            if(a.StartDepotConfig is not null || b.StartDepotConfig is not null)
                Assert.AreEqual(a.StartDepotConfig.ID, b.StartDepotConfig.ID);
        }

        public static void AssertFinishDepot(GeneralTask a, GeneralTask b)
        {
            if(a.FinishDepot is not null || b.FinishDepot is not null)
                Assert.AreEqual(a.FinishDepot.ID, b.FinishDepot.ID);
            if(a.FinishDepotConfig is not null || b.FinishDepotConfig is not null)
            Assert.AreEqual(a.FinishDepotConfig.ID, b.FinishDepotConfig.ID);
        }

        public static void AssertConfigs(GeneralTask a, GeneralTask b)
        {
            Assert.AreEqual(a.Hierarchy.Configs.Count, b.Hierarchy.Configs.Count);
            //TODO: By elements
        }

        public static void AssertHierarcy(GeneralTask a, GeneralTask b)
        {
            Assert.AreEqual(a.Hierarchy.HierarchyRecords.Count, b.Hierarchy.HierarchyRecords.Count);
            //TODO: By elements
        }

        public static void AssertProcessPrecedences(GeneralTask a, GeneralTask b)
        {
            Assert.AreEqual(a.Hierarchy.HierarchyRecords.Count, b.Hierarchy.HierarchyRecords.Count);
            //TODO: By elements
        }

        public static void AssertMotionPrecedences(GeneralTask a, GeneralTask b)
        {
            Assert.AreEqual(a.Hierarchy.HierarchyRecords.Count, b.Hierarchy.HierarchyRecords.Count);
            //TODO: By elements
        }

        public static void AssertOverrideCosts(GeneralTask a, GeneralTask b)
        {
            Assert.AreEqual(a.CostManager.OverrideCost.Count, b.CostManager.OverrideCost.Count);
            //TODO: By elements
        }

        public static void AssertDistanceFunction(GeneralTask a, GeneralTask b)
        {
            Assert.AreEqual(a.CostManager.DistanceFunction.FunctionName, b.CostManager.DistanceFunction.FunctionName);
            //TODO: Trapezoid, Matrix
        }

        public static void AssertIdlePenalty(GeneralTask a, GeneralTask b)
        {
            Assert.AreEqual(a.CostManager.IdlePenalty, b.CostManager.IdlePenalty);
        }

        public static void AssertBID(GeneralTask a, GeneralTask b)
        {
            Assert.AreEqual(a.Hierarchy.BidirectionalMotionDefault, b.Hierarchy.BidirectionalMotionDefault);
        }
        
        public static void AssertInMotion(GeneralTask a, GeneralTask b)
        {
            Assert.AreEqual(a.CostManager.AddMotionLengthToCost, b.CostManager.AddMotionLengthToCost);
        }

        public static void AssertResource(GeneralTask a, GeneralTask b)
        {
            Assert.AreEqual(a.CostManager.ResourceFunction.FunctionName, b.CostManager.ResourceFunction.FunctionName);
            //TODO: Resouce details
        }

        public static void AssertHeur(GeneralTask a, GeneralTask b)
        {
            Assert.AreEqual(a.SolverSettings.Metaheuristics, b.SolverSettings.Metaheuristics);
        }

        public static void AssertTimeLimit(GeneralTask a, GeneralTask b)
        {
            Assert.AreEqual(a.SolverSettings.TimeLimit, b.SolverSettings.TimeLimit);
        }

        public static void AssertUSA(GeneralTask a, GeneralTask b)
        {
            Assert.AreEqual(a.SolverSettings.UseShortcutInAlternatives, b.SolverSettings.UseShortcutInAlternatives);
        }

        public static void AssertMIP(GeneralTask a, GeneralTask b)
        {
            Assert.AreEqual(a.SolverSettings.UseMIPprecedenceSolver, b.SolverSettings.UseMIPprecedenceSolver);
        }
    }
}
