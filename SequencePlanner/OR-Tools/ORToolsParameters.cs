using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner
{
    public class ORToolsParameters
    {
        public int StartDepot { get; set; }
        public int[,] RoundedMatrix { get; set; }
        public List<ConstraintOrder> OrderConstraints {get;set;}
        public int TimeLimit { get; set; }
        public List<ConstraintDisjoint> DisjointConstraints { get; set; }
        public GTSPRepresentation GTSP { get; set; }
    }
}
