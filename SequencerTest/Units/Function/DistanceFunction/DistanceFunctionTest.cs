using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;

namespace SequencerTest.Units.DistanceFunction
{
    [TestClass]
    public class DistanceFunctionTest
    {
        IDistanceFunction func;
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

        [TestMethod()]
        public void EuclidianDistanceFunction()
        {
            func = new EuclidianDistanceFunction();
            Assert.AreEqual(5, func.ComputeDistance(A, B));
            Assert.AreEqual(5, func.ComputeDistance(A, C));
            Assert.AreEqual(5, func.ComputeDistance(A, D));
            Assert.AreEqual(Math.Round(8.660254,2), Math.Round(func.ComputeDistance(A, E),2));
        }

        [TestMethod()]
        public void MaxDistanceFunction()
        {
            func = new MaxDistanceFunction();
            //Assert.AreEqual(2, func.ComputeDistance(A, B));
            //Assert.AreEqual(2, func.ComputeDistance(A, C));
            //Assert.AreEqual(2, func.ComputeDistance(A, D));
            //Assert.AreEqual(2, func.ComputeDistance(A, E));
        }

        [TestMethod()]
        public void ManhattanDistanceFunction()
        {
            func = new ManhattanDistanceFunction();
            //Assert.AreEqual(3, func.ComputeDistance(A, B));
            //Assert.AreEqual(3, func.ComputeDistance(A, C));
            //Assert.AreEqual(3, func.ComputeDistance(A, D));
            //Assert.AreEqual(3, func.ComputeDistance(A, E));
        }

        [TestMethod()]
        public void TrapezoidTimeDistanceFunction()
        {
            func = new TrapezoidTimeDistanceFunction(new double[] { 1, 2 }, new double[] { 1, 2 });
            Assert.AreEqual(0, func.ComputeDistance(A, B));
            Assert.AreEqual(0, func.ComputeDistance(A, C));
            Assert.AreEqual(0, func.ComputeDistance(A, D));
            Assert.AreEqual(0, func.ComputeDistance(A, E));
        }

        [TestMethod()]
        public void TrapezoidTimeWithTimeBreakerDistanceFunction()
        {
            func = new TrapezoidTimeWithTimeBreakerDistanceFunction(new double[] { 1, 2 }, new double[] { 1, 2 });
            Assert.AreEqual(0, func.ComputeDistance(A, B));
            Assert.AreEqual(0, func.ComputeDistance(A, C));
            Assert.AreEqual(0, func.ComputeDistance(A, D));
            Assert.AreEqual(0, func.ComputeDistance(A, E));
        }

        [TestMethod()]
        public void MatrixDistanceFunction()
        {
            Exception expectedExcetpion = null;
            func = new MatrixDistanceFunction(new List<List<double>> { new List<double>{ 1, 2 }, new List<double> { 2, 1 } }, new List<int>() { A.GlobalID, B.GlobalID });
            Assert.AreEqual(2, func.ComputeDistance(A, B));
            Assert.AreEqual(2, func.ComputeDistance(B, A));
            Assert.AreEqual(1, func.ComputeDistance(A, A));
            Assert.AreEqual(1, func.ComputeDistance(B, B));
            try
            {
                func.ComputeDistance(A, C);
            }
            catch (SequencerException ex)
            {
                expectedExcetpion = ex;
            }
            Assert.IsNotNull(expectedExcetpion);

            try
            {
                func.ComputeDistance(D, A);
            }
            catch (SequencerException ex)
            {
                expectedExcetpion = ex;
            }
            Assert.IsNotNull(expectedExcetpion);
        }
    }
}