using SequencePlanner.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Model
{
    public class Alternative: BaseNode
    {
        public List<Task> Tasks { get; set; }

        public Alternative():base()
        {
            Name = UserID + "_Alternative_" + GlobalID;
            Tasks = new List<Task>();
        }
    }
}