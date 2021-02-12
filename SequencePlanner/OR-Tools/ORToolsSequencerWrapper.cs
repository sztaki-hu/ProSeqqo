using Google.OrTools.ConstraintSolver;
using Google.Protobuf.WellKnownTypes;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SequencePlanner.OR_Tools
{
    public class ORToolsSequencerWrapper
    {
        private readonly ORToolsTask param;
        private RoutingIndexManager manager;
        private RoutingModel routing;
        private RoutingSearchParameters searchParameters;
        private Assignment InitialSolution;

        private Stopwatch Timer;

        public ORToolsSequencerWrapper(ORToolsTask parameters)
        {
            param = parameters;
        }

        //Create OR-Tools Representation from SequencerTask
        public void Build()
        {
            SeqLogger.Info("ORTools building started!", nameof(ORToolsSequencerWrapper));
            SeqLogger.Indent++;
            Timer = new Stopwatch();
            SeqLogger.Info("GTSP Matrix Dimension: " + param.GTSPRepresentation.RoundedMatrix.GetLength(0) + "x"+ param.GTSPRepresentation.RoundedMatrix.GetLength(1), nameof(ORToolsSequencerWrapper));
            SeqLogger.Info("GTSP Start Depot seqID: " + param.GTSPRepresentation.StartDepot, nameof(ORToolsSequencerWrapper));
            manager = new RoutingIndexManager(param.GTSPRepresentation.RoundedMatrix.GetLength(0), 1, param.GTSPRepresentation.StartDepot);
            routing = new RoutingModel(manager);

            //Edge weight callback
            int transitCallbackIndex = routing.RegisterTransitCallback(
              (long fromIndex, long toIndex) => {
                  // Convert from routing variable Index to distance matrix NodeIndex.
                  var fromNode = Convert.ToInt32(manager.IndexToNode(fromIndex));
                  var toNode = Convert.ToInt32(manager.IndexToNode(toIndex));
                  return param.GTSPRepresentation.RoundedMatrix[fromNode, toNode];
              }
            );
            routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);

            //Add disjuction constraints
            SeqLogger.Info("GTSP Disjoint constraint number: " + param.GTSPRepresentation.DisjointConstraints.Count, nameof(ORToolsSequencerWrapper));
            foreach (var set in param.GTSPRepresentation.DisjointConstraints)
            {
                SeqLogger.Trace(set.ToString(), nameof(ORToolsSequencerWrapper));
                routing.AddDisjunction(set.DisjointSet);
            }

            SeqLogger.Info("GTSP Order constraint number: " + param.GTSPRepresentation.PrecedenceConstraints.Count, nameof(ORToolsSequencerWrapper));
            if (param.GTSPRepresentation.PrecedenceConstraints.Count > 0)
            {
                //Add distance dimension
                routing.AddDimension(transitCallbackIndex, 0, int.MaxValue - 100, true, "Distance");
                RoutingDimension distanceDimension = routing.GetMutableDimension("Distance");

                //Add order constraints
                Solver solver = routing.solver();
                for (int i = 0; i < param.GTSPRepresentation.PrecedenceConstraints.Count; i++)
                {
                    SeqLogger.Trace(param.GTSPRepresentation.PrecedenceConstraints[i].ToString(), nameof(ORToolsSequencerWrapper));
                    long beforeIndex = manager.NodeToIndex(param.GTSPRepresentation.PrecedenceConstraints[i].Before.SequencingID);
                    long afterIndex = manager.NodeToIndex(param.GTSPRepresentation.PrecedenceConstraints[i].After.SequencingID);
                    solver.Add(solver.MakeLessOrEqual(
                          distanceDimension.CumulVar(beforeIndex),
                          distanceDimension.CumulVar(afterIndex)));
                }
            }

            searchParameters = operations_research_constraint_solver.DefaultRoutingSearchParameters();
            
            //Set time limit
            if (param.TimeLimit != 0)
            {
                var sec = param.TimeLimit / 1000;
                var ns = (param.TimeLimit - (sec * 1000)) * 1000000;
                searchParameters.TimeLimit = new Duration { Seconds = sec, Nanos = ns };
                SeqLogger.Info("Time Limit: " + searchParameters.TimeLimit.ToDiagnosticString(), nameof(ORToolsSequencerWrapper));
            }else
                SeqLogger.Info("No time limit!", nameof(ORToolsSequencerWrapper));

            // Setting first solution heuristic.
            searchParameters.LocalSearchMetaheuristic = LocalSearchStrategieEnum.ResolveEnum(param.LocalSearchStrategie);
            SeqLogger.Info("First solution strategy: "+ searchParameters.FirstSolutionStrategy.ToString(), nameof(ORToolsSequencerWrapper));
            SeqLogger.Info("Use depth first search: "+ searchParameters.UseDepthFirstSearch.ToString(), nameof(ORToolsSequencerWrapper));
            SeqLogger.Info("Local search metahearustic "+ searchParameters.LocalSearchMetaheuristic.ToString(), nameof(ORToolsSequencerWrapper));
            //searchParameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;
            //searchParameters.UseCp = Google.OrTools.Util.OptionalBoolean.BoolTrue;
            //searchParameters.UseCpSat = Google.OrTools.Util.OptionalBoolean.BoolTrue;

            routing.CloseModelWithParameters(searchParameters);

            if (param.GTSPRepresentation.InitialRoutes!=null && param.GTSPRepresentation.InitialRoutes.Length > 0)
            {
                InitialSolution = routing.ReadAssignmentFromRoutes(param.GTSPRepresentation.InitialRoutes, false);
                if (InitialSolution == null)
                    SeqLogger.Error("Initial solution given, but not accepted!", nameof(ORToolsSequencerWrapper));
                else
                    SeqLogger.Info("Initial route given with " + param.GTSPRepresentation.InitialRoutes[0].Length+" element.", nameof(ORToolsSequencerWrapper));
            }else
                SeqLogger.Info("Initial route not given.", nameof(ORToolsSequencerWrapper));


            SeqLogger.Indent--;
            SeqLogger.Info("ORTools building finished!", nameof(ORToolsSequencerWrapper));
        }

        //Run VRP Solver
        public TaskResult Solve()
        {
            SeqLogger.Info("Solver running!", nameof(ORToolsSequencerWrapper));
            SeqLogger.Indent++;
            Timer.Start();
            Assignment solution;
            if (InitialSolution != null)
                solution = routing.SolveFromAssignmentWithParameters(InitialSolution, searchParameters);
            else
                solution = routing.SolveWithParameters(searchParameters);
            Timer.Stop();
            SeqLogger.Info("Solver stopped with status code: " + DecodeStatusCode(routing.GetStatus()), nameof(ORToolsSequencerWrapper));
            SeqLogger.Info("Solver run time: " + Timer.Elapsed.ToString(), nameof(ORToolsSequencerWrapper));
            SeqLogger.Indent--;
            SeqLogger.Info("Solver finished!", nameof(ORToolsSequencerWrapper));
            return ProcessSolution(routing, manager, solution, Timer.Elapsed);
        }

        private TaskResult ProcessSolution(in RoutingModel routing, in RoutingIndexManager manager, in Assignment solution, TimeSpan time)
        {
            List<long> rawSolution = new List<long>();
            if (routing.GetStatus() == 1)
            {
                var index = routing.Start(0);
                while (routing.IsEnd(index) == false)
                {
                    rawSolution.Add(manager.IndexToNode(index));
                    index = solution.Value(routing.NextVar(index));
                }
                rawSolution.Add(manager.IndexToNode(index));
            }

            TaskResult result = new TaskResult()
            {
                SolverTime = time,
                SolutionRaw = rawSolution,
                StatusCode = routing.GetStatus(),
                StatusMessage = DecodeStatusCode(routing.GetStatus())
            };
            SeqLogger.Info("Solution processed!", nameof(ORToolsSequencerWrapper));
            return result;
        }

        private string DecodeStatusCode(int status)
        {
            return status switch
            {
                0 => "0 - ROUTING_NOT_SOLVED: Problem not solved yet.",
                1 => "1 - ROUTING_SUCCESS: Problem solved successfully.",
                2 => "2 - ROUTING_FAIL: No solution found to the problem.",
                3 => "3 - ROUTING_FAIL_TIMEOUT: Time limit reached before finding a solution.",
                4 => "4 - ROUTING_INVALID: Model, model parameters, or flags are not valid.",
                _ => "NO_STATUS: Something went wrong. :(",
            };
        }
    }
}