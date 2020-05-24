using System.Collections.Generic;

namespace SequencePlanner.GTSP
{
    public class Task: NodeBase
    {
        public Alternative Alternative { get; set; }
        public Process Process { get; set; }
        public List<Position> Positions { get; set; }
    
        
        public Task(): base()
        {
                Name = "Task_" + ID;
                Positions = new List<Position>(); 
        }

        public Task(int id, string name = null) : this()
        {
            ID = id;
            if (name == null)
            {
                Name = "Task_" + ID;
            }
            else
            {
                Name = name;
            }

        }

        public override string ToString()
        {
            string tmp = "";
            foreach (var item in Positions)
            {
                tmp += item.Name + ", ";
            }
            return "[" + ID + "]" + Name + " Proc: " + Process.Name + " Alter: " + Alternative.Name + "  Positions: " + tmp;
        }
    }
}