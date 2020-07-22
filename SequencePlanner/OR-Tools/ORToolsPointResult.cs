using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Template;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner
{
    public class ORToolsPointResult : ORToolsResult
    {
        public List<Position> SolutionPoint { get; }
        private PointLikeTask Task { get; set; }

        public ORToolsPointResult(ORToolsResult result, PointLikeTask task): base(result)
        {
            SolutionPoint = new List<Position>();
            Task = task;
            ResolveSolution();
        }

        public void ResolveSolution()
        {
            if (SolutionRaw != null)
            {
                foreach (var item in SolutionRaw)
                {
                    foreach (var position in Task.GTSP.Positions)
                    {
                        if (position.ID == Convert.ToInt32(item))
                        {
                            SolutionPoint.Add(position);
                        }
                    }
                }
                for (int i = 1; i < SolutionPoint.Count; i++)
                {
                    Costs.Add(Task.GTSP.Graph.PositionMatrix[SolutionPoint[i - 1].ID, SolutionPoint[i].ID]);
                    CostSum += Costs[i - 1];
                }
            }
        }

        public override void CreateGraphViz(string graphviz)
        {
            GraphViz.CreateGraphViz(Task.GTSP, graphviz);
        }

        public override void WriteSimple()
        {
            WriteSolutionHeader();
            if (SolutionRaw != null)
            {
                for (int i = 0; i < SolutionPoint.Count - 1; i++)
                {
                    Console.Write("[" + SolutionPoint[i].GID + "] " + SolutionPoint[i].Name + "  --" + Costs[i].ToString("F4") + "-->  ");
                }
                Console.Write("[" + SolutionPoint[SolutionPoint.Count - 1].GID + "]" + SolutionPoint[SolutionPoint.Count - 1].Name + "\n");
                Console.WriteLine();
            }
        }

        public override void Write()
        {
            WriteSolutionHeader();
            if (SolutionRaw != null)
            {
                for (int i = 0; i < SolutionPoint.Count; i++)
                {
                    Console.WriteLine("\t[" + SolutionPoint[i].GID + "]" + SolutionPoint[i].Name + " " + SolutionPoint[i].ConfigString());
                }
                Console.WriteLine();
            }
        }

        public override void WriteFull()
        {
            WriteSolutionHeader();
            if (SolutionRaw != null)
            {
                    for (int i = 0; i < SolutionPoint.Count - 1; i++)
                    {
                        Console.WriteLine("\t| [" + SolutionPoint[i].GID + "]" + SolutionPoint[i].Name + " " + SolutionPoint[i].ConfigString());
                        //Console.WriteLine("\t| ");
                        Console.WriteLine("\t|--" + Costs[i].ToString("F4"));
                        //Console.WriteLine("\t| ");
                    }
                    Console.WriteLine("\t| [" + SolutionPoint[SolutionPoint.Count - 1].GID + "]" + SolutionPoint[SolutionPoint.Count - 1].Name + " " + SolutionPoint[SolutionPoint.Count - 1].ConfigString());
                    Console.WriteLine();
                }
        }

        public override void WriteOutputFile(String file)
        {
            if (SolutionRaw != null)
            {
                using (System.IO.StreamWriter f = new System.IO.StreamWriter(@file, false))
                {
                    f.WriteLine("Length: " + CostSum.ToString("F4"));
                    f.WriteLine("Number of items: " + SolutionPoint.Count);
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", Time.Hours, Time.Minutes, Time.Seconds, Time.Milliseconds / 10);
                    f.WriteLine("RunTime: " + elapsedTime);
                    f.WriteLine("Solution: ");


                    for (int i = 0; i < SolutionPoint.Count; i++)
                    {
                        f.WriteLine("\t[" + SolutionPoint[i].GID + "]" + SolutionPoint[i].Name + " " + SolutionPoint[i].ConfigString());
                    }

                    f.WriteLine("Timing: ");
                    for (int i = 0; i < SolutionPoint.Count - 1; i++)
                    {
                        f.Write("[" + SolutionPoint[i].GID + "] " + SolutionPoint[i].Name + "  --" + Costs[i].ToString("F4") + "-->  ");
                    }
                    f.Write("[" + SolutionPoint[SolutionPoint.Count - 1].GID + "]" + SolutionPoint[SolutionPoint.Count - 1].Name + "\n");
                    Console.WriteLine("Output file created at " + file + "!");
                }
            }
            Console.WriteLine("Output file NOT created at " + file + "!");
        }

        private void WriteSolutionHeader()
        {
            if (SolutionRaw == null)
            {
                Console.WriteLine("No solution!");
            }
            else
            {
                Console.WriteLine("Length: " + CostSum.ToString("F4"));
                Console.WriteLine("Number of items: " + SolutionPoint.Count);
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", Time.Hours, Time.Minutes, Time.Seconds, Time.Milliseconds / 10);
                Console.WriteLine("RunTime: " + elapsedTime);
                Console.WriteLine("Solution: ");
            }
        }

        public override void WriteMinimal()
        {
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", Time.Hours, Time.Minutes, Time.Seconds, Time.Milliseconds / 10);
            Console.Write("RunTime; " + elapsedTime+"; "+"CostSum; "+ CostSum+"\n");
        }
    }
}