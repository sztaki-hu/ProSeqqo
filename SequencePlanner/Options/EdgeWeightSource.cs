using SequencePlanner.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Options
{
    public class EdgeWeightSource : Option
    {

        public EdgeWeightSource()
        {
            Name = nameof(EdgeWeightSource);
            Required = true;
            Incluided = true;
            PosibbleValues = new List<Enum>();
            PosibbleValues.Add(ValueEnum.EdgeWeightSource.Calculate);
            PosibbleValues.Add(ValueEnum.EdgeWeightSource.Matrix);
        }
    }
}
