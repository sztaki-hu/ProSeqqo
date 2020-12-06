using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.GTSP;
using SequencePlanner.Model;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;
using System.Collections.Generic;
using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.GTSPTask.Task.LineLike;
using SequencePlanner.GTSPTask.Serialization.Task;

namespace SequencerTest.Units.Serialization
{
    [TestClass]
    public class LineLikeSEQSerializationTest
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
            }

            [TestMethod]
            public void GetterSetter()
            {
                LineLikeTask task = new LineLikeTask()
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
                    PositionMatrix = matrix
                };

            LineLikeTaskSerializer ser = new LineLikeTaskSerializer();
            ser.ExportSEQ(task,"LLTest.SEQ");
            ser = new LineLikeTaskSerializer();
            LineLikeTask import = ser.ImportSEQ("LLTest.SEQ");

                Assert.AreEqual(3, import.Dimension);
                Assert.AreEqual(5, import.TimeLimit);
                Assert.IsTrue(import.CyclicSequence);
                Assert.AreEqual(A.UserID, import.StartDepot.UserID);
                Assert.AreEqual(B.UserID, import.FinishDepot.UserID);
                Assert.AreEqual(0, import.WeightMultipier);
                Assert.AreEqual(1, import.ContourPenalty);
                Assert.AreEqual(linePrecedences.Count, import.LinePrecedences.Count);
                Assert.AreEqual(contourPrecedences.Count, import.ContourPrecedences.Count);
                Assert.AreEqual(contours.Count, import.Contours.Count);
                Assert.AreEqual(lines.Count, import.Lines.Count);
                
            }
        }
}
