using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProSeqqoLib.Function.ResourceFunction.ResourceDistanceLink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSeqqoLib.Function.ResourceFunction.ResourceDistanceLink.Token
{
    [TestClass()]
    public class AddResourceDistanceLinkFunctionTests
    {
        AddResourceDistanceLinkFunction func;
        double A;
        double B;
        double C;

        [TestInitialize()]
        public void Initialize()
        {
            A = 11;
            B = 5;
            C = 6;
            func = new AddResourceDistanceLinkFunction();
        }

        [TestMethod()]
        public void ComputeResourceDistanceCostTest()
        {
            Assert.AreEqual(11 + 5, func.ComputeResourceDistanceCost(A, B));
            Assert.AreEqual(11 + 6, func.ComputeResourceDistanceCost(A, C));
            Assert.AreEqual(11 + 5, func.ComputeResourceDistanceCost(B, A));
            Assert.AreEqual(11 + 11, func.ComputeResourceDistanceCost(A, A));
        }
    }
}