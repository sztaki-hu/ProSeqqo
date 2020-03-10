using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Options.Values
{
    public static class ValueEnum
    {
        public enum TaskType
        {
            PointLike,
            LineLike
        }

        public enum EdgeWeightSource
        {
            Matrix,
            Calculate
        }

        public enum DistanceFunction
        {
            Euclidian_Distance,
            Max_Distance,
            Trapezoid_Time,
            Manhattan_Distance
        }

        public enum WeightMultiplyer
        {
            Auto
        }

        public enum Missing
        {
            Missing
        }

    }
}
