﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.GTSP;
using SequencePlanner.Model;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;
using SequencerTest.Helper;
using System.Collections.Generic;
using SequencePlanner.GTSPTask.Task.LineLike;
using SequencePlanner.GTSPTask.Serialization.Task;

namespace SequencerTest.Units
{
    [TestClass]
    public class LineLikeTaskTest
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
                Vector = new double[] {1, 2, 3 }
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

            Assert.AreEqual(3, task.Dimension);
            Assert.AreEqual(5, task.TimeLimit);
            Assert.IsTrue(task.CyclicSequence);
            Assert.AreSame(A, task.StartDepot);
            Assert.AreSame(B, task.FinishDepot);
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
            //LineLikeTask task = new LineLikeTask()
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
            LineLikeTask task = new LineLikeTask()
            {
                Dimension = 3,
                TimeLimit = 5000,
                CyclicSequence = true,
                StartDepot = A,
                //FinishDepot = finish,
                WeightMultipier = 10,
                ContourPenalty = 1,
                LinePrecedences = linePrecedences,
                ContourPrecedences = contourPrecedences,
                Contours = contours,
                Lines = lines,
                PositionMatrix = matrix
            };
            var serializer = new LineLikeTaskSerializer();
            serializer.ExportJSON(task, "exportLL.json");
            serializer = new LineLikeTaskSerializer();
            task = serializer.ImportJSON("exportLL.json");

            Assert.AreEqual(3, task.Dimension);
            Assert.AreEqual(5000, task.TimeLimit);
            Assert.IsTrue(task.CyclicSequence);
            AssertSeq.AssertPosition(A, task.StartDepot);
            AssertSeq.AssertPosition(null, task.FinishDepot);
            //Assert.AreEqual(10, task.WeightMultipier);
            Assert.AreEqual(1, task.ContourPenalty);
            Assert.AreEqual(linePrecedences.Count, task.LinePrecedences.Count);
            Assert.AreEqual(contourPrecedences.Count, task.ContourPrecedences.Count);
            Assert.AreEqual(contours.Count, task.Contours.Count);
            Assert.AreEqual(lines.Count, task.Lines.Count);
            //Assert.AreSame(matrix, task.PositionMatrix);

            serializer = new LineLikeTaskSerializer();
            serializer.ExportJSON(task, "exportLLDual.json");
        }

        [TestMethod]
        public void ImportExportXMLTest()
        {
            LineLikeTask task = new LineLikeTask()
            {
                Dimension = 3,
                TimeLimit = 5000,
                CyclicSequence = true,
                StartDepot = A,
                //FinishDepot = finish,
                WeightMultipier = 10,
                ContourPenalty = 1,
                LinePrecedences = linePrecedences,
                ContourPrecedences = contourPrecedences,
                Contours = contours,
                Lines = lines,
                PositionMatrix = matrix
            };

            var serializer = new LineLikeTaskSerializer();
            serializer.ExportXML(task, "exportLL.xml");
            serializer = new LineLikeTaskSerializer();
            task = serializer.ImportXML("exportLL.xml");

            Assert.AreEqual(3, task.Dimension);
            Assert.AreEqual(5000, task.TimeLimit);
            Assert.IsTrue(task.CyclicSequence);
            AssertSeq.AssertPosition(A, task.StartDepot);
            AssertSeq.AssertPosition(null, task.FinishDepot);
            //Assert.AreEqual(10, task.WeightMultipier);
            Assert.AreEqual(1, task.ContourPenalty);
            Assert.AreEqual(linePrecedences.Count, task.LinePrecedences.Count);
            Assert.AreEqual(contourPrecedences.Count, task.ContourPrecedences.Count);
            Assert.AreEqual(contours.Count, task.Contours.Count);
            Assert.AreEqual(lines.Count, task.Lines.Count);
            //Assert.AreSame(matrix, task.PositionMatrix);

            serializer = new LineLikeTaskSerializer();
            serializer.ExportXML(task, "exportLLDual.xml");
        }

        [TestMethod]
        public void ImportExportSEQTest()
        {
            LineLikeTask task = new LineLikeTask()
            {
                Dimension = 3,
                TimeLimit = 5000,
                CyclicSequence = true,
                StartDepot = A,
                //FinishDepot = finish,
                WeightMultipier = 10,
                ContourPenalty = 1,
                LinePrecedences = linePrecedences,
                ContourPrecedences = contourPrecedences,
                Contours = contours,
                Lines = lines,
                PositionMatrix = matrix
            };

            var serializer = new LineLikeTaskSerializer();
            serializer.ExportSEQ(task, "exportLL.seq");
            serializer = new LineLikeTaskSerializer();
            task = serializer.ImportSEQ("exportLL.seq");
            serializer = new LineLikeTaskSerializer();
            serializer.ExportSEQ(task, "exportLLDual.seq");

            Assert.AreEqual(3, task.Dimension);
            Assert.AreEqual(5000, task.TimeLimit);
            Assert.IsTrue(task.CyclicSequence);
            AssertSeq.AssertPosition(A, task.StartDepot);
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