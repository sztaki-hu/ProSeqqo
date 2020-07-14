using SequencePlanner.GTSP;
using System;

namespace SequencePlanner.Phraser.Template
{
    public class LineLikeTask : CommonTask
    {
        public GTSPLineRepresentation GTSP { get; set; }
        public ORToolsParameters ORToolsParams { get; set; }
        private ORToolsWrapper ORTool { get; set; }
        private LineLikeTemplate Template { get; set; }

        public LineLikeTask()
        {
            GTSP = new GTSPLineRepresentation();
            ORToolsParams = new ORToolsParameters();
        }

        public LineLikeTask(LineLikeTemplate template, CommonTask common) : this()
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
            GTSP.WeightMultiplier = WeightMultiplier;
            GTSP.Build(common.PositionList, template.LineList, template.LinePrecedence, template.ContourPrecedence, template.ContourPenalty);
            FindStartDepo(template.StartDepotID);
        }

        public void Build()
        {
            GTSP.Build();
            ORToolsParameters parameters = new ORToolsParameters()
            {
                StartDepot = StartDepot.ID,
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

        public ORToolsLineResult Run()
        {
            if (!Built)
                Build();

            return new ORToolsLineResult(ORTool.Solve(), this);
        }

        private void FindStartDepo(int ID)
        {
            foreach (var line in GTSP.Lines)
            {
                if (line.UID == ID)
                    StartDepot = line;
            }
        }
    }
}