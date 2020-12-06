using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Result
{
    public interface ITaskResult
    {
        public TimeSpan Time { get; set; }
        public List<long> SolutionRaw { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public List<double> Costs { get; }
        public double CostSum { get; set; }


    }
}