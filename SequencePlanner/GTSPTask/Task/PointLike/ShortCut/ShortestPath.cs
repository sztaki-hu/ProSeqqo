using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Task.PointLike.ShortCut
{
    public class ShortestPath
    {
        public GTSPNode Front { get; set; }
        public GTSPNode Back { get; set; }
        public BaseNode Representer { get; set; }
        public List<GTSPNode> Cut { get; set; }
        public List<double> Costs { get; set; }
        public double Cost { get; set; }

        public ShortestPath(GTSPNode front, GTSPNode back, double cost)
        {
            Front = front;
            Back = back;
            Representer = null;
            Cut = new List<GTSPNode>();
            Costs = new List<double>();
            Cost = cost;
        }

        public ShortestPath(BaseNode representer, double cost)
        {
            Representer = representer;
            Cut = new List<GTSPNode>();
            Costs = new List<double>();
            Cost = cost;
        }
    }
}
