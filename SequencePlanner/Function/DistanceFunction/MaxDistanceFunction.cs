using SequencePlanner.Helper;
using SequencePlanner.Model;
using System;

namespace SequencePlanner.Function.DistanceFunction
{
    public class MaxDistanceFunction : DistanceFunction
    {
        public override string FunctionName { get { return "Max"; } }


        public override double ComputeDistance(Position A, Position B)
        {
            if (A == null || B == null)
                throw new SeqException("MaxDistanceFunction A/B position null!");
            if (A.Dimension != B.Dimension)
                throw new SeqException("MaxDistanceFunction found dimendion mismatch!", "Check dimension of Positions with " + A.UserID + ", " + B.UserID);

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
                SeqLogger.Error("MaxDistanceFunction find dimension mismatch position userids: " + A.UserID + "-" + B.UserID);
                return 0;
            }
        }
    }
}