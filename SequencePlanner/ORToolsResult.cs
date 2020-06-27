using SequencePlanner.GTSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner
{
    public class ORToolsResult
    {
        public List<Position> Solution { get; set; }
        public List<double> Costs { get; set; }
        public double CostSum { get; set; }

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
            }
        }

        public void WriteSimple()
        {
            Console.WriteLine();
            for (int i = 0; i < Solution.Count-1; i++)
            {
                Console.Write("["+ Solution[i].ID+ "]"+ "[" + Solution[i].PID+ "]"+Solution[i].Name+"--"+Costs[i].ToString("#.##") + "-->");
            }
            Console.Write("[" + Solution[Solution.Count-1].ID + "]" + "[" + Solution[Solution.Count-1].PID + "]" + Solution[Solution.Count-1].Name+"\n");

        }
        public void Write()
        {
            Console.WriteLine();
            for (int i = 0; i < Solution.Count; i++)
            {
                Console.WriteLine("[" + Solution[i].ID + "]" + "[" + Solution[i].PID + "]" + Solution[i].Name + Solution[i].ConfigString());
            }
            Console.WriteLine();
        }
        public void WriteFull()
        {
            Console.WriteLine();
            for (int i = 0; i < Solution.Count-1; i++)
            {
                Console.WriteLine("| [" + Solution[i].ID + "]" + "[" + Solution[i].PID + "]" + Solution[i].Name + Solution[i].ConfigString());
                Console.WriteLine("| ");
                Console.WriteLine("|- "+ Costs[i].ToString("#.##"));
                Console.WriteLine("| ");
            }
            Console.WriteLine("| [" + Solution[Solution.Count - 1].ID + "]" + "[" + Solution[Solution.Count - 1].PID + "]" + Solution[Solution.Count - 1].Name + Solution[Solution.Count - 1].ConfigString());
            Console.WriteLine();
        }
    }
}
