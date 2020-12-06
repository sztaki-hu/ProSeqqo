using SequencePlanner.Helper;
using SequencePlanner.Model;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;

namespace SequencePlanner.Function.ResourceFunction
{
    public class ConstantResourceFunction : IResourceFunction
    {
        public string FunctionName { get { return "ConstantResource"; } }
        public double Cost { get; }
        public IResourceDistanceLinkFunction LinkingFunction { get; set; }


        public ConstantResourceFunction(double constantResourceCost, IResourceDistanceLinkFunction link)
        {
            Cost = constantResourceCost;
            LinkingFunction = link;
            Validate();
        }

        public double ComputeResourceCost(Position A, Position B, double distance)
        {
            return LinkingFunction.ComputeResourceDistanceCost(Cost, distance);
        }

        public void Validate()
        {
            if(LinkingFunction == null)
            {
                throw new SequencerException("ConstantResourceFunction.LinkingFunction not given - NULL.");
            }
        }
    }
}