using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Result
{
    public class TaskResult : ITaskResult
    {
        public TimeSpan Time { get; set; }
        public List<long> SolutionRaw { get; set; }
        public List<double> Costs { get; }
        public double CostSum { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }

        public TaskResult(TaskResult task)
        {
            Time = task.Time;
            SolutionRaw = task.SolutionRaw;
            StatusCode = task.StatusCode;
            StatusMessage = task.StatusMessage;
            Costs = new List<double>();
        }

        public TaskResult()
        {
            SolutionRaw = new List<long>();
            Costs = new List<double>();
        }

        public override string ToString()
        {
            string a = "Time: " + Time;
            a += "\nSolution: ";
            foreach (var item in SolutionRaw)
            {
                a += item + ";";
            }
            a += "\nStatus message: "+StatusMessage;
            return a;
        }
    }
}
