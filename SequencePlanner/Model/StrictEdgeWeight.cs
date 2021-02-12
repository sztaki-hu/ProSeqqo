using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Model
{
    public class StrictEdgeWeight
    {
        public BaseNode A { get; set; }
        public BaseNode B { get; set; }
        public double Weight { get; set; }
        public bool Bidirectional { get; set; }

        public StrictEdgeWeight(BaseNode a, BaseNode b, double weight, bool bidirectional = false)
        {
            A = a;
            B = b;
            Weight = weight;
            Bidirectional = bidirectional;
        }

        public bool FitFor(BaseNode a, BaseNode b)
        {
            if(A is not null && B is not null && a is not null && b is not null)
            {
                if (Bidirectional)
                    return ((this.A.GlobalID == a.GlobalID && B.GlobalID == b.GlobalID) || (this.A.GlobalID == b.GlobalID && B.GlobalID == a.GlobalID));
                else
                    return (this.A.GlobalID == a.GlobalID && B.GlobalID == b.GlobalID);
            }
            return false;
        }

        public override string ToString()
        {
            return A.ToString() + ";" + B.ToString() + ";" + Weight + ";" + Bidirectional;
        }
    }
}