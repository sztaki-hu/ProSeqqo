using SequencePlanner.Helper;
using SequencePlanner.Model;

namespace SequencePlanner.Function.DistanceFunction
{
    public abstract class DistanceFunction: IDistanceFunction
    {
        public virtual string FunctionName { get; }


        public abstract double ComputeDistance(Position A, Position B);
        public virtual void Validate() { }
        public void ToLog(LogLevel level)
        {
            SeqLogger.WriteLog(level, "DistanceFunction: "+FunctionName, nameof(DistanceFunction));
            SeqLogger.Indent++;
            SeqLogger.WriteLog(level, "FunctionName: "+FunctionName, nameof(DistanceFunction));
            SeqLogger.Indent--;
        }
    }
}