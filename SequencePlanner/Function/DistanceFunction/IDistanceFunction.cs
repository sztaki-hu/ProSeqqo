using SequencePlanner.GeneralModels;
using SequencePlanner.Helper;

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