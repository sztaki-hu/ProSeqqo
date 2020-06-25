
using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;
using SequencePlanner.Phraser.Helper;

namespace SequencePlanner.Phraser.Options
{
    public class StartDepot: Option
    {
        public int Value { get; set; }

        public StartDepot()
        {
            Name = "StartDepot";
        }

        public override ValidationResult Validate()
        {
            Value = -1;
            try
            {
                Value = int.Parse(ValueString[1]);
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
        public bool ValidateByValue(List<PositionOptionValue> values)
        {
            foreach (var item in values)
            {
                if (item.ID == Value)
                {
                    return true;
                }
            }
            return false;
        }
    }
}