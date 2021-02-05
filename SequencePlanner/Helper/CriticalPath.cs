using SequencePlanner.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Helper
{
    public class CriticalPath
    {
        public BaseNode Front { get; set; }
        public BaseNode Back { get; set; }
        public List<BaseNode> Cut { get; set; }
        public List<double> Costs { get; set; }
        public double Cost { get; set; }

        public CriticalPath(BaseNode front, BaseNode back, double cost)
        {
            Front = front;
            Back = back;
            Cut = new List<BaseNode>();
            Costs = new List<double>();
            Cost = cost;
        }
    }
}
