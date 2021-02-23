using SequencePlanner.Helper;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Result
{
    public class PointTaskResult : TaskResult
    {
        public List<GTSPNode> PositionResult { get; set; }

        public PointTaskResult(TaskResult baseTask) : base(baseTask)
        {
            PositionResult = new List<GTSPNode>();
        }

        public PointTaskResult()
        {
            PositionResult = new List<GTSPNode>();
        }

        public void ToLog(LogLevel lvl)
        {
            SeqLogger.Info("Result: ");
            SeqLogger.Indent++;
            base.ToLog(lvl);
            if (StatusCode == 1)
            {
                SeqLogger.Info("Solution: ");
                SeqLogger.Indent++;
                if(PositionResult!=null && PositionResult.Count > 0)
                {
                    for (int i = 0; i < PositionResult.Count - 1; i++)
                    {
                        SeqLogger.Info(PositionResult[i].ToString());
                        SeqLogger.Info("--" + CostsRaw[i].ToString());
                    }
                    SeqLogger.Info(PositionResult[PositionResult.Count - 1].ToString());
                }
                SeqLogger.Indent--;
            }
            SeqLogger.Indent--;
        }

        public string ToCSV()
        {
            return base.ToCSV();
        }
    }
}