using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class Line : NodeBase
    {
        private static int maxLineID = 0;
        public Position Start { get; set; }
        public Position End { get; set; }
        public Contour Contour { get; set; }
        public double Length { get; internal set; }

        public Line(int UserID = -1, string name = null) : base()
        {
            ID = maxLineID++;
            UID = UserID;
            Name = "Line_" + UID;
        }

        public override string ToString()
        {
            return "[" + UID + "][C:" + Contour.UID + "] " + Name + ": " + Start.Name+" "+Start.ConfigString()+"]" + " --> " + End.Name+" " + End.ConfigString() + "";
        }
    }
}
