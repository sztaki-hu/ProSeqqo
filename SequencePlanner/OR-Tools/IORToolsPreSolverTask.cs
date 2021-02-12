﻿using SequencePlanner.GTSP;
using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.OR_Tools
{
    public interface IORToolsPreSolverTask
    {
        public int NumberOfNodes { get; set; }
        public int StartDepot { get; set; }
        public List<GTSPPrecedenceConstraint> OrderPrecedenceConstraints { get; set; }
        public List<GTSPDisjointConstraint> DisjointConstraints { get; set; }
    }
}