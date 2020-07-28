using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options;
using SequencePlanner.Phraser.Options.Values;
using SequencePlanner.Phraser.Template;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner
{
    public class CommonTask : IAbstractTask
    {
        public TaskTypeEnum TaskType { get; set; }
        public EdgeWeightCalculator DistanceFunction { get; set; }
        public int Dimension { get; set; }
        public int TimeLimit { get; set; }
        public bool CyclicSequence { get; set; }
        public NodeBase StartDepot { get; set; }
        public NodeBase FinishDepot { get; set; }
        public int WeightMultiplier { get; set; }
        public List<Position> PositionList { get; set; }
        public PositionMatrixOptionValue PositionMatrix { get; set; }

        protected bool Built { get; set; }
        protected OptionSet OptionSet { get; set; }
        protected CommonTemplate CommonTemplate { get; set; }

        public void Build() {}
        public ORToolsResult Run(){ return null; }
    }
}