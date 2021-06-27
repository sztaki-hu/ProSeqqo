using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;
using SequencePlanner.Helper;
using SequencePlanner.Model.Hierarchy;

namespace SequencePlanner.Function.ResourceFunction
{
    public class ConstantResourceFunction : IResourceFunction
    {
        public string FunctionName { get { return "Constant"; } }
        public double Cost { get; }
        public IResourceDistanceLinkFunction LinkingFunction { get; set; }


        public ConstantResourceFunction(double constantResourceCost, IResourceDistanceLinkFunction link)
        {
            Cost = constantResourceCost;
            LinkingFunction = link;
            Validate();
        }

        public double ComputeResourceCost(Config A, Config B, double distance)
        {
            if (A.Virtual || B.Virtual)
                return LinkingFunction.ComputeResourceDistanceCost(0, distance);
            if (A.Resource.ID != B.Resource.ID)
                return LinkingFunction.ComputeResourceDistanceCost(Cost, distance);
            else
                return distance;
        }

        public double GetResourceCost(Config A, Config B)
        {
            if (A.Virtual || B.Virtual)
                return 0;
            if (A.Resource.ID != B.Resource.ID)
                return Cost;
            else
                return 0;
        }

        public void Validate()
        {
            SeqLogger.Debug("ResourceFunction: " + FunctionName, nameof(ConstantResourceFunction));
            SeqLogger.Debug("ChangeoverConstant: " + Cost, nameof(ConstantResourceFunction));
            if (LinkingFunction == null)
            {
                throw new SeqException("ConstantResourceFunction.LinkingFunction not given.");
            }
            SeqLogger.Debug("LinkingFunction: " + LinkingFunction.FunctionName, nameof(ConstantResourceFunction));
        }

        public void ToLog(LogLevel level)
        {
            SeqLogger.Indent++;
            SeqLogger.WriteLog(level, "ResourceFunction: " + FunctionName, nameof(DistanceFunction));
            SeqLogger.Indent--;
        }
    }
}