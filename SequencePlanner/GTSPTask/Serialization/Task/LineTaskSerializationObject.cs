﻿using System.Collections.Generic;
using SequencePlanner.Model;
using SequencePlanner.Helper;
using SequencePlanner.GTSPTask.Task.Base;
using SequencePlanner.GTSPTask.Task.LineTask;
using SequencePlanner.GTSPTask.Serialization.SerializationObject;
using SequencePlanner.GTSPTask.Serialization.SerializationObject.Token;

namespace SequencePlanner.GTSPTask.Serialization.Task
{
    public class LineTaskSerializationObject: BaseTaskSerializationObject
    {
        public List<LineSerializationObject> LineList { get; set; }
        public List<OrderConstraintSerializationObject> LinePrecedences { get; set; }
        public List<OrderConstraintSerializationObject> ContourPrecedences { get; set; }
        public double ContourPenalty { get; set; }


        public LineTaskSerializationObject() : base() { }
        public LineTaskSerializationObject(List<string> seqString): base()
        {
            var tokenizer = new SEQTokenizer();
            tokenizer.Tokenize(seqString);
            FillBySEQTokens(tokenizer);
        }
        public LineTaskSerializationObject(LineTask task):base(task)
        {
            TaskType = "Line";
            LineList = new List<LineSerializationObject>();
            LinePrecedences = new List<OrderConstraintSerializationObject>();
            ContourPrecedences = new List<OrderConstraintSerializationObject>();
            ContourPenalty = task.ContourPenalty;
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


        public LineTask ToLineTask()
        {
            var LineTask = new LineTask();
            base.ToBaseTask((BaseTask) LineTask);
            LineTask.ContourPenalty = ContourPenalty;
            CreateLinesAndContours(LineTask);
            CreatePrecedences(LineTask);
            return LineTask;
        }
        public new void FillBySEQTokens(SEQTokenizer tokenizer)
        {
            base.FillBySEQTokens(tokenizer);
            ContourPenalty = tokenizer.GetDoubleByHeader("ContourPenalty");
            LinePrecedences = tokenizer.GetPrecedenceListByHeader("LinePrecedence");
            ContourPrecedences = tokenizer.GetPrecedenceListByHeader("ContourPrecedence");
            LineList = tokenizer.GetLineListByHeader("LineList");
        }
        public string ToSEQ()
        {
            string seq = "";
            string newline = "\n";
            seq+=base.ToSEQShort();
            seq += "ContourPenalty: " + ContourPenalty + newline;
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
      
        
        private void CreateLinesAndContours(LineTask lineTask)
        {
            foreach (var line in LineList)
            {
                Line lineObj = new Line()
                {
                    UserID = line.LineID,
                    Name = line.Name,
                    NodeA = FindPosition(line.PositionIDA, lineTask.PositionMatrix.Positions),
                    NodeB = FindPosition(line.PositionIDB, lineTask.PositionMatrix.Positions),
                    Bidirectional = line.Bidirectional,
                    ResourceID = line.ResourceID
                };
                lineTask.Lines.Add(lineObj);
            
                Contour contourObj = FindContour(line.ContourID, lineTask.Contours);
                if(contourObj == null)
                {
                    contourObj = new Contour()
                    {
                        UserID = line.ContourID
                    };
                    lineTask.Contours.Add(contourObj);
                }
                contourObj.Lines.Add(lineObj);
            }
        }
        private void CreatePrecedences(LineTask lineTask)
        {
            foreach (var linePrec in LinePrecedences)
            {
                var before = FindLine(linePrec.BeforeID, lineTask.Lines);
                var after = FindLine(linePrec.AfterID, lineTask.Lines);
                if (before == null || after == null)
                    throw new SeqException("Phrase error line precedence user id not found!");
                lineTask.LinePrecedences.Add(new GTSPPrecedenceConstraint()
                {
                    Before = before,
                    After = after
                });
            }

            foreach (var contourPrec in ContourPrecedences)
            {
                var before = FindContour(contourPrec.BeforeID, lineTask.Contours);
                var after = FindContour(contourPrec.AfterID, lineTask.Contours);
                if (before == null || after == null)
                    throw new SeqException("Phrase error contour precedence user id not found!");
                lineTask.ContourPrecedences.Add(new GTSPPrecedenceConstraint()
                {
                    Before = before,
                    After = after
                });
            }
        }
        private static Position FindPosition(int ID, List<GTSPNode> posList)
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
        private static Line FindLine(int ID, List<Line> lineList)
        {
            foreach (var line in lineList)
            {
                if (line.UserID == ID)
                    return line;
            }
            return null;
        }
        private static Contour FindContour(int ID, List<Contour> contourList)
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