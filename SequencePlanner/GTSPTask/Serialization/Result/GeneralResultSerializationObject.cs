using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Serialization.SerializationObject.Token;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Serialization.Result
{
    public class GeneralResultSerializationObject: TaskResultSerializationObject
    {
        public List<GTSPNode> PositionResult { get; set; }

        public GeneralResultSerializationObject() : base()
        {
            PositionResult = new List<GTSPNode>();
        }

        public GeneralResultSerializationObject(GeneralTaskResult result): base(result)
        {
            PositionResult = result.PositionResult;
        }

        public GeneralResultSerializationObject(List<string> seqString) : base(seqString)
        {
            var tokenizer = new SEQTokenizer();
            tokenizer.Tokenize(seqString);
            FillBySEQTokens(tokenizer);
        }

        private void FillBySEQTokens(SEQTokenizer tokenizer)
        {
            throw new NotImplementedException();
        }

        public GeneralTaskResult ToGeneralResult()
        {
            var result = new GeneralTaskResult();
            result = (GeneralTaskResult)base.ToTaskResult(result);
            result.PositionResult = PositionResult;
            return result;
        }

        public string ToSEQ()
        {
            string seq ="";
            string newline ="\n";
            string d = ";";
            seq += base.ToSEQ();
            if (StatusCode == 1)
            {
                //seq += nameof(CostsRaw) + ": " + SeqLogger.ToList(CostsRaw) + newline;
                seq += nameof(PositionResult) + ": " + newline;
                foreach (var position in PositionResult)
                {
                    seq += position.Node.UserID + d + "[" + SeqLogger.ToList(position.In.Vector) + "]" + d + position.Node.Name + d + position.Node.ResourceID + newline;
                }
                seq += nameof(Log) + ": " + newline;
                foreach (var line in Log)
                {
                    seq += line.Replace(':', '>') + newline;
                }
            }
            return seq;
        }
    }
}