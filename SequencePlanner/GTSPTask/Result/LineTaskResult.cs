using SequencePlanner.Helper;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Result
{
    public class LineTaskResult : TaskResult
    {
        public List<Line> LineResult { get; set; }

        public LineTaskResult(TaskResult baseTask) : base(baseTask)
        {
            LineResult = new List<Line>();
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
                for (int i = 0; i < LineResult.Count - 1; i++)
                {
                    SeqLogger.Info(LineResult[i].ToString());
                    SeqLogger.Info("--" + CostsRaw[i].ToString());
                }
                SeqLogger.Info(LineResult[LineResult.Count - 1].ToString());

                SeqLogger.Indent--;
            }
            SeqLogger.Indent--;
        }
    }
}
