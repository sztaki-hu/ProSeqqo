﻿using SequencePlanner.Helper;
using SequencePlanner.Model;
using System;

namespace SequencePlanner.Function.DistanceFunction
{
    public class EuclidianDistanceFunction : IDistanceFunction
    {
        public string FunctionName { get { return "EuclidianDistance"; } }

        public double ComputeDistance(Position A, Position B)
        {
            if (A == null || B == null)
                throw new SequencerException("EuclidianDistanceFunction A/B position null!");
            if (A.Dimension != B.Dimension)
                throw new SequencerException("EuclidianDistanceFunction found dimendion mismatch!", "Check dimension of Positions with " + A.UserID+", "+ B.UserID);
            double tmp = 0;
            for (int i = 0; i < A.Vector.Length; i++)
            {
                tmp += (A.Vector[i] - B.Vector[i]) * (A.Vector[i] - B.Vector[i]);
            }
            return Math.Sqrt(tmp);
        }

        public void Validate()
        {
            //Nothing to validate
        }
    }
}
