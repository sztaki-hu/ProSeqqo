using SequencePlanner.GTSP;
using System;

namespace SequencePlanner.Phraser.Template
{
    public class PointLikeTask: CommonTask
    {
        public GTSPPointRepresentation GTSP { get; set; }
        public ORToolsParameters ORToolsParams { get; set; }
        private ORToolsWrapper ORTool { get; set; }
        private bool Built { get; set; }

        public PointLikeTask()
        {
            GTSP = new GTSPPointRepresentation();
            ORToolsParams = new ORToolsParameters();
        }

        public PointLikeTask(CommonTask common):this()
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
        }

        public void Build()
        {
            GTSP.Build();
            ORToolsParameters parameters = new ORToolsParameters()
            {
                GTSP = GTSP,
                TimeLimit = TimeLimit,
                StartDepot = StartDepot,
                WeightMultiplier = WeightMultiplier
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