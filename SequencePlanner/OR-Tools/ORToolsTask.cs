using SequencePlanner.GTSPTask.Task.Base;

namespace SequencePlanner.OR_Tools
{
    public class ORToolsTask
    {
        public int TimeLimit { get; set; }
        public IGTSPRepresentation GTSPRepresentation { get; set; }
        public LocalSearchStrategyEnum.Metaheuristics LocalSearchStrategy { get; set; }
    }
}