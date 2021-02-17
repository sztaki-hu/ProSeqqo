using SequencePlanner.Helper;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Result
{
    public class LineTaskResult : TaskResult
    {
        public List<Line> LineResult { get; set; }
        public List<Position> PositionResult { get; set; }
        public double FullLength { get; internal set; }
        public double TravelLength { get; internal set; }
        public double Penalty { get; internal set; }
        public double LineLength { get; internal set; }

        public LineTaskResult(TaskResult baseTask) : base(baseTask)
        {
            LineResult = new List<Line>();
            PositionResult = new List<Position>();
        }

        public LineTaskResult()
        {
            LineResult = new List<Line>();
            PositionResult = new List<Position>();
        }

        public void ToLog(LogLevel lvl)
        {
            SeqLogger.Info("Result: ");
            SeqLogger.Indent++;
            base.ToLog(lvl);
            if (StatusCode == 1)
            {
                SeqLogger.Info("Penalty: "+Penalty);
                SeqLogger.Info("FullLength: "+FullLength);
                SeqLogger.Info("LineLength: " + LineLength);
                SeqLogger.Info("TravelLength: " + TravelLength);
                SeqLogger.Info("Solution: ");
                SeqLogger.Indent++;
                if(LineResult != null && LineResult.Count > 0)
                {
                    for (int i = 0; i < LineResult.Count - 1; i++)
                    {
                        SeqLogger.Info(LineResult[i].ToString());
                        SeqLogger.Info("--" + CostsRaw[i].ToString());
                    }
                    SeqLogger.Info(LineResult[LineResult.Count - 1].ToString());
                }
                SeqLogger.Indent--;

                SeqLogger.Info("Positions: ");
                SeqLogger.Indent++;
                if (PositionResult != null && PositionResult.Count > 0)
                {
                    for (int i = 0; i < PositionResult.Count - 1; i++)
                    {
                        PositionToSeq(PositionResult[i]);
                    }
                    PositionToSeq(PositionResult[PositionResult.Count - 1]);
                }
                SeqLogger.Indent--;
            }
            SeqLogger.Indent--;
        }

        public void PositionToSeq(Position position)
        {
            SeqLogger.Info("["+position.UserID + "] " + position.Name + " " + SeqLogger.ToList(position.Vector) + " ResID:" + position.ResourceID);
        }

        public string ToCSV()
        {
            return base.ToCSV();
        }
    }
}
