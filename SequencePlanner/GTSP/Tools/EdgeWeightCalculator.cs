using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class EdgeWeightCalculator
    {
        public delegate double EdgeWeightFunction(List<double> a, List<double> b);
        public DistanceFunctionEnum CalcFunction { get; set; }
        private TrapezoidParams TrapezoidParam { get; set; }
        private PositionMatrixOptionValue MatrixValue { get; set; }

        public EdgeWeightCalculator(DistanceFunctionEnum functionEnum)
        {
            CalcFunction = functionEnum;
        }

        public EdgeWeightCalculator(DistanceFunctionEnum functionEnum, TrapezoidParams trapezoidParams):this(functionEnum)
        {
            TrapezoidParam = trapezoidParams;
        }

        public EdgeWeightCalculator(DistanceFunctionEnum functionEnum, PositionMatrixOptionValue matrix) : this(functionEnum)
        {
            MatrixValue = matrix;
        }

        public double Calculate(Position a, Position b)
        {
            if(CalcFunction == DistanceFunctionEnum.FullMatrix)
            {
               return Calculate(a.UID, b.UID);
            }
            else
            {
               return Calculate(a.Configuration, b.Configuration);
            }

        }
        private double Calculate(List<double> a, List<double> b)
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
            if (CalcFunction == DistanceFunctionEnum.FullMatrix)
            {
                Console.WriteLine("EdgeWeightCalculator: FullMatrix, use Calculate with parameter (int AID, int BID)!");
                return 0.0;
            }
                
            return 0.0;
        }

        private double Calculate(int AUID, int BUID)
        {
            int aIndex = -1;
            int bIndex = -1;
            for (int i = 0; i < MatrixValue.ID.Count; i++)
            {
                if (MatrixValue.ID[i] == AUID)
                    aIndex = i;

                if (MatrixValue.ID[i] == BUID)
                    bIndex = i;
            }

            if(aIndex==-1 || bIndex == -1)
            {
                Console.WriteLine("EdgeWeightCalculator: Get distance from matrix by ID: ID not found!");
                return 0.0;
            }
            else
            {
                return MatrixValue.Matrix[aIndex, bIndex];
            }

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
        private double Max_Distance(List<double> a, List<double> b)
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
        private double Trapezoid_Time(List<double> a, List<double> b)
        {
            return TrapezoidTimeCalculation(a, b, false);
        }
        private double Trapezoid_Time_WithTieBreaker(List<double> a, List<double> b)
        {
            return TrapezoidTimeCalculation(a, b, true);
        }
        private double Manhattan_Distance(List<double> a, List<double> b)
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
