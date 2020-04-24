using SequencePlanner.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Options
{
    public class TaskType : Option<ValueEnum.TaskType>
    {
        public TaskType(): base()
        {
            Name = nameof(TaskType);
        }

        public new bool Validate(string value)
        {
            if (value.Contains("Line"))
            {
                Value = ValueEnum.TaskType.LineLike;
                return true;
            }
            if (value.Contains("Point"))
            {
                Value = ValueEnum.TaskType.PointLike;
                return true;
            }
            return false;
        }
    }

 
}
