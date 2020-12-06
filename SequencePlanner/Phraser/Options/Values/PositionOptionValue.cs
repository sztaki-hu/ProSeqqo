using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options.Values
{
    public class PositionOptionValue
    {
        public int ID { get; set; }
        public int Dim { get; set; }
        public List<double> Position{ get;set;}
        public string Name { get; set; }
        public int ResourceID { get; set; }

        public PositionOptionValue()
        {
            ID = -9999;
            Dim = 0;
            Position = new List<double>();
            Name = "*";
            ResourceID = -1;
        }

        public void FromString(string input)
        {
            string[] tmp= input.Split(';','[',']');     //1;[x;y;z;];Name--->[1][][x][y][z][][Name]
            ID = int.Parse(tmp[0]);
            int i = 2;
            int dim = 0;
            while (!tmp[i].Equals(""))
            {
                Position.Add(Convert.ToDouble(tmp[i]));
                dim++;
                i++;
            }
            i++;
            Dim = dim;
            if (tmp.Length != i)
            {
                Name = tmp[i];
            }
            i++;
            if (tmp.Length != i)
            {
                ResourceID = Convert.ToInt32(tmp[i]);
            }
        }
    }
}
