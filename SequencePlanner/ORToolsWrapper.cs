using Google.OrTools.ConstraintSolver;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SequencePlanner
{
    public class ORToolsWrapper
    {
        private readonly int DefaultTimeLimit = 10;
        private readonly SeqGTSPTask task;
        private RoutingIndexManager manager;
        private RoutingModel routing;
        private RoutingSearchParameters searchParameters;
        private Stopwatch Timer;

        public ORToolsWrapper(SeqGTSPTask seqTask)
        {
            task = seqTask;
        }

        //Create OR-Tools Representation from SequencerTask
        public void Build()
        {
            Timer = new Stopwatch();
            // Instantiate the data problem.
            manager = new RoutingIndexManager(task.GTSP.Graph.PositionMatrix.GetLength(0), 1, 0);

            // Create Routing Model.
            routing = new RoutingModel(manager);

            //Edge weight callback
            int transitCallbackIndex = routing.RegisterTransitCallback(
              (long fromIndex, long toIndex) => {
                  // Convert from routing variable Index to distance matrix NodeIndex.
                  var fromNode = Convert.ToInt32(manager.IndexToNode(fromIndex));
                  var toNode = Convert.ToInt32(manager.IndexToNode(toIndex));
                  return task.GTSP.Graph.PositionMatrixRound[fromNode,toNode];
              }
            );

            //Add disjuction constraints
            foreach (var set in task.GTSP.ConstraintsDisjoints)
            {
                routing.AddDisjunction(set.getIndices());
            }

            //Define cost of each arc.
            routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);

            //Add order constraints
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

            //Set time limit
            if(task.TimeLimit == 0)
                searchParameters.TimeLimit = new Duration { Seconds = DefaultTimeLimit };
            else
                searchParameters.TimeLimit = new Duration { Seconds = task.TimeLimit };
        }

        //Run VRP Solver
        public ORToolsResult Solve()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Assignment solution = routing.SolveWithParameters(searchParameters);
            stopWatch.Stop();
            var time = stopWatch.Elapsed;
            return PrintSolution(routing, manager, solution,time);
        }

        private ORToolsResult PrintSolution(in RoutingModel routing, in RoutingIndexManager manager, in Assignment solution, TimeSpan time)
        {
            Console.WriteLine("Solver status: {0}", DecodeStatusCode(routing.GetStatus()));
            ORToolsResult result = new ORToolsResult();
            result.Time = time;
            List<long> rawSolution = new List<long>();
            var index = routing.Start(0);
            rawSolution.Add(index);
            while (routing.IsEnd(index) == false)
            {
                var previousIndex = index;
                index = solution.Value(routing.NextVar(index));
                rawSolution.Add(index);
            }
            rawSolution.Add(routing.Start(0));
            result.ResolveSolution(rawSolution, task.GTSP);
            //result.WriteSimple();
            result.WriteFull();
            //result.Write();
            return result;
        }

        private string DecodeStatusCode(int status)
        {
            switch (status)
            {
               case 0: return "0 - ROUTING_NOT_SOLVED: Problem not solved yet."; break;
               case 1: return "1 - ROUTING_SUCCESS: Problem solved successfully."; break;
               case 2: return "2 - ROUTING_FAIL: No solution found to the problem."; break;
               case 3: return "3 - ROUTING_FAIL_TIMEOUT: Time limit reached before finding a solution."; break;
               case 4: return "4 - ROUTING_INVALID: Model, model parameters, or flags are not valid."; break;
               default: return "NO_STATUS: Something went wrong. :("; break;
            }
        }
    }
}