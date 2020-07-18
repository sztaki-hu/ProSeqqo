using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class Alternative: NodeBase
    {
        private static int maxAlternativeID = 3000;
        public List<Task> Tasks { get; set; }
        private Process process;
        public Process Process { get { return process; } set { process = value; } }
        public Position Start { get; private set; } 
        public Position Finish { get; private set; } 


        public Alternative(int UserID = -1) : base()
        {
            ID = maxAlternativeID++;
            UID = UserID;
            Name = "Alternative_" + UID;
            Tasks = new List<Task>();
            //Start = new Position()
            //{
            //    Name = Name + "_Start",
            //    Process = Process,
            //    Alternative = this,
            //    Virtual = true
            //};
            //Finish = new Position()
            //{
            //    Name = Name + "_Finish",
            //    Virtual = true,
            //    Process = Process,
            //    Alternative = this
            //};
        }

        public Alternative(int UserID = -1, String name = null) : this(UserID)
        {
            if (name != null)
            {
                Name = name;
            }
            //Start.Name = Name + "_Start";
            //Finish.Name = Name + "_Finish";
        }

        public override string ToString()
        {
            String tmp = "";
            foreach (var item in Tasks)
            {
                tmp += item.Name + ", ";
            }
            return "[" + UID + "]" + Name + " Proc: "+Process.Name +" Tasks: " + tmp;
        }
    }
}
