using Newtonsoft.Json;
using SequencePlanner.GTSPTask.Serialization.SerializationObject;
using SequencePlanner.GTSPTask.Serialization.SerializationObject.Token;
using SequencePlanner.GTSPTask.Task.Base;
using SequencePlanner.GTSPTask.Task.LineLike;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Serialization.Task
{
    public class LineLikeTaskSerializationObject: BaseTaskSerializationObject
    {
        [JsonProperty(Order = 12)]
        public List<LineSerializationObject> LineList { get; set; }
        [JsonProperty(Order = 13)]
        public List<OrderConstraintSerializationObject> LinePrecedences { get; set; }
        [JsonProperty(Order = 14)]
        public List<OrderConstraintSerializationObject> ContourPrecedences { get; set; }
        [JsonProperty(Order = 10)]
        public double ContourPenalty { get; set; }
        [JsonProperty(Order = 11)]
        public bool BidirectionLineDefault { get; set; }

        public LineLikeTaskSerializationObject() : base()
        {
        }
        public LineLikeTaskSerializationObject(List<string> seqString): base(seqString)
        {
            var tokenizer = new SEQTokenizer();
            tokenizer.Tokenize(seqString);
            FillBySEQTokens(tokenizer);
        }
        public LineLikeTaskSerializationObject(LineLikeTask task):base(task)
        {
            TaskType = "LineLike";
            LineList = new List<LineSerializationObject>();
            LinePrecedences = new List<OrderConstraintSerializationObject>();
            ContourPrecedences = new List<OrderConstraintSerializationObject>();
            ContourPenalty = task.ContourPenalty;
            BidirectionLineDefault = Line.BIDIRECTIONAL_DEFAULT;
            foreach (var contour in task.Contours)
            {
                foreach (var line in contour.Lines)
                {
                    LineList.Add(new LineSerializationObject()
                    {
                        LineID = line.UserID,
                        PositionIDA = line.NodeA.UserID,
                        PositionIDB = line.NodeB.UserID,
                        ContourID = contour.UserID,
                        Name = line.Name,
                        Bidirectional = line.Bidirectional,
                        ResourceID = line.ResourceID
                    });
                }
            }
            foreach (var linePrec in task.LinePrecedences)
            {
                LinePrecedences.Add(new OrderConstraintSerializationObject()
                {
                    BeforeID = linePrec.Before.UserID,
                    AfterID = linePrec.After.UserID
                });
            }
            foreach (var countourPrec in task.ContourPrecedences)
            {
                ContourPrecedences.Add(new OrderConstraintSerializationObject()
                {
                    BeforeID = countourPrec.Before.UserID,
                    AfterID = countourPrec.After.UserID
                });
            }
        }

        public LineLikeTask ToLineLikeTask()
        {
            var LineLikeTask = new LineLikeTask();
            base.ToBaseTask((BaseTask) LineLikeTask);
            LineLikeTask.ContourPenalty = ContourPenalty;
            Line.BIDIRECTIONAL_DEFAULT = BidirectionLineDefault;
            CreateLinesAndContours(LineLikeTask);
            CreatePrecedences(LineLikeTask);
            return LineLikeTask;
        }
        public void FillBySEQTokens(SEQTokenizer tokenizer)
        {
            base.FillBySEQTokens(tokenizer);
            ContourPenalty = TokenConverter.GetDoubleByHeader("ContourPenalty", tokenizer);
            BidirectionLineDefault = TokenConverter.GetBoolByHeader("BidirectionLineDefault", tokenizer);
            Line.BIDIRECTIONAL_DEFAULT = BidirectionLineDefault;
            LinePrecedences = TokenConverter.GetPrecedenceListByHeader("LinePrecedence", tokenizer);
            ContourPrecedences = TokenConverter.GetPrecedenceListByHeader("ContourPrecedence", tokenizer);
            LineList = TokenConverter.GetLineListByHeader("LineList", tokenizer);
        }
        public string ToSEQ()
        {
            string seq = "";
            string newline = "\n";
            seq+=base.ToSEQShort();
            seq += "ContourPenalty: " + ContourPenalty + newline;
            seq += "BidirectionLineDefault: " + BidirectionLineDefault + newline;
            seq += newline + "LinePrecedence:" + newline;
            foreach (var prec in LinePrecedences)
            {
                seq += prec.ToSEQ();
            }
            seq += newline + "ContourPrecedence:" + newline;
            foreach (var prec in ContourPrecedences)
            {
                seq += prec.ToSEQ();
            }
            seq += newline + base.ToSEQLong();
            seq += newline + "LineList:" + newline;
            foreach (var line in LineList)
            {
                seq += line.ToSEQ();
            }
            return seq;
        }
        private void CreateLinesAndContours(LineLikeTask lineLikeTask)
        {
            foreach (var line in LineList)
            {
                Line lineObj = new Line()
                {
                    UserID = line.LineID,
                    Name = line.Name,
                    NodeA = FindPosition(line.PositionIDA, lineLikeTask.PositionMatrix.Positions),
                    NodeB = FindPosition(line.PositionIDB, lineLikeTask.PositionMatrix.Positions),
                    Bidirectional = line.Bidirectional,
                    ResourceID = line.ResourceID
                };
                lineLikeTask.Lines.Add(lineObj);
            
                Contour contourObj = FindContour(line.ContourID, lineLikeTask.Contours);
                if(contourObj == null)
                {
                    contourObj = new Contour()
                    {
                        UserID = line.ContourID
                    };
                    lineLikeTask.Contours.Add(contourObj);
                }
                contourObj.Lines.Add(lineObj);
            }
        }
        private void CreatePrecedences(LineLikeTask lineLikeTask)
        {
            foreach (var linePrec in LinePrecedences)
            {
                var before = FindLine(linePrec.BeforeID, lineLikeTask.Lines);
                var after = FindLine(linePrec.AfterID, lineLikeTask.Lines);
                if (before == null || after == null)
                    throw new SeqException("Phrase error line precedence user id not found!");
                lineLikeTask.LinePrecedences.Add(new GTSP.GTSPPrecedenceConstraint()
                {
                    Before = before,
                    After = after
                });
            }

            foreach (var contourPrec in ContourPrecedences)
            {
                var before = FindContour(contourPrec.BeforeID, lineLikeTask.Contours);
                var after = FindContour(contourPrec.AfterID, lineLikeTask.Contours);
                if (before == null || after == null)
                    throw new SeqException("Phrase error contour precedence user id not found!");
                lineLikeTask.ContourPrecedences.Add(new GTSP.GTSPPrecedenceConstraint()
                {
                    Before = before,
                    After = after
                });
            }
        }
        private Position FindPosition(int ID, List<GTSPNode> posList)
        {
            foreach (var pos in posList)
            {
                if (pos.In.UserID == ID)
                    return pos.In;
                if (pos.Out.UserID == ID)
                    return pos.Out;
            }
            return null;
        }
        private Line FindLine(int ID, List<Line> lineList)
        {
            foreach (var line in lineList)
            {
                if (line.UserID == ID)
                    return line;
            }
            return null;
        }
        private Contour FindContour(int ID, List<Contour> contourList)
        {
            foreach (var contour in contourList)
            {
                if (contour.UserID == ID)
                    return contour;
            }
            return null;
        }
    }
}