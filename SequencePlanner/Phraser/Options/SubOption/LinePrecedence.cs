
using SequencePlanner.Phraser.Helper;
using SequencePlanner.Phraser.Options.Values;
using SequencePlanner.Phraser.Template;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options
{
    public class LinePrecedence : Option
    {
        public List<PrecedenceOptionValue> Value { get; set; }

        public LinePrecedence()
        {
            Name = "LinePrecedence";
        }
        public override ValidationResult Validate()
        {
            try
            {
                if (ValueString.Count == 0)
                {
                    return new ValidationResult() { Validated = false };
                }
                Value = new List<PrecedenceOptionValue>();
                for (int i = 1; i < ValueString.Count; i++)
                {
                    PrecedenceOptionValue tmp = new PrecedenceOptionValue();
                    tmp.fromString(ValueString[i]);
                    Value.Add(tmp);
                    if (!Validated)
                        Validated = true;
                }
                return new ValidationResult() { Validated = Validated };
            }
            catch (Exception e)
            {
                Validated = false;
                if (TemplateManager.DEBUG)
                    Console.WriteLine("Error in validation: " + this.GetType().Name + " " + e.Message);
                return null;
            }
        }
    }
}