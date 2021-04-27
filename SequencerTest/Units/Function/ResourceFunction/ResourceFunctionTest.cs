using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;
using SequencePlanner.Model.Hierarchy;
using System.Collections.Generic;

namespace SequencerTest.Units.Function.ResourceFunction
{
    [TestClass]
    public class ResourceFunctionTest
    {
        IResourceFunction func;
        Config A;
        Config B;
        Config C;

        [TestInitialize()]
        public void Initialize()
        {
            A = new Config(1, new List<double> { 0, 0, 0 }) { Resource = new Resource(0, "") };
            B = new Config(1, new List<double> { 5, 0, 0 }) { Resource = new Resource(1, "") };
            C = new Config(1, new List<double> { 0, 5, 0 }) { Resource = new Resource(2, "") };
        }

        [TestMethod]
        public void ConstantResourceFunctionGetResourceValue()
        {
            func = new ConstantResourceFunction(1, new AddResourceDistanceLinkFunction());
            Assert.AreEqual(1, func.GetResourceCost(A, B));
            Assert.AreEqual(1, func.GetResourceCost(A, C));
            Assert.AreEqual(1, func.GetResourceCost(B, A));
            Assert.AreEqual(0, func.GetResourceCost(A, A));

            func = new ConstantResourceFunction(10, new MaxResourceDistanceLinkFunction());
            Assert.AreEqual(10, func.GetResourceCost(A, B));
            Assert.AreEqual(10, func.GetResourceCost(A, C));
            Assert.AreEqual(10, func.GetResourceCost(B, A));
            Assert.AreEqual(0,  func.GetResourceCost(A, A));
        }

        [TestMethod]
        public void ConstantResourceFunction()
        {
            func = new ConstantResourceFunction(1, new AddResourceDistanceLinkFunction());
            Assert.AreEqual(2,  func.ComputeResourceCost(A, B, 1));
            Assert.AreEqual(3,  func.ComputeResourceCost(A, C, 2));
            Assert.AreEqual(-4, func.ComputeResourceCost(B, A,-5));
            Assert.AreEqual(0,  func.ComputeResourceCost(A, A, 0));

            func = new ConstantResourceFunction(1, new MaxResourceDistanceLinkFunction());
            Assert.AreEqual(1, func.ComputeResourceCost(A, B, 1));
            Assert.AreEqual(2, func.ComputeResourceCost(A, C, 2));
            Assert.AreEqual(1, func.ComputeResourceCost(B, A,-5));
            Assert.AreEqual(0, func.ComputeResourceCost(A, A, 0));
        }

        [TestMethod]
        public void MatrixResourceFunctionGetResourceValue()
        {
            func = new MatrixResourceFunction(new List<List<double>> { new List<double> { 1, 2, 3 },
                                                                       new List<double> { 1, 2, 3 },
                                                                       new List<double> { 1, 2, 3 } },
                                                                       new List<int> { A.Resource.ID, B.Resource.ID, C.Resource.ID }, 
                                                                       new AddResourceDistanceLinkFunction());
            Assert.AreEqual(2, func.GetResourceCost(A, B));
            Assert.AreEqual(3, func.GetResourceCost(A, C));
            Assert.AreEqual(1, func.GetResourceCost(B, A));
            Assert.AreEqual(1, func.GetResourceCost(A, A));
        }

        [TestMethod]
        public void MatrixResourceFunction()
        {
            func = new MatrixResourceFunction(new List<List<double>> { new List<double> { 1, 2, 3 },
                                                                       new List<double> { 1, 2, 3 },
                                                                       new List<double> { 1, 2, 3 } },
                                                                       new List<int> { A.Resource.ID, B.Resource.ID, C.Resource.ID }, 
                                                                       new AddResourceDistanceLinkFunction());
            Assert.AreEqual(3, func.ComputeResourceCost(A, B, 1));
            Assert.AreEqual(5, func.ComputeResourceCost(A, C, 2));
            Assert.AreEqual(2, func.ComputeResourceCost(B, A, 1));
            Assert.AreEqual(2, func.ComputeResourceCost(A, A, 1));


            func = new MatrixResourceFunction(new List<List<double>> { new List<double> { 1, 2, 3 },
                                                                       new List<double> { 1, 2, 3 },
                                                                       new List<double> { 1, 2, 3 } },
                                                                       new List<int> { A.Resource.ID, B.Resource.ID, C.Resource.ID },
                                                                       new MaxResourceDistanceLinkFunction());
            Assert.AreEqual(2, func.ComputeResourceCost(A, B, 1));
            Assert.AreEqual(3, func.ComputeResourceCost(A, C, 2));
            Assert.AreEqual(1, func.ComputeResourceCost(B, A, 1));
            Assert.AreEqual(1, func.ComputeResourceCost(A, A, 1));
        }
    }
}
