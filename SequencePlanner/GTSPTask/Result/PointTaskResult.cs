using SequencePlanner.Helper;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Result
{
    public class PointTaskResult : TaskResult
    {
        public TimeSpan PreSolverTime { get; set; }
        public List<Position> PositionResult { get; set; }

        public PointTaskResult(TaskResult baseTask) : base(baseTask)
        {
            PositionResult = new List<Position>();
        }

        public void ToLog(LogLevel lvl)
        {
            SeqLogger.Info("Result: ");
            SeqLogger.Indent++;
            base.ToLog(lvl);
            if (StatusCode == 1)
            {
                SeqLogger.Info("PreSolverTime: " + PreSolverTime);
                SeqLogger.Info("Solution: ");
                SeqLogger.Indent++;
                for (int i = 0; i < PositionResult.Count; i++)
                {
                     SeqLogger.Info(PositionResult[i].ToString());
                }
                SeqLogger.Indent--;
            }
            SeqLogger.Indent--;
        }
    }
}