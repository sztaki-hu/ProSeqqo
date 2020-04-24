using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Template
{
    public class ProcessHierarchy
    {
        public Position Position { get; set; }
        public int TaskID { get; set; }
        public int AlternativeID { get; set; }
        public int ProcessID { get; set; }

        public ProcessHierarchy(Position position, int taskID, int alternativeID, int processID)
        {
            Position = position;
            TaskID = taskID;
            AlternativeID = alternativeID;
            ProcessID = processID;
        }
    }
}
