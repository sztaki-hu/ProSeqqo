using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Model;
using SequencerTest.Units.Helper;

namespace SequencerTest.Units.Model
{
    [TestClass]
    public class StrictEdgeWeightTest
    {
        [TestInitialize()]
        public void Initialize()
        {
        }

        [TestMethod()]
        public void ConnstructorTest()
        {
            StrictEdgeWeight edge = new StrictEdgeWeight(TestObjects.GetPosA(), TestObjects.GetPosB(), 4, true);
            Assert.IsTrue(TestObjects.CheckPosA((Position)edge.A));
            Assert.IsTrue(TestObjects.CheckPosB((Position)edge.B));
            Assert.AreEqual(edge.Weight, 4);
            Assert.IsTrue(edge.Bidirectional);
        }

        [TestMethod()]
        public void GetterSetterTest()
        {
            StrictEdgeWeight edge = new StrictEdgeWeight(TestObjects.GetPosA(),TestObjects.GetPosB(), 4, true);
            edge.B = TestObjects.GetPosA();
            edge.A = TestObjects.GetPosB();
            edge.Bidirectional = false;
            edge.Weight = 1;
            Assert.IsTrue(TestObjects.CheckPosA((Position)edge.B));
            Assert.IsTrue(TestObjects.CheckPosB((Position)edge.A));
            Assert.AreEqual(edge.Weight, 1);
            Assert.IsFalse(edge.Bidirectional);
        }

        [TestMethod()]
        public void FitForTest()
        {
            Position a = TestObjects.GetPosA();
            Position b = TestObjects.GetPosB();
            StrictEdgeWeight edge = new StrictEdgeWeight(a, b, 4, true);
            Assert.IsTrue(edge.FitFor(a,b));
            Assert.IsTrue(edge.FitFor(b,a));
            edge.Bidirectional = false;
            Assert.IsTrue(edge.FitFor(a, b));
            edge.A = null;
            Assert.IsFalse(edge.FitFor(a, b));
            edge.A = a;
            Assert.IsFalse(edge.FitFor(a, null));
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Position a = TestObjects.GetPosA();
            Position b = TestObjects.GetPosB();
            StrictEdgeWeight edge = new StrictEdgeWeight(a, b, 4, true);
            var tmp = edge.ToString();
            Assert.IsTrue(tmp.Contains("4"));
            Assert.IsTrue(tmp.Contains("True"));
            Assert.IsTrue(tmp.Contains(a.UserID.ToString()));
            Assert.IsTrue(tmp.Contains(b.UserID.ToString()));
        }
    }
}
