using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options.Values
{
    public class ProcessHierarchyOptionValue
    {
        public int PositionID { get; set; }
        public int TaskID { get; set; }
        public int AlternativeID { get; set; }
        public int ProcessID { get; set; }

        public ProcessHierarchyOptionValue()
        {
            PositionID = -1;
            TaskID = -1;
            AlternativeID = -1;
            ProcessID = -1;
        }

        public void FromString(string input)
        {
            string[] tmp = input.Split(';');     //1;2;3--->[1][2][3]
            PositionID = int.Parse(tmp[0]);
            TaskID = int.Parse(tmp[1]);
            AlternativeID = int.Parse(tmp[2]);
            ProcessID = int.Parse(tmp[3]);
        }
    }
}