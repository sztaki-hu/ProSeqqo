using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.GTSPTask.Task.PointLike.ShortCut;
using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencerTest.Units.Helper
{
    [TestClass]
    public class ShortestPathSearchTest
    {
        private ShortestPathSearch Search;
        private List<Task> Tasks;

        [TestInitialize()]
        public void Initialize()
        {
            Tasks = new List<Task>();
            Task TA = new Task();
            TA.Positions.Add(TestObjects.GetPosA());
            TA.Positions.Add(TestObjects.GetPosB());
            Task TB = new Task();
            TB.Positions.Add(TestObjects.GetPosA());
            TB.Positions.Add(TestObjects.GetPosB());
            Task TC = new Task();
            TC.Positions.Add(TestObjects.GetPosA());
            TC.Positions.Add(TestObjects.GetPosB());
            Tasks.Add(TA);
            Tasks.Add(TB);
            Tasks.Add(TC);
            EuclidianDistanceFunction euclidianDistanceFunction = new EuclidianDistanceFunction();
            NoResourceFunction noResource = new NoResourceFunction();
            Search = new ShortestPathSearch(Tasks, euclidianDistanceFunction, noResource);
        }

        [TestMethod]
        public void Run_3Task2Pos()
        {
            var tmp = Search.CalculateCriticalRoute(Tasks[0].Positions[0], Tasks[2].Positions);
            Assert.AreEqual(tmp.Count, 2);
            Assert.IsTrue(TestObjects.CheckPosA((Position)tmp[0].Front));
            Assert.IsTrue(TestObjects.CheckPosA((Position)tmp[0].Back));
            Assert.AreEqual(tmp[0].Costs.Count, 2);
            Assert.AreEqual(tmp[0].Cut.Count, 3);
            Assert.IsTrue(TestObjects.CheckPosA((Position)tmp[1].Front));
            Assert.IsTrue(TestObjects.CheckPosB((Position)tmp[1].Back));
            Assert.AreEqual(tmp[1].Costs.Count, 2);
            Assert.AreEqual(tmp[1].Cut.Count, 3);

            tmp = Search.CalculateCriticalRoute(Tasks[0].Positions[1], Tasks[2].Positions);
            Assert.AreEqual(tmp.Count, 2);
            Assert.IsTrue(TestObjects.CheckPosB((Position)tmp[0].Front));
            Assert.IsTrue(TestObjects.CheckPosA((Position)tmp[0].Back));
            Assert.AreEqual(tmp[0].Costs.Count, 2);
            Assert.AreEqual(tmp[0].Cut.Count, 3);
            Assert.IsTrue(TestObjects.CheckPosB((Position)tmp[1].Front));
            Assert.IsTrue(TestObjects.CheckPosB((Position)tmp[1].Back));
            Assert.AreEqual(tmp[1].Costs.Count, 2);
            Assert.AreEqual(tmp[1].Cut.Count, 3);
        }
    }
}
