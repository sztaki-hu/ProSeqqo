using SequencePlanner.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Options
{
    public interface IOption
    {

        bool Validated { get; set; }
        string Name { get; }
        bool Required { get; }
        bool Incluided { get; set; }

        bool IsNameFits(string name);
        bool IsValueFits(String value);
        bool Validate(String value);
    }
}
