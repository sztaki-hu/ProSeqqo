using System;
using System.Collections.Generic;
using SequencePlanner.Helper;

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
        public List<string> Log { get; set; }           //Console log of task running
        public List<string> ErrorMessage { get; set; }  //Console log of task running


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
            ErrorMessage = new List<string>();
        }
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
            ErrorMessage = task.ErrorMessage;
        }


        public virtual void Calculate()
        {
            CostSum = 0;
            foreach (var cost in CostsRaw)
            {
                CostSum += cost;
            }
        }
        public virtual void Delete(int index) { }
        public virtual void DeleteFirst()
        {
            Delete(0);
        }
        public virtual void DeleteLast()
        {
            Delete(SolutionRaw.Count - 1);
        }

        public static string ToCSVHeader()
        {
            var s = ";";
            return nameof(FullTime)+"[ms]" + s + nameof(SolverTime)+"[ms]" + s + nameof(PreSolverTime)+ "[ms]" + s + nameof(StatusCode) + s + nameof(StatusMessage) + s + nameof(CostSum) + s + nameof(CostsRaw) + s + nameof(SolutionRaw);
        }
        public string ToCSV()
        {
            var s = ";";
            return FullTime.TotalMilliseconds.ToString()+s+SolverTime.TotalMilliseconds.ToString()+s+PreSolverTime.TotalMilliseconds.ToString()+s+StatusCode+s+StatusMessage+s+CostSum+s+CostsRaw.ToListString() + s+SolutionRaw.ToListString();
        }
        public void ToLog(LogLevel lvl)
        {
            if(StatusCode == 1)
            {
                SeqLogger.WriteLog(lvl, "StatusCode: " + StatusCode);
                SeqLogger.WriteLog(lvl, "StatusMessage: " + StatusMessage);
                SeqLogger.WriteLog(lvl, "FullTime: "+ FullTime);
                SeqLogger.WriteLog(lvl, "SolverTime: " + SolverTime);
                SeqLogger.WriteLog(lvl, "PreSolverTime: " + PreSolverTime);
                SeqLogger.WriteLog(lvl, "SolutionRaw: " + SolutionRaw.ToListString());
                SeqLogger.WriteLog(lvl, "CostsRaw: " + CostsRaw.ToListString());
                SeqLogger.WriteLog(lvl, "CostSum: " + CostSum);
                SeqLogger.WriteLog(lvl, "Log size: " + Log.Count+" lines");
                var error = "";
                foreach (var item in ErrorMessage)
                {
                    error = item + "\n";
                }
                SeqLogger.WriteLog(lvl, "Error messages: " + error);
            }
            else
            {
                SeqLogger.WriteLog(lvl, "StatusCode: " + StatusCode);
                SeqLogger.WriteLog(lvl, "StatusMessage: " + StatusMessage);
                SeqLogger.WriteLog(lvl, "FullTime: "+ FullTime);
                SeqLogger.WriteLog(lvl, "SolverTime: " + SolverTime);
                SeqLogger.WriteLog(lvl, "Log size: " + Log.Count+" lines");
                var error = "";
                foreach (var item in ErrorMessage)
                {
                    error = item + "\n";
                }
                SeqLogger.WriteLog(lvl, "Error messages: " + error);
            }
        }
    }
}