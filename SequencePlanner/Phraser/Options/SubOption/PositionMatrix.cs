using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options
{
    public class PositionMatrix : Option
    {
        public PositionMatrixOptionValue Value { get; set; }

        public override ValidationResult Validate()
        {
            try
            {
                Value = new PositionMatrixOptionValue();
                Value.fromString(ValueString);
                Validated = true;
                return new ValidationResult() { Validated = true };
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