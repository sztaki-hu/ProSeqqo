namespace ProSeqqoLib.Helper
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
}
