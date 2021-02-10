using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.GTSPTask.Serialization.Result;
using SequencerTest.Units.Helper;
using System;

namespace SequencerTest.Units.Serialization.Result
{
    [TestClass]
    public class LineLikeResultSerializationObjectTest
    {
        private LineLikeResultSerializationObject result;

        [TestInitialize()]
        public void Initialize()
        {
            result = new LineLikeResultSerializationObject();
            result = new LineLikeResultSerializationObject();
            result.SolutionRaw.Add(0);
            result.CostsRaw.Add(1);
            result.CostSum = 2;
            result.FullTime = DateTime.Today.ToString();
            result.PreSolverTime = DateTime.Today.ToString();
            result.SolverTime = DateTime.Today.ToString();
            result.StatusCode = 3;
            result.StatusMessage = "Status";
            result.Log.Add("FirstLog");
            result.LineResult.Add(TestObjects.GetLine());
            result.PositionResult.Add(TestObjects.GetLinePosA());
            result.PositionResult.Add(TestObjects.GetLinePosB());
        }

        [TestMethod]
        public void GetSet()
        {
            Assert.AreEqual(result.SolutionRaw[0], 0);
            Assert.AreEqual(result.CostsRaw[0], 1);
            Assert.AreEqual(result.CostSum, 2);
            Assert.AreEqual(result.FullTime, DateTime.Today.ToString());
            Assert.AreEqual(result.PreSolverTime, DateTime.Today.ToString());
            Assert.AreEqual(result.SolverTime, DateTime.Today.ToString());
            Assert.AreEqual(result.StatusCode, 3);
            Assert.AreEqual(result.StatusMessage, "Status");
            Assert.AreEqual(result.Log[0], "FirstLog");
            Assert.IsTrue(TestObjects.CheckLine(result.LineResult[0]));
            Assert.IsTrue(TestObjects.CheckLinePosA(result.PositionResult[0]));
            Assert.IsTrue(TestObjects.CheckLinePosB(result.PositionResult[1]));
        }


        [TestMethod]
        public void EmptyCalls()
        {
            var result = new LineLikeResultSerializationObject();
            result.ToLineLikeResult();
            result.ToSEQ();
            result.ToString();
        }

        [TestMethod]
        public void FilledCalls()
        {
            result.ToLineLikeResult();
            result.ToSEQ();
            result.ToString();

        }
    }
}
