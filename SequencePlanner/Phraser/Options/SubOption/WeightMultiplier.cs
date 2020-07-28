using SequencePlanner.Phraser.Helper;
using SequencePlanner.Phraser.Template;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options
{
    public class WeightMultiplier: Option
    {
        public int Value { get; set; }

        public WeightMultiplier()
        {
            Name = "WeightMultiplier";
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
                if (ValueString[1].Equals("Auto"))
                {
                    Value = -1;
                    Validated = true;
                }
                else
                {
                    Value = int.Parse(ValueString[1]);
                    Validated = true;
                }
                return new ValidationResult() { Validated = true};
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