using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Serialization.SerializationObject;
using SequencePlanner.GTSPTask.Serialization.SerializationObject.Token;
using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Serialization.Result
{
    internal class LineLikeResultSerializationObject: TaskResultSerializationObject
    {
        public List<LineSerializationObject> LineResults { get; set; }

        public LineLikeResultSerializationObject(LineTaskResult result): base(result)
        {
            LineResults = new List<LineSerializationObject>();
            foreach (var line in result.LineResult)
            {
                LineResults.Add(new LineSerializationObject(line));
            }
        }

        public LineLikeResultSerializationObject(List<string> seqString): base(seqString)
        {
            var tokenizer = new SEQTokenizer();
            tokenizer.Tokenize(seqString);
            FillBySEQTokens(tokenizer);
        }

        private void FillBySEQTokens(SEQTokenizer tokenizer)
        {
            throw new NotImplementedException();
        }

        public LineTaskResult ToLineLikeResult()
        {
            throw new NotImplementedException();
        }

        public string ToSEQ()
        {
            throw new NotImplementedException();
        }
    }
}