using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class Line
    {
        private static int maxLID = 0;
        public int ID { get; set; }
        public int LID { get; set; }
        public string Name { get; set; }
        public Position Start { get; set; }
        public Position End { get; set; }
        public Contour Contour { get; set; }

        public Line()
        {
            LID = maxLID;
            maxLID++;
        }
    }
}
