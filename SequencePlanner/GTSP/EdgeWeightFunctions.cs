using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class EdgeWeightFunctions
    {
        public delegate double EdgeWeightFunction(List<double> a, List<double> b, params object[] param);

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

        public static double Trapezoid_Time(List<double> a, List<double> b, params object[] param)
        {
            if (a.Count == b.Count)
            {
                Console.WriteLine("EdgeWeightFunctions:Trapezoid_Time not implemented yet!");
                return 0;
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
