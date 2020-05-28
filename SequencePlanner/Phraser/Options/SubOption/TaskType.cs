using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options
{
    public class TaskType : Option
    {
        public new TaskTypeEnum Value { get; set; }

        public override ValidationResult Validate()
        {
            try
            {
                string tmp = ValueString[1].ToUpper();
                List<string> newInclude = new List<string>();
                switch (tmp)
                {
                    case "LINE_LIKE":
                        Value = TaskTypeEnum.Line_Like;
                        newInclude.Add("Line");
                        newInclude.Add("LinePrecedence");
                        newInclude.Add("ContourPrecedence");
                        newInclude.Add("ContourPenalty");
                        Validated = true;
                        break;
                    case "POINT_LIKE":
                        Value = TaskTypeEnum.Point_Like;
                        newInclude.Add("ProcessHierarchy");
                        newInclude.Add("ProcessPrecedence");
                        newInclude.Add("PositionPrecedence");
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
                Console.WriteLine("Error in validation: " + this.GetType().Name + " " + e.Message);
                return null;
            }
        }
    }
}