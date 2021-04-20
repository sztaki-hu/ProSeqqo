using SequencePlanner.GTSPTask.Task.General;

namespace SequencePlanner.OR_Tools
{
    public class ORToolsTask
    {
        public int TimeLimit { get; set; }
        public GeneralGTSPRepresentation GTSPRepresentation { get; set; }
        public LocalSearchStrategyEnum.Metaheuristics LocalSearchStrategy { get; set; }
    }
}