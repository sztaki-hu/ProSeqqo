using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class EdgeWeightCalculator
    {
        public delegate double EdgeWeightFunction(List<double> a, List<double> b);
        private DistanceFunctionEnum CalcFunction { get; set; }
        private TrapezoidParams TrapezoidParam { get; set; }

        public EdgeWeightCalculator(DistanceFunctionEnum functionEnum)
        {
            CalcFunction = functionEnum;
        }

        public EdgeWeightCalculator(DistanceFunctionEnum functionEnum, TrapezoidParams trapezoidParams):this(functionEnum)
        {
            TrapezoidParam = trapezoidParams;
        }

        public double Calculate(List<double> a, List<double> b)
        {
            if (CalcFunction == DistanceFunctionEnum.Euclidian_Distance)
                return Euclidian_Distance(a,b);
            if (CalcFunction == DistanceFunctionEnum.Max_Distance)
                return Max_Distance(a, b);
            if (CalcFunction == DistanceFunctionEnum.Trapezoid_Time)
                return Trapezoid_Time(a, b);
            if (CalcFunction == DistanceFunctionEnum.Trapezoid_Time_WithTieBreaker)
                return Trapezoid_Time_WithTieBreaker(a, b);
            if (CalcFunction == DistanceFunctionEnum.Manhattan_Distance)
                return Manhattan_Distance(a,b);
            return 0.0;
        }

        private double Euclidian_Distance(List<double> a, List<double> b)
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

        public double Max_Distance(List<double> a, List<double> b)
        {
            if (a.Count == b.Count)
            {
                double max = 0;
                for (int i = 0; i < a.Count; i++)
                {
                    if (Math.Abs(a[i] - b[i]) > max)
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

        public double Trapezoid_Time(List<double> a, List<double> b)
        {
            return TrapezoidTimeCalculation(a, b, false);
        }

        public double Trapezoid_Time_WithTieBreaker(List<double> a, List<double> b)
        {
            return TrapezoidTimeCalculation(a, b, true);
        }

        public double Manhattan_Distance(List<double> a, List<double> b)
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

        private double TrapezoidTimeCalculation(List<double> a, List<double> b, bool withTieBreaker)
        {
            if (TrapezoidParam != null)
            {
                int Dimension = a.Count;
                if (b.Count == Dimension && TrapezoidParam.JointAcceleration.Length == Dimension && TrapezoidParam.JointSpeed.Length == Dimension)
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
                        if (angleDiff < TrapezoidParam.JointThresholdDist[i])
                            // Triangle profile
                            time = Math.Sqrt(4.0 * angleDiff / TrapezoidParam.JointAcceleration[i]);
                        else
                            // Trapezoid profile
                            time = TrapezoidParam.JointThresholdTime[i] + (angleDiff - TrapezoidParam.JointThresholdDist[i]) / TrapezoidParam.JointSpeed[i];
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
            Console.WriteLine("Implementation error: EdgeWeightCalculator.TrapezoidParams need to be set!");
            return 0.0;
        }
    }
}
