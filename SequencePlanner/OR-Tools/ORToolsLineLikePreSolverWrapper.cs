using Google.OrTools.LinearSolver;
using SequencePlanner.GTSP;
using SequencePlanner.Helper;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SequencePlanner.OR_Tools
{
    public class ORToolsLineLikePreSolverWrapper
    {
        private readonly ORToolsLineLikePreSolverTask parameters;
        private Stopwatch timer;
        private Variable[] x;                 
        private Variable[] position; 

        public ORToolsLineLikePreSolverWrapper(ORToolsLineLikePreSolverTask parameters)
        {
            this.parameters = parameters;
        }

        public List<int> Solve()
        {
            // Init
            SeqLogger.Info("ORTools building started!", nameof(ORToolsSequencerWrapper));
            SeqLogger.Indent++;
            Solver solver = Solver.CreateSolver("CBC_MIXED_INTEGER_PROGRAMMING");
            x = solver.MakeIntVarArray(parameters.NumberOfNodes, 0.0, 1.0, "x");                                                     // Boolean, indicates if node is selected
            position = solver.MakeIntVarArray(parameters.NumberOfNodes, 0.0, parameters.DisjointConstraints.Count - 1, "pos");       // Int, indicates order of nodes

            // Precedences
            if (parameters.StartDepot > -1)                                                                                         //If Start depo exist, position = 0 and selected x = 1
            {
                solver.Add(position[parameters.StartDepot] == 0.0);
                solver.Add(x[parameters.StartDepot] == 1.0);
            }
            AddDisjointConstraints(solver, parameters.DisjointConstraints)    ;                                                      //Add disjoint sets of alternative nodes
            AddPrecedenceConstraints(solver, parameters.OrderPrecedenceConstraints);                                                 //Add order precedences, node1 should be before node2 in the solution if both are selected
            SeqLogger.Info("Number of variables = " + solver.NumVariables(), nameof(ORToolsPointLikePreSolverWrapper));
            SeqLogger.Info("Number of constraints = " + solver.NumConstraints(), nameof(ORToolsPointLikePreSolverWrapper));

            // Solve
            Solver.ResultStatus resultStatus = RunSolver(solver);
            return ProcessSolution(solver, resultStatus);
        }

        private Solver.ResultStatus RunSolver(Solver solver)
        {
            //Solve            
            SeqLogger.Info("Solver running!", nameof(ORToolsPointLikePreSolverWrapper));
            Solver.ResultStatus resultStatus = solver.Solve();
            SeqLogger.Info("Solver finished!", nameof(ORToolsPointLikePreSolverWrapper));
            return resultStatus;
        }

        private void AddPrecedenceConstraints(Solver solver, List<GTSPPrecedenceConstraint> precedenceConstraints)
        {
            SeqLogger.Trace("Precedences: ", nameof(ORToolsPointLikePreSolverWrapper)); SeqLogger.Indent++;
            foreach (var item in parameters.OrderPrecedenceConstraints)
            {
                solver.Add(position[item.Before.SequencingID] +1 <= position[item.After.SequencingID] );
                SeqLogger.Trace(item.ToString(), nameof(ORToolsPointLikePreSolverWrapper));
            }
            SeqLogger.Indent--;
        }

        private void AddDisjointConstraints(Solver solver, List<GTSPDisjointConstraint> disjointConstraints)
        {
            SeqLogger.Trace("DisjointSets: ", nameof(ORToolsPointLikePreSolverWrapper)); SeqLogger.Indent++;
            foreach (var item in parameters.DisjointConstraints)
            {
                solver.Add(CreateDisjointConstraint(item, x));
                SeqLogger.Trace(item.ToString(), nameof(ORToolsPointLikePreSolverWrapper));
            }
            SeqLogger.Indent--;
        }

        private List<int> ProcessSolution(Solver solver, Solver.ResultStatus resultStatus)
        {
            var solution = new List<int>();
            if (resultStatus == Solver.ResultStatus.OPTIMAL)
            {
                var tmpStringSolution = "Initial solution found: ";
                for (int p = 0; p < parameters.NumberOfNodes; p++)
                {
                    for (int i = 0; i < parameters.NumberOfNodes; i++)
                    {
                        if (x[i].SolutionValue() == 1.0 && position[i].SolutionValue() == p)
                        {
                            if (i != parameters.StartDepot)
                            {
                                tmpStringSolution += i.ToString() + ",";
                                solution.Add(i);
                            }
                        }
                    }
                    if (x[p].SolutionValue() == 1)
                        SeqLogger.Trace("i: " + p + " X = " + x[p].SolutionValue() + ", Position = " + position[p].SolutionValue(), nameof(ORToolsLineLikePreSolverWrapper));
                }
                SeqLogger.Info(tmpStringSolution.Remove(tmpStringSolution.Length - 1), nameof(ORToolsLineLikePreSolverWrapper));
            }
            else
            {
                SeqLogger.Info("Solver stopped with status code: " + DecodeStatusCode(resultStatus), nameof(ORToolsLineLikePreSolverWrapper));
                SeqLogger.Error("Can not find optimal initial solution!", nameof(ORToolsLineLikePreSolverWrapper));
                throw new SequencerException("Can not find optimal initial solution with MIP solver!");
            }
            SeqLogger.Info("Solver stopped with status code: " + DecodeStatusCode(resultStatus), nameof(ORToolsLineLikePreSolverWrapper));
            SeqLogger.Info("Problem solved in " + solver.WallTime() + " milliseconds", nameof(ORToolsLineLikePreSolverWrapper));
            SeqLogger.Info("Problem solved in " + solver.Nodes() + " branch-and-bound nodes", nameof(ORToolsLineLikePreSolverWrapper));
            SeqLogger.Indent--;
            SeqLogger.Info("ORTools building finished!", nameof(ORToolsLineLikePreSolverWrapper));
            return solution;
        }

        private LinearConstraint CreateDisjointConstraint(GTSPDisjointConstraint disjoint, Variable[] x)
        {
            LinearExpr constraintExpr = new LinearExpr();
            foreach (var item in disjoint.DisjointSet)
            {
                constraintExpr += x[item];
            }
            LinearConstraint contraint = constraintExpr == 1.0;
            return contraint;
        }

        private string DecodeStatusCode(Solver.ResultStatus status)
        {
            return status switch
            {
                Solver.ResultStatus.OPTIMAL => "0 - OPTIMAL: Problem solved.",
                Solver.ResultStatus.FEASIBLE => "1 - FEASIBLE: Problem solved successfully.",
                Solver.ResultStatus.INFEASIBLE => "2 - INFEASIBLE: No solution found to the problem.",
                Solver.ResultStatus.UNBOUNDED => "3 - UNBOUNDED: Time limit reached before finding a solution.",
                Solver.ResultStatus.ABNORMAL => "4 - ABNORMAL: Model, model parameters, or flags are not valid.",
                Solver.ResultStatus.NOT_SOLVED => "6 - NOT_SOLVED: Model, model parameters, or flags are not valid.",
                _ => "NO_STATUS: Something went wrong. :(",
            };
        }
    }
}