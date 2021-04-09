using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;
using System;
using System.Collections.Generic;

namespace SequencerTest.Units.Model
{
    [TestClass]
    public class PositionMatrixTest
    {
        GTSPNode start;
        GTSPNode finish;
        List<GTSPNode> positionList;
        double[,] matrix;
        IDistanceFunction distFunc;
        IResourceFunction resourceFunc;
        
        [TestInitialize()]
        public void Initialize()
        {
            start = new GTSPNode(new Position(){ Vector = new double[]{1,1,1} });
            finish = new GTSPNode(new Position() { Vector = new double[] { 3, 3, 3 } });
            matrix = new double[,] { { 1, 2 }, { 2, 1 } };
            positionList = new List<GTSPNode>() { start, finish };
            resourceFunc = new ConstantResourceFunction(1, new AddResourceDistanceLinkFunction());
            distFunc = new EuclidianDistanceFunction();
        }

        [TestMethod]
        public void ConstructorTest()
        {
            PositionMatrix posMatrix = new PositionMatrix(positionList, distFunc, resourceFunc);
            Assert.AreSame(positionList, posMatrix.Positions);
            Assert.AreEqual(2, posMatrix.Matrix.GetLength(0));
            Assert.AreEqual(2, posMatrix.Matrix.GetLength(1));
            //Assert.AreEqual(1, posMatrix.Matrix[0,0]);
            //Assert.AreEqual(Math.Round(4.46410,2), Math.Round(posMatrix.Matrix[0,1],2));
            //Assert.AreEqual(Math.Round(4.46410, 2), Math.Round(posMatrix.Matrix[1,0],2));
            //Assert.AreEqual(1, posMatrix.Matrix[1,1]);
            Assert.AreSame(distFunc, posMatrix.DistanceFunction);
            Assert.AreSame(resourceFunc, posMatrix.ResourceFunction);
        }

        [TestMethod]
        public void ConstructorExceptionTest()
        {
            Exception lastCatchedException =null;
            //A
            try
            {
                PositionMatrix posMatrix = new PositionMatrix(null, distFunc, resourceFunc);
                Assert.Fail();
            }catch(SeqException e)
            {
                lastCatchedException = e;
            }
            Assert.IsInstanceOfType(lastCatchedException, typeof(SeqException));
            lastCatchedException = null;
            
            //B
            try
            {
                PositionMatrix posMatrix = new PositionMatrix(positionList, null, resourceFunc);
                Assert.Fail();
            }
            catch (SeqException e)
            {
                lastCatchedException = e;
            }
            Assert.IsInstanceOfType(lastCatchedException, typeof(SeqException));
            lastCatchedException = null;
            
            //C
            try
            {
                PositionMatrix posMatrix = new PositionMatrix(positionList, distFunc, null);
                Assert.Fail();
            }
            catch (SeqException e)
            {
                lastCatchedException = e;
            }
            Assert.IsInstanceOfType(lastCatchedException, typeof(SeqException));
        }


        [TestMethod]
        public void GetterSetterTest()
        {
            PositionMatrix posMatrix = new PositionMatrix()
            {
                Positions = positionList,
                Matrix = matrix,
                DistanceFunction = distFunc,
                ResourceFunction = resourceFunc
            };
            Assert.AreSame(positionList,  posMatrix.Positions);
            Assert.AreSame(matrix, posMatrix.Matrix);
            Assert.AreSame(distFunc, posMatrix.DistanceFunction);
            Assert.AreSame(resourceFunc, posMatrix.ResourceFunction);
        }
    }
}