using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.GTSP;
using SequencePlanner.Model;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;
using System.Collections.Generic;
using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.GTSPTask.Task.LineTask;
using SequencePlanner.GTSPTask.Serialization.Task;

namespace SequencerTest.Units.Serialization
{
    [TestClass]
    public class LineSEQSerializationTest
    {
        GTSPNode A;
        GTSPNode B;
        GTSPNode C;
        GTSPNode D;
            Line line;
            Line line2;
            Contour contour;
            Contour contour2;
            List<GTSPNode> positionList;
            PositionMatrix matrix;
            List<Line> lines;
            List<Contour> contours;
            List<GTSPPrecedenceConstraint> linePrecedences;
            List<GTSPPrecedenceConstraint> contourPrecedences;

            [TestInitialize()]
            public void Initialize()
            {
                A = new GTSPNode(new Position()
                {
                    Name = "A",
                    UserID = 1,
                    ResourceID = 1,
                    Virtual = false,
                    Vector = new double[] { 1, 2, 3 }
                });

                B = new GTSPNode(new Position()
                {
                    Name = "B",
                    UserID = 2,
                    ResourceID = 1,
                    Virtual = false,
                    Vector = new double[] { 1, 2, 3 }
                });

                C = new GTSPNode(new Position()
                {
                    Name = "C",
                    UserID = 3,
                    ResourceID = 1,
                    Virtual = false,
                    Vector = new double[] { 1, 2, 3 }
                });

                D = new GTSPNode(new Position()
                {
                    Name = "D",
                    UserID = 4,
                    ResourceID = 1,
                    Virtual = false,
                    Vector = new double[] { 1, 2, 3 }
                });

                contour = new Contour();
                contour2 = new Contour();

                line = new Line()
                {
                    NodeA = A.In,
                    NodeB = B.Out
                };

                line2 = new Line()
                {
                    NodeA = C.In,
                    NodeB = D.Out
                };

                contour.Lines.Add(line);
                contour2.Lines.Add(line2);

                positionList = new List<GTSPNode>() { A, B, C, D };
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
                LineTask task = new LineTask()
                {
                    Dimension = 3,
                    TimeLimit = 5,
                    CyclicSequence = true,
                    StartDepot = A.In,
                    FinishDepot = B.Out,
                    WeightMultipier = 10,
                    ContourPenalty = 1,
                    LinePrecedences = linePrecedences,
                    ContourPrecedences = contourPrecedences,
                    Contours = contours,
                    Lines = lines,
                    PositionMatrix = matrix
                };

            LineTaskSerializer ser = new LineTaskSerializer();
            ser.ExportSEQ(task,"LLTest.SEQ");
            ser = new LineTaskSerializer();
            LineTask import = ser.ImportSEQ("LLTest.SEQ");

                Assert.AreEqual(3, import.Dimension);
                Assert.AreEqual(5, import.TimeLimit);
                Assert.IsTrue(import.CyclicSequence);
                Assert.AreEqual(A.Node.UserID, import.StartDepot.UserID);
                Assert.AreEqual(B.Node.UserID, import.FinishDepot.UserID);
                Assert.AreEqual(1000, import.WeightMultipier);
                Assert.AreEqual(1, import.ContourPenalty);
                Assert.AreEqual(linePrecedences.Count, import.LinePrecedences.Count);
                Assert.AreEqual(contourPrecedences.Count, import.ContourPrecedences.Count);
                Assert.AreEqual(contours.Count, import.Contours.Count);
                Assert.AreEqual(lines.Count, import.Lines.Count);
                
            }
        }
}
