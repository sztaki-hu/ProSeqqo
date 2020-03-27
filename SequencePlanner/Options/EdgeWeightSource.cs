using SequencePlanner.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Options
{
    public class EdgeWeightSource : Option<ValueEnum.EdgeWeightSource>
    {

        public EdgeWeightSource()
        {
            Name = nameof(EdgeWeightSource);
            Required = true;
            Incluided = true;
        }
    }
}
