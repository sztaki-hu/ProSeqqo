
using SequencePlanner.Phraser.Helper;
using SequencePlanner.Phraser.Options.Values;
using SequencePlanner.Phraser.Template;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options
{
    public class BidirectionLineDefault : Option
    {
        public bool Value { get; set; }

        public BidirectionLineDefault()
        {
            Name = "BidirectionLineDefault";
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
                if (ValueString[1].ToUpper().Equals("TRUE"))
                {
                    LineListOptionValue.BidirectionalDefault = true;
                    Validated = true;
                }
                if (ValueString[1].ToUpper().Equals("FALSE"))
                {
                    LineListOptionValue.BidirectionalDefault = false;
                    Validated = true;
                }
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