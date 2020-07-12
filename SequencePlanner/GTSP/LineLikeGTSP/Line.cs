using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class Line : NodeBase
    {
        private static int maxID = 0;
        public Position Start { get; set; }
        public Position End { get; set; }
        public Contour Contour { get; set; }

        public Line()
        {
            ID = maxID;
            maxID++;
        }

        public override string ToString()
        {
            return "[" + UID + "][C:" + Contour.UID + "] " + Name + ": " + Start.Name+" "+Start.ConfigString()+"]" + " --> " + End.Name+" " + End.ConfigString() + "";
        }
    }
}
