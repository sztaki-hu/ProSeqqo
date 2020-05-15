using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class Alternative: NodeBase
    {
        public List<Task> Tasks { get; set; }
        public Process Process { get; set; }
        public Position Start { get; private set; } 
        public Position Finish { get; private set; } 


        public Alternative() : base()
        {
            Name = "Alternative_" + ID;
            Start = new Position();
            Start.Name = Name + "_Start";
            Start.Virtual = true;
            Finish = new Position();
            Finish.Name = Name + "_Finish";
            Finish.Virtual = true;
            Tasks = new List<Task>();
        }

        public Alternative(String name):this()
        {
            Name = name;
        }

        public override string ToString()
        {
            String tmp = "";
            foreach (var item in Tasks)
            {
                tmp += item.Name + ", ";
            }
            return "[" + ID + "]" + Name + " Proc: "+Process.Name +" Tasks: " + tmp;
        }
    }
}
