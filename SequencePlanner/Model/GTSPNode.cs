using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePlanner.Model
{
    public class GTSPNode
    {
        public BaseNode Node { get; set; }
        public Position In { get; set; }
        public Position Out { get; set; }
        public double Weight { get; set; }
        public double AdditionalWeightIn { get; set; }
        public double AdditionalWeightOut { get; set; }
        public double OverrideWeightIn { get; set; }
        public double OverrideWeightOut { get; set; }
        public bool Bidirectional { get; set; }

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


        //DEFAULT
        public GTSPNode GetCopy()
        {
            var node = new GTSPNode()
            {
                Node = Node,
                In = In,
                Out = Out,
                Weight = Weight,
                AdditionalWeightIn = AdditionalWeightIn,
                AdditionalWeightOut = AdditionalWeightOut,
                OverrideWeightIn = OverrideWeightIn,
                OverrideWeightOut = OverrideWeightOut,
                Bidirectional = Bidirectional
            };
            return node;
        }

        public override string ToString()
        {
            var tmp = Node.ToString() + ":  ";
            tmp += In.ToString();
            if (In.GlobalID != Out.GlobalID)
                tmp += " --To--> " + Out.ToString();
            tmp += " Weight: " + Weight;
            return tmp;
            //return Node.ToString() + "In: " + In.ToString() + "Out: " + Out.ToString();
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                GTSPNode node = (GTSPNode)obj;
                base.Equals((GTSPNode)node);
                if (node.Node.Equals(node))
                    return false;
                if (node.In.Equals(node))
                    return false;
                if (node.Out.Equals(node))
                    return false;
                if (node.Weight.Equals(node))
                    return false;
                if (node.AdditionalWeightIn == AdditionalWeightIn)
                    return false;
                if (node.AdditionalWeightOut == AdditionalWeightOut)
                    return false;
                if (node.OverrideWeightIn == OverrideWeightIn)
                    return false;
                if (node.OverrideWeightOut == OverrideWeightOut)
                    return false;
                if (node.Bidirectional == Bidirectional)
                    return false;
                return true;
            }
        }

        public override int GetHashCode()
        {
            return Node.GetHashCode() + In.GetHashCode() + Out.GetHashCode() + Weight.GetHashCode() + AdditionalWeightIn.GetHashCode() + AdditionalWeightOut.GetHashCode() + OverrideWeightIn.GetHashCode() + OverrideWeightOut.GetHashCode() + Bidirectional.GetHashCode();
        }
    }
}