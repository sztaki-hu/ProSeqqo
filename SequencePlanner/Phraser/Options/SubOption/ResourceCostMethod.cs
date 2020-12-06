
using SequencePlanner.Phraser.Helper;
using SequencePlanner.Phraser.Options.Values;
using SequencePlanner.Phraser.Template;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options
{
    public class ResourceCostMethod: Option
    {
        public ResourceCostMethodEnum Value { get; set; }

        public ResourceCostMethod()
        {
            Name = "ResourceCostMethod";
            IncludeableNames = new List<string> { };
        }

        public override ValidationResult Validate()
        {
            try
            {
                if (ValueString.Count == 0)
                {
                    return new ValidationResult() { Validated = false };
                }
                string tmp = ValueString[1].ToUpper();
                List<string> newInclude = new List<string>();
                switch (tmp)
                {
                    case "ADD":
                        Value = ResourceCostMethodEnum.Add;
                        break;
                    case "MAX":
                        Value = ResourceCostMethodEnum.Max;
                        break;
                    default:
                        throw new SequencerException("ResourceCostMethod parameter unknown!", "Make sure ResourceCostMethod: Add/Max one of them given.");
                }
                Validated = true;
                return new ValidationResult()
                {
                    Validated = this.Validated
                };
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