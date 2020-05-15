using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class Process : NodeBase
    {
        public List<Alternative> Alternatives {get; set;}
        public Position Start { get; private set; }
        public Position Finish { get; private set; }


        public Process() : base()
        {
            Name = "Process_" + ID;
            Start = new Position();
            Start.Name = Name + "_Start";
            Start.Virtual = true;
            Finish = new Position();
            Finish.Name = Name + "_Finish";
            Finish.Virtual = true;
            Alternatives = new List<Alternative>();
        }

        public Process(String name) : this()
        {
            Name = name;
        }

        public override string ToString()
        {
            String tmp = "";
            foreach (var item in Alternatives)
            {
                tmp += item.Name + ", ";
            }
            return "[" + ID + "]" + Name + ": Alter: " + tmp;
        }
    }
}
