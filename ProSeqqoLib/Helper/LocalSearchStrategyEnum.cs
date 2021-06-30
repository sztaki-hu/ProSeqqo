using Google.OrTools.ConstraintSolver;

namespace SequencePlanner.Helper
{
    public class LocalSearchStrategyEnum
    {
        public static LocalSearchMetaheuristic.Types.Value ResolveEnum(Metaheuristics metaheuristics)
        {
            switch (metaheuristics)
            {
                case Metaheuristics.Automatic:
                    return LocalSearchMetaheuristic.Types.Value.Automatic;
                case Metaheuristics.GreedyDescent:
                    return LocalSearchMetaheuristic.Types.Value.GreedyDescent;
                case Metaheuristics.GuidedLocalSearch:
                    return LocalSearchMetaheuristic.Types.Value.GuidedLocalSearch;
                case Metaheuristics.SimulatedAnnealing:
                    return LocalSearchMetaheuristic.Types.Value.SimulatedAnnealing;
                case Metaheuristics.TabuSearch:
                    return LocalSearchMetaheuristic.Types.Value.TabuSearch;
                case Metaheuristics.ObjectiveTabuSearch:
                    return LocalSearchMetaheuristic.Types.Value.GenericTabuSearch;
                default:
                    SeqLogger.Warning("Unknown Metaheuristics, changed for Automatic", nameof(LocalSearchStrategyEnum));
                    return LocalSearchMetaheuristic.Types.Value.Automatic;
            }
        }

        public static Metaheuristics ResolveEnum(string metaheuristics)
        {
            if (metaheuristics != null)
            {
                metaheuristics = metaheuristics.ToUpper();
                if (metaheuristics.Contains(Metaheuristics.Automatic.ToString().ToUpper()))
                    return Metaheuristics.Automatic;
                if (metaheuristics.Contains(Metaheuristics.GreedyDescent.ToString().ToUpper()))
                    return Metaheuristics.GreedyDescent;
                if (metaheuristics.Contains(Metaheuristics.GuidedLocalSearch.ToString().ToUpper()))
                    return Metaheuristics.GuidedLocalSearch;
                if (metaheuristics.Contains(Metaheuristics.SimulatedAnnealing.ToString().ToUpper()))
                    return Metaheuristics.SimulatedAnnealing;
                if (metaheuristics.Contains(Metaheuristics.ObjectiveTabuSearch.ToString().ToUpper()))
                    return Metaheuristics.ObjectiveTabuSearch;
                if (metaheuristics.Contains(Metaheuristics.TabuSearch.ToString().ToUpper()))
                    return Metaheuristics.TabuSearch;
            }
            SeqLogger.Warning("Unknown Metaheuristics, changed for Automatic", nameof(LocalSearchStrategyEnum));
            return Metaheuristics.Automatic;
        }
    }
}