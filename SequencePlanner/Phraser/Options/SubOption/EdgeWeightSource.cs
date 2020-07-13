using SequencePlanner.Phraser.Helper;
using SequencePlanner.Phraser.Options.Values;
using SequencePlanner.Phraser.Template;
using System;
using System.Collections.Generic;

namespace SequencePlanner.Phraser.Options
{
    public class EdgeWeightSource : Option
    {
        public EdgeWeightSourceEnum Value { get; set; }

        public EdgeWeightSource()
        {
            Name = "EdgeWeightSource";
            IncludeableNames = new List<string> { "PositionList", "DistanceFunction", "PositionNumber" };
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
                    case "FULL_MATRIX":
                        Value = EdgeWeightSourceEnum.FullMatrix;
                        newInclude.Add("PositionNumber");
                        newInclude.Add("PositionMatrix");
                        Validated = true;
                        break;
                    case "CALCULATE_FROM_POSITIONS":
                        Value = EdgeWeightSourceEnum.CalculateFromPositions;
                        newInclude.Add("PositionList");
                        newInclude.Add("DistanceFunction");
                        Validated = true;

                        break;
                    default:
                        Console.WriteLine("Fail TaskType");
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
