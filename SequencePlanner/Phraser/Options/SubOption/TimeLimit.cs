﻿using System;
using System.Collections.Generic;
using System.Text;
using SequencePlanner.Phraser.Helper;

namespace SequencePlanner.Phraser.Options
{
    public class TimeLimit: Option
    {
        public int Value { get; set; }

        public TimeLimit()
        {
            Name = "TimeLimit";
            IncludeableNames = new List<string> { };
            Need = true;
        }
        public override ValidationResult Validate()
        {
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
    }
}