using System;
using System.Collections.Generic;
using SequencePlanner.Helper;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Serialization.SerializationObject.Token;

namespace SequencePlanner.GTSPTask.Serialization.Result
{
    public class TaskResultSerializationObject
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


        public TaskResultSerializationObject()
        {
            SolutionRaw = new List<long>();
            CostsRaw = new List<double>();
            Log = new List<string>();
        }
        public TaskResultSerializationObject(List<string> seqString)
        {
            var tokenizer = new SEQTokenizer();
            tokenizer.Tokenize(seqString);
            FillBySEQTokens(tokenizer);
        }
        public TaskResultSerializationObject(TaskResult result)
        {
            FullTime = result.FullTime.ToString();
            SolverTime = result.SolverTime.ToString();
            PreSolverTime = result.PreSolverTime.ToString();
            SolutionRaw = result.SolutionRaw;
            CostsRaw = result.CostsRaw;
            CostSum = result.CostSum;
            StatusCode = result.StatusCode;
            StatusMessage = result.StatusMessage;
            Log = result.Log;
        }


        public TaskResult ToTaskResult(TaskResult result)
        {
            if(FullTime is not null)
                result.FullTime = TimeSpan.Parse(FullTime);
            if(SolverTime is not null)
                result.SolverTime =  TimeSpan.Parse(SolverTime);
            if(PreSolverTime is not null)
                result.PreSolverTime =  TimeSpan.Parse(PreSolverTime);
            result.SolutionRaw = SolutionRaw;
            result.CostsRaw = CostsRaw;
            result.CostSum = CostSum;
            result.StatusCode = StatusCode;
            result.StatusMessage = StatusMessage;
            result.Log = Log;
            return result;
        }
        private void FillBySEQTokens(SEQTokenizer tokenizer)
        {
            throw new NotImplementedException();
        }
        public  string ToSEQ()
        {
            var seq = "";
            var newline = "\n";
            seq += nameof(Created) + ": " + Created + newline;
            seq += nameof(StatusCode) + ": " + StatusCode + newline;
            seq += nameof(StatusMessage) + ": " + StatusMessage + newline;
            seq += nameof(FullTime) + ": " + FullTime + newline;
            seq += nameof(SolverTime) + ": " + SolverTime + newline;
            seq += nameof(PreSolverTime) + ": " + PreSolverTime + newline;
            seq += nameof(CostSum) + ": " + CostSum + newline;
            seq += nameof(SolutionRaw) + ": " + SolutionRaw.ToListString() + newline;
            seq += nameof(CostsRaw) + ": " + CostsRaw.ToListString() + newline;
            return seq;
        }
    }
}