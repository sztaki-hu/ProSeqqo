using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options.Values
{
    public class PrecedenceOptionValue
    {
        public int BeforeID { get; set; }
        public int AfterID { get; set; }
        public string Name { get; set; }

        public void fromString(string input)
        {
            string[] tmp = input.Split(';');     //1;2;Name--->[1][2][Name]
            BeforeID = int.Parse(tmp[0]);
            AfterID = int.Parse(tmp[1]);
            if (tmp.Length < 2)
            {
                Name = tmp[2];
            }
        }

    }
}
