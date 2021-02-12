namespace SequencePlanner.Function.ResourceFunction.ResourceDistanceLink
{
    public class AddResourceDistanceLinkFunction : IResourceDistanceLinkFunction
    {
        public string FunctionName { get { return "Add"; } }

        public double ComputeResourceDistanceCost(double resourceCost, double distanceCost)
        {
            return resourceCost + distanceCost;
        }
    }
}
