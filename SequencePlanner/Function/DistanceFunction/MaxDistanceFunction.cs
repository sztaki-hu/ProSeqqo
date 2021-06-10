using SequencePlanner.Helper;
using SequencePlanner.Model.Hierarchy;
using System;

namespace SequencePlanner.Function.DistanceFunction
{
    public class MaxDistanceFunction : DistanceFunction
    {
        public override string FunctionName { get { return "Max"; } }


        public override double ComputeDistance(Config A, Config B)
        {
            if (A == null || B == null)
                throw new SeqException("MaxDistanceFunction A/B configuration null!");
            if (A.Configuration.Count != B.Configuration.Count)
                throw new SeqException("MaxDistanceFunction found dimendion mismatch!", "Check dimension of configurations with " + A.ID + ", " + B.ID);

            if (A.Configuration.Count == B.Configuration.Count)
            {
                double max = 0;
                for (int i = 0; i < A.Configuration.Count; i++)
                {
                    if (Math.Abs(A.Configuration[i] - B.Configuration[i]) > max)
                    {
                        max = Math.Abs(A.Configuration[i] - B.Configuration[i]);
                    }
                }
                return max;
            }
            else
            {
                SeqLogger.Error("MaxDistanceFunction find dimension mismatch of configs with ID: " + A.ID + ", " + B.ID);
                return 0;
            }
        }
    }
}