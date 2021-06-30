using SequencePlanner.Model.Hierarchy;
using SequencePlanner.Helper;
using System.Collections.Generic;
using System.Linq;

namespace SequencePlanner.Model
{
    public class MotionDisjointSet : DisjointSet<Motion>
    {
        public int[] SequencMatrixcIDs { get { return GetSeqIDs(); } }
        public List<int> IDs { get { return GetIDs(); } }

        private List<int> GetIDs()
        {
           return Elements.Select(r => r.ID).ToList();
        }

        private int[] GetSeqIDs()
        {
            return Elements.Select(r => r.SequenceMatrixID).ToArray();
        }

        public override string ToString()
        {
            return "Disjoint set: " + Elements.ToIDListString();
        }
    }
}