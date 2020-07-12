using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSP
{
    public class ConstraintDisjoint
    {
        public long[] DisjointSet { get { return NodeListID.ToArray(); } private set { } }
        private List<long> NodeListID;
        private List<NodeBase> NodeList;

        public ConstraintDisjoint()
        {
            NodeListID = new List<long>();
            NodeList = new List<NodeBase>();
        }

        public void Add(NodeBase node)
        {
            NodeList.Add(node);
            NodeListID.Add(node.ID);
        }

        public void Add(List<NodeBase> lines)
        {
            foreach (var line in lines)
            {
                Add(line);
            }
        }
    }
}