using ProSeqqoLib.Helper;
using ProSeqqoLib.Model.Hierarchy;

namespace ProSeqqoLib.Function.DistanceFunction
{
    public interface IDistanceFunction
    {
        public string FunctionName { get; }

        public double ComputeDistance(Config A, Config B);
        public void Validate();
        public void ToLog(LogLevel level);
    }
}