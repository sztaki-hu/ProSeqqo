using SequencePlanner.Helper;
using SequencePlanner.Model;

namespace SequencePlanner.Function.DistanceFunction
{
    public abstract class DistanceFunction: IDistanceFunction
    {
        public string FunctionName { get; protected set; }

        public DistanceFunction()
        {

        }

        public abstract double ComputeDistance(Position A, Position B);
        public abstract void Validate();
        public void ToLog(LogLevel level)
        {
            SeqLogger.WriteLog(level, "DistanceFunction: "+FunctionName, nameof(DistanceFunction));
            SeqLogger.Indent++;
            SeqLogger.WriteLog(level, "FunctionName: "+FunctionName, nameof(DistanceFunction));

            SeqLogger.Indent--;
        }
    }
}