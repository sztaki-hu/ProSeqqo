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
        public bool Required { get; protected set; }
        public bool Validated { get; set; }
        public bool Incluided { get; set; }

        public Option()
        {
            Name = "";
            Required = false;
            Validated = false;
            Incluided = false;
        }
        public bool IsNameFits(string name)
        {
            return (Name == name);
        }

        public bool IsValueFits(string value)
        {
            throw new NotImplementedException();
        }

        public bool Validate(string value)
        {
            throw new NotImplementedException();
        }
    }
}
