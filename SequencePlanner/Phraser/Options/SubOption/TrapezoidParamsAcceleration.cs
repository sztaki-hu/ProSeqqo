using System;
using System.Collections.Generic;
using System.Text;
using SequencePlanner.Phraser.Helper;

namespace SequencePlanner.Phraser.Options
{
    public class TrapezoidParamsAcceleration : Option
    {
        public List<double> Value { get; set; }

        public TrapezoidParamsAcceleration()
        {
            Name = "TrapezoidParams/Acceleration";
        }

        public override ValidationResult Validate()
        {
            try
            {
                if (ValueString.Count == 0)
                {
                    return new ValidationResult() { Validated = false };
                }
                string[] list = ValueString[1].Split(';', '[', ']');
                List<double> ret = new List<double>();
                int i = 1;
                while (!list[i].Equals(""))
                {
                    ret.Add(double.Parse(list[i]));
                    i++;
                }
                Value = ret;
                Validated = true;
                return new ValidationResult() { Validated = true};
            }
            catch (Exception e)
            {
                if (SeqGTSPTask.DEBUG)
                    Console.WriteLine("Error in validation: " + this.GetType().Name + " " + e.Message);
                return null;
            }
        }
    }
}