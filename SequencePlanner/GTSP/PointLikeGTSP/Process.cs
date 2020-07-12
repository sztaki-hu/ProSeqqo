using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class Process : NodeBase
    {
        private static int ProcID=5000;
        public List<Alternative> Alternatives {get; set;}
        public Position Start { get; private set; }
        public Position Finish { get; private set; }


        public Process() : base()
        {
            Name = "Process_" + ProcID++;
            Start = new Position()
            {
                GID = ProcID++,
                Name = Name + "_Start",
                Virtual = true,
                Process = this
            };
            Finish = new Position()
            {
                GID = ProcID++,
                Name = Name + "_Finish",
                Process = this,
                Virtual = true
            };
            Alternatives = new List<Alternative>();
        }

        public Process(int id, String name = null) : this()
        {
            GID = id;
            if(name == null)
            {
                Name = "Process_" + GID;
            }
            else
            {
                Name = name;
            }

            Start.Name = Name + "_Start";
            Finish.Name = Name + "_Finish";

        }

        public override string ToString()
        {
            String tmp = "";
            foreach (var item in Alternatives)
            {
                tmp += item.Name + ", ";
            }
            return "[" + GID + "]" + Name + ": Alter: " + tmp;
        }
    }
}
