﻿using SequencePlanner.Phraser.Helper;
using SequencePlanner.Phraser.Options.Values;
using SequencePlanner.Phraser.Template;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options
{
    public class PositionList : Option
    {
        public new List<PositionOptionValue> Value { get; set; }

        public PositionList()
        {
            Name = "PositionList";
        }
        public override ValidationResult Validate()
        {
            try
            {
                if (ValueString.Count == 0)
                {
                    return new ValidationResult() { Validated = false };
                }
                Value = new List<PositionOptionValue>();
                for (int i = 1; i < ValueString.Count; i++)
                {
                    PositionOptionValue tmp = new PositionOptionValue();
                    tmp.fromString(ValueString[i]);
                    Value.Add(tmp);
                    if (!Validated)
                        Validated = true;
                }
                
                return new ValidationResult() { Validated = Validated };
            }
            catch (Exception e)
            {
                Validated = false;
                if (TemplateManager.DEBUG)
                    Console.WriteLine("Error in validation: "+ this.GetType().Name+" " + e.Message);
                return null;
            }
        }

        public bool ValidateByValues(int Dimension)
        {
            foreach (var item in Value)
            {
                if (item.Dim != Dimension)
                {
                    return false;
                }
            }
            return true;
        }
    }
}