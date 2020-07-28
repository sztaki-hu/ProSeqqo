
using SequencePlanner.Phraser.Helper;
using SequencePlanner.Phraser.Template;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options
{
    public class CyclicSequence: Option
    {
        public bool Value { get; set; }

        public CyclicSequence()
        {
            Name = "CyclicSequence";
            IncludeableNames = new List<string> { };
            Need = true;
        }

        public override ValidationResult Validate()
        {
            try
            {
                if (ValueString.Count == 0)
                {
                    return new ValidationResult() { Validated = false };
                }
                Value = ValueString[1].ToUpper() == "TRUE";
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