using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProSeqqoLib.Function.ResourceFunction;
using ProSeqqoLib.Function.ResourceFunction.ResourceDistanceLink;
using ProSeqqoLib.Model.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSeqqoLib.Function.ResourceFunction.Token
{
    [TestClass()]
    public class MatrixResourceFunctionTests
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

        [TestMethod()]
        public void MatrixResourceFunctionTest()
        {

        }

        [TestMethod()]
        public void ComputeResourceCostTest()
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

        [TestMethod()]
        public void GetResourceCostTest()
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

        [TestMethod()]
        public void ValidateTest()
        {

        }

        [TestMethod()]
        public void ToLogTest()
        {

        }
    }
}