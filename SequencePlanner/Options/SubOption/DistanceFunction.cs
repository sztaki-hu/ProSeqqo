using SequencePlanner.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Options
{
    public class DistanceFunction : Option<bool>
    {

        public DistanceFunction()
        {
            Name = nameof(DistanceFunction);

        }
    }
}
