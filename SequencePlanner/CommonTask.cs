using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options.Values;
using SequencePlanner.Phraser.Template;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner
{
    public class CommonTask
    {
        public TaskTypeEnum TaskType { get; set; }
        public EdgeWeightSourceEnum EdgeWeightSource { get; set; }
        public EdgeWeightCalculator DistanceFunction { get; set; }
        public int Dimension { get; set; }
        public int TimeLimit { get; set; }
        public bool CyclicSequence { get; set; }
        public Position StartDepot { get; set; }
        public Position FinishDepot { get; set; }
        public int WeightMultiplier { get; set; }
        public List<Position> PositionList { get; set; }
        public PositionMatrixOptionValue PositionMatrix { get; set; }
    }
}