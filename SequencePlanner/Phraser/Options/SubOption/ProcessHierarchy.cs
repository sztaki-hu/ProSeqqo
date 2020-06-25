﻿using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;
using SequencePlanner.Phraser.Helper;

namespace SequencePlanner.Phraser.Options
{
    public class ProcessHierarchy : Option
    {
        public List<ProcessHierarchyOptionValue> Value { get;set; }

        public ProcessHierarchy()
        {
            Name = "ProcessHierarchy";
        }
        public override ValidationResult Validate()
        {
            try
            {
                Value = new List<ProcessHierarchyOptionValue>();
                for (int i = 1; i < ValueString.Count; i++)
                {
                    ProcessHierarchyOptionValue tmp = new ProcessHierarchyOptionValue();
                    tmp.FromString(ValueString[i]);
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
        public bool ValidateByValues(List<PositionOptionValue> values)
        {
            foreach (var item in values)
            {
                
            }
            return true;
        }
    }
}