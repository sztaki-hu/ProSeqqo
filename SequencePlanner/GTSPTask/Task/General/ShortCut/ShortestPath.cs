using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Task.General.ShortCut
{
    public class ShortestPath
    {
        public GTSPNode Front { get; set; }
        public GTSPNode Back { get; set; }
        public GTSPNode Representer { get; set; }
        public List<GTSPNode> Cut { get; set; }
        public List<double> Costs { get; set; }
        public double Cost { get { return cost; } set { setCost(value);  } }
        private double cost;

        public ShortestPath(GTSPNode front, GTSPNode back, double cost)
        {
            Front = front;
            Back = back;
            Representer = new GTSPNode(new Line() { 
                NodeA = front.In,
                NodeB = back.Out,
                Virtual = true,
                Name = "Shortcut_" + Front.In.UserID + "_" + Back.Out.UserID
            });
            Representer.Weight = cost;
            Representer.AdditionalWeightIn = cost;
            Cut = new List<GTSPNode>();
            Costs = new List<double>();
            Cost = cost;
        }

        private void setCost(double cost)
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