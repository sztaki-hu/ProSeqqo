using SequencePlanner.Helper;
using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Result
{
    public class TaskResult : ITaskResult
    {
        public TimeSpan FullTime { get; set; }          //Run time of build and execute task
        public TimeSpan SolverTime { get; set; }        //Run time of OR-Tools VRP solver
        public TimeSpan PreSolverTime { get; set; }     //Run time of MIP presolver
        public List<long> SolutionRaw { get; set; }     //Solution of 
        public List<double> CostsRaw { get; set; }      //Costs of solution between the steps
        public double CostSum { get; set; }             //Sum of solution costs (CostsRaw)
        public int StatusCode { get; set; }             //OR-Tools exit status code
        public string StatusMessage { get; set; }       //OR-Tools exit message
        public List<string> Log { get; set; }                 //Console log of task running

        public TaskResult(TaskResult task)
        {
            FullTime = task.FullTime;
            SolverTime = task.SolverTime;
            SolutionRaw = task.SolutionRaw;
            CostsRaw = task.CostsRaw;
            CostSum = task.CostSum;
            StatusCode = task.StatusCode;
            StatusMessage = task.StatusMessage;
            Log = task.Log;
    }

        public TaskResult()
        {
            FullTime = new TimeSpan();
            SolverTime = new TimeSpan();
            SolutionRaw = new List<long>();
            CostsRaw = new List<double>();
            CostSum = 0;
            StatusCode = -1;
            StatusMessage = "Not filled yet!";
            Log = new List<string>();
        }

        public virtual void Calculate()
        {
            CostSum = 0;
            foreach (var cost in CostsRaw)
            {
                CostSum += cost;
            }
        }

        public override string ToString()
        {
            string a = "Time: " + FullTime;
            a += "\nSolution: ";
            foreach (var item in SolutionRaw)
            {
                a += item + ";";
            }
            a += "\nStatus message: "+StatusMessage;
            return a;
        }

        public void ToLog(LogLevel lvl)
        {
            if(StatusCode == 1)
            {
                SeqLogger.Info("StatusCode: " + StatusCode);
                SeqLogger.Info("StatusMessage: " + StatusMessage);
                SeqLogger.Info("FullTime: "+ FullTime);
                SeqLogger.Info("SolverTime: " + SolverTime);
                SeqLogger.Info("PreSolverTime: " + PreSolverTime);
                SeqLogger.Info("SolutionRaw: " + SeqLogger.ToList(SolutionRaw));
                SeqLogger.Info("CostsRaw: " + SeqLogger.ToList(CostsRaw));
                SeqLogger.Info("CostSum: " + CostSum);
                SeqLogger.Info("Log size: " + Log.Count+" lines");
            }
            else
            {
                SeqLogger.Info("StatusCode: " + StatusCode);
                SeqLogger.Info("StatusMessage: " + StatusMessage);
                SeqLogger.Info("FullTime: "+ FullTime);
                SeqLogger.Info("SolverTime: " + SolverTime);
                SeqLogger.Info("Log size: " + Log.Count+" lines");
            }
        }
    }
}