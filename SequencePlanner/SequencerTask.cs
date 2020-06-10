using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner
{
    public class SequencerTask
    {
        public static bool DEBUG = false;

        public TaskTypeEnum TaskType { get; set; }
        public EdgeWeightSourceEnum EdgeWeightSource { get; set; }
        public DistanceFunctionEnum DistanceFunction { get; set; }
        public int Dimension { get; set; }
        public int TimeLimit { get; set; }
        public bool CyclicSequence { get; set; }
        public int StartDepotID { get; set; }
        public int FinishDepotID { get; set; }
        public bool WeightMultiplierAuto { get; set; }
        public int WeightMultiplier { get; set; }
        public GTSPRepresentation GTSP { get; set; }
        public List<Position> Solution { get; private set; }
        public List<Position> CleanSolution { get; private set; }
        private bool Built { get; set; }
        private ORToolsWrapper ORtool { get; set; }

        public SequencerTask()
        {
            ORtool = new ORToolsWrapper(this);
            GTSP = new GTSPRepresentation();
        }

        public void Build()
        {
            GTSP.Build();
            ORtool = new ORToolsWrapper(this);
            ORtool.Build();
            Built = true;
        }

        public List<Position> Run()
        {
            if (!Built)
                Build();
            ORtool.Solve();
            return CleanSolution;
        }
    }
}