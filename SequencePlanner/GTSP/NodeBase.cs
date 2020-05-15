using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class NodeBase
    {
        private static int maxID = 0;
        public String Name { get; set; }
        public int ID { get; set; }
        public bool Virtual { get; set; }

        public NodeBase()
        {
            ID = maxID++;
            Name = "NodeBase_" + ID;
        } 
        public NodeBase(string name, int id)
        {
            Name = name;
            ID = id;
        }
    }
}
