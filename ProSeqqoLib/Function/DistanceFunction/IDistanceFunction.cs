using SequencePlanner.Helper;
using SequencePlanner.Model.Hierarchy;

namespace SequencePlanner.Function.DistanceFunction
{
    public interface IDistanceFunction
    {
        public string FunctionName { get; }

        public double ComputeDistance(Config A, Config B);
        public void Validate();
        public void ToLog(LogLevel level);
    }
}