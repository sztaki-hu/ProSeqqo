using System;
using System.Collections.Generic;
using System.Text;
using SequencePlanner.Phraser.Helper;
using SequencePlanner.Phraser.Template;

namespace SequencePlanner.Phraser.Options
{
    public class TimeLimit: Option
    {
        public int Value { get; set; }

        public TimeLimit()
        {
            Name = "TimeLimit";
            IncludeableNames = new List<string> { };
            Need = false;
        }
        public override ValidationResult Validate()
        {
            try
            {
                if (ValueString.Count == 0)
                {
                    return new ValidationResult() { Validated = false };
                }
                Value = int.Parse(ValueString[1]);
                Validated = true;
                return new ValidationResult() { Validated = true };
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