using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Task.PointLike.ShortCut
{
    public class ShortestPath
    {
        public BaseNode Front { get; set; }
        public BaseNode Back { get; set; }
        public List<BaseNode> Cut { get; set; }
        public List<double> Costs { get; set; }
        public double Cost { get; set; }

        public ShortestPath(BaseNode front, BaseNode back, double cost)
        {
            Front = front;
            Back = back;
            Cut = new List<BaseNode>();
            Costs = new List<double>();
            Cost = cost;
        }
    }
}
