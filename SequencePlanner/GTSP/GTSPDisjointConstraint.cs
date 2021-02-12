using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.GTSP
{
    public class GTSPDisjointConstraint
    {
        public long[] DisjointSet { get { GetSeqIDs(); return NodeListID.ToArray(); } private set { } }
        private List<long> NodeListID;
        private readonly List<BaseNode> NodeList;

        public GTSPDisjointConstraint()
        {
            NodeListID = new List<long>();
            NodeList = new List<BaseNode>();
        }

        public void Add(BaseNode node)
        {
            NodeList.Add(node);
            NodeListID.Add(node.SequencingID);
        }

        public void Add(List<BaseNode> lines)
        {
            foreach (var line in lines)
            {
                Add(line);
            }
        }

        private void GetSeqIDs()
        {
            NodeListID = new List<long>();
            foreach (var item in NodeList)
            {
                NodeListID.Add(item.SequencingID);
            }
        }

        public override string ToString()
        {
            var tmp = "";
            foreach (var item in NodeList)
            {
                tmp += item.ToString() + ", ";
            }
            return "Disjoint set: " + tmp;
        }
    }
}