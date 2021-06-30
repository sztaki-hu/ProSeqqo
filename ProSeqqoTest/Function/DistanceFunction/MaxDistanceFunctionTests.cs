using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Model.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePlanner.Function.DistanceFunction.Token
{
    [TestClass()]
    public class MaxDistanceFunctionTests
    {
        MaxDistanceFunction func;
        Config A;
        Config B;
        Config C;
        Config D;
        Config E;

        [TestInitialize()]
        public void Initialize()
        {
            A = new Config(1, new List<double> { 0, 0, 0 });
            B = new Config(2, new List<double> { 5, 0, 0 });
            C = new Config(3, new List<double> { 0, 5, 0 });
            D = new Config(4, new List<double> { 0, 0, 5 });
            E = new Config(5, new List<double> { 5, 5, 5 });
            func = new MaxDistanceFunction();
        }

        [TestMethod()]
        public void ComputeDistanceTest()
        {
            Assert.AreEqual(0, func.ComputeDistance(A, A));
            Assert.AreEqual(5, func.ComputeDistance(A, B));
            Assert.AreEqual(5, func.ComputeDistance(A, C));
            Assert.AreEqual(5, func.ComputeDistance(A, D));
            Assert.AreEqual(5, func.ComputeDistance(A, E));
        }
    }
}