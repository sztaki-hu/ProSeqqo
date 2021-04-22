using SequencePlanner.Helper;
using System;
using System.Collections.Generic;

namespace SequencePlanner.GeneralModels.Result
{
    public class TaskResult
    {
        public TimeSpan FullTime {get;set;}
        public TimeSpan SolverTime {get;set;}
        public TimeSpan PreSolverTime {get;set;}
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string ErrorMessage { get; set; }
        public List<int> Solution { get; set; }
        public List<Motion> SolutionMotion { get; set; }
        public List<Config> SolutionConfig { get; set; }
        public List<HierarchyRecord> SolutionHierarchy { get; set; }
        public List<double> Costs { get; set; }
        public double Cost { get; set; }
        public List<DetailedMotionCost> CostsBetweenMotions { get; set; }
        public List<DetailedConfigCost> CostsBetweenConfigs { get; set; }

        public TaskResult()
        {
            StatusCode = -1;
            StatusMessage = "Result initalized.";
            ErrorMessage = "No Error";
            FullTime = new TimeSpan();
            SolverTime = new TimeSpan();
            PreSolverTime = new TimeSpan();
            Solution = new List<int>();
            SolutionMotion = new List<Motion>();
            SolutionConfig = new List<Config>();
            SolutionHierarchy = new List<HierarchyRecord>();
            CostsBetweenConfigs = new List<DetailedConfigCost>();
            CostsBetweenMotions = new List<DetailedMotionCost>();
        }

        public void ToLog(LogLevel logLevel)
        {
            SeqLogger.WriteLog(logLevel, "Status code: " + StatusCode);
            SeqLogger.WriteLog(logLevel, "Status message: " + StatusMessage);
            SeqLogger.WriteLog(logLevel, "Error message: " + ErrorMessage);
            SeqLogger.WriteLog(logLevel, "Full Time:" + FullTime);
            SeqLogger.WriteLog(logLevel, "Solver Time: " + SolverTime);
            SeqLogger.WriteLog(logLevel, "MIP Solver Time: " + PreSolverTime);
            SeqLogger.WriteLog(logLevel, "Full Cost: " + Cost);
            SeqLogger.WriteLog(logLevel, "Solution MotionIDs:" + Solution.ToListString());
        }
    }
}
