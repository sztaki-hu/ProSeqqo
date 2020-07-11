using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options.Values
{
    public class LineListOptionValue
    {
        public int ContourID { get; set; }
        public int LineID { get; set; }
        public int Dim { get; set; }
        public int PositionA { get; set; }
        public int PositionB { get; set; }
        public string Name { get; set; }

        public LineListOptionValue()
        {
            LineID = -9999;
            ContourID = -9999;
            Dim = 0;
            PositionA = -1;
            PositionB = -1;
            Name = "*";
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
        }

        //public void fromStringXYZ(string input)
        //{
        //    string[] tmp = input.Split(';', '[', ']');     //1;2;[x;y;z;];[x;y;z;];Name--->[1][2][][x][y][z][][][x][y][z][][Name]
        //    LineID = int.Parse(tmp[0]);
        //    ContourID = int.Parse(tmp[1]);
        //    int i = 3;
        //    int dim = 0;
        //    while (!tmp[i].Equals(""))
        //    {
        //        PositionA.Add(Convert.ToDouble(tmp[i]));
        //        dim++;
        //        i++;
        //    }
        //    i++;
        //    i++;
        //    Dim = dim;
        //    while (!tmp[i].Equals(""))
        //    {
        //        PositionB.Add(Convert.ToDouble(tmp[i]));
        //        dim++;
        //        i++;
        //    }
        //    Dim = dim;
        //    i++;
        //    if (tmp.Length != i)
        //    {
        //        Name = tmp[i];
        //    }
        //}
    }
}
