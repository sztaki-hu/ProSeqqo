using ProSeqqoLib.Model.Hierarchy;
using ProSeqqoLib.Task;
using System.Collections.Generic;

namespace ProSeqqoLib.Model.ShortestPath
{
    public class ShortAlernative: Alternative
    {
        private static int ShortAlternativeIDMax = 8000000;
        public Alternative Alternative { get; set; }
        public List<HierarchyRecord> Records { get; set; }
        public Graph Graph { get; set; }
        
        public ShortAlernative(Alternative alternative, List<HierarchyRecord> records, CostManager costManager): base()
        {
            ID = ShortAlternativeIDMax++;
            Alternative = alternative;
            Records = records;
            Graph = new Graph(records, costManager);
            Graph.ComputeShortestPaths();
        }



        public List<MotionPath> GetShortestPath()
        {
            return Graph.GetShortestPaths();
        }
    }
}
