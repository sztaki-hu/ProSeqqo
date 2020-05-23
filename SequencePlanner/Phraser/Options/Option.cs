using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options
{
    public abstract class Option
    {
        public String Name { get; set; }
        public List<String> ValueString { get; set; }
        public List<String> IncludeableNames { get; set; }
        public List<String> IncludedNames { get; set; }

        public bool Need { get; set; }
        public bool Included { get; set; }
        public bool Validated { get; set; }

        public Option()
        {
            ValueString = new List<string>();
            IncludeableNames = new List<string>();
            Need = false;
        }

        public abstract ValidationResult Validate();

        public override string ToString()
        {
            var tmp = "";
            foreach (var item in ValueString)
            {
                tmp += item + "\n";
            }
            return "Option name: " + Name + "\n Values: \n" + tmp;
        }
    }
}