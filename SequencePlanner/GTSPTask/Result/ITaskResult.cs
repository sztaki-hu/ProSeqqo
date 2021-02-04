using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Result
{
    public interface ITaskResult
    {
        public TimeSpan FullTime { get; set; }
        public TimeSpan SolverTime { get; set; }
        public List<long> SolutionRaw { get; set; }
        public List<double> CostsRaw { get; set; }
        public double CostSum { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
    }
}