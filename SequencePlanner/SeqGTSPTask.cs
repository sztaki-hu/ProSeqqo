using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options.Values;
using SequencePlanner.Phraser.Template;
using System.Collections.Generic;

namespace SequencePlanner
{
    public class SeqGTSPTask: GTSPTask
    {
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


        public override void Build()
        {
            GTSP.Build();
            ORtool = new ORToolsWrapper(this);
            ORtool.Build();
            Built = true;
        }

        public override void Run()
        {
            if (!Built)
                Build();
            ORtool.Solve();
        }

        public override void Templateing(Template template)
        {
            throw new System.NotImplementedException();
        }

        public override void Validate()
        {
            throw new System.NotImplementedException();
        }
    }
}