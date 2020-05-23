using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options
{
    public class PositionList : Option
    {
        public new List<PositionOptionValue> Value { get; set; }

        public override ValidationResult Validate()
        {
            try
            {
                Value = new List<PositionOptionValue>();
                for (int i = 1; i < ValueString.Count; i++)
                {
                    PositionOptionValue tmp = new PositionOptionValue();
                    tmp.fromString(ValueString[i]);
                    Value.Add(tmp);
                }
                Validated = true;
                return new ValidationResult() { Validated = true };
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in validation: "+ this.GetType().Name+" " + e.Message);
                return null;
            }
        }
    }
}