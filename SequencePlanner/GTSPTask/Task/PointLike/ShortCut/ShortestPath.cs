using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Task.PointLike.ShortCut
{
    public class ShortestPath
    {
        public BaseNode Front { get; set; }
        public BaseNode Back { get; set; }
        public BaseNode Representer { get; set; }
        public List<BaseNode> Cut { get; set; }
        public List<double> Costs { get; set; }
        public double Cost { get; set; }

        public ShortestPath(BaseNode front, BaseNode back, double cost)
        {
            Front = front;
            Back = back;
            Representer = null;
            Cut = new List<BaseNode>();
            Costs = new List<double>();
            Cost = cost;
        }

        public ShortestPath(BaseNode representer, double cost)
        {
            Representer = representer;
            Cut = new List<BaseNode>();
            Costs = new List<double>();
            Cost = cost;
        }
    }
}
