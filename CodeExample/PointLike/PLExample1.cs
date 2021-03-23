using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Task.PointLike;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using System.Collections.Generic;
using static SequencePlanner.OR_Tools.LocalSearchStrategyEnum;

namespace CodeExample.PointLike
{
    public class PLExample1
    {
        private PointLikeTask Task { get; set; }
        private PointTaskResult Result { get; set; }

        public PLExample1(){
            SeqLogger.LogLevel = LogLevel.Error;
            Task = new PointLikeTask() {
                CyclicSequence = false,
                Dimension = 1,
                LocalSearchStrategy = Metaheuristics.Automatic,
                UseShortcutInAlternatives = true,
                UseMIPprecedenceSolver = true,
                WeightMultipier = -1,
                TimeLimit = 100
            };

            //Create Positions
            Task.PositionMatrix = new PositionMatrix();
            Task.PositionMatrix.Positions = new List<GTSPNode>();
            Task.PositionMatrix.DistanceFunction = new EuclidianDistanceFunction();
            //Task.PositionMatrix.DistanceFunction = new MatrixDistanceFunction()
            Task.PositionMatrix.DistanceFunction.StrictUserEdgeWeights = new StrictEdgeWeightSet();
            Task.PositionMatrix.ResourceFunction = new ConstantResourceFunction(2,new AddResourceDistanceLinkFunction());
            Task.PositionMatrix.ResourceFunction = new NoResourceFunction();

            //Build hierarchy
            Task.Processes = new List<Process>();
            Task.Alternatives = new List<Alternative>();
            Task.Tasks = new List<SequencePlanner.Model.Task>();

            Task.StartDepot = new Position();
            Task.FinishDepot = new Position();
            //Add precedences
            Task.PositionPrecedence = new List<SequencePlanner.GTSP.GTSPPrecedenceConstraint>();
            Task.ProcessPrecedence = new List<SequencePlanner.GTSP.GTSPPrecedenceConstraint>();
        }

        public void Run()
        {
            Result = Task.RunModel();
        }

        public void PrintSolution()
        {
            Result.ToLog(SequencePlanner.Helper.LogLevel.Info);
        }
    }
}
