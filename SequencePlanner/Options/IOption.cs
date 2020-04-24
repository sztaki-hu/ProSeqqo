using SequencePlanner.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Options
{
    public interface IOption
    {
        string Name { get; }
        bool Validate(List<string> value);
    }
}
