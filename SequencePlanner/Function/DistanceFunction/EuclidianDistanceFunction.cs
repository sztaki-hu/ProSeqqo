using SequencePlanner.Helper;
using SequencePlanner.Model.Hierarchy;
using System;

namespace SequencePlanner.Function.DistanceFunction
{
    public class EuclidianDistanceFunction : DistanceFunction
    {
        public override string FunctionName { get { return "Euclidian"; } }


        public override double ComputeDistance(Config A, Config B)
        {
            if (A == null || B == null)
                throw new SeqException("EuclidianDistanceFunction A/B position null! A ID: "+A.ID+" B ID: "+B.ID);
            if (A.Configuration.Count != B.Configuration.Count)
                throw new SeqException("EuclidianDistanceFunction found dimendion mismatch!", "Check dimension of Positions with " + A.ID + ", " + B.ID);

            double tmp = 0;
            for (int i = 0; i < A.Configuration.Count; i++)
            {
                tmp += (A.Configuration[i] - B.Configuration[i]) * (A.Configuration[i] - B.Configuration[i]);
            }
            return Math.Sqrt(tmp);
        }
    }
}