using SequencePlanner.GTSP;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Task.Base
{
    public interface IGTSPRepresentation
    {
        public int StartDepot { get; set; }
        public int FinishDepot { get; set; }
        public List<GTSPPrecedenceConstraint> PrecedenceConstraints { get; set; }
        public List<GTSPDisjointConstraint> DisjointConstraints { get; set; }
        public int[,] RoundedMatrix { get; set; }
        public double[,] Matrix { get; set; }
        public long[][] InitialRoutes { get; set; }
    }
}
