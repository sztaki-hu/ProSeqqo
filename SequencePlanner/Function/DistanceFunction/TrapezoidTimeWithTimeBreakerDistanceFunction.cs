using SequencePlanner.Model;

namespace SequencePlanner.Function.DistanceFunction
{
    public class TrapezoidTimeWithTimeBreakerDistanceFunction : TrapezoidTimeDistanceFunction
    {
        public string FunctionName { get { return "TrapezoidTimeWithTimeBreaker"; } }
        public TrapezoidTimeWithTimeBreakerDistanceFunction(double[] maxAcceleration, double[] maxSpeed): base(maxAcceleration, maxSpeed)
        {
        }
        
        public override double ComputeDistance(Position A, Position B)
        {
            return TrapezoidTimeCalculation(A, B, true);
        }
    }
}
