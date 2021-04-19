using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePlanner.GeneralModels
{
    public class Node
    {
        private static int NextMaxGlobalID = 0;
        public int ID { get; set; }
        public int GlobalID { get; private set; }
        public string Name { get; private set; }

        protected Node()
        {
            GlobalID = NextMaxGlobalID++;
            Name = "Node";
        }

        public Node(int id, string name): this()
        {
            ID = id;
            Name = name;
        }
    }
}
