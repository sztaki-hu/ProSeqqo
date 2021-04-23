using SequencePlanner.Helper;
using SequencePlanner.Model.Hierarchy;
using System;

namespace SequencePlanner.Function.DistanceFunction
{
    public class TrapezoidTimeDistanceFunction : DistanceFunction
    {
        public override string FunctionName { get { return "TrapezoidTime"; } }
        public double[] MaxAcceleration { get; set; }
        public double[] MaxSpeed { get; set; }
        protected double[] JointThresholdTime;
        protected double[] JointThresholdDist;


        public TrapezoidTimeDistanceFunction(double[] maxAcceleration, double[] maxSpeed) : base()
        {
            MaxAcceleration = maxAcceleration;
            MaxSpeed = maxSpeed;
            InitParameters();
            Validate();
        }


        public override double ComputeDistance(Config A, Config B)
        {
            return TrapezoidTimeCalculation(A, B, false);
        }

        public override void Validate()
        {
            if (MaxAcceleration.Length != MaxSpeed.Length)
                throw new SeqException("MaxDistanceFunction found dimendion mismatch!", "Check dimension of MaxAcceleration/Speed.");
            if (JointThresholdTime.Length != JointThresholdDist.Length)
                throw new SeqException("MaxDistanceFunction found dimendion mismatch!", "Check dimension of JointThresholdTime/JointThresholdDist.");
            if (JointThresholdTime.Length != MaxAcceleration.Length)
                throw new SeqException("MaxDistanceFunction found dimendion mismatch!", "Check dimension of MaxAcceleration/JointThresholdTime.");
            for (int i = 0; i < MaxSpeed.Length; i++)
            {
                if(MaxAcceleration[i]<=0)
                    throw new SeqException("MaxAcceleration contains >=0 !");
                if(MaxSpeed[i]<=0)
                    throw new SeqException("MaxSpeed contains >=0 !");
                if(JointThresholdTime[i]<=0)
                    throw new SeqException("JointThresholdTime contains >=0 !");
                if(JointThresholdDist[i]<=0)
                    throw new SeqException("JointThresholdDist contains >=0 !");
            }
        }

        protected double TrapezoidTimeCalculation(Config A, Config B, bool withTieBreaker)
        {
            if (A == null || B == null)
                throw new SeqException("TrapezoidTimeDistanceFunction A/B position null!");
            if (A.Configuration.Count != B.Configuration.Count)
                throw new SeqException("TrapezoidTimeDistanceFunction found dimendion mismatch!", "Check dimension of Positions with UserID:" + A.ID + ", " + B.ID);
            int Dimension = A.Configuration.Count;
            if (B.Configuration.Count == Dimension && MaxAcceleration.Length == Dimension && MaxSpeed.Length == Dimension)
            {
                double maxTime = 0;
                double sumTime = 0;
                // Max of joint travel times
                for (int i = 0; i < Dimension; i++)
                {
                    double angleDiff = Math.Abs(A.Configuration[i] - B.Configuration[i]);
                    // This version is for the 2*PI simmetry
                    //double angleDiff = min(abs(c1.x[i] - c2.x[i]), min(abs(c1.x[i] - c2.x[i] + TWOPI), abs(c1.x[i] - c2.x[i] - TWOPI)));
                    double time;
                    if (angleDiff < JointThresholdDist[i])
                        // Triangle profile
                        time = Math.Sqrt(4.0 * angleDiff / MaxAcceleration[i]);
                    else
                        // Trapezoid profile
                        time = JointThresholdTime[i] + (angleDiff - JointThresholdDist[i]) / MaxSpeed[i];
                    if (maxTime < time)
                        maxTime = time;
                    sumTime += time;
                }
                if (withTieBreaker)
                    return maxTime + 0.01 * (sumTime / Dimension);
                else
                    return maxTime;
            }
            else
            {
                Console.WriteLine("EdgeWeightFunctions:Trapezoid_Time dimension mismatch!");
                return 0.0;
            }
        }

        private void InitParameters()
        {
            if(MaxAcceleration.Length != MaxSpeed.Length)
                throw new SeqException("MaxDistanceFunction found dimendion mismatch!", "Check dimension of MaxAcceleration/Speed.");
            int Dimension = MaxSpeed.Length;
            JointThresholdTime = new double[Dimension];
            JointThresholdDist = new double[Dimension];

            if (MaxAcceleration.Length == Dimension && MaxSpeed.Length == Dimension)
            {
                for (int i = 0; i < Dimension; i++)
                {
                    if (MaxAcceleration[i] == 0.0)
                    {
                        throw new DivideByZeroException("Trapezoid_Time Error: Acceleration is zero!");
                    }
                    else
                    {
                        JointThresholdDist[i] = MaxSpeed[i] * MaxSpeed[i] / MaxAcceleration[i];
                        JointThresholdTime[i] = Math.Sqrt(4.0 * JointThresholdDist[i] / MaxAcceleration[i]);
                    }
                }
            }
        }
    }
}