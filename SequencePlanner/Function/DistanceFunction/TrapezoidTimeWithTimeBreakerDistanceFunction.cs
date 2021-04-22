using SequencePlanner.GeneralModels;

namespace SequencePlanner.Function.DistanceFunction
{
    public class TrapezoidTimeWithTimeBreakerDistanceFunction : TrapezoidTimeDistanceFunction
    {
        public override string FunctionName { get { return "TrapezoidTimeWithTimeBreaker"; } }


        public TrapezoidTimeWithTimeBreakerDistanceFunction(double[] maxAcceleration, double[] maxSpeed): base(maxAcceleration, maxSpeed){}
        

        public override double ComputeDistance(Config A, Config B)
        {
            return TrapezoidTimeCalculation(A, B, true);
        }
    }
}
