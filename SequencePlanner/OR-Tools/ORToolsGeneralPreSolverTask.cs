using SequencePlanner.GeneralModels;
using System.Collections.Generic;

namespace SequencePlanner.OR_Tools
{
    public class ORToolsGeneralPreSolverTask
    {
        public int NumberOfNodes { get; set; }
        public int StartDepot { get; set; }
        public int FinishDepot { get; set; }
        public List<Process> Processes { get; set; }
        public List<MotionPrecedenceList> StrictOrderPrecedenceHierarchy { get; set; }
        public List<GeneralModels.MotionPrecedence> OrderPrecedenceConstraints { get; set; }
        public List<GeneralModels.MotionDisjointSet> DisjointConstraints { get; set; }
    }
}