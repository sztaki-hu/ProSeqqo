using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Serialization.SerializationObject.Token;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Serialization.Result
{
    public class LineLikeResultSerializationObject: TaskResultSerializationObject
    {
        public List<Line> LineResult { get; set; }
        public List<Position> PositionResult { get; set; }

        public LineLikeResultSerializationObject() : base()
        {
            LineResult = new List<Line>();
            PositionResult = new List<Position>();
        }

        public LineLikeResultSerializationObject(LineTaskResult result): base(result)
        {
            LineResult = result.LineResult;
            PositionResult = result.PositionResult;
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
            var result = new LineTaskResult();
            result = (LineTaskResult)base.ToTaskResult(result);
            result.LineResult = LineResult;
            result.PositionResult = PositionResult;
            return result;
        }

        public string ToSEQ()
        {
            string seq = "";
            string newline = "\n";
            string d = ";";
            seq += base.ToSEQ();
            //seq += nameof(CostsRaw) + ": " + SeqLogger.ToList(CostsRaw) + newline;
            seq += nameof(LineResult) + ": " + newline;
            foreach (var line in LineResult)
            {
                //seq += line.LineID + d + line.PositionIDA + d + line.PositionIDA + line.Name + d + line.ResourceID + newline;
                seq += line.UserID + d + line.NodeA.UserID + d + line.NodeB.UserID +d+ line.Name + d + line.ResourceID + newline;
            }
            seq += nameof(PositionResult) + ": " + newline;
            foreach (var postition in PositionResult)
            {
                seq += postition.UserID + d + SeqLogger.ToList(postition.Vector) + d + postition.Name + d + postition.ResourceID + newline;
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