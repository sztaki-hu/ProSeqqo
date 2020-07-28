using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class Contour : NodeBase
    {
        private static int maxContouID = 0;

        public List<Line> Lines { get; set; }

        public Contour(int UserID = -1): base()
        {
            ID = maxContouID++;
            UID = UserID;
            Lines = new List<Line>();
        }

        public Contour(List<Line> lines) : this()
        {
            Lines = lines;
        }

        public void AddLine(Line line)
        {
            Lines.Add(line);
        }
    }
}
