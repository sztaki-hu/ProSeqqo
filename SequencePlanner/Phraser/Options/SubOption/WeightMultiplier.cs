using SequencePlanner.Phraser.Helper;
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
                if (ValueString[1].Equals("Auto"))
                {
                    Value = -1;
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
                if (SequencerTask.DEBUG)
                    Console.WriteLine("Error in validation: " + this.GetType().Name + " " + e.Message);
                return null;
            }
        }
    }
}