using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options
{
    public interface IOption
    {
        string Name { get; }
        bool Validate(List<string> value);
    }
}
