using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Helper;
using SequencePlanner.Model.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePlanner.Function.DistanceFunction.Token
{
    [TestClass()]
    public class TrapezoidTimeDistanceFunctionTests
    {
        TrapezoidTimeDistanceFunction func;
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
            func = new TrapezoidTimeDistanceFunction(new double[] { 1, 2, 3 }, new double[] { 1, 2, 3 });
        }

        [TestMethod()]
        public void TrapezoidTimeDistanceFunctionTest()
        {
            Assert.ThrowsException<SeqException>(() => new TrapezoidTimeDistanceFunction(null, new double[] { 1, 2, 3 }));
            Assert.ThrowsException<SeqException>(() => new TrapezoidTimeDistanceFunction(new double[] { 1, 2, 3 }, null));
            Assert.ThrowsException<SeqException>(() => new TrapezoidTimeDistanceFunction(new double[] { }, new double[] { 1, 2, 3 }));
            Assert.ThrowsException<SeqException>(() => new TrapezoidTimeDistanceFunction(new double[] { 1, 2, 3 }, new double[] { }));
            Assert.ThrowsException<SeqException>(() => new TrapezoidTimeDistanceFunction(new double[] { 1, 2, 3, 4 }, new double[] { 1, 2, 3 }));
            Assert.ThrowsException<SeqException>(() => new TrapezoidTimeDistanceFunction(new double[] { 1, 2, 3 }, new double[] { 1, 2, 3, 4 }));
        }

        [TestMethod()]
        public void ComputeDistanceTest()
        {
            Assert.AreEqual("TrapezoidTime", func.FunctionName);
            Assert.AreEqual(6, func.ComputeDistance(A, B));
            Assert.AreEqual(6, func.ComputeDistance(B, A));
            Assert.AreEqual(3.5, func.ComputeDistance(A, C));
            Assert.AreEqual(2.67, Math.Round(func.ComputeDistance(A, D), 2));
            Assert.AreEqual(6, func.ComputeDistance(A, E));
        }

        [TestMethod()]
        public void ValidateTest()
        {

        }
    }
}