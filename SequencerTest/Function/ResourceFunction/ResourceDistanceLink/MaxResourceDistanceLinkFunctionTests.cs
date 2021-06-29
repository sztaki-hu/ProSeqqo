using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePlanner.Function.ResourceFunction.ResourceDistanceLink.Token
{
    [TestClass()]
    public class MaxResourceDistanceLinkFunctionTests
    {
        MaxResourceDistanceLinkFunction func;
        double A;
        double B;
        double C;

        [TestInitialize()]
        public void Initialize()
        {
            A = 11;
            B = 5;
            C = 6;
            func = new MaxResourceDistanceLinkFunction();
        }

        [TestMethod()]
        public void ComputeResourceDistanceCostTest()
        {
            Assert.AreEqual(11, func.ComputeResourceDistanceCost(A, B));
            Assert.AreEqual(11, func.ComputeResourceDistanceCost(A, C));
            Assert.AreEqual(11, func.ComputeResourceDistanceCost(B, A));
        }
    }
}