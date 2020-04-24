using SequencePlanner.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Options
{
    public class Option<T> : IOption
    {
        public string Name { get; protected set; }
        public T Value { get; set; }

        public Option()
        {
            Name = "";
        }
        
        public bool Validate(List<string> value)
        {
            throw new NotImplementedException();
        }
    }
}
