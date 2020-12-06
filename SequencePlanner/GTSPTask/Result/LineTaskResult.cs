using SequencePlanner.Model;
using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Result
{
    public class LineTaskResult : TaskResult, ITaskResult
    {
        public List<Line> LineResult { get; set; }
        public List<int> Result { get; set; }
        public List<int> ResultWithVirtual { get; set; }
        public double CostBetweenLines { get; set; }
        public double CostBetweenLinesNoPenalty { get; set; }
        public double Penalty { get; set; }
        public double CostSumNoPenalty { get; set; }
        public double CostOfLines { get; set; }
        public List<double> CostOfLinesList { get; set; }

        public LineTaskResult(TaskResult baseTask) : base(baseTask)
        {
            LineResult = new List<Line>();
            Result = new List<int>();
            ResultWithVirtual = new List<int>();
        }

        public void WriteFull()
        {
            for (int i = 0; i < LineResult.Count; i++)
            {
                if(i!=0)
                    Console.WriteLine("\t|--" + Costs[i-1].ToString("F4"));
                Console.WriteLine("\t| " + LineResult[i]+"["+LineResult[i].NodeA.Name+";"+LineResult[i].NodeB.Name+"]" + " Cost: " + LineResult[i].Length.ToString("F4"));
            }
            Console.WriteLine();
        }
        public void WriteHeader()
        {
            Console.WriteLine(base.ToString());
            Console.WriteLine("Full length: " + CostSum.ToString("F4"));
            Console.WriteLine("Length without penalty: " + CostSumNoPenalty.ToString("F4"));
            Console.WriteLine("Penalty: " + Penalty.ToString("F4"));
            Console.WriteLine("Length of lines: " + CostOfLines.ToString("F4"));
            Console.WriteLine("Length between lines: " + CostBetweenLines.ToString("F4"));
            Console.WriteLine("Length between lines without penalty: " + CostBetweenLinesNoPenalty.ToString("F4"));
            Console.WriteLine("Rate length and length between lines (without penalty): " + ((CostBetweenLinesNoPenalty / CostSumNoPenalty) * 100).ToString("F1") + "%");
            Console.WriteLine("Number of items: " + LineResult.Count);
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", Time.Hours, Time.Minutes, Time.Seconds, Time.Milliseconds / 10);
            Console.WriteLine("RunTime: " + elapsedTime);
        }
        public override string ToString()
        {
            WriteHeader();
            WriteFull();
            return "";
        }
    }
}
