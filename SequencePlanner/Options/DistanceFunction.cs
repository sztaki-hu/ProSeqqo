using SequencePlanner.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Options
{
    public class DistanceFunction : Option
    {

        public DistanceFunction()
        {
            Name = nameof(DistanceFunction);
            Required = false;
            Incluided = false;
            PosibbleValues = new List<Enum>();
            PosibbleValues.Add(ValueEnum.DistanceFunction.Euclidian_Distance);
            PosibbleValues.Add(ValueEnum.DistanceFunction.Manhattan_Distance);
            PosibbleValues.Add(ValueEnum.DistanceFunction.Max_Distance);
            PosibbleValues.Add(ValueEnum.DistanceFunction.Trapezoid_Time);
        }
    }
}
