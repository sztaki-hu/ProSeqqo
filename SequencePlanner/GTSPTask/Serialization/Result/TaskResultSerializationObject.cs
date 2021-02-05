using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Serialization.SerializationObject.Token;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSPTask.Serialization.Result
{
    public class TaskResultSerializationObject
    {
        public string Time { get; set; }
        public List<long> SolutionRaw { get; set; }
        public List<double> Costs { get; }
        public double CostSum { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }

        public TaskResultSerializationObject(TaskResult result)
        {
            Time = result.FullTime.ToString();
            SolutionRaw = result.SolutionRaw;
            Costs = result.CostsRaw;
            CostSum = result.CostSum;
            StatusCode = result.StatusCode;
            StatusMessage = result.StatusMessage;

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
            seq += nameof(SolutionRaw) + ": " + SeqLogger.ToList(SolutionRaw) + newline;
            seq += nameof(CostsRaw) + ": " + SeqLogger.ToList(CostsRaw) + newline;
            seq += nameof(Log) + ": " + newline;
            foreach (var line in Log)
            {
                seq += line.Replace(':', '>') + newline;
            }
            return seq;

        }

        public TaskResultSerializationObject(List<string> seqString)
        {
            var tokenizer = new SEQTokenizer();
            tokenizer.Tokenize(seqString);
            FillBySEQTokens(tokenizer);
        }

        private void FillBySEQTokens(SEQTokenizer tokenizer)
        {
            throw new NotImplementedException();
        }
    }
}
