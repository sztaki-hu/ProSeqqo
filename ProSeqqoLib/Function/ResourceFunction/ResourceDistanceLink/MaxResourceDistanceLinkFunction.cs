using System;

namespace SequencePlanner.Function.ResourceFunction.ResourceDistanceLink
{
    public class MaxResourceDistanceLinkFunction : IResourceDistanceLinkFunction
    {
        public string FunctionName { get { return "Max"; } }


        public double ComputeResourceDistanceCost(double resourceCost, double distanceCost)
        {
            return Math.Max(resourceCost, distanceCost);
        }
    }
}
