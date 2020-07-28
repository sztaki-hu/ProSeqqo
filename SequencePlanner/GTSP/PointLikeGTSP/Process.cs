using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class Process : NodeBase
    {
        private static int maxProcessID=4000;
        public List<Alternative> Alternatives {get; set;}
        public Position Start { get; private set; }
        public Position Finish { get; private set; }


        public Process(int UserID=-1) : base()
        {
            ID = maxProcessID++;
            UID = UserID;
            Name = "Process_" + UID;
            Alternatives = new List<Alternative>();
            //Start = new Position()
            //{
            //    GID = maxProcessID++,
            //    Name = Name + "_Start",
            //    Virtual = true,
            //    Process = this
            //};
            //Finish = new Position()
            //{
            //    GID = maxProcessID++,
            //    Name = Name + "_Finish",
            //    Process = this,
            //    Virtual = true
            //};
        }

        public override string ToString()
        {
            String tmp = "";
            foreach (var item in Alternatives)
            {
                tmp += item.Name + ", ";
            }
            return "[" + UID + "]" + Name + ": Alter: " + tmp;
        }
    }
}
