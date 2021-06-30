using ProSeqqoLib.Model;
using ProSeqqoLib.Model.Hierarchy;
using System.Collections.Generic;

namespace ProSeqqoLib.OR_Tools
{
    public class ORToolsGeneralPreSolverTask
    {
        public int NumberOfNodes { get; set; }
        public int StartDepot { get; set; }
        public int FinishDepot { get; set; }
        public Hierarchy Hierarchy { get; set; }
        public List<MotionPrecedenceList> StrictOrderPrecedenceHierarchy { get; set; }
        public List<MotionPrecedence> OrderPrecedenceConstraints { get; set; }
        public List<MotionDisjointSet> DisjointConstraints { get; set; }
    }
}