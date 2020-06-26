using Google.OrTools.ConstraintSolver;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner
{
    public class ORToolsWrapper
    {
        private readonly SequencerTask task;
        private RoutingIndexManager manager;
        private RoutingModel routing;
        private RoutingSearchParameters searchParameters;

        public ORToolsWrapper(SequencerTask seqTask)
        {
            task = seqTask;
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

        //Create OR-Tools Representation from SequencerTask
        public void Build()
        {
            // Instantiate the data problem.
            manager = new RoutingIndexManager(
                task.GTSP.Graph.PositionMatrix.GetLength(0),
                1,
                0);

            // Create Routing Model.
            routing = new RoutingModel(manager);

            int transitCallbackIndex = routing.RegisterTransitCallback(
              (long fromIndex, long toIndex) => {
                  // Convert from routing variable Index to distance matrix NodeIndex.
                  var fromNode = Convert.ToInt32(manager.IndexToNode(fromIndex));
                  var toNode = Convert.ToInt32(manager.IndexToNode(toIndex));
                  return task.GTSP.Graph.PositionMatrixRound[fromNode,toNode];
              }
            );

            foreach (var set in task.GTSP.Graph.ConstraintsDisjoints)
            {
                routing.AddDisjunction(set.getIndices());
            }

            // Define cost of each arc.
            routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);

            // Setting first solution heuristic.
            searchParameters = operations_research_constraint_solver.DefaultRoutingSearchParameters();
            searchParameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;

            // Solve the problem.

        }

        //Run VRP Solver
        public void Solve()
        {
            Assignment solution = routing.SolveWithParameters(searchParameters);

            // Print solution on console.
            PrintSolution(routing, manager, solution);
        }

        private void PrintSolution(in RoutingModel routing, in RoutingIndexManager manager, in Assignment solution)
        {
            Console.WriteLine("Objective: {0} ", solution.ObjectiveValue());
            // Inspect solution.
            Console.WriteLine("Route:");
            long routeDistance = 0;
            var index = routing.Start(0);
            while (routing.IsEnd(index) == false)
            {
                string trajStr;
                if (manager.IndexToNode((int)index) == 0)
                {
                    trajStr = "START";
                }
                else
                {
                    int traj = (manager.IndexToNode((int)index) - 1) / 2;
                    trajStr = task.GTSP.FindPosition(traj).Name;
                    int isReversed = (manager.IndexToNode((int)index) - 1) % 2;
                    if (isReversed == 1)
                    {
                        trajStr += "R";
                    }
                }

                var previousIndex = index;
                index = solution.Value(routing.NextVar(index));
                long dist = routing.GetArcCostForVehicle(previousIndex, index, 0);
                routeDistance += dist;

                //            Console.Write("{0} -[{1}]-> ", trajStr, dist);
                Console.Write("{0} -> ", trajStr);
            }
            Console.WriteLine("STOP");
            //Console.WriteLine("Route distance: {0}", routeDistance);
        }

    }
}
