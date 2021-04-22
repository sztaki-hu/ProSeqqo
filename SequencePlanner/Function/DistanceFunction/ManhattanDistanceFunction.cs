using SequencePlanner.GeneralModels;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;

namespace SequencePlanner.Function.DistanceFunction
{
    public class ManhattanDistanceFunction : DistanceFunction
    {
        public override string FunctionName { get { return "Manhattan"; } }
     

        public override double ComputeDistance(Config A, Config B)
        {
            if (A == null || B == null)
                throw new SeqException("ManhattanDistanceFunction A/B position null!");
            if (A.Configuration.Count != B.Configuration.Count)
                throw new SeqException("ManhattanDistanceFunction found dimendion mismatch!", "Check dimension of Positions with " + A.ID + ", " + B.ID);

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
                SeqLogger.Error("ManhattanDistanceFunction find dimension mismatch position userids: "+A.ID+"-"+B.ID);
                return 0;
            }
        }
    }
}
