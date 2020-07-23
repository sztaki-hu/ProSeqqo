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
        public double Penalty { get; set; }
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
                       //if(!line.Virtual)
                        SolutionLine.Add(line);
                    }
                }
            }
            for (int i = 1; i < SolutionLine.Count; i++)
            {
                Costs.Add(Task.GTSP.Graph.PositionMatrix[SolutionLine[i - 1].ID, SolutionLine[i].ID]);
                CostBetweenLines += Costs[i - 1];
                if (SolutionLine[i - 1].Contour.UID != SolutionLine[i].Contour.UID && !SolutionLine[i - 1].Contour.Virtual && !SolutionLine[i].Contour.Virtual)
                {
                    CostBetweenLinesNoPenalty -= Task.GTSP.ContourPenalty;
                    Penalty += Task.GTSP.ContourPenalty;
                }
            }
            CostSum = CostBetweenLines;
            CostSumNoPenalty += CostBetweenLinesNoPenalty;
            CostBetweenLinesNoPenalty += CostBetweenLines;
             
            foreach (var line in SolutionLine)
            {
                if(!line.Virtual)
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

            if (SolutionLine[0].Virtual) { 
                if (!SolutionLine[0].End.Virtual) {
                    Console.WriteLine("\t| Start: " + SolutionLine[0].End.Name + " " + SolutionLine[0].End.ConfigString());
                    Console.WriteLine("\t|--" + Costs[0].ToString("F4"));
                }
            }
            else
                Console.WriteLine("\t| " + SolutionLine[0] + " Cost: " + Task.DistanceFunction.Calculate(SolutionLine[0].Start, SolutionLine[0].End).ToString("F4"));
            for (int i = 1; i < SolutionLine.Count - 1; i++)
            {
                Console.WriteLine("\t| " + SolutionLine[i]+ " Cost: "+Task.DistanceFunction.Calculate(SolutionLine[i].Start, SolutionLine[i].End).ToString("F4"));
                Console.WriteLine("\t|--" + Costs[i].ToString("F4"));
            }
            if(SolutionLine[SolutionLine.Count - 1].Virtual)
            {
                if (!SolutionLine[SolutionLine.Count - 1].Start.Virtual)
                {
                    Console.WriteLine("\t| Finish: " + SolutionLine[SolutionLine.Count - 1].Start.Name + " " + SolutionLine[SolutionLine.Count - 1].Start.ConfigString());
                }
            }
            else
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
            Console.WriteLine("Full length: " + CostSum.ToString("F4"));
            Console.WriteLine("Length without penalty: " + CostSumNoPenalty.ToString("F4"));
            Console.WriteLine("Penalty: " + Penalty.ToString("F4"));
            Console.WriteLine("Length of lines: " + CostOfLines.ToString("F4"));
            Console.WriteLine("Length between lines: " + CostBetweenLines.ToString("F4"));
            Console.WriteLine("Length between lines without penalty: " + CostBetweenLinesNoPenalty.ToString("F4"));
            Console.WriteLine("Rate length and length between lines (without penalty): " + ((CostBetweenLinesNoPenalty / CostSumNoPenalty) *100).ToString("F1")+"%");
            Console.WriteLine("Number of items: " + SolutionLine.Count);
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", Time.Hours, Time.Minutes, Time.Seconds, Time.Milliseconds / 10);
            Console.WriteLine("RunTime: " + elapsedTime);
            Console.WriteLine("Solution: ");
        }
    }
}