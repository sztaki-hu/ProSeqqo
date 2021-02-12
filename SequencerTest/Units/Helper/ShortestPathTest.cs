using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Helper;
using SequencePlanner.Model;

namespace SequencerTest.Units.Helper
{
    [TestClass]
    public class ShortestPathTest
    {
        private ShortestPath Path;

        [TestInitialize()]
        public void Initialize()
        {
            Path = new ShortestPath(TestObjects.GetPosA(), TestObjects.GetPosB(), 2);
            Path.Costs.Add(9);
            Path.Costs.Add(10);
            Path.Cut.Add(TestObjects.GetPosA());
            Path.Cut.Add(TestObjects.GetPosB());
        }

        [TestMethod]
        public void GetSet()
        {
            Assert.IsTrue(TestObjects.CheckPosA((Position)Path.Front));
            Assert.IsTrue(TestObjects.CheckPosB((Position)Path.Back));
            Assert.IsTrue(TestObjects.CheckPosA((Position)Path.Cut[0]));
            Assert.IsTrue(TestObjects.CheckPosB((Position)Path.Cut[1]));
            Assert.AreEqual(Path.Costs[0], 9);
            Assert.AreEqual(Path.Costs[1], 10);
            Assert.AreEqual(Path.Cost, 2);
        }
    }
}
