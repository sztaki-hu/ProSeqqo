using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;
using SequencePlanner.Phraser.Helper;
using SequencePlanner.Phraser.Template;

namespace SequencePlanner.Phraser.Options
{
    public class TaskType : Option
    {
        public new TaskTypeEnum Value { get; set; }

        public TaskType()
        {
            Name = "TaskType";
            IncludeableNames = new List<string> { "ProcessHierarchy", "ProcessPrecedence", "PositionPrecedence", "Line", "Line Precedence", "Contour Precedence", "ContourPenalty" };
            Need = true;
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
                Validated = false;
                if (TemplateManager.DEBUG)
                    Console.WriteLine("Error in validation: " + this.GetType().Name + " " + e.Message);
                return null;
            }
        }
    }
}