
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options
{
    public class CyclicSequence: Option
    {
        public bool Value { get; set; }

        public override ValidationResult Validate()
        {
            try
            {
                Value = ValueString[1].ToUpper() == "TRUE";
                Validated = true;
                return new ValidationResult() { Validated = true };
            }
            catch (Exception e)
            {
                if (SequencerTask.DEBUG)
                    Console.WriteLine("Error in validation: " + this.GetType().Name + " " + e.Message);
                return null;
            }
        }
    }
}