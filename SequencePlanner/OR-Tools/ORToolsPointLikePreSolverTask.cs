using SequencePlanner.GTSP;
using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.OR_Tools
{
    public class ORToolsPointLikePreSolverTask: IORToolsPreSolverTask
    {
        public int NumberOfNodes { get; set; }
        public int StartDepot { get; set; }
        public List<GTSPPrecedenceConstraint> OrderPrecedenceConstraints { get; set; }
        public List<GTSPPrecedenceConstraint> StrictOrderPrecedenceHierarchy { get; set; }
        public List<GTSPDisjointConstraint> DisjointConstraints { get; set; }
        public List<Process> Processes { get; internal set; }
    }
}
