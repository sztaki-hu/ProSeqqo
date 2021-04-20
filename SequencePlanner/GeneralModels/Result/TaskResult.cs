using System;
using System.Collections.Generic;

namespace SequencePlanner.GeneralModels.Result
{
    public class TaskResult
    {
        public DateTime FullTime {get;set;}
        public DateTime SolverTime {get;set;}
        public DateTime PreSolverTime {get;set;}
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string ErrorMessage { get; set; }
        public List<int> Solution { get; set; }
        public List<Motion> SolutionMotion { get; set; }
        public List<Config> SolutionConfig { get; set; }
        public List<HierarchyRecord> SolutionHierarchy { get; set; }
        public List<double> Costs { get; set; }
        public double Cost { get; set; }
        public List<Cost<Motion>> CostsBetweenMotions { get; set; }
        public List<Cost<Config>> CostsBetweenConfigs { get; set; }
    }
}
