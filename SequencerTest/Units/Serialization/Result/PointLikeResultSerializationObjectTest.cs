using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.GTSPTask.Serialization.Result;
using SequencerTest.Units.Helper;
using System;

namespace SequencerTest.Units.Serialization.Result
{
    [TestClass]
    public class PointLikeResultSerializationObjectTest
    {
        private PointLikeResultSerializationObject result;

        [TestInitialize()]
        public void Initialize()
        {
            result = new PointLikeResultSerializationObject();
            result.SolutionRaw.Add(0);
            result.CostsRaw.Add(1);
            result.CostSum = 2;
            result.FullTime = new TimeSpan().ToString();
            result.PreSolverTime = new TimeSpan().ToString();
            result.SolverTime = new TimeSpan().ToString();
            result.StatusCode = 3;
            result.StatusMessage = "Status";
            result.Log.Add("FirstLog");
            result.PositionResult.Add(TestObjects.GetPosA());
            result.PositionResult.Add(TestObjects.GetPosB());
        }

        [TestMethod]
        public void GetSet()
        {
            Assert.AreEqual(result.SolutionRaw[0], 0);
            Assert.AreEqual(result.CostsRaw[0], 1);
            Assert.AreEqual(result.CostSum, 2);
            Assert.AreEqual(result.FullTime, new TimeSpan().ToString());
            Assert.AreEqual(result.PreSolverTime, new TimeSpan().ToString());
            Assert.AreEqual(result.SolverTime, new TimeSpan().ToString());
            Assert.AreEqual(result.StatusCode, 3);
            Assert.AreEqual(result.StatusMessage, "Status");
            Assert.AreEqual(result.Log[0], "FirstLog");
            Assert.IsTrue(TestObjects.CheckPosA(result.PositionResult[0].In));
            Assert.IsTrue(TestObjects.CheckPosB(result.PositionResult[1].In));
        }


        [TestMethod]
        public void EmptyCalls()
        {
            var result = new PointLikeResultSerializationObject();
            result.ToPointLikeResult();
            result.ToSEQ();
            result.ToString();
        }

        [TestMethod]
        public void FilledCalls()
        {
            result.ToPointLikeResult();
            result.ToSEQ();
            result.ToString();
        }
    }
}
