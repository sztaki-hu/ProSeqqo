using SequencePlanner.Helper;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Result
{
    public class PointTaskResult : TaskResult
    {
        public List<Position> PositionResult { get; set; }

        public PointTaskResult(TaskResult baseTask) : base(baseTask)
        {
            PositionResult = new List<Position>();
        }

        public override void Calculate() => base.Calculate();

        public void ToLog(LogLevel lvl)
        {
            SeqLogger.Info("Result: ");
            SeqLogger.Indent++;
            base.ToLog(lvl);
            if (StatusCode == 1)
            {
                SeqLogger.Info("Solution: ");
                SeqLogger.Indent++;
                for (int i = 0; i < PositionResult.Count-1; i++)
                {
                     SeqLogger.Info(PositionResult[i].ToString());
                     SeqLogger.Info("--"+CostsRaw[i].ToString());
                }
                SeqLogger.Info(PositionResult[PositionResult.Count-1].ToString());

                SeqLogger.Indent--;
            }
            SeqLogger.Indent--;
        }
    }
}