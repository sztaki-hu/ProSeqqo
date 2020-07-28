using SequencePlanner.GTSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner
{
    public class ORToolsResult
    {
        public List<long> SolutionRaw { get; set; }
        public List<double> Costs { get;}
        public double CostSum { get; protected set; }
        public TimeSpan Time { get; set; }

        public ORToolsResult()
        {
            SolutionRaw = new List<long>();
            Costs = new List<double>();
        }
        public ORToolsResult(ORToolsResult result)
        {
            if (result != null)
            {
                SolutionRaw = result.SolutionRaw;
                Costs = result.Costs;
                CostSum = result.CostSum;
                Time = result.Time;
            }
        }


        public virtual void CreateGraphViz(string graphviz) { }

        public virtual void WriteFull() { }

        public virtual void WriteSimple() { }

        public virtual void Write() { }

        public virtual void WriteOutputFile(string File) { }

        public virtual void WriteMinimal() { }
    }
}