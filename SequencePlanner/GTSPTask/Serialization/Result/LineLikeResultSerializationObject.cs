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
        public List<int> Result { get; set; }
        public List<int> ResultWithVirtual { get; set; }
        public double CostBetweenLines { get; set; }
        public double CostBetweenLinesNoPenalty { get; set; }
        public double Penalty { get; set; }
        public double CostSumNoPenalty { get; set; }
        public double CostOfLines { get; set; }
        public List<double> CostOfLinesList { get; set; }

        public LineLikeResultSerializationObject(LineTaskResult result): base(result)
        {
            CostBetweenLines = result.CostBetweenLines;
            Penalty = result.Penalty;
            LineResults = new List<LineSerializationObject>();
            foreach (var line in result.LineResult)
            {
                LineResults.Add(new LineSerializationObject()
                {
                    LineID = line.UserID,
                    PositionIDA = line.NodeA.UserID,
                    PositionIDB = line.NodeB.UserID,
                    Name = line.Name,
                    Bidirectional = line.Bidirectional,
                    ResourceID = line.ResourceID
                });
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