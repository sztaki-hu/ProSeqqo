using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Serialization.SerializationObject.Token;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Serialization.Result
{
    public class PointLikeResultSerializationObject: TaskResultSerializationObject
    {
        public List<Position> PositionResult { get; set; }
        public List<int> ResultIDs { get; set; }

        public PointLikeResultSerializationObject(PointTaskResult result): base(result)
        {
            PositionResult = result.PositionResult;
            ResultIDs = result.ResultIDs;
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
            throw new NotImplementedException();
        }
    }
}