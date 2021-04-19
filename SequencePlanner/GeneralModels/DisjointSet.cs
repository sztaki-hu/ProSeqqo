using System.Collections.Generic;

namespace SequencePlanner.GeneralModels
{
    public class DisjointSet<Base>
    {
        public List<Base> Elements { get; set; }
        public List<int> SequencIDs { get; }
        public List<int> IDs { get; }
    }
}