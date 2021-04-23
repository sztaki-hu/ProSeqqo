using System;
using System.Collections.Generic;
using SequencePlanner.Helper;
using SequencePlanner.GeneralModels;
using SequencePlanner.Function.DistanceFunction;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SequencerTest.Units.DistanceFunction
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
            A = new Config(1,new List<double> { 0, 0, 0 } );
            B = new Config(1,new List<double> { 5, 0, 0 } );
            C = new Config(1,new List<double> { 0, 5, 0 } );
            D = new Config(1,new List<double> { 0, 0, 5 } );
            E = new Config(1,new List<double> { 5, 5, 5 } );
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
            func = new MatrixDistanceFunction(new List<List<double>> { new List<double>{ 1, 2 }, new List<double> { 2, 1 } }, new List<int>() { A.ID, B.ID });
            Assert.AreEqual(1, func.ComputeDistance(A, B));
            Assert.AreEqual(1, func.ComputeDistance(B, A));
            Assert.AreEqual(1, func.ComputeDistance(A, A));
            Assert.AreEqual(1, func.ComputeDistance(B, B));
            try
            {
                func.ComputeDistance(A, C);
            }
            catch (SeqException ex)
            {
                expectedExcetpion = ex;
            }
            //Assert.IsNotNull(expectedExcetpion);

            try
            {
                func.ComputeDistance(D, A);
            }
            catch (SeqException ex)
            {
                expectedExcetpion = ex;
            }
            //Assert.IsNotNull(expectedExcetpion);
        }
    }
}