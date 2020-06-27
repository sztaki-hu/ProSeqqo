using SequencePlanner.GTSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner
{
    public class ORToolsResult
    {
        public List<Position> Solution { get; set; }
        public double[] Costs { get; set; }
        public double CostSum { get; set; }

        public void ResolveSolution(int[] solution, GTSPRepresentation gtsp)
        {
            foreach (var item in solution)
            {
                foreach (var position in gtsp.Positions)
                {
                    if(position.PID == item)
                    {
                        Solution.Add(position);
                    }
                }
            }
        }

        public void WriteSimple()
        {
            for (int i = 0; i < Solution.Count; i++)
            {
                Console.Write("["+ Solution[i].ID+ "]"+ "[" + Solution[i].PID+ "]"+Solution[i].Name+"--"+Costs[i]+"-->");
            }
        }
        public void Write()
        {
            for (int i = 0; i < Solution.Count; i++)
            {
                Console.WriteLine("[" + Solution[i].ID + "]" + "[" + Solution[i].PID + "]" + Solution[i].Name + Solution[i].Configuration);
            }
        }
        public void WriteFull()
        {
            for (int i = 0; i < Solution.Count; i++)
            {
                Console.WriteLine("| [" + Solution[i].ID + "]" + "[" + Solution[i].PID + "]" + Solution[i].Name + Solution[i].Configuration);
                Console.WriteLine("|- "+ Costs[i]);
            }
        }
    }
}
