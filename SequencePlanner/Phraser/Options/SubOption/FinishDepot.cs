
using SequencePlanner.Phraser.Helper;
using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options
{
    public class FinishDepot: Option
    {
        public int Value { get; set; }

        public FinishDepot()
        {
            Name = "FinishDepot";
        }

        public override ValidationResult Validate()
        {
            Value = -1;
            try
            {
                if (ValueString.Count == 0)
                {
                    return new ValidationResult() { Validated = false };
                }
                Value = int.Parse(ValueString[1]);
                Validated = true;
                return new ValidationResult() { Validated = true};
            }
            catch (Exception e)
            {
                Validated = false;
                if (SeqGTSPTask.DEBUG)
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