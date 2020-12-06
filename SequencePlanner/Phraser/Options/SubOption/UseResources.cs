
using SequencePlanner.Phraser.Helper;
using SequencePlanner.Phraser.Template;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options
{
    public class UseResources: Option
    {
        public bool Value { get; set; }

        public UseResources()
        {
            Name = "UseResources";
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

                string tmp = ValueString[1].ToUpper();
                List<string> newInclude = new List<string>();
                List<string> newIncludeOptional = new List<string>();
                switch (tmp)
                {
                    case "TRUE":
                        Value = true;
                        newInclude.Add("ResourceCostMethod");
                        newIncludeOptional.Add("ResourceCostMatrix");
                        newIncludeOptional.Add("ResourceCostValue");
                        break;
                    case "FALSE":
                        Value = false;
                        break;
                    default:
                        throw new SequencerException("UseResource parameter unknown!", "Make sure UseResources: True/False one of them given.");
                }
                Validated = true;
                return new ValidationResult()
                {
                    Validated = true,
                    NewIncludeNeed = newInclude,
                    NewIncludeOptional = newIncludeOptional,
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