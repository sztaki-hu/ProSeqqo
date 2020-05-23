using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options.Values
{
    public enum TaskTypeEnum
    {
        Line_Like,
        Point_Like
    }
    public enum EdgeWeightSourceEnum
    {
        FullMatrix,
        CalculateFromPositions
    }
    public enum DistanceFunctionEnum
    {
        Euclidian_Distance,
        Max_Distance,
        Trapezoid_Time,
        Manhattan_Distance
    }
}
