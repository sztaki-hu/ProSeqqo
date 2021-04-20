using SequencePlanner.GeneralModels;
using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.OR_Tools
{
    public interface IORToolsPreSolverTask
    {
        public int StartDepot { get; set; }
        public int NumberOfNodes { get; set; }
        public List<MotionPrecedence> OrderPrecedenceConstraints { get; set; }
        public List<MotionDisjointSet> DisjointConstraints { get; set; }
    }
}
