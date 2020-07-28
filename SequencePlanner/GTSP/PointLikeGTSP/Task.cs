using System.Collections.Generic;

namespace SequencePlanner.GTSP
{
    public class Task: NodeBase
    {
        private static int maxTaskID = 2000;
        public Alternative Alternative { get; set; }
        public Process Process { get; set; }
        public List<Position> Positions { get; set; }
    
        
        public Task(int UserID = -1): base()
        {
            ID = maxTaskID++;
            UID = UserID;
            Name = "Task_" + UID;
            Positions = new List<Position>(); 
        }

        public override string ToString()
        {
            string tmp = "";
            foreach (var item in Positions)
            {
                tmp += item.Name + ", ";
            }
            return "[" + UID + "]" + Name + " Proc: " + Process.Name + " Alter: " + Alternative.Name + "  Positions: " + tmp;
        }
    }
}