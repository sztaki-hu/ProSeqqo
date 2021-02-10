using SequencePlanner.GTSP;
using SequencePlanner.GTSPTask.Task.PointLike;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Task.LineLike
{
    internal class PointLikeTaskValidator
    {
        public PointLikeTaskValidator()
        {

        }

        //TODO: PositionPrecedence only between processes, no inside alternatives and between alternatives in one process.

        public void Validate(PointLikeTask lineLikeTask)
        {
            SeqLogger.Info("Validation started!", nameof(PointLikeTaskValidator));
            SeqLogger.Indent++;
            if (lineLikeTask.PositionMatrix.DistanceFunction.FunctionName!="MatrixDistance")
                CheckDimension(lineLikeTask.Dimension);
            CheckCycle(lineLikeTask.CyclicSequence, lineLikeTask.StartDepot, lineLikeTask.FinishDepot);
            CheckTimeLimit(lineLikeTask.TimeLimit);
            CheckPositionMatrix(lineLikeTask.PositionMatrix);
            SeqLogger.Indent--;
            SeqLogger.Info("Validation finished!", nameof(PointLikeTaskValidator));
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
            SeqLogger.Trace("CheckCycle validated!", nameof(PointLikeTaskValidator));
        }

        private void CheckDimension(int dimension)
        {
            if (dimension <= 0)
                throw new SeqException("Dimension should be > 0!");
            SeqLogger.Trace("CheckDimension validated!", nameof(PointLikeTaskValidator));
        }

        private void CheckTimeLimit(int timelimit)
        {
            if (timelimit < 0)
                throw new SeqException("Timelimit should be positive, 0 - NO LIMIT");
            SeqLogger.Trace("CheckDimension validated!", nameof(PointLikeTaskValidator));
        }

        private void CheckPositionMatrix(PositionMatrix positionMatrix)
        {
            if (positionMatrix == null)
                throw new SeqException("PositionMatrix.PositionMatrix not given.");
            else
            {
                positionMatrix.Validate();
            }
            SeqLogger.Trace("CheckPositionMatrix validated!", nameof(PointLikeTaskValidator));
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