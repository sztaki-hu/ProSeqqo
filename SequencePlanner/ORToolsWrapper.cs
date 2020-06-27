using Google.OrTools.ConstraintSolver;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner
{
    public class ORToolsWrapper
    {
        private readonly SeqGTSPTask task;
        private RoutingIndexManager manager;
        private RoutingModel routing;
        private RoutingSearchParameters searchParameters;

        public ORToolsWrapper(SeqGTSPTask seqTask)
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

            foreach (var set in task.GTSP.ConstraintsDisjoints)
            {
                routing.AddDisjunction(set.getIndices());
            }

            // Define cost of each arc.
            routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);

            //routing.AddDimension(transitCallbackIndex, 0, int.MaxValue,
            //        true,  // start cumul to zero
            //        "Distance");
            //RoutingDimension distanceDimension = routing.GetMutableDimension("Distance");
            //distanceDimension.SetGlobalSpanCostCoefficient(int.MaxValue);

            //Solver solver = routing.solver();
            //for (int i = 0; i < task.GTSP.ConstraintsOrder.Count; i++)
            //{
            //    long pickupIndex = manager.NodeToIndex(task.GTSP.ConstraintsOrder[i].Before.PID);
            //    long deliveryIndex = manager.NodeToIndex(task.GTSP.ConstraintsOrder[i].After.PID);
            //    long pickupIndex = 11;
            //    long deliveryIndex = 0;
            //    routing.AddPickupAndDelivery(pickupIndex, deliveryIndex);
            //    solver.Add(solver.MakeEquality(
            //          routing.VehicleVar(pickupIndex),
            //          routing.VehicleVar(deliveryIndex)));
            //    solver.Add(solver.MakeLessOrEqual(
            //          distanceDimension.CumulVar(pickupIndex),
            //          distanceDimension.CumulVar(deliveryIndex)));
            //}

            // Setting first solution heuristic.
            searchParameters = operations_research_constraint_solver.DefaultRoutingSearchParameters();
            searchParameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;

            // Solve the problem.
            searchParameters.TimeLimit = new Duration { Seconds = 10 };
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
            Console.WriteLine("Solver status: {0}", routing.GetStatus());
            Console.WriteLine("Objective: {0} ", solution.ObjectiveValue());
            // Inspect solution.
            Console.WriteLine("Route:");
            long routeDistance = 0;
            var index = routing.Start(0);
            while (routing.IsEnd(index) == false)
            {
                string trajStr;
                int traj = (manager.IndexToNode((int)index));
                if (task.GTSP.FindPositionByPID(traj)!=null)
                    trajStr = task.GTSP.FindPositionByPID(traj).Name+"["+ traj + "]";
                else
                    trajStr = "null[" + traj + "]";

                var previousIndex = index;
                index = solution.Value(routing.NextVar(index));
                long dist = routing.GetArcCostForVehicle(previousIndex, index, 0);
                //long dist = task.GTSP.Graph.PositionMatrixRound[previousIndex-1, index-1];
                routeDistance += dist;

                //            Console.Write("{0} -[{1}]-> ", trajStr, dist);
                Console.Write("{0} --{1}--> ", trajStr, dist);
            }
            Console.WriteLine("STOP");
            //Console.WriteLine("Route distance: {0}", routeDistance);
        }
    }
}