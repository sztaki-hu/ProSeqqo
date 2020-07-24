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
                        //if (!line.Virtual)
                        //{
                            SolutionLine.Add(line);
                            line.Length = Task.DistanceFunction.Calculate(line.Start, line.End);
                        //}
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
            GraphViz.CreateGraphVizLineLike(Task.GTSP, graphviz);
        }

        public override void WriteFull() {
            WriteHeader();

            if (SolutionLine[0].Virtual) { 
                if (!SolutionLine[0].End.Virtual) {
                    Console.WriteLine("\t| Start: " + SolutionLine[0].End.Name + " " + SolutionLine[0].End.ConfigString());
                }
            }
            else
            {
                Console.WriteLine("\t| " + SolutionLine[0] + " Cost: " + SolutionLine[0].Length.ToString("F4"));
            }
            for (int i = 1; i < SolutionLine.Count - 1; i++)
            {
                Console.WriteLine("\t|--" + Costs[i-1].ToString("F4"));
                Console.WriteLine("\t| " + SolutionLine[i]+ " Cost: "+ SolutionLine[i].Length.ToString("F4"));
                
            }
            var last = SolutionLine.Count - 1;
            if (last - 1 >= 0)
                Console.WriteLine("\t|--" + Costs[last - 1].ToString("F4"));
            if (SolutionLine[last].Virtual)
            {
                if (!SolutionLine[last].Start.Virtual)
                {
                    Console.WriteLine("\t| Finish: " + SolutionLine[last].Start.Name + " " + SolutionLine[last].Start.ConfigString());
                }
            }
            else
            {
                Console.WriteLine("\t| " + SolutionLine[last] + " Cost: " + SolutionLine[last].Length.ToString("F4"));
            }
            Console.WriteLine();
        }

        public override void WriteSimple() { }

        public override void Write() { }

        public override void WriteOutputFile(string File) {
            if (SolutionRaw != null)
            {
                using (System.IO.StreamWriter f = new System.IO.StreamWriter(@File, false))
                {
                    f.WriteLine("\n#LineID;ContourID;LineName;Length;PointA-ID;PointA-Name;PointA-Config;PointB-ID;PointB-Name;PointB-Config\n");

                    if (SolutionLine[0].Virtual)
                    {
                        if (!SolutionLine[0].End.Virtual)
                        {
                            f.WriteLine("Start: " + SolutionLine[0].End.UID + ";" + SolutionLine[0].End.Name + ";" + SolutionLine[0].End.ConfigString());
                        }
                    }
                    else
                    {
                        f.WriteLine(SolutionLine[0].UID + ";" + SolutionLine[0].Contour.UID + ";" + SolutionLine[0].Name + ";" + SolutionLine[0].Length.ToString("F4") + ";" + SolutionLine[0].Start.UID + ";" + SolutionLine[0].Start.Name + ";" + SolutionLine[0].Start.ConfigString() + ";" + SolutionLine[0].End.UID + ";" + SolutionLine[0].End.Name + ";" + SolutionLine[0].End.ConfigString());
                    }

                    for (int i = 1; i < SolutionLine.Count - 1; i++)
                    {
                        f.WriteLine(Costs[i-1]);
                        f.WriteLine(SolutionLine[i].UID + ";"+ SolutionLine[i].Contour.UID+ ";" + SolutionLine[i].Name+ ";" + SolutionLine[i].Length.ToString("F4")  +";"+ SolutionLine[i].Start.UID+";"+SolutionLine[i].Start.Name + ";" + SolutionLine[i].Start.ConfigString() + ";" + SolutionLine[i].End.UID + ";" + SolutionLine[i].End.Name + ";" + SolutionLine[i].End.ConfigString());
                    }

                    var last = SolutionLine.Count - 1;
                    if (last - 1 >= 0)
                        f.WriteLine(Costs[last - 1]);
                    if (SolutionLine[last].Virtual)
                    {
                        if (!SolutionLine[last].Start.Virtual)
                        {
                            f.WriteLine("Finish: " + SolutionLine[last].Start.UID + ";" + SolutionLine[last].Start.Name + ";" + SolutionLine[last].Start.ConfigString());
                        }
                    }
                    else
                    {
                        f.WriteLine(SolutionLine[last].UID + ";" + SolutionLine[last].Contour.UID + ";" + SolutionLine[last].Name + ";" + SolutionLine[last].Length.ToString("F4") + ";" + SolutionLine[last].Start.UID + ";" + SolutionLine[last].Start.Name + ";" + SolutionLine[last].Start.ConfigString() + ";" + SolutionLine[last].End.UID + ";" + SolutionLine[last].End.Name + ";" + SolutionLine[last].End.ConfigString());
                    }



                    f.WriteLine("\n#Solution params: ");
                    f.WriteLine("#Full length: " + CostSum.ToString("F4"));
                    f.WriteLine("#Length without penalty: " + CostSumNoPenalty.ToString("F4"));
                    f.WriteLine("#Penalty: " + Penalty.ToString("F4"));
                    f.WriteLine("#Length of lines: " + CostOfLines.ToString("F4"));
                    f.WriteLine("#Length between lines: " + CostBetweenLines.ToString("F4"));
                    f.WriteLine("#Length between lines without penalty: " + CostBetweenLinesNoPenalty.ToString("F4"));
                    f.WriteLine("#Rate length and length between lines (without penalty): " + ((CostBetweenLinesNoPenalty / CostSumNoPenalty) * 100).ToString("F1") + "%");
                    f.WriteLine("#Number of items: " + SolutionLine.Count);
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", Time.Hours, Time.Minutes, Time.Seconds, Time.Milliseconds / 10);
                    f.WriteLine("#RunTime: " + elapsedTime);


                    f.WriteLine("\n#Task params: ");
                    f.WriteLine("#TaskType: " + Task?.TaskType.ToString());
                    f.WriteLine("#Dimension: " + Task?.Dimension.ToString());
                    f.WriteLine("#DistanceFunction: " + Task?.DistanceFunction?.CalcFunction.ToString());
                    f.WriteLine("#CyclicSequence: " + Task?.CyclicSequence.ToString());
                    f.WriteLine("#StartDepot: " + Task?.StartDepot?.UID.ToString());
                    f.WriteLine("#FinishDepot: " + Task?.FinishDepot?.UID.ToString());
                    f.WriteLine("#ContourPenalty: " + Task?.GTSP?.ContourPenalty.ToString());
                    f.WriteLine("#BidirectionLineDefault: " + Task?.GTSP?.BidirectionLineDefault.ToString());
                    f.WriteLine("#WeightMultiplier: " + (Task?.WeightMultiplier == -1 ? "Auto" : Task?.WeightMultiplier.ToString()));
                    f.WriteLine("#TimeLimit: " + Task?.TimeLimit.ToString());
                    f.WriteLine("#LinePrecedences: ");
                    foreach (var item in Task?.GTSP?.ConstraintsOrder)
                    {
                        f.WriteLine("#" + item.Before.UID + ";" + item.After.UID);
                    }
                    f.WriteLine("#DisjointSets: ");
                    foreach (var set in Task?.GTSP.ConstraintsDisjoints)
                    {
                        var tmp = "#";
                        foreach (var item in set.DisjointSet)
                        {
                            tmp += item.ToString() + ";";
                        }
                        f.WriteLine(tmp);
                    }
                    Console.WriteLine("Output file created at " + File + "!");
                }
            }
            else
            {
                Console.WriteLine("Output file NOT created at " + File + "!");
            }
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