using SequencePlanner.Model;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;
using SequencePlanner.Helper;

namespace SequencePlanner.Function.ResourceFunction
{
    public class NoResourceFunction : IResourceFunction
    {
        public string FunctionName { get { return "NoResource"; } }
        public IResourceDistanceLinkFunction LinkingFunction { get; set; }

        public double ComputeResourceCost(Position A, Position B, double distance) => distance;
        public void Validate(){}
        public void ToLog(LogLevel level)
        {
            SeqLogger.Indent++;
            SeqLogger.WriteLog(level, "ResourceFunction: " + FunctionName, nameof(DistanceFunction));
            SeqLogger.Indent--;
        }
    }
}