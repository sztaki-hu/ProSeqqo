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
    public class MatrixDistanceFunctionTests
    {
        MatrixDistanceFunction func;
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
            func = new MatrixDistanceFunction(new List<List<double>> { new List<double> { 1, 2 }, new List<double> { 3, 4 } }, new List<int>() { A.ID, B.ID });
        }

        [TestMethod()]
        public void MatrixDistanceFunctionTest()
        {
            Assert.ThrowsException<SeqException>(() => new MatrixDistanceFunction(null, new List<int>() { A.ID, B.ID }));
            Assert.ThrowsException<SeqException>(() => new MatrixDistanceFunction(new List<List<double>> { new List<double> { 1, 2 }, new List<double> { 3, 4 } }, null));
            Assert.ThrowsException<SeqException>(() => new MatrixDistanceFunction(new List<List<double>> { new List<double> { 1, 2 }, new List<double> { 3, 4 } }, new List<int>() { A.ID, B.ID, 5 }));
            Assert.ThrowsException<SeqException>(() => new MatrixDistanceFunction(new List<List<double>> { new List<double> { 1, 2 }, new List<double> { 1, 2, 4 }, new List<double> { 3, 4 } }, new List<int>() { A.ID, B.ID }));

        }

        [TestMethod()]
        public void ComputeDistanceTest()
        {

            Assert.AreEqual("Matrix", func.FunctionName);
            Assert.AreEqual(2, func.ComputeDistance(A, B));
            Assert.AreEqual(3, func.ComputeDistance(B, A));
            Assert.AreEqual(1, func.ComputeDistance(A, A));
            Assert.AreEqual(4, func.ComputeDistance(B, B));

            Assert.ThrowsException<SeqException>(() => func.ComputeDistance(A, C));
            Assert.ThrowsException<SeqException>(() => func.ComputeDistance(D, A));
            Assert.ThrowsException<SeqException>(() => func.ComputeDistance(A, null));
            Assert.ThrowsException<SeqException>(() => func.ComputeDistance(new Config(99, new List<double>() { 8, 7 }), A));
            Assert.ThrowsException<SeqException>(() => func.ComputeDistance(null, A));
        }

        [TestMethod()]
        public void ValidateTest()
        {

        }
    }
}