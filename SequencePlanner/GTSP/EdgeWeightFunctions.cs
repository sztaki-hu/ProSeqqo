using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class EdgeWeightFunctions
    {
        public delegate double EdgeWeightFunction(List<double> a, List<double> b, params object[] param);
        public static double[] JointAcceleration;
        public static double[] JointSpeed;
        public static double[] JointThresholdTime;
        public static double[] JointThresholdDist;

        public static double Euclidian_Distance(List<double> a, List<double> b, params object[] param)
        {
            if (a.Count == b.Count)
            {
                double tmp = 0;
                for (int i = 0; i < a.Count; i++)
                {
                    tmp += (a[i] - b[i]) * (a[i] - b[i]);
                }
                return Math.Sqrt(tmp);
            }
            else
            {
                Console.WriteLine("EdgeWeightFunctions:Euclidian_Distance find dimension mismatch!");
                return 0;
            }
        }

        public static double Max_Distance(List<double> a, List<double> b, params object[] param)
        {
            if (a.Count == b.Count)
            {
                double max = 0;
                for (int i = 0; i < a.Count; i++)
                {
                    if(Math.Abs(a[i] - b[i]) > max)
                    {
                        max = Math.Abs(a[i] - b[i]);
                    }
                }
                return max;
            }
            else
            {
                Console.WriteLine("EdgeWeightFunctions:Euclidian_Distance find dimension mismatch!");
                return 0;
            }
        }

        public static void setTrapezoidParam(double[] maxAcc, double[] maxSpeed)
        {
            JointAcceleration = maxAcc;
            JointSpeed = maxSpeed;
            int Dimension = maxAcc.Length;
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


        public static double Trapezoid_Time(List<double> a, List<double> b, params object[] param)
        {
            int Dimension = a.Count;

            if (b.Count == Dimension && JointAcceleration.Length == Dimension && JointSpeed.Length == Dimension)
            { 
                double maxTime = 0;
                double sumTime = 0;
                // Max of joint travel times
                for (int i = 0; i < Dimension; i++)
                {
                    double angleDiff = Math.Abs(a[i] - b[i]);
                    // This version is for the 2*PI simmetry
                    //double angleDiff = min(abs(c1.x[i] - c2.x[i]), min(abs(c1.x[i] - c2.x[i] + TWOPI), abs(c1.x[i] - c2.x[i] - TWOPI)));
                    double time;
                    if (angleDiff < JointThresholdDist[i])
                        // Triangle profile
                        time = Math.Sqrt(4.0 * angleDiff / JointAcceleration[i]);
                    else
                        // Trapezoid profile
                        time = JointThresholdTime[i] + (angleDiff - JointThresholdDist[i]) / JointSpeed[i];
                    if (maxTime < time)
                        maxTime = time;
                    sumTime += time;
                }
                return maxTime;// + 0.01 * (sumTime / Dimension);
            }
            else
            {
                Console.WriteLine("EdgeWeightFunctions:Trapezoid_Time find dimension mismatch!");
                return 0;
            }
        }

        public static double Manhattan_Distance(List<double> a, List<double> b, params object[] param)
        {
            if (a.Count == b.Count)
            {
                double tmp = 0;
                for (int i = 0; i < a.Count; i++)
                {
                    tmp += Math.Abs(a[i] - b[i]);
                }
                return tmp;
            }
            else
            {
                Console.WriteLine("EdgeWeightFunctions:Manhattan_Distance find dimension mismatch!");
                return 0;
            }
        }

        public static EdgeWeightFunction toFunction(DistanceFunctionEnum functionEnum)
        {
            if (functionEnum == DistanceFunctionEnum.Euclidian_Distance)
                return Euclidian_Distance;
            if (functionEnum == DistanceFunctionEnum.Max_Distance)
                return Max_Distance;
            if (functionEnum == DistanceFunctionEnum.Trapezoid_Time)
                return Trapezoid_Time;
            if (functionEnum == DistanceFunctionEnum.Manhattan_Distance)
                return Manhattan_Distance;
            return null;
        }
    }
}
