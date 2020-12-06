namespace SequencePlanner.Function.ResourceFunction.ResourceDistanceLink
{
    public interface IResourceDistanceLinkFunction
    {
        public string FunctionName { get; }
        public double ComputeResourceDistanceCost(double resourceCost, double distanceCost);
    }
}
