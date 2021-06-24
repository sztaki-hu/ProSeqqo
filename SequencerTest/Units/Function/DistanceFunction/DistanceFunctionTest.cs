using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Helper;
using SequencePlanner.Model.Hierarchy;
using System;
using System.Collections.Generic;

namespace SequencerTest.Units.Function.DistanceFunction
{
    [TestClass]
    public class DistanceFunctionTest
    {
        IDistanceFunction func;
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
        }

        [TestMethod()]
        public void EuclidianDistanceFunction()
        {
            func = new EuclidianDistanceFunction();
            Assert.AreEqual(5, func.ComputeDistance(A, B));
            Assert.AreEqual(5, func.ComputeDistance(A, C));
            Assert.AreEqual(5, func.ComputeDistance(A, D));
            Assert.AreEqual(Math.Round(8.660254, 2), Math.Round(func.ComputeDistance(A, E), 2));
        }

        [TestMethod()]
        public void MaxDistanceFunction()
        {
            func = new MaxDistanceFunction();
            Assert.AreEqual(0, func.ComputeDistance(A, A));
            Assert.AreEqual(5, func.ComputeDistance(A, B));
            Assert.AreEqual(5, func.ComputeDistance(A, C));
            Assert.AreEqual(5, func.ComputeDistance(A, D));
            Assert.AreEqual(5, func.ComputeDistance(A, E));
        }

        [TestMethod()]
        public void ManhattanDistanceFunction()
        {
            func = new ManhattanDistanceFunction();
            Assert.AreEqual(5, func.ComputeDistance(A, B));
            Assert.AreEqual(5, func.ComputeDistance(A, C));
            Assert.AreEqual(5, func.ComputeDistance(A, D));
            Assert.AreEqual(15, func.ComputeDistance(A, E));
        }

        [TestMethod()]
        public void TrapezoidTimeDistanceFunction()
        {
            Assert.ThrowsException<SeqException>(() =>    new TrapezoidTimeDistanceFunction(null,                        new double[] { 1, 2, 3 }));
            Assert.ThrowsException<SeqException>(() => new TrapezoidTimeDistanceFunction(new double[] { 1, 2, 3 },    null));
            Assert.ThrowsException<SeqException>(() => new TrapezoidTimeDistanceFunction(new double[] { },            new double[] { 1, 2, 3 }));
            Assert.ThrowsException<SeqException>(() => new TrapezoidTimeDistanceFunction(new double[] { 1, 2, 3 },    new double[] { }));
            Assert.ThrowsException<SeqException>(() => new TrapezoidTimeDistanceFunction(new double[] { 1, 2, 3, 4 }, new double[] { 1, 2, 3 }));
            Assert.ThrowsException<SeqException>(() => new TrapezoidTimeDistanceFunction(new double[] { 1, 2, 3 },    new double[] { 1, 2, 3, 4 }));

           
            func = new TrapezoidTimeDistanceFunction(new double[] { 1, 2, 3 }, new double[] { 1, 2, 3 });
            Assert.AreEqual("TrapezoidTime", func.FunctionName);
            Assert.AreEqual(6, func.ComputeDistance(A, B));
            Assert.AreEqual(6, func.ComputeDistance(B, A));
            Assert.AreEqual(3.5, func.ComputeDistance(A, C));
            Assert.AreEqual(2.67, Math.Round(func.ComputeDistance(A, D),2));
            Assert.AreEqual(6, func.ComputeDistance(A, E));
        }

        [TestMethod()]
        public void TrapezoidTimeWithTimeBreakerDistanceFunction()
        {
            func = new TrapezoidTimeWithTimeBreakerDistanceFunction(new double[] { 1, 2, 3 }, new double[] { 1, 2, 3 });
            Assert.AreEqual(6.02,  func.ComputeDistance(A, B));
            Assert.AreEqual(6.02,  func.ComputeDistance(B, A));
            Assert.AreEqual(3.512, Math.Round(func.ComputeDistance(A, C), 3));
            Assert.AreEqual(2.68,  Math.Round(func.ComputeDistance(A, D), 2));
            Assert.AreEqual(6.04,  Math.Round(func.ComputeDistance(A, E), 2));
        }

        [TestMethod()]
        public void MatrixDistanceFunction()
        {
            Assert.ThrowsException<SeqException>(() => new MatrixDistanceFunction(null, new List<int>() { A.ID, B.ID }));
            Assert.ThrowsException<SeqException>(() => new MatrixDistanceFunction(new List<List<double>> { new List<double> { 1, 2 }, new List<double> { 3, 4 } }, null));
            Assert.ThrowsException<SeqException>(() => new MatrixDistanceFunction(new List<List<double>> { new List<double> { 1, 2 }, new List<double> { 3, 4 } }, new List<int>() { A.ID, B.ID, 5 }));
            Assert.ThrowsException<SeqException>(() => new MatrixDistanceFunction(new List<List<double>> { new List<double> { 1, 2 }, new List<double> { 1, 2, 4 }, new List<double> { 3, 4 } }, new List<int>() { A.ID, B.ID }));
            
            func = new MatrixDistanceFunction(new List<List<double>> { new List<double> { 1, 2 }, new List<double> { 3, 4 } }, new List<int>() { A.ID, B.ID });
            Assert.AreEqual("Matrix", func.FunctionName);
            Assert.AreEqual(2, func.ComputeDistance(A, B));
            Assert.AreEqual(3, func.ComputeDistance(B, A));
            Assert.AreEqual(1, func.ComputeDistance(A, A));
            Assert.AreEqual(4, func.ComputeDistance(B, B));
           
            Assert.ThrowsException<SeqException>(() => func.ComputeDistance(A, C));
            Assert.ThrowsException<SeqException>(() => func.ComputeDistance(D, A));
            Assert.ThrowsException<SeqException>(() => func.ComputeDistance(A, null));
            Assert.ThrowsException<SeqException>(() => func.ComputeDistance(new Config(99, new List<double>() {8, 7}), A));
            Assert.ThrowsException<SeqException>(() => func.ComputeDistance(null, A));


        }
    }
}