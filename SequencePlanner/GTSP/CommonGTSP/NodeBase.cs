using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class NodeBase
    {
        private static int maxGID = 0;
        public String Name { get; set; }
        public int GID { get; set; }   //ID globaly unique
        public int UID { get; set; }   //ID given by User
        public int ID { get; set; }   //ID unique by type
        public bool Virtual { get; set; }

        public NodeBase()
        {
            GID = maxGID++;
            Name = "NodeBase_" + GID;
        } 
        public NodeBase(string name)
        {
            GID = maxGID++;
            Name = name;
        }
    }
}
