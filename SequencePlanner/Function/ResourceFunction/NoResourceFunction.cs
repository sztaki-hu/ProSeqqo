using SequencePlanner.Model;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;

namespace SequencePlanner.Function.ResourceFunction
{
    public class NoResourceFunction : IResourceFunction
    {
        public string FunctionName { get { return "NoResource"; } }
        public double Cost { get; }
        public IResourceDistanceLinkFunction LinkingFunction { get; set; }

        public double ComputeResourceCost(Position A, Position B, double distance)
        {
            return distance;
        }

        public void Validate()
        {
            
        }
    }
}