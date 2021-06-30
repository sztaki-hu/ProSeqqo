using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;
using SequencePlanner.Helper;
using SequencePlanner.Model.Hierarchy;

namespace SequencePlanner.Function.ResourceFunction
{
    public class NoResourceFunction : IResourceFunction
    {
        public string FunctionName { get { return "None"; } }
        public IResourceDistanceLinkFunction LinkingFunction { get; set; }

        public double ComputeResourceCost(Config A, Config B, double distance) => distance;
        public double GetResourceCost(Config A, Config B) => 0;
        public void Validate()
        {
            SeqLogger.Debug("ResourceFunction: " + FunctionName);
        }
        public void ToLog(LogLevel level)
        {
            SeqLogger.Indent++;
            SeqLogger.WriteLog(level, "ResourceFunction: " + FunctionName, nameof(NoResourceFunction));
            SeqLogger.Indent--;
        }
    }
}