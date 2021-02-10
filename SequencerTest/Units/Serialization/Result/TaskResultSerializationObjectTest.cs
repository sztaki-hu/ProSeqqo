using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Serialization.Result;
using System;

namespace SequencerTest.Units.Serialization.Result
{
    [TestClass]
    public class TaskResultSerializationObjectTest
    {
        private TaskResultSerializationObject result;

        [TestInitialize()]
        public void Initialize()
        {
            result = new TaskResultSerializationObject();
            result = new TaskResultSerializationObject();
            result.SolutionRaw.Add(0);
            result.CostsRaw.Add(1);
            result.CostSum = 2;
            result.FullTime = new TimeSpan().ToString();
            result.PreSolverTime = new TimeSpan().ToString();
            result.SolverTime = new TimeSpan().ToString();
            result.StatusCode = 3;
            result.StatusMessage = "Status";
            result.Log.Add("FirstLog");
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
        }


        [TestMethod]
        public void EmptyCalls()
        {
            var result = new TaskResultSerializationObject();
            var taskResult = new TaskResult();
            result.ToSEQ();
            result.ToTaskResult(taskResult);
            result.ToString();
        }

        [TestMethod]
        public void FilledCalls()
        {
            var taskResult = new TaskResult();
            result.ToSEQ();
            result.ToTaskResult(taskResult);
            result.ToString();
        }
    }
}