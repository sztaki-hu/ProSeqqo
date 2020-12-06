using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;
using SequencePlanner.GTSP;
using SequencePlanner.GTSPTask.Task.LineLike;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;

namespace SequencerTest.Integration.Sequencing
{
    [TestClass]
    public class LineLikeEqual
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
        public void Running()
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

            task.ValidateModel();
            var result = task.RunModel();
            Console.WriteLine(result);
        }
    }
}