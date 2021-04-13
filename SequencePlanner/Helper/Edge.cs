using SequencePlanner.Model;

namespace SequencePlanner.Helper
{
    internal class Edge
    {
        public GTSPNode A { get; set; }
        public GTSPNode B { get; set; }
        public double Weight { get; set; }
    }
}
