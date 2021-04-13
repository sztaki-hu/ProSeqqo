﻿using SequencePlanner.Helper;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;

namespace SequencePlanner.Function.DistanceFunction
{
    public class ManhattanDistanceFunction : DistanceFunction
    {
        public override string FunctionName { get { return "Manhattan"; } }
     

        public override double ComputeDistance(Position A, Position B)
        {
            if (A == null || B == null)
                throw new SeqException("ManhattanDistanceFunction A/B position null!");
            if (A.Dimension != B.Dimension)
                throw new SeqException("ManhattanDistanceFunction found dimendion mismatch!", "Check dimension of Positions with " + A.UserID + ", " + B.UserID);

            if (A.Vector.Length == B.Vector.Length)
            {
                double tmp = 0;
                for (int i = 0; i < A.Vector.Length; i++)
                {
                    tmp += Math.Abs(A.Vector[i] - B.Vector[i]);
                }
                return tmp;
            }
            else
            {
                SeqLogger.Error("ManhattanDistanceFunction find dimension mismatch position userids: "+A.UserID+"-"+B.UserID);
                return 0;
            }
        }
    }
}
