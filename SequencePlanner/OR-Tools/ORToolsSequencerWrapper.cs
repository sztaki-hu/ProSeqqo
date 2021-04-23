using System;
using System.Diagnostics;
using System.Collections.Generic;
using Google.OrTools.ConstraintSolver;
using Google.Protobuf.WellKnownTypes;
using SequencePlanner.Helper;

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
            SeqLogger.Debug("ORTools building started!", nameof(ORToolsSequencerWrapper));
            SeqLogger.Indent++;
            Timer = new Stopwatch();
            SeqLogger.Debug("GTSP Matrix Dimension: " + param.GTSPRepresentation.RoundedCostMatrix.GetLength(0) + "x"+ param.GTSPRepresentation.RoundedCostMatrix.GetLength(1), nameof(ORToolsSequencerWrapper));
            SeqLogger.Debug("GTSP Start Depot SeqID: " + param.GTSPRepresentation.StartDepot, nameof(ORToolsSequencerWrapper));
            if(param.GTSPRepresentation.FinishDepot == null)
                manager = new RoutingIndexManager(param.GTSPRepresentation.RoundedCostMatrix.GetLength(0), 1, param.GTSPRepresentation.StartDepot.SequenceMatrixID );
            else
            {
                manager = new RoutingIndexManager(param.GTSPRepresentation.RoundedCostMatrix.GetLength(0), 1, new int[] { param.GTSPRepresentation.StartDepot.SequenceMatrixID }, new int[] { param.GTSPRepresentation.FinishDepot.SequenceMatrixID });
                SeqLogger.Debug("GTSP Finish Depot SeqID: " + param.GTSPRepresentation.FinishDepot, nameof(ORToolsSequencerWrapper));
            }
            routing = new RoutingModel(manager);

            //Edge weight callback
            int transitCallbackIndex = routing.RegisterTransitCallback(
              (long fromIndex, long toIndex) => {
                  // Convert from routing variable Index to distance matrix NodeIndex.
                  var fromNode = Convert.ToInt32(manager.IndexToNode(fromIndex));
                  var toNode = Convert.ToInt32(manager.IndexToNode(toIndex));
                  return param.GTSPRepresentation.RoundedCostMatrix[fromNode, toNode];
              }
            );
            routing.SetArcCostEvaluatorOfAllVehicles(transitCallbackIndex);

            //Add disjuction constraints
            SeqLogger.Debug("GTSP Disjoint constraint number: " + param.GTSPRepresentation.DisjointSets.Count, nameof(ORToolsSequencerWrapper));
            foreach (var set in param.GTSPRepresentation.DisjointSets)
            {
                if (set.Elements.Count > 1)
                {
                    SeqLogger.Trace(set.ToString(), nameof(ORToolsSequencerWrapper));
                    routing.AddDisjunction(manager.NodesToIndices(set.SequencMatrixcIDs));
                }
            }

            SeqLogger.Debug("GTSP Order constraint number: " + param.GTSPRepresentation.MotionPrecedences.Count, nameof(ORToolsSequencerWrapper));
            if (param.GTSPRepresentation.MotionPrecedences.Count > 0)
            {
                //Add distance dimension
                routing.AddDimension(transitCallbackIndex,0, int.MaxValue - 100, true, "Distance");
                RoutingDimension distanceDimension = routing.GetMutableDimension("Distance");

                //Add order constraints
                Solver solver = routing.solver();
                for (int i = 0; i < param.GTSPRepresentation.MotionPrecedences.Count; i++)
                {
                    SeqLogger.Trace(param.GTSPRepresentation.MotionPrecedences[i].ToString(), nameof(ORToolsSequencerWrapper));
                    long beforeIndex = manager.NodeToIndex(param.GTSPRepresentation.MotionPrecedences[i].Before.SequenceMatrixID);
                    long afterIndex = manager.NodeToIndex(param.GTSPRepresentation.MotionPrecedences[i].After.SequenceMatrixID);
                    solver.Add(routing.ActiveVar(beforeIndex) * routing.ActiveVar(afterIndex) * distanceDimension.CumulVar(beforeIndex) <= distanceDimension.CumulVar(afterIndex));
                }
            }

            searchParameters = operations_research_constraint_solver.DefaultRoutingSearchParameters();
            
            //Set time limit
            if (param.TimeLimit != 0)
            {
                var sec = param.TimeLimit / 1000;
                var ns = (param.TimeLimit - (sec * 1000)) * 1000000;
                searchParameters.TimeLimit = new Duration { Seconds = sec, Nanos = ns };
                SeqLogger.Debug("Time Limit: " + searchParameters.TimeLimit.ToDiagnosticString(), nameof(ORToolsSequencerWrapper));
            }else
                SeqLogger.Debug("No time limit!", nameof(ORToolsSequencerWrapper));

            // Setting first solution heuristic.
            searchParameters.LocalSearchMetaheuristic = LocalSearchStrategyEnum.ResolveEnum(param.LocalSearchStrategy);
            SeqLogger.Debug("First solution strategy: "+ searchParameters.FirstSolutionStrategy.ToString(), nameof(ORToolsSequencerWrapper));
            SeqLogger.Debug("Use depth first search: "+ searchParameters.UseDepthFirstSearch.ToString(), nameof(ORToolsSequencerWrapper));
            SeqLogger.Debug("Local search metahearustic "+ searchParameters.LocalSearchMetaheuristic.ToString(), nameof(ORToolsSequencerWrapper));
            //searchParameters.FirstSolutionStrategy = FirstSolutionStrategy.Types.Value.PathCheapestArc;
            //searchParameters.UseCp = Google.OrTools.Util.OptionalBoolean.BoolTrue;
            //searchParameters.UseCpSat = Google.OrTools.Util.OptionalBoolean.BoolTrue;

            routing.CloseModelWithParameters(searchParameters);

            if (param.GTSPRepresentation.InitialRoutes!=null && param.GTSPRepresentation.InitialRoutes.Length > 0)
            {
                SeqLogger.Debug("Initial route: "+ param.GTSPRepresentation.InitialRoutes[0].ToListString(), nameof(ORToolsSequencerWrapper));
                InitialSolution = routing.ReadAssignmentFromRoutes(param.GTSPRepresentation.InitialRoutes, true);
                if (InitialSolution == null)
                    SeqLogger.Error("Initial solution given, but not accepted!", nameof(ORToolsSequencerWrapper));
                else
                    SeqLogger.Debug("Initial route given with " + param.GTSPRepresentation.InitialRoutes[0].Length+" element.", nameof(ORToolsSequencerWrapper));
            }else
                SeqLogger.Debug("Initial route not given.", nameof(ORToolsSequencerWrapper));
            SeqLogger.Indent--;
            SeqLogger.Debug("ORTools building finished!", nameof(ORToolsSequencerWrapper));
        }
        public ORToolsResult Solve()
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
            SeqLogger.Debug("Solver stopped with status code: " + DecodeStatusCode(routing.GetStatus()), nameof(ORToolsSequencerWrapper));
            SeqLogger.Debug("Solver run time: " + Timer.Elapsed.ToString(), nameof(ORToolsSequencerWrapper));
            SeqLogger.Indent--;
            SeqLogger.Info("Solver finished!", nameof(ORToolsSequencerWrapper));
            return ProcessSolution(routing, manager, solution, Timer.Elapsed);
        }

        private static ORToolsResult ProcessSolution(in RoutingModel routing, in RoutingIndexManager manager, in Assignment solution, TimeSpan time)
        {
            List<int> rawSolution = new List<int>();
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

            ORToolsResult result = new ORToolsResult()
            {
                Time = time,
                Solution = rawSolution,
                StatusCode = routing.GetStatus(),
                StatusMessage = DecodeStatusCode(routing.GetStatus())
            };
            SeqLogger.Debug("Solution processed!", nameof(ORToolsSequencerWrapper));
            return result;
        }
        private static string DecodeStatusCode(int status)
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