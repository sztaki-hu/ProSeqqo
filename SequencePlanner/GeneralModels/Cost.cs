namespace SequencePlanner.GeneralModels
{
    public class Cost<Base>
    {
        public Base A { get; set; }
        public Base B { get; set; }
        public double OverrideCost { get; set; }
        public double AdditionalCost { get; set; }
        public bool Bidirectional { get; set; }
    }
}