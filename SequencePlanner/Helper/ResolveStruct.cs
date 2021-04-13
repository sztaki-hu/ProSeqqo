using System.Collections.Generic;
using SequencePlanner.Model;

namespace SequencePlanner.Helper
{
    public class ResolveStruct
    {
        public int SeqID { get; set; }
        public int GID { get; set; }
        public double Cost { get; set; }
        public bool Resolved { get; set; }
        public GTSPNode Node { get; set; }
        public List<GTSPNode> Resolve { get; set; }
        public List<double> ResolveCost { get; set; }

        public ResolveStruct()
        {
            Resolve = new List<GTSPNode>();
            ResolveCost = new List<double>();
        }
    }
}
