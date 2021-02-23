using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.GTSP;
using SequencePlanner.Model;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;
using System.Collections.Generic;
using SequencePlanner.GTSPTask.Task.PointLike;
using SequencePlanner.GTSPTask.Serialization.Task;

namespace SequencerTest.Units
{
    [TestClass]
    public class PointLikeTaskTest
    {
        GTSPNode A;
        GTSPNode B;
        GTSPNode C;
        GTSPNode D;
        Task T1;
        Task T2;
        Task T3;
        Alternative A1;
        Alternative A2;
        Alternative A3;
        Process P1;
        Process P2;
        List<GTSPNode> positionList;
        PositionMatrix matrix;
        List<Task> tasks;
        List<Alternative> alternatives;
        List<Process> processes;
        List<GTSPPrecedenceConstraint> positionPrecedences;
        List<GTSPPrecedenceConstraint> processPrecedences;

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

            T1 = new Task() { Positions = new List<GTSPNode>() { A } };
            T2 = new Task() { Positions = new List<GTSPNode>() { B } };
            T3 = new Task() { Positions = new List<GTSPNode>() { C, D } };
            A1 = new Alternative() { Tasks = new List<Task>() { T1 } };
            A2 = new Alternative() { Tasks = new List<Task>() { T2 } };
            A3 = new Alternative() { Tasks = new List<Task>() { T3 } };
            P1 = new Process() { Alternatives = new List<Alternative>() { A1 } };
            P2 = new Process() { Alternatives = new List<Alternative>() { A2,A3 } };
            

            positionList = new List<GTSPNode>() { A, B, C, D };
            matrix = new PositionMatrix()
            {
                Positions = positionList,
                Matrix = new double[,] { { 1, 2, 3, 4 }, { 1,2,3,4 }, { 1, 2, 3, 4 }, { 1, 2, 3, 4 } },
                DistanceFunction = new EuclidianDistanceFunction(),
                ResourceFunction = new ConstantResourceFunction(1, new AddResourceDistanceLinkFunction())
            };

            tasks = new List<Task>() { T1, T2, T3 };
            alternatives = new List<Alternative>() {A1,A2, A3};
            processes = new List<Process>() {P1, P2 };
            positionPrecedences = new List<GTSPPrecedenceConstraint>() { new GTSPPrecedenceConstraint() { Before = A.In, After = B.In} };
            processPrecedences = new List<GTSPPrecedenceConstraint>() { new GTSPPrecedenceConstraint() { Before = P1, After = P2} };
        }

        [TestMethod]
        public void GetterSetter()
        {
            PointLikeTask task = new PointLikeTask()
            {
                Dimension = 3,
                TimeLimit =5,
                CyclicSequence = false,
                StartDepot = A.In,
                FinishDepot = B.Out,
                WeightMultipier = 10,
                PositionMatrix = matrix,
                Tasks = tasks,
                Alternatives = alternatives,
                Processes = processes, 
                PositionPrecedence = positionPrecedences,
                ProcessPrecedence = processPrecedences,
            };

            Assert.AreEqual(3, task.Dimension);
            Assert.AreEqual(5, task.TimeLimit);
            Assert.IsFalse(task.CyclicSequence);
            Assert.AreSame(A.In, task.StartDepot);
            Assert.AreSame(B.In, task.FinishDepot);
            Assert.AreEqual(10, task.WeightMultipier);
            Assert.AreSame(matrix, task.PositionMatrix);
            Assert.AreSame(tasks, task.Tasks);
            Assert.AreSame(alternatives, task.Alternatives);
            Assert.AreSame(processes, task.Processes);
            Assert.AreSame(positionPrecedences, task.PositionPrecedence);
            Assert.AreSame(processPrecedences, task.ProcessPrecedence);
        }

        [TestMethod]
        private void ImportExportJSONTest()
        {
            PointLikeTask task = new PointLikeTask()
            {
                Dimension = 3,
                TimeLimit = 5,
                CyclicSequence = true,
                StartDepot = A.Out,
                FinishDepot = B.Out,
                WeightMultipier = 10,
                PositionMatrix = matrix,
                Tasks = tasks,
                Alternatives = alternatives,
                Processes = processes,
                PositionPrecedence = positionPrecedences,
                ProcessPrecedence = processPrecedences,
            };

            var serializer = new PointLikeTaskSerializer();
            serializer.ExportJSON(task, "Resources/Example/Export/exportPL.json");
            serializer = new PointLikeTaskSerializer();
            serializer.ImportJSON("Resources/Example/Export/exportPL.json");

            Assert.AreEqual(3, task.Dimension);
            Assert.AreEqual(5, task.TimeLimit);
            Assert.IsTrue(task.CyclicSequence);
            Assert.AreSame(A, task.StartDepot);
            Assert.AreSame(B, task.FinishDepot);
            Assert.AreEqual(10, task.WeightMultipier);
            Assert.AreSame(matrix, task.PositionMatrix);
            Assert.AreSame(tasks, task.Tasks);
            Assert.AreSame(alternatives, task.Alternatives);
            Assert.AreSame(processes, task.Processes);
            Assert.AreSame(positionPrecedences, task.PositionPrecedence);
            Assert.AreSame(processPrecedences, task.ProcessPrecedence);
        }

        [TestMethod]
        private void ImportExportXMLTest()
        {
            PointLikeTask task = new PointLikeTask()
            {
                Dimension = 3,
                TimeLimit = 5,
                CyclicSequence = true,
                StartDepot = A.Out,
                FinishDepot = B.Out,
                WeightMultipier = 10,
                PositionMatrix = matrix,
                Tasks = tasks,
                Alternatives = alternatives,
                Processes = processes,
                PositionPrecedence = positionPrecedences,
                ProcessPrecedence = processPrecedences,
            };

            var serializer = new PointLikeTaskSerializer();
            serializer.ExportXML(task, "Resources/Example/Export/exportPL.xml");
            serializer = new PointLikeTaskSerializer();
            serializer.ImportXML("Resources/Example/Export/exportPL.xml");

            Assert.AreEqual(3, task.Dimension);
            Assert.AreEqual(5, task.TimeLimit);
            Assert.IsTrue(task.CyclicSequence);
            Assert.AreSame(A, task.StartDepot);
            Assert.AreSame(B, task.FinishDepot);
            Assert.AreEqual(10, task.WeightMultipier);
            Assert.AreSame(matrix, task.PositionMatrix);
            Assert.AreSame(tasks, task.Tasks);
            Assert.AreSame(alternatives, task.Alternatives);
            Assert.AreSame(processes, task.Processes);
            Assert.AreSame(positionPrecedences, task.PositionPrecedence);
            Assert.AreSame(processPrecedences, task.ProcessPrecedence);
        }

        [TestMethod]
        private void ImportExportSEQTest()
        {
            PointLikeTask task = new PointLikeTask()
            {
                Dimension = 3,
                TimeLimit = 5,
                CyclicSequence = true,
                StartDepot = A.In,
                FinishDepot = B.Out,
                WeightMultipier = 10,
                PositionMatrix = matrix,
                Tasks = tasks,
                Alternatives = alternatives,
                Processes = processes,
                PositionPrecedence = positionPrecedences,
                ProcessPrecedence = processPrecedences,
            };

            var serializer = new PointLikeTaskSerializer();
            serializer.ExportSEQ(task, "Resources/Example/Export/exportPL.seq");
            serializer = new PointLikeTaskSerializer();
            serializer.ImportSEQ("Resources/Example/Export/exportPL.seq");

            Assert.AreEqual(3, task.Dimension);
            Assert.AreEqual(5, task.TimeLimit);
            Assert.IsTrue(task.CyclicSequence);
            Assert.AreSame(A, task.StartDepot);
            Assert.AreSame(B, task.FinishDepot);
            Assert.AreEqual(10, task.WeightMultipier);
            Assert.AreSame(matrix, task.PositionMatrix);
            Assert.AreSame(tasks, task.Tasks);
            Assert.AreSame(alternatives, task.Alternatives);
            Assert.AreSame(processes, task.Processes);
            Assert.AreSame(positionPrecedences, task.PositionPrecedence);
            Assert.AreSame(processPrecedences, task.ProcessPrecedence);
        }
    }
}