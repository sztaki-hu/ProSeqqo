using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSP
{
    public class ConstraintDisjoint
    {
        public List<NodeBase> NodeList;


        public ConstraintDisjoint()
        {
            NodeList = new List<NodeBase>();
        }

        public void addConstraint(NodeBase node)
        {
            NodeList.Add(node);
        }

         public long[] getIndices()
        {
            var tmp = new long[NodeList.Count];
            for (int i = 0; i < NodeList.Count; i++)
            { 
                tmp[i] = Convert.ToInt32(NodeList[i]);
            }
            return tmp;
        }
    }
}