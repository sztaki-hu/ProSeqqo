using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options.Values
{
    public class LineListOptionValue
    {
        public static bool BidirectionalDefault = true;

        public int ContourID { get; set; }
        public int LineID { get; set; }
        public int Dim { get; set; }
        public int PositionA { get; set; }
        public int PositionB { get; set; }
        public bool Bidirectional { get; set; }
        public string Name { get; set; }
        

        public LineListOptionValue()
        {
            LineID = -9999;
            ContourID = -9999;
            Dim = 0;
            PositionA = -1;
            PositionB = -1;
            PositionB = -1;
            Bidirectional = BidirectionalDefault;
            Name = "Line_";
        }

        public void fromString(string input)
        {
            string[] tmp = input.Split(';', '[', ']');
            LineID = int.Parse(tmp[0]);
            ContourID = int.Parse(tmp[1]);
            PositionA = int.Parse(tmp[2]);
            PositionB = int.Parse(tmp[3]);
            if (tmp.Length != 4)
            {
                Name = tmp[4];
            }
            if (tmp.Length != 5)
            {
                if (tmp[5].ToUpper().Equals("TRUE"))
                    Bidirectional = true;
                if (tmp[5].ToUpper().Equals("FALSE"))
                    Bidirectional = false;
            }
        }
    }
}