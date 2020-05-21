using SequencePlanner;
using SequencePlanner.GTSP;
using SequencePlanner.Options;
using System;
using DistanceFunction = SequencePlanner.DistanceFunction;
using EdgeWeightSource = SequencePlanner.EdgeWeightSource;

namespace Seqencer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //SequencerOptionPhraser sequencerOptions = new SequencerOptionPhraser();
            //sequencerOptions.ReadFile("test.txt");

            Test test = new Test();
            test.RepresentationTest();
            Console.ReadKey();


            //Solver solver = new Solver();
            //solver.RunFile();

            //GraphRepresentation graph = new GraphRepresentation();

            //Process process = new Process();
            //Alternative alternative = new Alternative();
            //Task task = new Task();
            //Position position = new Position();

            //graph.addProcess(process);
            //graph.addAlternative(process, alternative);
            //graph.addTask(alternative, task);
            //graph.addPosition(task, position);

            //graph.build();

            SequencerTask sTask = new SequencerTask(graph);
            sTask.TaskType = SequencePlanner.TaskType.PointLike;
            sTask.TaskType = SequencePlanner.TaskType.LineLike;
            sTask.EdgeWeightSource = EdgeWeightSource.FullMatrix;
            sTask.EdgeWeightSource = EdgeWeightSource.CalculateFromPositions;
            sTask.DistanceFunction = DistanceFunction.Euclidian_Distance;
            sTask.DistanceFunction = DistanceFunction.Max_Distance;
            sTask.DistanceFunction = DistanceFunction.Trapezoid_Time;
            sTask.DistanceFunction = DistanceFunction.Manhattan_Distance;
            sTask.Dimensions = 3;
            sTask.TimeLimit = 300;
            sTask.CyclicSequence = true;
            sTask.StartDepot = 99;
            sTask.FinishDepot = 100;
            sTask.WeightMultiplierAuto = true;
            sTask.WeightMultiplier = 100;


            //solver.Run();
        }
    }
}