using SequencePlanner.GTSP;
using System;

namespace SequencePlanner.Phraser.Template
{
    public class LineLikeTask: CommonTask
    {
        public GTSPLineRepresentation GTSP { get; set; }
        public ORToolsParametersRaw ORToolsParams { get; set; }
        private ORToolsWrapper ORTool { get; set; }
        private bool Built { get; set; }

        public LineLikeTask()
        {
            GTSP = new GTSPLineRepresentation();
            ORToolsParams = new ORToolsParametersRaw();
        }

        public LineLikeTask(CommonTask common) : this()
        {
            TaskType = common.TaskType;
            EdgeWeightSource = common.EdgeWeightSource;
            DistanceFunction = common.DistanceFunction;
            Dimension = common.Dimension;
            TimeLimit = common.TimeLimit;
            CyclicSequence = common.CyclicSequence;
            StartDepot = common.StartDepot;
            FinishDepot = common.FinishDepot;
            WeightMultiplier = common.WeightMultiplier;
            PositionList = common.PositionList;
            PositionMatrix = common.PositionMatrix;
            GTSP.EdgeWeightCalculator = DistanceFunction;
        }

        public void Build()
        {
            GTSP.Build();
            ORToolsParametersRaw parameters = new ORToolsParametersRaw()
            {
                StartDepot = StartDepot.PID,
                TimeLimit = TimeLimit,
                RoundedMatrix = GTSP.Graph.PositionMatrixRound,
                OrderConstraints = GTSP.ConstraintsOrder,
                DisjointConstraints = GTSP.ConstraintsDisjoints,
                GTSP = GTSP
            };
            ORTool = new ORToolsWrapper(parameters);
            ORTool.Build();
            Built = true;
        }

        public ORToolsResult Run()
        {
            if (!Built)
                Build();
            return ORTool.Solve();
        }
    }
}