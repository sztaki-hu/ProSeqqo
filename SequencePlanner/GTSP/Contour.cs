using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class Contour
    {
        private static int maxCID = 0;
        public int CID;
        public int ID;
        public List<Line> Lines { get; set; }

        public Contour()
        {
            CID = maxCID;
            CID++;
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
