namespace SequencePlanner.GeneralModels
{
    public class DetailedCost<Base>
    {
        public Base A { get; set; }
        public Base B { get; set; }
        public double FinalCost { get; set; }
        public double ResourceChangeoverCost { get; set; }
        public double DistanceFunctionCost { get; set; }
        public double OverrideCost { get; set; }
        public double AdditionalCost { get; set; }
        public bool Bidirectional { get; set; }
    }
}