using SequencePlanner.GTSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner
{
    public class ORToolsResult
    {
        public List<Position> Solution { get; }
        public List<double> Costs { get;}
        public double CostSum { get; private set; }
        public TimeSpan Time { get; set; }

        public ORToolsResult()
        {
            Solution = new List<Position>();
            Costs = new List<double>();
        }

        public void ResolveSolution(List<long> solution, GTSPRepresentation gtsp)
        {
            foreach (var item in solution)
            {
                foreach (var position in gtsp.Positions)
                {
                    if(position.PID == Convert.ToInt32(item))
                    {
                        Solution.Add(position);
                    }
                }
            }
            for (int i = 1; i < Solution.Count; i++)
            {
                Costs.Add(gtsp.Graph.PositionMatrix[Solution[i-1].PID,Solution[i].PID]);
                CostSum += Costs[i-1];
            }
        }

        public void WriteSimple()
        {
            WriteSolutionHeader();
            for (int i = 0; i < Solution.Count-1; i++)
            {
                Console.Write("["+ Solution[i].ID+ "] "+ Solution[i].Name+"  --"+Costs[i].ToString("F4") + "-->  ");
            }
            Console.Write("[" + Solution[Solution.Count-1].ID + "]" + Solution[Solution.Count-1].Name+"\n");
            Console.WriteLine();
        }

        public void Write()
        {
            WriteSolutionHeader();
            for (int i = 0; i < Solution.Count; i++)
            {
                Console.WriteLine("\t[" + Solution[i].ID + "]" + Solution[i].Name + " "+ Solution[i].ConfigString());
            }
            Console.WriteLine();
        }

        public void WriteFull()
        {
            WriteSolutionHeader();
            for (int i = 0; i < Solution.Count-1; i++)
            {
                Console.WriteLine("\t| [" + Solution[i].ID + "]"  + Solution[i].Name+" " + Solution[i].ConfigString());
                //Console.WriteLine("\t| ");
                Console.WriteLine("\t|--" + Costs[i].ToString("F4"));
                //Console.WriteLine("\t| ");
            }
            Console.WriteLine("\t| [" + Solution[Solution.Count - 1].ID + "]"  + Solution[Solution.Count - 1].Name+ " " + Solution[Solution.Count - 1].ConfigString());
            Console.WriteLine();
        }

        private void WriteSolutionHeader()
        {
            Console.WriteLine("Completed! ");
            Console.WriteLine("Length: " + CostSum.ToString("F4"));
            Console.WriteLine("Number of items: " + Solution.Count);
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", Time.Hours, Time.Minutes, Time.Seconds, Time.Milliseconds / 10);
            Console.WriteLine("RunTime: " + elapsedTime);
            Console.WriteLine("Solution: ");
        }
    }
}