using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePlanner.Model
{
    public class GTSPNode
    {
        public Position In { get; set; }
        public Position Out { get; set; }
        public double Weight { get; set; }
        public double AdditionalWeightIn { get; set; }
        public double AdditionalWeightOut { get; set; }
        public double OverrideWeightIn { get; set; }
        public double OverrideWeightOut { get; set; }
        public bool Bidirectional { get; set; }
        public BaseNode Node { get; set; }

        public GTSPNode()
        {
        }

        public GTSPNode(Position position)
        {
            Node = position;
            In = position;
            Out = position;
            Bidirectional = false;
        }

        public GTSPNode(Line line)
        {
            Node = line;
            In = line.NodeA;
            Out = line.NodeB;
            Bidirectional = line.Bidirectional;
        }

        public override string ToString()
        {
            var tmp = Node.ToString()+":  ";
            tmp += In.ToString();
            if (In.GlobalID != Out.GlobalID)
                tmp += " --To--> " + Out.ToString();
            tmp += " Weight: " + Weight;
            return tmp;
        }

        public GTSPNode GetBidirectional()
        {
            return new GTSPNode()
            {
                Node = this.Node.GetReverse(),
                In = this.Out,
                Out = this.In,
                OverrideWeightIn = this.OverrideWeightOut,
                OverrideWeightOut = this.OverrideWeightIn,
                AdditionalWeightIn = this.AdditionalWeightOut,
                AdditionalWeightOut = this.AdditionalWeightIn,
                Bidirectional = true,
                Weight = this.Weight
            };
        }
    }
}