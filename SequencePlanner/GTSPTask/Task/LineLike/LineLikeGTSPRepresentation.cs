﻿using SequencePlanner.GTSP;
using SequencePlanner.GTSPTask.Task.Base;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Task.LineLike
{
    public class LineLikeGTSPRepresentation : IGTSPRepresentation
    {
        public List<GTSPPrecedenceConstraint> PrecedenceConstraints { get; set; }
        public List<GTSPDisjointConstraint> DisjointConstraints { get; set; }
        public int[,] RoundedMatrix { get; set; }
        public double[,] Matrix { get; set; }
        public int StartDepot { get; set; }
        public long[][] InitialRoutes { get; set; }
    }
}