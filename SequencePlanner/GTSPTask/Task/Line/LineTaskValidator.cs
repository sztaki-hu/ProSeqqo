using SequencePlanner.Model;
using SequencePlanner.GTSPTask.Task.Base;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Task.LineTask
{
    internal class LineTaskValidator
    {
        public LineTaskValidator()
        {

        }

        public void Validate(LineTask lineTask)
        {
            SeqLogger.Debug("Validation started!", nameof(LineTaskValidator));
            SeqLogger.Indent++;
            
            if (lineTask.PositionMatrix.DistanceFunction.FunctionName!="Matrix")
                CheckDimension(lineTask.Dimension);
            CheckCycle(lineTask.CyclicSequence, lineTask.StartDepot, lineTask.FinishDepot);
            CheckTimeLimit(lineTask.TimeLimit);
            CheckPositionMatrix(lineTask.PositionMatrix);
            BaseTaskValidator baseTaskValidator = new BaseTaskValidator();
            baseTaskValidator.Validate((BaseTask)lineTask);
            CheckContourPenalty(lineTask.ContourPenalty);
            CheckLines(lineTask.Lines);
            CheckContours(lineTask.Contours);
            CheckLinePrecedences(lineTask.LinePrecedences, lineTask.Lines);
            CheckContourPrecedences(lineTask.ContourPrecedences, lineTask.Contours);
            SeqLogger.Indent--;
            SeqLogger.Debug("Validation finished!", nameof(LineTaskValidator));
        }

        private void CheckCycle(bool cyclicSequence, Position startDepot, Position finishDepot)
        {
            if (cyclicSequence)
            {
                if(startDepot == null)
                    throw new SeqException("In case of CyclicSequence StartDepot needed!");
                if(finishDepot != null)
                    throw new SeqException("In case of CyclicSequence FinishDepot not useable!");
            }
            SeqLogger.Trace("CheckCycle validated!", nameof(LineTaskValidator));
        }

        private void CheckDimension(int dimension)
        {
            if (dimension <= 0)
                throw new SeqException("Dimension should be > 0!");
            SeqLogger.Trace("CheckDimension validated!", nameof(LineTaskValidator));
        }

        private void CheckTimeLimit(int timelimit)
        {
            if (timelimit < 0)
                throw new SeqException("Timelimit should be positive, 0 - NO LIMIT");
            SeqLogger.Trace("CheckTimeLimit validated!", nameof(LineTaskValidator));
        }

        private void CheckPositionMatrix(PositionMatrix positionMatrix)
        {
            if (positionMatrix == null)
                throw new SeqException("PositionMatrix.PositionMatrix not given.");
            else
            {
                positionMatrix.Validate();
            }

            SeqLogger.Trace("CheckPositionMatrix validated!", nameof(LineTaskValidator));
        }

        private void CheckContourPenalty(double contourPenalty)
        {
            if (contourPenalty < 0)
                throw new SeqException("LineTask.ContourPenalty should be >=0");

            SeqLogger.Trace("CheckContourPenalty validated!", nameof(LineTaskValidator));
        }

        private void CheckLines(List<Line> lines)
        {
            foreach (var line in lines)
            {
                line.Validate();
            }
            SeqLogger.Trace("CheckLines validated!", nameof(LineTaskValidator));
        }

        private void CheckContours(List<Contour> contours)
        {
            foreach (var contour in contours)
            {
                contour.Validate();
            }
            SeqLogger.Trace("CheckContours validated!", nameof(LineTaskValidator));
        }

        private void CheckLinePrecedences(List<GTSPPrecedenceConstraint> linePrecedences, List<Line> lines)
        {
            foreach (var precedence in linePrecedences)
            {
                precedence.Validate();
                ListContainsPrecedenceItems(precedence, lines);
            }

            SeqLogger.Trace("CheckLinePrecedences validated!", nameof(LineTaskValidator));
        }

        private void CheckContourPrecedences(List<GTSPPrecedenceConstraint> contourPrecedences, List<Contour> contours)
        {
            foreach (var precedence in contourPrecedences)
            {
                precedence.Validate();
                ListContainsPrecedenceItems(precedence, contours);
            }

            SeqLogger.Trace("CheckContourPrecedences validated!", nameof(LineTaskValidator));
        }

        private void ListContainsPrecedenceItems(GTSPPrecedenceConstraint precedence, IEnumerable<BaseNode> nodes)
        {
            var findAfter = false;
            var findBefore = false;
            foreach (var node in nodes)
            {
                if (node.GlobalID == precedence.Before.GlobalID)
                    findBefore = true;
                if (node.GlobalID == precedence.After.GlobalID)
                    findAfter = true;
                if (findBefore && findAfter)
                    return;
            }
            if (!findBefore || !findAfter)
                throw new SeqException("Precedence list item.Before/After not found in Line/Contour list.");
        }
    }
}