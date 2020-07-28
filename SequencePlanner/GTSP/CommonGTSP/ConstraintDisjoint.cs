using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSP
{
    public class ConstraintDisjoint
    {
        public long[] DisjointSet { get { return NodeListID.ToArray(); } private set { } }
        private readonly List<long> NodeListID;
        private readonly List<NodeBase> NodeList;

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

        public override string ToString()
        {
            var tmp = "";
            foreach (var item in NodeList)
            {
                tmp += item.UID + ", ";
            }
            return "Set: " + tmp;
        }
    }
}