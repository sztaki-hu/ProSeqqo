using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class TrapezoidParams
    {
        public double[] JointAcceleration;
        public double[] JointSpeed;
        public double[] JointThresholdTime;
        public double[] JointThresholdDist;

        public TrapezoidParams(double[] maxAcceleration, double[] maxSpeed)
        {
            JointAcceleration = maxAcceleration;
            JointSpeed = maxSpeed;
            int Dimension = maxSpeed.Length;
            JointThresholdTime = new double[Dimension];
            JointThresholdDist = new double[Dimension];

            if (JointAcceleration.Length == Dimension && JointSpeed.Length == Dimension)
            {
                for (int i = 0; i < Dimension; i++)
                {
                    if (JointAcceleration[i] == 0.0)
                    {
                        throw new DivideByZeroException("Trapezoid_Time Error: Acceleration is zero!");
                    }
                    else
                    {
                        JointThresholdDist[i] = JointSpeed[i] * JointSpeed[i] / JointAcceleration[i];
                        JointThresholdTime[i] = Math.Sqrt(4.0 * JointThresholdDist[i] / JointAcceleration[i]);
                    }
                }
            }
        }
    }
}
