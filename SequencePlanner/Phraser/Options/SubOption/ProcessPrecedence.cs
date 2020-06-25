using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options
{
    public class ProcessPrecedence : Option
    {
        public List<PrecedenceOptionValue> Value { get; set; }
        public ProcessPrecedence()
        {
            Name = "ProcessPrecedence";
        }
        public override ValidationResult Validate()
        {
            try
            {
                Value = new List<PrecedenceOptionValue>();
                for (int i = 1; i < ValueString.Count; i++)
                {
                    PrecedenceOptionValue tmp = new PrecedenceOptionValue();
                    tmp.fromString(ValueString[i]);
                    Value.Add(tmp);
                }
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