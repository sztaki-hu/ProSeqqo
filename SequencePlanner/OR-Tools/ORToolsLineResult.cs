using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Template;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner
{
    public class ORToolsLineResult : ORToolsResult
    {
        public List<Line> SolutionLine { get; }
        public double CostBetweenLines { get; set; }
        public double CostBetweenLinesNoPenalty { get; set; }
        public double CostSumNoPenalty { get; set; }
        public double CostOfLines { get; set; }
        private LineLikeTask Task { get; set; }
      

        public ORToolsLineResult(ORToolsResult result, LineLikeTask task) : base(result)
        {
            SolutionLine = new List<Line>();
            Task = task;
            ResolveSolution();
        }

        private void ResolveSolution()
        {
            foreach (var solutionID in SolutionRaw)
            {
                foreach (var line in Task.GTSP.Lines)
                {
                    if (line.ID == solutionID)
                    {
                       // if(!line.Virtual)
                        SolutionLine.Add(line);
                    }
                }
            }
            for (int i = 1; i < SolutionLine.Count; i++)
            {
                Costs.Add(Task.GTSP.Graph.PositionMatrix[SolutionLine[i - 1].ID, SolutionLine[i].ID]);
                CostBetweenLines += Costs[i - 1];
                if (SolutionLine[i - 1].Contour.UID != SolutionLine[i].Contour.UID)
                    CostBetweenLinesNoPenalty -= Task.GTSP.ContourPenalty;
            }
            CostSum = CostBetweenLines;
            CostSumNoPenalty += CostBetweenLinesNoPenalty;
            CostBetweenLinesNoPenalty += CostBetweenLines;
             
            foreach (var line in SolutionLine)
            {
                CostOfLines += Task.DistanceFunction.Calculate(line.Start, line.End);
            }
            CostSum += CostOfLines;
            CostSumNoPenalty += CostSum;
        }


        public override void CreateGraphViz(string graphviz)
        {
            Console.WriteLine("Output file NOT created at " + graphviz + "!");
            Console.WriteLine("LineLike task have no GraphViz representation yet.");
        }

        public override void WriteFull() {
            WriteHeader();
            for (int i = 0; i < SolutionLine.Count - 1; i++)
            {
                Console.WriteLine("\t| " + SolutionLine[i]+ " Cost: "+Task.DistanceFunction.Calculate(SolutionLine[i].Start, SolutionLine[i].End).ToString("F4"));
                Console.WriteLine("\t|--" + Costs[i].ToString("F4"));
            }
            Console.WriteLine("\t| " + SolutionLine[SolutionLine.Count - 1] + " Cost: " + Task.DistanceFunction.Calculate(SolutionLine[SolutionLine.Count - 1].Start, SolutionLine[SolutionLine.Count - 1].End).ToString("F4"));
            Console.WriteLine();
        }

        public override void WriteSimple() { }

        public override void Write() { }

        public override void WriteOutputFile(string File) {

            Console.WriteLine("Output file NOT created at " + File + "!");//NOT CREATED
        }

        public override void WriteMinimal() { }

        private void WriteHeader()
        {
            Console.WriteLine("Length: " + CostSum.ToString("F4"));
            Console.WriteLine("Length without penalty: " + CostSumNoPenalty.ToString("F4"));
            Console.WriteLine("Length of lines: " + CostOfLines.ToString("F4"));
            Console.WriteLine("Length between lines: " + CostBetweenLines.ToString("F4"));
            Console.WriteLine("Length between lines without penalty: " + CostBetweenLinesNoPenalty.ToString("F4"));
            Console.WriteLine("Rate of between lines: " + ((CostBetweenLines / CostSum) *100).ToString("F1")+"%");
            Console.WriteLine("Number of items: " + SolutionLine.Count);
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", Time.Hours, Time.Minutes, Time.Seconds, Time.Milliseconds / 10);
            Console.WriteLine("RunTime: " + elapsedTime);
            Console.WriteLine("Solution: ");
        }
    }
}