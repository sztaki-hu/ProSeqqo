using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options.Values;
using SequencePlanner.Phraser.Template;
using System;
using System.Collections.Generic;
using System.Text;
using SequencePlanner.Phraser.Helper;

namespace SequencePlanner.Phraser.Options
{
    public class PositionMatrix : Option
    {
        public PositionMatrixOptionValue Value { get; set; }

        public PositionMatrix()
        {
            Name = "PositionMatrix";
        }
        public override ValidationResult Validate()
        {
            try
            {
                if (ValueString.Count == 0)
                {
                    return new ValidationResult() { Validated = false };
                }
                Value = new PositionMatrixOptionValue();
                Value.fromString(ValueString);
                Validated = true;
                return new ValidationResult() { Validated = true };
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