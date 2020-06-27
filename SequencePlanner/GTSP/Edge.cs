using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class Edge
    {
        public string Tag { get; set; }
        public Position NodeA { get; set; }
        public Position NodeB { get; set; }
        public bool Directed { get; set; }
        public double Weight { get; set; }

        public override string ToString()
        {
            if (Directed)
                return NodeA.Name+"["+NodeA.PID+"]" + "---" + Weight + "--->" + NodeB.Name+ "[" + NodeB.PID + "]";
            else
                return NodeA.Name + "<---" + Weight + "--->" + NodeB.Name;
        }
    }
}
