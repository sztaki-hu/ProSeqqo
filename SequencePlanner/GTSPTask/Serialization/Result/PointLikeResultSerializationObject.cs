using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Serialization.SerializationObject.Token;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Serialization.Result
{
    public class PointLikeResultSerializationObject: TaskResultSerializationObject
    {
        public List<Position> PositionResult { get; set; }

        public PointLikeResultSerializationObject(PointTaskResult result): base(result)
        {
            PositionResult = result.PositionResult;
        }

        public PointLikeResultSerializationObject(List<string> seqString) : base(seqString)
        {
            var tokenizer = new SEQTokenizer();
            tokenizer.Tokenize(seqString);
            FillBySEQTokens(tokenizer);
        }

        private void FillBySEQTokens(SEQTokenizer tokenizer)
        {
            throw new NotImplementedException();
        }

        public PointTaskResult ToPointLikeResult()
        {
            throw new NotImplementedException();
        }

        public string ToSEQ()
        {
            string seq ="";
            string newline ="\n";
            string d = ";";
            seq += base.ToSEQ();
            //seq += nameof(CostsRaw) + ": " + SeqLogger.ToList(CostsRaw) + newline;
            seq += nameof(PositionResult) + ": " + newline;
            foreach (var position in PositionResult)
            {
                seq += position.UserID +d+ "["+SeqLogger.ToList(position.Vector)+"]"+d+ position.Name +d +position.ResourceID+ newline;
            }
            seq += nameof(Log) + ": " + newline;
            foreach (var line in Log)
            {
                seq += line.Replace(':', '>') + newline;
            }
            return seq;
        }
    }
}