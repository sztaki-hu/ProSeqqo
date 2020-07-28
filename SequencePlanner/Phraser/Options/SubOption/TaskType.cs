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
            IncludeableNames = new List<string> { "ProcessHierarchy", "ProcessPrecedence", "PositionPrecedence", "LineList", "LinePrecedence", "ContourPrecedence", "ContourPenalty" };
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
                List<string> newIncludeNeed = new List<string>();
                List<string> newIncludeOptional = new List<string>();
                switch (tmp)
                {
                    case "LINE_LIKE":
                        Value = TaskTypeEnum.Line_Like;
                        newIncludeOptional.Add("BidirectionLineDefault"); //Before the Line!!
                        newIncludeNeed.Add("LineList");
                        newIncludeOptional.Add("LinePrecedence");
                        newIncludeOptional.Add("ContourPrecedence");
                        newIncludeOptional.Add("ContourPenalty");
                        newIncludeOptional.Add("PositionList");
                        newIncludeOptional.Add("PositionMatrix");
                        Validated = true;
                        break;
                    case "POINT_LIKE":
                        Value = TaskTypeEnum.Point_Like;
                        newIncludeNeed.Add("ProcessHierarchy");
                        newIncludeOptional.Add("ProcessPrecedence");
                        newIncludeOptional.Add("PositionPrecedence");
                        newIncludeOptional.Add("PositionList");
                        newIncludeOptional.Add("PositionMatrix");
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
                    NewIncludeNeed = newIncludeNeed,
                    NewIncludeOptional = newIncludeOptional
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