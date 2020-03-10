using SequencePlanner.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Options
{
    public class Option : IOption
    {
        public string Name { get; protected set; }
        public IList<Enum> PosibbleValues { get; protected set; }
        public Enum Value { get; set; }
        public bool Required { get; protected set; }
        public bool Incluided { get; set; }
        public bool Validated { get; set; }

        public Option()
        {
            Name = "";
            PosibbleValues = new List<Enum>();
            Value = ValueEnum.Missing.Missing;
            Required = false;
            Validated = false;
            Incluided = false;
        }
        public bool IsNameFits(string name)
        {
            return (Name == name);
        }

        public bool IsValueFits(IValue value)
        {
            foreach (var item in PosibbleValues)
            {
                if (item == value)
                    return true;
            }
            return false;
        }

    }
}
