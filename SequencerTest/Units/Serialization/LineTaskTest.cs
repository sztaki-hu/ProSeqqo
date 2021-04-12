using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Model;
using SequencePlanner.Model;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;
using SequencerTest.Helper;
using System.Collections.Generic;
using SequencePlanner.GTSPTask.Task.LineTask;
using SequencePlanner.GTSPTask.Serialization.Task;
using SequencePlanner.GTSPTask.Task.LineTask;

namespace SequencerTest.Units
{
    [TestClass]
    public class LineTaskTest
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
                Vector = new double[] {1, 2, 3 }
            });

            B = new GTSPNode(new Position()
            {
                Name = "B",
                UserID = 2,
                ResourceID = 1,
                Virtual = false,
                Vector = new double[] { 1, 2, 3 }
            });

            C = new GTSPNode( new Position()
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
                NodeB = B.In
            };

            line2 = new Line()
            {
                NodeA = C.In,
                NodeB = D.In
            };

            contour.Lines.Add(line);
            contour2.Lines.Add(line2);

            positionList = new List<GTSPNode> () { A, B, C, D };
            matrix = new PositionMatrix()
            {
                Positions = positionList,
                Matrix = new double[,] { {1,2,3,4}, {2,1,3,4}, { 2, 1, 3, 4 }, { 2, 1, 3, 4 } },
                DistanceFunction = new EuclidianDistanceFunction(),
                ResourceFunction = new ConstantResourceFunction(1, new AddResourceDistanceLinkFunction())
            };
            lines = new List<Line>() { line, line2 };
            contours = new List<Contour>() { contour, contour2 };
            linePrecedences = new List<GTSPPrecedenceConstraint>() { new GTSPPrecedenceConstraint(line,line2) };
            contourPrecedences = new List<GTSPPrecedenceConstraint>() { new GTSPPrecedenceConstraint(contour, contour2)};
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
                FinishDepot = B.In,
                WeightMultipier = 10,
                ContourPenalty = 1,
                LinePrecedences = linePrecedences,
                ContourPrecedences = contourPrecedences,
                Contours = contours,
                Lines = lines,
                PositionMatrix = matrix
            };

            Assert.AreEqual(3, task.Dimension);
            Assert.AreEqual(5, task.TimeLimit);
            Assert.IsTrue(task.CyclicSequence);
            Assert.AreSame(A.In, task.StartDepot);
            Assert.AreSame(B.In, task.FinishDepot);
            Assert.AreEqual(10, task.WeightMultipier);
            Assert.AreEqual(1, task.ContourPenalty);
            Assert.AreSame(linePrecedences, task.LinePrecedences);
            Assert.AreSame(contourPrecedences, task.ContourPrecedences);
            Assert.AreSame(contours, task.Contours);
            Assert.AreSame(lines, task.Lines);
            Assert.AreSame(matrix, task.PositionMatrix);
        }

        [TestMethod]
        public void Example()
        {
            //LineTask task = new LineTask()
            //{
            //    Dimension = 3,
            //    TimeLimit = 200,
            //    CyclicSequence = true,
            //    StartDepot = A,
            //    //FinishDepot = finish,
            //    WeightMultipier = 10,
            //    ContourPenalty = 1,
            //    LinePrecedences = linePrecedences,
            //    ContourPrecedences = contourPrecedences,
            //    Contours = contours,
            //    Lines = lines,
            //    PositionMatrix = matrix
            //};
            //task.ValidateModel();
            //task.GenerateModel();
            //task.RunModel();
        }

        [TestMethod]
        public void ImportExportJSONTest()
        {
            LineTask task = new LineTask()
            {
                Dimension = 3,
                TimeLimit = 5000,
                CyclicSequence = true,
                StartDepot = A.In,
                //FinishDepot = finish,
                WeightMultipier = 10,
                ContourPenalty = 1,
                LinePrecedences = linePrecedences,
                ContourPrecedences = contourPrecedences,
                Contours = contours,
                Lines = lines,
                PositionMatrix = matrix
            };
            var serializer = new LineTaskSerializer();
            serializer.ExportJSON(task, "exportLL.json");
            serializer = new LineTaskSerializer();
            task = serializer.ImportJSON("exportLL.json");

            Assert.AreEqual(3, task.Dimension);
            Assert.AreEqual(5000, task.TimeLimit);
            Assert.IsTrue(task.CyclicSequence);
            AssertSeq.AssertPosition(A.In, task.StartDepot);
            AssertSeq.AssertPosition(null, task.FinishDepot);
            //Assert.AreEqual(10, task.WeightMultipier);
            Assert.AreEqual(1, task.ContourPenalty);
            Assert.AreEqual(linePrecedences.Count, task.LinePrecedences.Count);
            Assert.AreEqual(contourPrecedences.Count, task.ContourPrecedences.Count);
            Assert.AreEqual(contours.Count, task.Contours.Count);
            Assert.AreEqual(lines.Count, task.Lines.Count);
            //Assert.AreSame(matrix, task.PositionMatrix);

            serializer = new LineTaskSerializer();
            serializer.ExportJSON(task, "exportLLDual.json");
        }

        [TestMethod]
        public void ImportExportXMLTest()
        {
            LineTask task = new LineTask()
            {
                Dimension = 3,
                TimeLimit = 5000,
                CyclicSequence = true,
                StartDepot = A.In,
                //FinishDepot = finish,
                WeightMultipier = 10,
                ContourPenalty = 1,
                LinePrecedences = linePrecedences,
                ContourPrecedences = contourPrecedences,
                Contours = contours,
                Lines = lines,
                PositionMatrix = matrix
            };

            var serializer = new LineTaskSerializer();
            serializer.ExportXML(task, "exportLL.xml");
            serializer = new LineTaskSerializer();
            task = serializer.ImportXML("exportLL.xml");

            Assert.AreEqual(3, task.Dimension);
            Assert.AreEqual(5000, task.TimeLimit);
            Assert.IsTrue(task.CyclicSequence);
            AssertSeq.AssertPosition(A.In, task.StartDepot);
            AssertSeq.AssertPosition(null, task.FinishDepot);
            //Assert.AreEqual(10, task.WeightMultipier);
            Assert.AreEqual(1, task.ContourPenalty);
            Assert.AreEqual(linePrecedences.Count, task.LinePrecedences.Count);
            Assert.AreEqual(contourPrecedences.Count, task.ContourPrecedences.Count);
            Assert.AreEqual(contours.Count, task.Contours.Count);
            Assert.AreEqual(lines.Count, task.Lines.Count);
            //Assert.AreSame(matrix, task.PositionMatrix);

            serializer = new LineTaskSerializer();
            serializer.ExportXML(task, "exportLLDual.xml");

            
        }

        [TestMethod]
        public void ImportExportSEQTest()
        {
            LineTask task = new LineTask()
            {
                Dimension = 3,
                TimeLimit = 5000,
                CyclicSequence = true,
                StartDepot = A.In,
                //FinishDepot = finish,
                WeightMultipier = 10,
                ContourPenalty = 1,
                LinePrecedences = linePrecedences,
                ContourPrecedences = contourPrecedences,
                Contours = contours,
                Lines = lines,
                PositionMatrix = matrix
            };

            var serializer = new LineTaskSerializer();
            serializer.ExportSEQ(task, "exportLL.seq");
            serializer = new LineTaskSerializer();
            task = serializer.ImportSEQ("exportLL.seq");
            serializer = new LineTaskSerializer();
            serializer.ExportSEQ(task, "exportLLDual.seq");

            Assert.AreEqual(3, task.Dimension);
            Assert.AreEqual(5000, task.TimeLimit);
            Assert.IsTrue(task.CyclicSequence);
            AssertSeq.AssertPosition(A.In, task.StartDepot);
            AssertSeq.AssertPosition(null, task.FinishDepot);
            //Assert.AreEqual(10, task.WeightMultipier);
            Assert.AreEqual(1, task.ContourPenalty);
            Assert.AreEqual(linePrecedences.Count, task.LinePrecedences.Count);
            Assert.AreEqual(contourPrecedences.Count, task.ContourPrecedences.Count);
            Assert.AreEqual(contours.Count, task.Contours.Count);
            Assert.AreEqual(lines.Count, task.Lines.Count);
            //Assert.AreSame(matrix, task.PositionMatrix);
        }
    }
}