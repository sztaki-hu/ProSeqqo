using ProSeqqoLib.Helper;

namespace ProSeqqoLib.Task
{
    public class SolverSettings
    {
        public Metaheuristics Metaheuristics { get; set; }
        public int TimeLimit { get; set; }
        public bool UseShortcutInAlternatives { get; set; }
        public bool UseMIPprecedenceSolver { get; set; }

        public SolverSettings()
        {
            Metaheuristics = Metaheuristics.Automatic;
            TimeLimit = 0;
            UseMIPprecedenceSolver = true;
            UseShortcutInAlternatives = false;
        }
    }
}