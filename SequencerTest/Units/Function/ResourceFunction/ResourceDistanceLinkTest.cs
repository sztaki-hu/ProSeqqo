using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;

namespace SequencerTest.Units.Function.ResourceFunction
{
    [TestClass]
    public class ResourceDistanceLinkTest
    {
        IResourceDistanceLinkFunction func;
        double A;
        double B;
        double C;

        [TestInitialize()]
        public void Initialize()
        {
            A = 11;
            B = 5;
            C = 6;
        }

        [TestMethod]
        public void AddResourceDistanceLinkFunction()
        {
            func = new AddResourceDistanceLinkFunction();
            Assert.AreEqual(11 + 5, func.ComputeResourceDistanceCost(A, B));
            Assert.AreEqual(11 + 6, func.ComputeResourceDistanceCost(A, C));
            Assert.AreEqual(11 + 5, func.ComputeResourceDistanceCost(B, A));
            Assert.AreEqual(11 + 11, func.ComputeResourceDistanceCost(A, A));
        }

        [TestMethod]
        public void MaxResourceDistanceLinkFunction()
        {
            func = new MaxResourceDistanceLinkFunction();
            Assert.AreEqual(11, func.ComputeResourceDistanceCost(A, B));
            Assert.AreEqual(11, func.ComputeResourceDistanceCost(A, C));
            Assert.AreEqual(11, func.ComputeResourceDistanceCost(B, A));
        }
    }
}
