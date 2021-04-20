namespace SequencePlanner.GeneralModels
{
    public class SolverSettings
    {
       public OR_Tools.LocalSearchStrategyEnum.Metaheuristics Metaheuristics { get; set; }
       public int TimeLimit { get; set; }
       public bool UseShortcutInAlternatives { get; set; }
       public bool UseMIPprecedenceSolver { get; set; }

       public SolverSettings()
        {
            Metaheuristics = OR_Tools.LocalSearchStrategyEnum.Metaheuristics.Automatic;
            TimeLimit = 0;
            UseMIPprecedenceSolver = true;
            UseShortcutInAlternatives = false;
        }
    }
}