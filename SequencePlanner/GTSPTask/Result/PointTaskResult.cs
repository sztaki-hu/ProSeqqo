using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Result
{
    public class PointTaskResult : TaskResult
    {
        public List<Position> PositionResult { get; set; }
        public List<int> ResultIDs { get; set; }

        public PointTaskResult(TaskResult baseTask) : base(baseTask)
        {
            PositionResult = new List<Position>();
            ResultIDs = new List<int>();
        }
    }
}