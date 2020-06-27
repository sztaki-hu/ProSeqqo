using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner
{
    public class ORToolsParameters
    {
        public GTSPRepresentation GTSP { get; set; }
        public Position StartDepot { get; set; }
        public int TimeLimit { get; set; }
        public int WeightMultiplier { get; set; }
    }
}
