using ProSeqqoLib.Helper;
using ProSeqqoLib.Model.Hierarchy;

namespace ProSeqqoLib.Function.DistanceFunction
{
    public abstract class DistanceFunction : IDistanceFunction
    {
        public virtual string FunctionName { get; }


        public abstract double ComputeDistance(Config A, Config B);
        public virtual void Validate() { }
        public void ToLog(LogLevel level)
        {
            SeqLogger.Indent++;
            SeqLogger.WriteLog(level, "DistanceFunction: " + FunctionName, nameof(DistanceFunction));
            SeqLogger.Indent--;
        }
    }
}