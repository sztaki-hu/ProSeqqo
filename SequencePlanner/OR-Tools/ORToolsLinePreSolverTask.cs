﻿using SequencePlanner.Model;
using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.OR_Tools
{
    public class ORToolsLinePreSolverTask: IORToolsPreSolverTask
    {
        public int NumberOfNodes { get; set; }
        public int StartDepot { get; set; }
        public int FinishDepot { get; set; }
        public List<GTSPPrecedenceConstraint> OrderPrecedenceConstraints { get; set; }
        public List<GTSPDisjointConstraint> DisjointConstraints { get; set; }
    }
}
