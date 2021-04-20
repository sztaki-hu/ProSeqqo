using System.Collections.Generic;

namespace SequencePlanner.GeneralModels
{
    public class DisjointSet<Base>
    {
        public List<Base> Elements { get; set; }
        public int[] SequencMatrixcIDs { get; }
        public List<int> IDs { get; }
    }
}