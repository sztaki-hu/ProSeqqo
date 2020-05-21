using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner
{
    public class ORToolsWrapper
    {
        public ORToolsWrapper()
        {
            //RoutingIndexManager manager = new RoutingIndexManager(
            //    data.DistanceMatrix.GetLength(0),
            //    data.VehicleNumber,
            //    data.Depot);

            //// Create Routing Model.
            //RoutingModel routing = new RoutingModel(manager);

            //int transitCallbackIndex = routing.RegisterTransitCallback(
            //  (long fromIndex, long toIndex) => {
            //  // Convert from routing variable Index to distance matrix NodeIndex.
            //  var fromNode = manager.IndexToNode(fromIndex);
            //      var toNode = manager.IndexToNode(toIndex);
            //      return data.DistanceMatrix[fromNode, toNode];
            //  }
            //);

            //for (int i = 1; i < data.DistanceMatrix.GetLength(0); ++i)
            //{
            //    routing.rout
            //    routing.AddDisjunction(
            //        new long[] { manager.NodeToIndex(i), manager.NodeToIndex(i + 1) }, -1, 1);
            //    i++;
            //}

            //// Define cost of each arc.
            //routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);

            //// Setting first solution heuristic.
            //RoutingSearchParameters searchParameters =
            //  operations_research_constraint_solver.DefaultRoutingSearchParameters();
            //searchParameters.FirstSolutionStrategy =
            //  FirstSolutionStrategy.Types.Value.PathCheapestArc;

            //// Solve the problem.
            //Assignment solution = routing.SolveWithParameters(searchParameters);

            //// Print solution on console.
            //PrintSolution(routing, manager, solution, data);
            //data.printRefSol();
        }
    }
}
