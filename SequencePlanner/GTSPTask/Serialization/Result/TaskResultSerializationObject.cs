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
