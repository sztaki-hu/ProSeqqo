using System;
using System.Collections.Generic;
using SequencePlanner.Helper;
using SequencePlanner.Task;

namespace SequencePlanner.GTSPTask.Serialization.Result
{
    public class GeneralResultSerializationObject
    {
        public string Created { get { return DateTime.UtcNow.ToString(); } }
        public string FullTime { get; set; }
        public string SolverTime { get; set; }
        public string PreSolverTime { get; set; }
        public List<long> SolutionRaw { get; set; }
        public List<double> CostsRaw { get; }
        public double CostSum { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public List<string> Log { get; set; }
        //public List<GTSPNode> PositionResult { get; set; }

        public GeneralResultSerializationObject()
        {
            SolutionRaw = new List<long>();
            CostsRaw = new List<double>();
            Log = new List<string>();
            //PositionResult = new List<GTSPNode>();
        }

        public GeneralResultSerializationObject(GeneralTaskResult result)
        {
            //FullTime = result.FullTime.ToString();
            //SolverTime = result.SolverTime.ToString();
            //PreSolverTime = result.PreSolverTime.ToString();
            //SolutionRaw = result.SolutionRaw;
            //CostsRaw = result.CostsRaw;
            //CostSum = result.CostSum;
            //StatusCode = result.StatusCode;
            //StatusMessage = result.StatusMessage;
            //Log = result.Log;
            //PositionResult = result.PositionResult;
        }

        public GeneralResultSerializationObject(List<string> seqString) 
        {
            throw new NotImplementedException();
        }

        public GeneralTaskResult ToGeneralResult()
        {
            var result = new GeneralTaskResult();
            //result.PositionResult = PositionResult;
            return result;
        }

        public GeneralTaskResult ToTaskResult(GeneralTaskResult result)
        {
            //if (FullTime is not null)
            //    result.FullTime = TimeSpan.Parse(FullTime);
            //if (SolverTime is not null)
            //    result.SolverTime = TimeSpan.Parse(SolverTime);
            //if (PreSolverTime is not null)
            //    result.PreSolverTime = TimeSpan.Parse(PreSolverTime);
            //result.SolutionRaw = SolutionRaw;
            //result.CostsRaw = CostsRaw;
            //result.CostSum = CostSum;
            //result.StatusCode = StatusCode;
            //result.StatusMessage = StatusMessage;
            //result.Log = Log;
            return result;
        }

        public new string ToSEQ()
        {
            string seq ="";
            string newline ="\n";
            string d = ";";
            seq += nameof(Created) + ": " + Created + newline;
            seq += nameof(StatusCode) + ": " + StatusCode + newline;
            seq += nameof(StatusMessage) + ": " + StatusMessage + newline;
            seq += nameof(FullTime) + ": " + FullTime + newline;
            seq += nameof(SolverTime) + ": " + SolverTime + newline;
            seq += nameof(PreSolverTime) + ": " + PreSolverTime + newline;
            seq += nameof(CostSum) + ": " + CostSum + newline;
            seq += nameof(SolutionRaw) + ": " + SolutionRaw.ToListString() + newline;
            seq += nameof(CostsRaw) + ": " + CostsRaw.ToListString() + newline;
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