using SequencePlanner.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Options
{
    public class TaskType: Option
    {

        public TaskType()
        {
            Name = nameof(TaskType);
            Required = true;
            Incluided = true;
            PosibbleValues = new List<Enum>();
            PosibbleValues.Add(ValueEnum.TaskType.LineLike);
            PosibbleValues.Add(ValueEnum.TaskType.PointLike);
        }
    }
}
