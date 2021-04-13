using System.Collections.Generic;
using SequencePlanner.Helper;
using SequencePlanner.Model;

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


        public LineTaskResult()
        {
            LineResult = new List<Line>();
            PositionResult = new List<Position>();
        }
        public LineTaskResult(TaskResult baseTask) : base(baseTask)
        {
            LineResult = new List<Line>();
            PositionResult = new List<Position>();
        }


        public override void Delete(int index)
        {
            if (SolutionRaw.Count <= index)
                throw new SeqException("Result delete failed: index > Solution.Length" + index + ">" + SolutionRaw.Count);
            if (SolutionRaw.Count > 0)
            {
                if (index != 0)
                {
                    CostSum -= CostsRaw[index - 1];
                    CostsRaw.RemoveAt(index - 1);
                }
                else
                {
                    CostSum -= CostsRaw[index];
                    CostsRaw.RemoveAt(index);
                }
                SolutionRaw.RemoveAt(index);
                LineResult.RemoveAt(index);
                PositionResult.RemoveAt(2*index);
                PositionResult.RemoveAt(2*index);
            }
        }
        public static void PositionToSeq(Position position)
        {
            SeqLogger.Info("["+position.UserID + "] " + position.Name + " " + position.Vector.ToListString() + " ResID:" + position.ResourceID);
        }
        
        public static new string ToCSVHeader() => TaskResult.ToCSVHeader();
        public new string ToCSV() => base.ToCSV();
        public new void ToLog(LogLevel lvl)
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
                    SeqLogger.Info(LineResult[^1].ToString());
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
                    PositionToSeq(PositionResult[^1]);
                }
                SeqLogger.Indent--;
            }
            SeqLogger.Indent--;
        }
    }
}
