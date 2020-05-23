using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options
{
    public class ProcessHierarchy : Option
    {
        public List<ProcessHierarchyOptionValue> Value { get;set; }
        public override ValidationResult Validate()
        {
            try
            {
                Value = new List<ProcessHierarchyOptionValue>();
                for (int i = 1; i < ValueString.Count; i++)
                {
                    ProcessHierarchyOptionValue tmp = new ProcessHierarchyOptionValue();
                    tmp.fromString(ValueString[i]);
                    Value.Add(tmp);
                }
                Validated = true;
                return new ValidationResult() { Validated = true };
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in validation: " + this.GetType().Name + " " + e.Message);
                return null;
            }
        }
    }
}