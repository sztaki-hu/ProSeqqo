using SequencePlanner.Helper;
using SequencePlanner.Model;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;

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

        public double ComputeResourceCost(Position A, Position B, double distance)
        {
            return LinkingFunction.ComputeResourceDistanceCost(Cost, distance);
        }

        public void Validate()
        {
            SeqLogger.Info("ResourceFunction: " + FunctionName, nameof(ConstantResourceFunction));
            SeqLogger.Info("ResourceConstant: " + Cost, nameof(ConstantResourceFunction));
            if(LinkingFunction == null)
            {
                throw new SeqException("ConstantResourceFunction.LinkingFunction not given.");
            }
            SeqLogger.Info("LinkingFunction: " + LinkingFunction.FunctionName, nameof(ConstantResourceFunction));
        }

        public void ToLog(LogLevel level)
        {
            SeqLogger.Indent++;
            SeqLogger.WriteLog(level, "ResourceFunction: " + FunctionName, nameof(DistanceFunction));
            SeqLogger.Indent--;
        }
    }
}