using SequencePlanner.GTSP;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Task.LineLike
{
    internal class LineLikeTaskValidator
    {
        public LineLikeTaskValidator()
        {

        }

        public void Validate(LineLikeTask lineLikeTask)
        {
            SeqLogger.Info("Validation started!", nameof(LineLikeTaskValidator));
            SeqLogger.Indent++;
            if (lineLikeTask.PositionMatrix.DistanceFunction.FunctionName!="MatrixDistance")
                CheckDimension(lineLikeTask.Dimension);
            CheckCycle(lineLikeTask.CyclicSequence, lineLikeTask.StartDepot, lineLikeTask.FinishDepot);
            CheckTimeLimit(lineLikeTask.TimeLimit);
            CheckPositionMatrix(lineLikeTask.PositionMatrix);
            //---BaseTask
            CheckContourPenalty(lineLikeTask.ContourPenalty);
            CheckLines(lineLikeTask.Lines);
            CheckContours(lineLikeTask.Contours);
            CheckLinePrecedences(lineLikeTask.LinePrecedences, lineLikeTask.Lines);
            CheckContourPrecedences(lineLikeTask.ContourPrecedences, lineLikeTask.Contours);
            SeqLogger.Indent--;
            SeqLogger.Info("Validation finished!", nameof(LineLikeTaskValidator));
        }

        private void CheckCycle(bool cyclicSequence, Position startDepot, Position finishDepot)
        {
            if (cyclicSequence)
            {
                if(startDepot == null)
                    throw new SequencerException("In case of CyclicSequence StartDepot needed!");
                if(finishDepot != null)
                    throw new SequencerException("In case of CyclicSequence FinishDepot not useable!");
            }
            SeqLogger.Trace("CheckCycle validated!", nameof(LineLikeTaskValidator));
        }

        private void CheckDimension(int dimension)
        {
            if (dimension <= 0)
                throw new SequencerException("Dimension should be > 0!");
            SeqLogger.Trace("CheckDimension validated!", nameof(LineLikeTaskValidator));
        }

        private void CheckTimeLimit(int timelimit)
        {
            if (timelimit < 0)
                throw new SequencerException("Timelimit should be positive, 0 - NO LIMIT");
            SeqLogger.Trace("CheckTimeLimit validated!", nameof(LineLikeTaskValidator));
        }

        private void CheckPositionMatrix(PositionMatrix positionMatrix)
        {
            if (positionMatrix == null)
                throw new SequencerException("PositionMatrix.PositionMatrix not given.");
            else
            {
                positionMatrix.Validate();
            }

            SeqLogger.Trace("CheckPositionMatrix validated!", nameof(LineLikeTaskValidator));
        }

        private void CheckContourPenalty(double contourPenalty)
        {
            if (contourPenalty < 0)
                throw new SequencerException("LineLike.ContourPenalty should be >=0");

            SeqLogger.Trace("CheckContourPenalty validated!", nameof(LineLikeTaskValidator));
        }

        private void CheckLines(List<Line> lines)
        {
            foreach (var line in lines)
            {
                line.Validate();
            }
            SeqLogger.Trace("CheckLines validated!", nameof(LineLikeTaskValidator));
        }

        private void CheckContours(List<Contour> contours)
        {
            foreach (var contour in contours)
            {
                contour.Validate();
            }
            SeqLogger.Trace("CheckContours validated!", nameof(LineLikeTaskValidator));
        }

        private void CheckLinePrecedences(List<GTSPPrecedenceConstraint> linePrecedences, List<Line> lines)
        {
            foreach (var precedence in linePrecedences)
            {
                precedence.Validate();
                ListContainsPrecedenceItems(precedence, lines);
            }

            SeqLogger.Trace("CheckLinePrecedences validated!", nameof(LineLikeTaskValidator));
        }

        private void CheckContourPrecedences(List<GTSPPrecedenceConstraint> contourPrecedences, List<Contour> contours)
        {
            foreach (var precedence in contourPrecedences)
            {
                precedence.Validate();
                ListContainsPrecedenceItems(precedence, contours);
            }

            SeqLogger.Trace("CheckContourPrecedences validated!", nameof(LineLikeTaskValidator));
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
                throw new SequencerException("Precedence list item.Before/After not found in Line/Contour list.");
        }
    }
}