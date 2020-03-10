using SequencePlanner.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Options
{
    public interface IOption
    {
        string Name { get; }
        Enum Value { get; }
        IList<Enum> PosibbleValues { get; }
        bool Required { get; }
        bool Incluided { get; set; }


        bool IsNameFits(string name);
        bool IsValueFits(IValue value);
    }
}
