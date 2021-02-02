using SequencePlanner.Helper;
using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.Function.DistanceFunction
{
    public class ManhattanDistanceFunction : DistanceFunction
    {
        public ManhattanDistanceFunction(): base()
        {
            FunctionName = "ManhattanDistance";
        }

        public override double ComputeDistance(Position A, Position B)
        {
            if (A == null || B == null)
                throw new SequencerException("ManhattanDistanceFunction A/B position null!");
            if (A.Dimension != B.Dimension)
                throw new SequencerException("ManhattanDistanceFunction found dimendion mismatch!", "Check dimension of Positions with " + A.UserID + ", " + B.UserID);

            var givenDistance = GetStrictEdgeWeight(A, B);
            if (givenDistance != null)
                return givenDistance.Weight;
            else
            {
                SeqLogger.Error("ManhattanDistanceFunction not implemented yet!", nameof(ManhattanDistanceFunction));
                throw new SequencerException("ManhattanDistanceFunction not implemented yet!");
                return 3;
            }
        }

        public override void Validate()
        {
            //Nothing to Validate
        }
    }
}
