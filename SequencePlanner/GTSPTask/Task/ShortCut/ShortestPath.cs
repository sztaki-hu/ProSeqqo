using System.Collections.Generic;
using SequencePlanner.Model;

namespace SequencePlanner.GTSPTask.Task.General.ShortCut
{
    public class ShortestPath
    {
        private double cost;

        public GTSPNode Front { get; set; }
        public GTSPNode Back { get; set; }
        public GTSPNode Representer { get; set; }
        public List<GTSPNode> Cut { get; set; }
        public List<double> Costs { get; set; }
        public double Cost { get { return cost; } set { SetCost(value);  } }


        public ShortestPath(GTSPNode front, GTSPNode back, double cost)
        {
            Front = front;
            Back = back;
            Representer = new GTSPNode(
                new Line()
                {
                    NodeA = front.In,
                    NodeB = back.Out,
                    Virtual = true,
                    Name = "Shortcut_" + Front.In.UserID + "_" + Back.Out.UserID
                })
            {
                Weight = cost,
                AdditionalWeightIn = cost
            };
            Cut = new List<GTSPNode>();
            Costs = new List<double>();
            Cost = cost;
        }


        private void SetCost(double cost)
        {
            this.cost = cost;
            if (Representer != null)
            {
                Representer.Weight = cost;
                Representer.AdditionalWeightIn = cost;
            }
        }
    }
}