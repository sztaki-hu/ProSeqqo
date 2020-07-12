using SequencePlanner.Phraser.Helper;
using SequencePlanner.Phraser.Options.Values;
using SequencePlanner.Phraser.Template;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options
{
    public class DistanceFunction : Option
    {

        public DistanceFunctionEnum Value { get; set; }

        public DistanceFunction()
        {
            Name = "DistanceFunction";
            IncludeableNames = new List<string> { "TrapezoidParams/Acceleration", "TrapezoidParams/Speed" };
            Need = false;
        }

        public override ValidationResult Validate()
        {
            try
            {
                if (ValueString.Count == 0)
                {
                    return new ValidationResult() { Validated = false };
                }
                string tmp = ValueString[1].ToUpper();
                List<string> newInclude = new List<string>();
                switch (tmp)
                {
                    case "EUCLIDIAN_DISTANCE":
                        Value = DistanceFunctionEnum.Euclidian_Distance;
                        Validated = true;
                        break;
                    case "MAX_DISTANCE":
                        Value = DistanceFunctionEnum.Max_Distance;
                        Validated = true;
                        break;
                    case "TRAPEZOID_TIME":
                        Value = DistanceFunctionEnum.Trapezoid_Time;
                        newInclude.Add("TrapezoidParams/Acceleration");
                        newInclude.Add("TrapezoidParams/Speed");
                        Validated = true;
                        break;
                    case "TRAPEZOID_TIME_WITHTIEBREAKER":
                        Value = DistanceFunctionEnum.Trapezoid_Time_WithTieBreaker;
                        newInclude.Add("TrapezoidParams/Acceleration");
                        newInclude.Add("TrapezoidParams/Speed");
                        Validated = true;
                        break;
                    case "MANHATTAN_DISTANCE":
                        Value = DistanceFunctionEnum.Manhattan_Distance;
                        Validated = true;
                        break;
                    default:
                        Console.WriteLine("Fail DistanceFunction");
                        break;
                }
                Validated = true;
                return new ValidationResult()
                {
                    Validated = this.Validated,
                    NewInclude = newInclude
                };
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
