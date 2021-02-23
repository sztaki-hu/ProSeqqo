using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Task.LineLike;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SequencePlanner.OR_Tools.LocalSearchStrategieEnum;

namespace CodeExample.LineLike
{
    public class LLExample1
    {
        private LineLikeTask Task { get; set; }
        private LineTaskResult Result { get; set; }

        public LLExample1()
        {
            SeqLogger.LogLevel = LogLevel.Info;
            Task = new LineLikeTask()
            {
                CyclicSequence = false,
                Dimension = 1,
                LocalSearchStrategie = Metaheuristics.Automatic,
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
            Task.PositionMatrix.ResourceFunction = new ConstantResourceFunction(2, new AddResourceDistanceLinkFunction());
            Task.PositionMatrix.ResourceFunction = new NoResourceFunction();

            //Build hierarchy
            Task.Lines = new List<Line>();
            Task.StartDepot = new Position();
            Task.FinishDepot = new Position();

            //Add precedences
            Task.LinePrecedences = new List<SequencePlanner.GTSP.GTSPPrecedenceConstraint>();
            Task.ContourPrecedences = new List<SequencePlanner.GTSP.GTSPPrecedenceConstraint>();
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
