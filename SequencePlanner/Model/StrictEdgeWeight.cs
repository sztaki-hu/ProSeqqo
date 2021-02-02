using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Model
{
    public class StrictEdgeWeight
    {
        public Position A { get; set; }
        public Position B { get; set; }
        public double Weight { get; set; }
        public bool Bidirectional { get; set; }

        public StrictEdgeWeight(Position a, Position b, double weight, bool bidirectional = false)
        {
            A = a;
            B = b;
            Weight = weight;
            Bidirectional = bidirectional;
        }

        public bool FitFor(BaseNode a, BaseNode b)
        {
            if (Bidirectional)
                return ((this.A.GlobalID == a.GlobalID && B.GlobalID == b.GlobalID) || (this.A.GlobalID == b.GlobalID && B.GlobalID == a.GlobalID));
            else
                return (this.A.GlobalID == a.GlobalID && B.GlobalID == b.GlobalID);
        }
    }
}