using ProSeqqoLib.Helper;
using ProSeqqoLib.Model.Hierarchy;
using System;

namespace ProSeqqoLib.Function.DistanceFunction
{
    public class ManhattanDistanceFunction : DistanceFunction
    {
        public override string FunctionName { get { return "Manhattan"; } }


        public override double ComputeDistance(Config A, Config B)
        {
            if (A == null || B == null)
                throw new SeqException("ManhattanDistanceFunction A/B configuration is null!");
            if (A.Configuration.Count != B.Configuration.Count)
                throw new SeqException("ManhattanDistanceFunction found dimendion mismatch!", "Check dimension of configurations of " + A.ID + ", " + B.ID);

            if (A.Configuration.Count == B.Configuration.Count)
            {
                double tmp = 0;
                for (int i = 0; i < A.Configuration.Count; i++)
                {
                    tmp += Math.Abs(A.Configuration[i] - B.Configuration[i]);
                }
                return tmp;
            }
            else
            {
                throw new SeqException("ManhattanDistanceFunction find dimension mismatch of configuration with id: " + A.ID + "-" + B.ID);
            }
        }
    }
}
