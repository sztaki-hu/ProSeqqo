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
    public class ManhattanDistanceFunctionTests
    {
        ManhattanDistanceFunction func;
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
            func = new ManhattanDistanceFunction();
        }

        [TestMethod()]
        public void ComputeDistanceTest()
        {
            Assert.AreEqual(5, func.ComputeDistance(A, B));
            Assert.AreEqual(5, func.ComputeDistance(A, C));
            Assert.AreEqual(5, func.ComputeDistance(A, D));
            Assert.AreEqual(15, func.ComputeDistance(A, E));
        }
    }
}