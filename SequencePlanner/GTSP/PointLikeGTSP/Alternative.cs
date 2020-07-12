using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class Alternative: NodeBase
    {
        private static int AID = 1000;
        public List<Task> Tasks { get; set; }
        private Process process;
        public Process Process { get { return process; } set { process = value; Start.Process = process; Finish.Process = process; } }
        public Position Start { get; private set; } 
        public Position Finish { get; private set; } 


        public Alternative()
        {
            Name = "Alternative_" + AID++;
            Start = new Position()
            {
                GID = AID++,
                Name = Name + "_Start",
                Process = Process,
                Alternative = this,
                Virtual = true
            };
            Finish = new Position()
            {
                GID = AID++,
                Name = Name + "_Finish",
                Virtual = true,
                Process = Process,
                Alternative = this
            };
            Tasks = new List<Task>();
        }

        public Alternative(int id, String name = null) : this()
        {
            GID = id;
            if (name == null)
            {
                Name = "Alternative_" + GID;
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
            foreach (var item in Tasks)
            {
                tmp += item.Name + ", ";
            }
            return "[" + GID + "]" + Name + " Proc: "+Process.Name +" Tasks: " + tmp;
        }
    }
}
