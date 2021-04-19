namespace SequencePlanner.GeneralModels
{
    public class SolverSettings
    {
       public OR_Tools.LocalSearchStrategyEnum.Metaheuristics Metaheuristics { get; set; }
       public int TimeLimit { get; set; }
       public bool UseShortcutInAlternatives { get; set; }
       public bool UseMIPprecedenceSolver { get; set; }
    }
}