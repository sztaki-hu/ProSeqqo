using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class Contour : NodeBase
    {
        private static int maxID = 0;

        public List<Line> Lines { get; set; }

        public Contour()
        {
            ID = maxID;
            ID++;
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
