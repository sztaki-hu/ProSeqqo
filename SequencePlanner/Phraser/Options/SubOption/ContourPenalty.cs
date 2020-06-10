
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options
{
    public class ContourPenalty: Option
    {
        public int Value { get; set; }

        public override ValidationResult Validate()
        {
            try
            {
                Value = int.Parse(ValueString[1]);
                Validated = true;
                return new ValidationResult() { Validated = true };
            }
            catch(Exception e)
            {
                Validated = false;
                if (SequencerTask.DEBUG)
                    Console.WriteLine("Error in validation: " + this.GetType().Name + " " + e.Message);
                return null;
            }
        }
    }
}