using SequencePlanner.Helper;
using SequencePlanner.Model;
using System;
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
                if (A.Vector.Length == B.Vector.Length)
                {
                    double max = 0;
                    for (int i = 0; i < A.Vector.Length; i++)
                    {
                        if (Math.Abs(A.Vector[i] - B.Vector[i]) > max)
                        {
                            max = Math.Abs(A.Vector[i] - B.Vector[i]);
                        }
                    }
                    return max;
                }
                else
                {
                    SeqLogger.Error("ManhattanDistanceFunction find dimension mismatch position userids: " + A.UserID + "-" + B.UserID);
                    return 0;
                }

            }
        }

        public override void Validate()
        {
            //Nothing to validate
        }
    }
}
