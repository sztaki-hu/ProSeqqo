using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Model;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;
using System.Collections.Generic;

namespace SequencerTest.Units
{
    [TestClass]
    public class ResourceFunctionTest
    {
        IResourceFunction func;
        Position A;
        Position B;
        Position C;
        Position D;
        Position E;

        [TestInitialize()]
        public void Initialize()
        {
            A = new Position() { Vector = new double[] { 0, 0, 0 } };
            B = new Position() { Vector = new double[] { 5, 0, 0 } };
            C = new Position() { Vector = new double[] { 0, 5, 0 } };
            D = new Position() { Vector = new double[] { 0, 0, 5 } };
            E = new Position() { Vector = new double[] { 5, 5, 5 } };
        }

        [TestMethod]
        public void ConstantResourceFunction()
        {
            func = new ConstantResourceFunction(1, new AddResourceDistanceLinkFunction());
            Assert.AreEqual(2, func.ComputeResourceCost(A, B, 1));
            Assert.AreEqual(3, func.ComputeResourceCost(A, C, 2));
            Assert.AreEqual(-4, func.ComputeResourceCost(B, A, -5));
            Assert.AreEqual(1, func.ComputeResourceCost(A, A, 0));
            func = new ConstantResourceFunction(1, new MaxResourceDistanceLinkFunction());
            Assert.AreEqual(1, func.ComputeResourceCost(A, B, 1));
            Assert.AreEqual(2, func.ComputeResourceCost(A, C, 2));
            Assert.AreEqual(1, func.ComputeResourceCost(B, A, -5));
            Assert.AreEqual(1, func.ComputeResourceCost(A, A, 0));
        }

        [TestMethod]
        public void MatrixResourceFunction()
        {
            func = new MatrixResourceFunction(new List<List<double>> { new List<double> { 1, 2, 3 },
                                                                       new List<double> { 1, 2, 3 },
                                                                       new List<double> { 1, 2, 3 } },
                                                                       new List<int> { A.GlobalID, B.GlobalID, C.GlobalID }, 
                                                                       new AddResourceDistanceLinkFunction());
            Assert.AreEqual(2, func.ComputeResourceCost(A, B, 1));
            Assert.AreEqual(3, func.ComputeResourceCost(A, C, 2));
            Assert.AreEqual(1, func.ComputeResourceCost(B, A, 1));
            Assert.AreEqual(1, func.ComputeResourceCost(A, A, 1));

            func = new MatrixResourceFunction(new List<List<double>> { new List<double> { 1, 2, 3 },
                                                                       new List<double> { 1, 2, 3 },
                                                                       new List<double> { 1, 2, 3 } }, 
                                                                       new List<int> { A.GlobalID, B.GlobalID, C.GlobalID },
                                                                       new MaxResourceDistanceLinkFunction());
            Assert.AreEqual(2, func.ComputeResourceCost(A, B, 1));
            Assert.AreEqual(3, func.ComputeResourceCost(A, C, 1));
            Assert.AreEqual(3, func.ComputeResourceCost(B, C, 1));
            Assert.AreEqual(1, func.ComputeResourceCost(A, A, 1));
        }

        [TestMethod]
        public void PositionBasedResourceFunction()
        {
            func = new PositionBasedResourceFunction(new EuclidianDistanceFunction(), new AddResourceDistanceLinkFunction());
            Assert.AreEqual(0, func.ComputeResourceCost(A, B, 1));
            Assert.AreEqual(0, func.ComputeResourceCost(A, C, 1));
            Assert.AreEqual(0, func.ComputeResourceCost(B, A, 1));
            Assert.AreEqual(0, func.ComputeResourceCost(A, A, 1));

            func = new PositionBasedResourceFunction(new EuclidianDistanceFunction(), new MaxResourceDistanceLinkFunction());
            Assert.AreEqual(0, func.ComputeResourceCost(A, B, 1));
            Assert.AreEqual(0, func.ComputeResourceCost(A, C, 1));
            Assert.AreEqual(0, func.ComputeResourceCost(B, A, 1));
            Assert.AreEqual(0, func.ComputeResourceCost(A, A, 1));
        }
    }
}
