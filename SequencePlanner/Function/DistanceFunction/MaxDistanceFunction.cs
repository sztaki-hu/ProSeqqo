using SequencePlanner.Helper;
using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.Function.DistanceFunction
{
    public class MaxDistanceFunction : DistanceFunction
    {
        public MaxDistanceFunction() : base()
        {
            FunctionName = "MaxDistance";
        }

        public override double ComputeDistance(Position A, Position B)
        {
            if (A == null || B == null)
                throw new SequencerException("MaxDistanceFunction A/B position null!");
            if (A.Dimension != B.Dimension)
                throw new SequencerException("MaxDistanceFunction found dimendion mismatch!", "Check dimension of Positions with " + A.UserID + ", " + B.UserID);
            var givenDistance = GetStrictEdgeWeight(A, B);
            if (givenDistance != null)
                return givenDistance.Weight;
            else
            {
                SeqLogger.Error("MaxDistanceFunction not implemented yet!", nameof(MaxDistanceFunction));
                throw new SequencerException("MaxDistanceFunction not implemented yet!");
                return 2;
            }
        }

        public override void Validate()
        {
            //Nothing to validate
        }
    }
}
