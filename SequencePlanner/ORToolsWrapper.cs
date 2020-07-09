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
        private readonly ORToolsParameters param;
        private RoutingIndexManager manager;
        private RoutingModel routing;
        private RoutingSearchParameters searchParameters;
        private Stopwatch Timer;

        public ORToolsWrapper(ORToolsParameters parameters)
        {
            param = parameters;
        }

        //Create OR-Tools Representation from SequencerTask
        public void Build()
        {
            Timer = new Stopwatch();
            //manager = new RoutingIndexManager(param.GTSP.Graph.PositionMatrix.GetLength(0), 1, param.StartDepot.PID);
            manager = new RoutingIndexManager(param.GTSP.Graph.PositionMatrix.GetLength(0), 1, 0);
            // Create Routing Model.
            routing = new RoutingModel(manager);

            //Edge weight callback
            int transitCallbackIndex = routing.RegisterTransitCallback(
              (long fromIndex, long toIndex) => {
                  // Convert from routing variable Index to distance matrix NodeIndex.
                  var fromNode = Convert.ToInt32(manager.IndexToNode(fromIndex));
                  var toNode = Convert.ToInt32(manager.IndexToNode(toIndex));
                  return param.GTSP.Graph.PositionMatrixRound[fromNode, toNode];
              }
            );
            routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);

            //Add disjuction constraints
            foreach (var set in param.GTSP.ConstraintsDisjoints)
            {
                routing.AddDisjunction(set.getIndices());
            }

            //Add distance dimension
            routing.AddDimension(transitCallbackIndex, 0, int.MaxValue-100, true, "Distance");
            RoutingDimension distanceDimension = routing.GetMutableDimension("Distance");

            //Add order constraints
            Solver solver = routing.solver();
            for (int i = 0; i < param.GTSP.ConstraintsOrder.Count; i++)
            {
                long beforeIndex = manager.NodeToIndex(param.GTSP.ConstraintsOrder[i].Before.PID);
                long afterIndex = manager.NodeToIndex(param.GTSP.ConstraintsOrder[i].After.PID);
                solver.Add(solver.MakeLessOrEqual(
                      distanceDimension.CumulVar(beforeIndex),
                      distanceDimension.CumulVar(afterIndex)));
            }

            // Setting first solution heuristic.
            searchParameters = operations_research_constraint_solver.DefaultRoutingSearchParameters();
            searchParameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;
            
            //Set time limit
            if(param.TimeLimit == 0)
                searchParameters.TimeLimit = new Duration { Seconds = DefaultTimeLimit };
            else
                searchParameters.TimeLimit = new Duration { Seconds = param.TimeLimit };
        }

        //Run VRP Solver
        public ORToolsResult Solve()
        {
            Stopwatch stopWatch = new Stopwatch();
            Console.WriteLine("\nSolver running!");
            stopWatch.Start();
            Assignment solution = routing.SolveWithParameters(searchParameters);
            stopWatch.Stop();
            var time = stopWatch.Elapsed;
            Console.WriteLine("Solver status: {0}", DecodeStatusCode(routing.GetStatus()));
            if(routing.GetStatus()==1)
                return PrintSolution(routing, manager, solution,time);
            return null;
        }

        private ORToolsResult PrintSolution(in RoutingModel routing, in RoutingIndexManager manager, in Assignment solution, TimeSpan time)
        {
            ORToolsResult result = new ORToolsResult();
            result.Time = time;
            List<long> rawSolution = new List<long>();
            var index = routing.Start(0);
            while (routing.IsEnd(index) == false)
            {
                rawSolution.Add(manager.IndexToNode(index));
                index = solution.Value(routing.NextVar(index));
            }
            rawSolution.Add(manager.IndexToNode(index));
            result.ResolveSolution(rawSolution, param.GTSP);
            return result;
        }

        private string DecodeStatusCode(int status)
        {
            switch (status)
            {
               case 0: return "0 - ROUTING_NOT_SOLVED: Problem not solved yet.";
               case 1: return "1 - ROUTING_SUCCESS: Problem solved successfully.";
                case 2: return "2 - ROUTING_FAIL: No solution found to the problem.";
               case 3: return "3 - ROUTING_FAIL_TIMEOUT: Time limit reached before finding a solution.";
               case 4: return "4 - ROUTING_INVALID: Model, model parameters, or flags are not valid.";
               default: return "NO_STATUS: Something went wrong. :(";
            }
        }
    }
}