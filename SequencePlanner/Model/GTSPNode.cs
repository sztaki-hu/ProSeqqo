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

        public GTSPNode(Position position)
        {
            In = position;
            Out = position;
            Weight = 0;
        }

        public GTSPNode(Line line)
        {
            In = line.NodeA;
            Out = line.NodeB;
            Weight = line.Length;
        }
    }
}
