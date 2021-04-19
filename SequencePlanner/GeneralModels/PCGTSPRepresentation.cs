using System.Collections.Generic;

namespace SequencePlanner.GeneralModels
{
    public class PCGTSPRepresentation
    {
        public Motion StartDepot { get; set; }
        public Motion FinishDepot { get; set; }
        public double[][] CostMatrix { get;set;}
        public double[][] RoundedCostMatrix { get; set; }
        public int CostMultiplier { get; set; }
        public List<MotionPrecedence> MotionPrecedences { get; set; }
        public List<MotionDisjointSet> DisjointSets { get; set; }
        public List<Motion> InitialSolution { get; set; }
    }
}