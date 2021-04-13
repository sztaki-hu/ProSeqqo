using SequencePlanner.Model;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;
using SequencePlanner.Helper;

namespace SequencePlanner.Function.ResourceFunction

{
    public interface IResourceFunction
    {
        public string FunctionName { get; }
        public IResourceDistanceLinkFunction LinkingFunction { get; set; }


        public double ComputeResourceCost(Position A, Position B, double distance);
        public void Validate();
        public void ToLog(LogLevel level);
    }
}