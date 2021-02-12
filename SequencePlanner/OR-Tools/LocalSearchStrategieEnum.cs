using Google.OrTools.ConstraintSolver;
using SequencePlanner.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePlanner.OR_Tools
{
    public class LocalSearchStrategieEnum
    {
        public enum Metaheuristics
        {
            Automatic,          //   Lets the solver select the metaheuristic.
            GreedyDescent,      //  Accepts improving (cost-reducing) local search neighbors until a local minimum is reached.
            GuidedLocalSearch,  // Uses guided local search to escape local minima(cf.http://en.wikipedia.org/wiki/Guided_Local_Search); this is generally the most efficient metaheuristic for vehicle routing.
            SimulatedAnnealing, // Uses simulated annealing to escape local minima (cf.http://en.wikipedia.org/wiki/Simulated_annealing).
            TabuSearch,         // Uses tabu search to escape local minima (cf.http://en.wikipedia.org/wiki/Tabu_search).
            ObjectiveTabuSearch  //Uses tabu search on the objective value of solution to escape local minima
        }

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
                    return LocalSearchMetaheuristic.Types.Value.Automatic;
            }
        }

        public static Metaheuristics ResolveEnum(string metaheuristics)
        {
            if(metaheuristics != null)
            {
                metaheuristics = metaheuristics.ToUpper();
                if(metaheuristics.Contains(Metaheuristics.Automatic.ToString().ToUpper()))
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
            SeqLogger.Error("Unknown Metaheuristics, changed for Automatic", nameof(LocalSearchStrategieEnum));
            return Metaheuristics.Automatic;
        }
    }
}