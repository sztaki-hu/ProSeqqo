using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Model;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;
using SequencePlanner.Helper;

namespace SequencePlanner.Function.ResourceFunction
{
    public class PositionBasedResourceFunction : IResourceFunction
    {
        public string FunctionName { get { return "PositionBasedResource"; } }
        public IResourceDistanceLinkFunction LinkingFunction { get; set; }
        public PositionBasedResourceFunction(IDistanceFunction distanceFunction, IResourceDistanceLinkFunction resourceDistanceLink)
        {
            //TODO:PositionBasedResourceFunction
            Validate();
        }

        public double ComputeResourceCost(Position A, Position B, double distance)
        {
            //TODO: PositionBasedResourceFunction
            return 0;
        }

        public void Validate()
        {

        }
        public void ToLog(LogLevel level)
        {
            SeqLogger.Indent++;
            SeqLogger.WriteLog(level, "ResourceFunction: " + FunctionName, nameof(DistanceFunction));
            SeqLogger.Indent--;
        }
    }
}
