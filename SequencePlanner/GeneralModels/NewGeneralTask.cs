using System.Collections.Generic;

namespace SequencePlanner.GeneralModels
{
    public class NewGeneralTask
    {
        public bool Validate { get; set; }
        public bool Cyclic { get; set; }
        public Motion StartDepot { get; set; }
        public Motion FinishDepot { get; set; }

        public List<Motion> Motions { get; set; }
        public List<Config> Configs { get; set; }
        public CostManager CostManager { get; set; }
        public Hierarchy Hierarchy { get; set; }
        public SolverSettings SolverSettings { get; set; }
        public List<ProcessPrecedence> Processrecedences { get; set; }
        public List<MotionPrecedence> MotionPrecedences { get; set; }
        public PCGTSPRepresentation PCGTSPRepresentation { get; set; }
    }
}
