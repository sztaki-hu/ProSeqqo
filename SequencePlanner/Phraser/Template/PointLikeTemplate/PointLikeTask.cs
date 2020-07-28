using SequencePlanner.GTSP;
using System;

namespace SequencePlanner.Phraser.Template
{
    public class PointLikeTask : CommonTask
    {
        public GTSPPointRepresentation GTSP { get; set; }
        public ORToolsParameters ORToolsParams { get; set; }
        private ORToolsWrapper ORTool { get; set; }
        //private PointLikeTemplate Template { get; set; }

        public PointLikeTask()
        {
            GTSP = new GTSPPointRepresentation();
            ORToolsParams = new ORToolsParameters();
        }

        public PointLikeTask(PointLikeTemplate template, CommonTask common):this()
        {
            TaskType = common.TaskType;
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
            GTSP.WeightMultiplier = WeightMultiplier;
            GTSP.PositionMatrixOriginal = PositionMatrix;
            if (PositionMatrix != null)
                GTSP.PositionMatrix = PositionMatrix.Matrix;
            GTSP.Build(common.PositionList, template.ProcessHierarchy, template.ProcessPrecedence, template.PositionPrecedence, CyclicSequence, (Position)StartDepot, (Position)FinishDepot);
        }

        public new void Build()
        {
            GTSP.Build();
            ORToolsParameters parameters = new ORToolsParameters()
            {
                StartDepot = GTSP.StartID,
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

        public new ORToolsResult Run()
        {
            if (!Built)
                Build();
            return new ORToolsPointResult(ORTool.Solve(), this);
        }
    }
}