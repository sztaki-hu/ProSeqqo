using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSP
{
    public class ConstraintDisjoint
    {
        public List<Position> NodeList;


        public ConstraintDisjoint()
        {
            NodeList = new List<Position>();
        }

        public void addConstraint(Position node)
        {
            NodeList.Add(node);
        }
        public void addConstraint(List<Position> nodes)
        {
            NodeList.AddRange(nodes);
        }

        public long[] getIndices()
        {
            var tmp = new long[NodeList.Count];
            for (int i = 0; i < NodeList.Count; i++)
            { 
                tmp[i] = Convert.ToInt32(NodeList[i].PID);
            }
            return tmp;
        }

        public override string ToString()
        {
            var tmp = "";
            foreach (var item in NodeList)
            {
                tmp += "[" + item.ID + "]"+ "[PID:" + item.PID +"]"+ item.Name+"; ";
            }
            return "DisjointSet("+NodeList.Count+" constraints): " + tmp;
        }
    }
}