using SequencePlanner.Helper;
using SequencePlanner.Task;

namespace SequencePlanner.OR_Tools
{
    public class ORToolsTask
    {
        public int TimeLimit { get; set; }
        public GTSPRepresentation GTSPRepresentation { get; set; }
        public Metaheuristics LocalSearchStrategy { get; set; }
    }
}