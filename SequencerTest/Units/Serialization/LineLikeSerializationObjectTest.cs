using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.GTSP;
using SequencePlanner.Model;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;
using System.Collections.Generic;
using SequencePlanner.GTSPTask.Task.LineLike;
using SequencePlanner.GTSPTask.Serialization.Task;
using SequencePlanner.Function.DistanceFunction;

namespace SequencerTest.Units.Serialization
{
    [TestClass]
    public class LineLikeSerializationObjectTest
    {
        Position A;
        Position B;
        Position C;
        Position D;
        Line line;
        Line line2;
        Contour contour;
        Contour contour2;
        List<Position> positionList;
        PositionMatrix matrix;
        List<Line> lines;
        List<Contour> contours;
        List<GTSPPrecedenceConstraint> linePrecedences;
        List<GTSPPrecedenceConstraint> contourPrecedences;
        LineLikeTask LineLikeTask;
        LineLikeTaskSerializationObject SerObj;

        [TestInitialize()]
        public void Initialize()
        {
            A = new Position()
            {
                Name = "A",
                UserID = 1,
                ResourceID = 1,
                Virtual = false,
                Vector = new double[] { 1, 2, 3 }
            };

            B = new Position()
            {
                Name = "B",
                UserID = 2,
                ResourceID = 1,
                Virtual = false,
                Vector = new double[] { 1, 2, 3 }
            };

            C = new Position()
            {
                Name = "C",
                UserID = 3,
                ResourceID = 1,
                Virtual = false,
                Vector = new double[] { 1, 2, 3 }
            };

            D = new Position()
            {
                Name = "D",
                UserID = 4,
                ResourceID = 1,
                Virtual = false,
                Vector = new double[] { 1, 2, 3 }
            };

            contour = new Contour();
            contour2 = new Contour();

            line = new Line()
            {
                NodeA = A,
                NodeB = B
            };

            line2 = new Line()
            {
                NodeA = C,
                NodeB = D
            };

            contour.Lines.Add(line);
            contour2.Lines.Add(line2);

            positionList = new List<Position>() { A, B, C, D };
            matrix = new PositionMatrix()
            {
                Positions = positionList,
                Matrix = new double[,] { { 1, 2, 3, 4 }, { 2, 1, 3, 4 }, { 2, 1, 3, 4 }, { 2, 1, 3, 4 } },
                DistanceFunction = new EuclidianDistanceFunction(),
                ResourceFunction = new ConstantResourceFunction(1, new AddResourceDistanceLinkFunction())
            };
            lines = new List<Line>() { line, line2 };
            contours = new List<Contour>() { contour, contour2 };
            linePrecedences = new List<GTSPPrecedenceConstraint>() { new GTSPPrecedenceConstraint(line, line2) };
            contourPrecedences = new List<GTSPPrecedenceConstraint>() { new GTSPPrecedenceConstraint(contour, contour2) };

            LineLikeTask = new LineLikeTask()
            {
                Dimension = 3,
                TimeLimit = 5,
                CyclicSequence = true,
                StartDepot = A,
                FinishDepot = B,
                WeightMultipier = 10,
                ContourPenalty = 1,
                LinePrecedences = linePrecedences,
                ContourPrecedences = contourPrecedences,
                Contours = contours,
                Lines = lines,
                PositionMatrix = matrix,
            };
            SerObj = new LineLikeTaskSerializationObject(LineLikeTask);

        }

        [TestMethod]
        public void BaseObjectTest()
        {
            Assert.AreEqual(LineLikeTask.Dimension, SerObj.Dimension);
            Assert.AreEqual("Line_Like", SerObj.TaskType);
            Assert.AreEqual(LineLikeTask.StartDepot.UserID, SerObj.StartDepot);
            Assert.AreEqual(LineLikeTask.FinishDepot.UserID, SerObj.FinishDepot);
            Assert.AreEqual(LineLikeTask.CyclicSequence, SerObj.CyclicSequence);
            Assert.AreEqual(LineLikeTask.PositionMatrix.DistanceFunction.FunctionName, SerObj.DistanceFunction.Function);
            Assert.AreEqual(null, SerObj.DistanceFunction.TrapezoidSpeed);
            Assert.AreEqual(null, SerObj.DistanceFunction.TrapezoidAcceleration);
            Assert.AreEqual(null, SerObj.DistanceFunction.DistanceMatrix);
            Assert.AreEqual(LineLikeTask.PositionMatrix.ResourceFunction.FunctionName, SerObj.ResourceFunction.ResourceSource);
            Assert.AreEqual(1, SerObj.ResourceFunction.ResourceCostConstant);
            Assert.AreNotEqual(null, SerObj.ResourceFunction.ResourceCostMatrix2);
            Assert.AreEqual("Add", SerObj.ResourceFunction.ResourceDistanceFunction);

            foreach (var serPos in SerObj.PositionList)
            {
                var found = false;
                foreach (var pos in LineLikeTask.PositionMatrix.Positions)
                {
                    if (serPos.ID == pos.UserID)
                    {
                        found = true;
                        Assert.AreEqual(pos.UserID, serPos.ID);
                        Assert.AreEqual(pos.Name, serPos.Name);
                        Assert.AreEqual(pos.ResourceID, serPos.ResourceID);
                        for (int i = 0; i < serPos.Position.Length; i++)
                        {
                            Assert.AreEqual(pos.Vector[i], serPos.Position[i]);
                        }

                    }
                }

                if (!found)
                    Assert.Fail();
            }
            }

        [TestMethod]
        public void LineLikeObjectTest()
        {
            Assert.AreEqual(Line.BIDIRECTIONAL_DEFAULT, SerObj.BidirectionLineDefault);
            Assert.AreEqual(LineLikeTask.ContourPenalty, SerObj.ContourPenalty);
            bool found;
            foreach (var serLine in SerObj.LineList)
            {
                found = false;
                foreach (var contour in LineLikeTask.Contours)
                {
                    foreach (var line in contour.Lines)
                    {
                        if(line.UserID == serLine.LineID)
                        {
                            found = true;
                            Assert.AreEqual(line.UserID, serLine.LineID);
                            Assert.AreEqual(contour.UserID, serLine.ContourID);
                            Assert.AreEqual(line.Name, serLine.Name);
                            Assert.AreEqual(line.NodeA.UserID, serLine.PositionIDA);
                            Assert.AreEqual(line.NodeB.UserID, serLine.PositionIDB);
                            Assert.AreEqual(line.ResourceID, serLine.ResourceID);
                        }
                    }
                }
                if (!found)
                    Assert.Fail();
            }

            foreach (var serPrec in SerObj.LinePrecedences)
            {
                found = false;
                foreach (var prec in LineLikeTask.LinePrecedences)
                {
                    if (prec.Before.UserID == serPrec.BeforeID && prec.After.UserID == serPrec.AfterID)
                    {
                        found = true;
                        break;
                    }
                    if (!found)
                        Assert.Fail();
                }
            }

            foreach (var serPrec in SerObj.ContourPrecedences)
            {
                found = false;
                foreach (var prec in LineLikeTask.ContourPrecedences)
                {
                    if (prec.Before.UserID == serPrec.BeforeID && prec.After.UserID == serPrec.AfterID)
                    {
                        found = true;
                        break;
                    }
                    if (!found)
                        Assert.Fail();
                }
            }
        }
    }
}