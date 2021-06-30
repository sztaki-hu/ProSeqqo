using ProSeqqoLib.Helper;
using ProSeqqoLib.Task;

namespace ProSeqqoLib.OR_Tools
{
    public class ORToolsTask
    {
        public int TimeLimit { get; set; }
        public GTSPRepresentation GTSPRepresentation { get; set; }
        public Metaheuristics LocalSearchStrategy { get; set; }
    }
}