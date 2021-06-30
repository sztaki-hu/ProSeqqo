using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProSeqqoLib.Task;
using System;

namespace SequencerTest.Helper
{
    public static class AssertGeneralResult
    {
        public static void AssertResults(GeneralTaskResult a, GeneralTaskResult b)
        {
            AssertFullTime(a, b);
            AssertSolverTime(a, b);
            AssertPreSolverTime(a, b);
            AssertStatusCode(a, b);
            AssertStatus(a, b);
            AssertError(a, b);
            AssertSolution(a, b);
        }

        public static void AssertFullTime(GeneralTaskResult a, GeneralTaskResult b)
        {
            //Assert.IsTrue(Math.Abs((a.FullTime - b.FullTime).TotalSeconds) < a.FullTime.TotalSeconds*0.4);
        }
        public static void AssertSolverTime(GeneralTaskResult a, GeneralTaskResult b)
        {
            //Assert.IsTrue(Math.Abs((a.SolverTime - b.SolverTime).TotalSeconds) < a.SolverTime.TotalSeconds * 0.4);
        }
        public static void AssertPreSolverTime(GeneralTaskResult a, GeneralTaskResult b)
        {
            //Assert.IsTrue(Math.Abs((a.PreSolverTime - b.PreSolverTime).TotalSeconds) < a.PreSolverTime.TotalSeconds * 0.4);
        }
        public static void AssertStatusCode(GeneralTaskResult a, GeneralTaskResult b)
        {
            Assert.AreEqual(a.StatusCode, b.StatusCode);
        }
        public static void AssertStatus(GeneralTaskResult a, GeneralTaskResult b)
        {
            Assert.AreEqual(a.StatusMessage, b.StatusMessage);
        }
        public static void AssertError(GeneralTaskResult a, GeneralTaskResult b)
        {
            Assert.AreEqual(a.ErrorMessage, b.ErrorMessage);
        }
        public static void AssertSolution(GeneralTaskResult a, GeneralTaskResult b)
        {
            Assert.AreEqual(a.SolutionMotionIDs.Count, b.SolutionMotionIDs.Count);
            Assert.AreEqual(a.SolutionConfigIDs.Count, b.SolutionConfigIDs.Count);
            Assert.AreEqual(a.SolutionMotion.Count, b.SolutionMotion.Count);
            Assert.AreEqual(a.SolutionConfig.Count, b.SolutionConfig.Count);
            Assert.AreEqual(a.SolutionHierarchy.Count, b.SolutionHierarchy.Count);
            Assert.AreEqual(a.ConfigCosts.Count, b.ConfigCosts.Count);
            Assert.AreEqual(a.MotionCosts.Count, b.MotionCosts.Count);
            Assert.IsTrue(Math.Abs(a.FullMotionCost - b.FullMotionCost)<a.FullMotionCost*0.1);
            Console.WriteLine(a.FullMotionCost+" " + b.FullMotionCost);
            Assert.IsTrue(Math.Abs(a.FullConfigCost-b.FullConfigCost)< a.FullConfigCost * 0.1);
            Assert.AreEqual(a.CostsBetweenMotions.Count, b.CostsBetweenMotions.Count);
            Assert.AreEqual(a.CostsBetweenConfigs.Count, b.CostsBetweenConfigs.Count);
            Assert.IsTrue(Math.Abs(a.DetailedConfigCost.FinalCost-b.DetailedConfigCost.FinalCost)< a.DetailedConfigCost.FinalCost * 0.1);
            Assert.IsTrue(Math.Abs(a.DetailedMotionCost.FinalCost-b.DetailedMotionCost.FinalCost)< a.DetailedMotionCost.FinalCost * 0.1);
        }
    }
}
