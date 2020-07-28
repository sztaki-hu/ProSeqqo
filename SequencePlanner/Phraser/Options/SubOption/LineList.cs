
using SequencePlanner.Phraser.Options.Values;
using SequencePlanner.Phraser.Template;
using System;
using System.Collections.Generic;
using System.Text;
using SequencePlanner.Phraser.Helper;

namespace SequencePlanner.Phraser.Options
{
    public class LineList : Option
    {
        public List<LineListOptionValue> Value { get; set; }

        public LineList()
        {
            Name = "LineList";
        }
        public override ValidationResult Validate()
        {
            try
            {
                if (ValueString.Count == 0)
                {
                    return new ValidationResult() { Validated = false };
                }
                Value = new List<LineListOptionValue>();
                for (int i = 1; i < ValueString.Count; i++)
                {
                    LineListOptionValue tmp = new LineListOptionValue();
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

        public void ValidateByValue()
        {

        }

    }
}