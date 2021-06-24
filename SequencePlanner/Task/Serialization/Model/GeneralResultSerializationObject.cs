using SequencePlanner.Helper;
using SequencePlanner.Model.Hierarchy;
using SequencePlanner.Task;
using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Serialization.Result
{
    public class GeneralResultSerializationObject
    {
        public string Created { get { return DateTime.UtcNow.ToString(); } }
        public string FullTime { get; set; }
        public string SolverTime { get; set; }
        public string PreSolverTime { get; set; }
        public List<int> MotionIDs { get; set; }
        public List<double> MotionCosts { get; }
        public double CostSum { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public List<string> Log { get; set; }
        public List<Motion> MotionResult { get; set; }
        public List<Config> ConfigResult { get; set; }

        public GeneralResultSerializationObject()
        {
            MotionIDs = new List<int>();
            MotionCosts = new List<double>();
            Log = new List<string>();
            MotionResult = new List<Motion>();
            ConfigResult = new List<Config>();
        }

        public GeneralResultSerializationObject(GeneralTaskResult result)
        {
            FullTime = result.FullTime.ToString();
            SolverTime = result.SolverTime.ToString();
            PreSolverTime = result.PreSolverTime.ToString();
            MotionIDs = result.SolutionMotionIDs;
            MotionCosts = result.MotionCosts;
            CostSum = result.FullMotionCost;
            StatusCode = result.StatusCode;
            StatusMessage = result.StatusMessage;
            //Log = result.Log;
            MotionResult = result.SolutionMotion;
            ConfigResult = result.SolutionConfig;
        }

        public GeneralResultSerializationObject(List<string> seqString)
        {
            throw new NotImplementedException();
        }

        public GeneralTaskResult ToGeneralResult()
        {
            var result = new GeneralTaskResult();
            if (FullTime is not null)
                result.FullTime = TimeSpan.Parse(FullTime);
            if (SolverTime is not null)
                result.SolverTime = TimeSpan.Parse(SolverTime);
            if (PreSolverTime is not null)
                result.PreSolverTime = TimeSpan.Parse(PreSolverTime);
            result.SolutionMotion = MotionResult;
            result.SolutionConfig = ConfigResult;
            result.FullMotionCost = CostSum;
            result.StatusCode = StatusCode;
            result.StatusMessage = StatusMessage;
            //result.Log = Log;
            return result;
        }

        public new string ToSEQ()
        {
            string seq = "";
            string newline = "\n";
            string d = ";";
            seq += nameof(Created) + ": " + Created + newline;
            seq += nameof(StatusCode) + ": " + StatusCode + newline;
            seq += nameof(StatusMessage) + ": " + StatusMessage + newline;
            seq += nameof(FullTime) + ": " + FullTime + newline;
            seq += nameof(SolverTime) + ": " + SolverTime + newline;
            seq += nameof(PreSolverTime) + ": " + PreSolverTime + newline;
            seq += nameof(CostSum) + ": " + CostSum + newline;
            seq += nameof(MotionIDs) + ": " + MotionIDs.ToListString() + newline;
            seq += nameof(MotionCosts) + ": " + MotionCosts.ToListString() + newline;
            //if (StatusCode == 1)
            //{
            //    //seq += nameof(CostsRaw) + ": " + SeqLogger.ToList(CostsRaw) + newline;
            //    seq += nameof(PositionResult) + ": " + newline;
            //    foreach (var position in PositionResult)
            //    {
            //        seq += position.Node.UserID + d + "[" + position.In.Vector.ToListString() + "]" + d + position.Node.Name + d + position.Node.ResourceID + newline;
            //    }
            //    seq += nameof(Log) + ": " + newline;
            //    foreach (var line in Log)
            //    {
            //        seq += line.Replace(':', '>') + newline;
            //    }
            //}
            return seq;
        }
    }
}