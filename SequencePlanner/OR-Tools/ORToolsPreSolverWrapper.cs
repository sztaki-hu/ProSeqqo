using Google.OrTools.LinearSolver;
using SequencePlanner.GTSP;
using SequencePlanner.Helper;
using System.Collections.Generic;
using System.Diagnostics;

namespace SequencePlanner.OR_Tools
{
    public class ORToolsPreSolverWrapper
    {
        private readonly ORToolsPreSolverTask param;
        private Stopwatch Timer;

        public ORToolsPreSolverWrapper(ORToolsPreSolverTask parameters)
        {
            param = parameters;
        }

        public List<int> Solve()
        {
            SeqLogger.Info("ORTools building started!", nameof(ORToolsSequencerWrapper));
            SeqLogger.Indent++;
            // [START solver]
            // Create the linear solver with the CBC backend.
            Solver solver = Solver.CreateSolver("CBC_MIXED_INTEGER_PROGRAMMING");

            // [START variables]
            Variable[] x = solver.MakeIntVarArray(param.NumberOfNodes, 0.0, 1.0, "x");  // Boolean, indicates if node is selected
            Variable[] pos = solver.MakeIntVarArray(param.NumberOfNodes, 0.0, param.NumberOfNodes - 1, "pos");  // Boolean, indicates if node is selected


            // [END variables]

            // [START constraints]
            // At least one node selected from each disjunctive
            foreach (var item in param.DisjointConstraints)
            {
                solver.Add(CreateLinearConstraint(item, x));
                SeqLogger.Trace(item.ToString(), nameof(ORToolsPreSolverWrapper));
            }

            if (param.StartDepot > -1)
                solver.Add(pos[param.StartDepot] == 0.0);

            // Precedences
            foreach (var item in param.PrecedenceConstraints)
            {
                solver.Add(CreateLinearConstraint(item, pos, x, param.DisjointConstraints.Count));
                SeqLogger.Trace(item.ToString(), nameof(ORToolsPreSolverWrapper));
            }
            SeqLogger.Info("Number of variables = " + solver.NumVariables(), nameof(ORToolsPreSolverWrapper));
            SeqLogger.Info("Number of constraints = " + solver.NumConstraints(), nameof(ORToolsPreSolverWrapper));
            // [END constraints]

            SeqLogger.Info("Solver running!", nameof(ORToolsPreSolverWrapper));
            Solver.ResultStatus resultStatus = solver.Solve();
            SeqLogger.Info("Solver finished!", nameof(ORToolsPreSolverWrapper));


            var solution = new List<int>();
            // [START print_solution]
            if (resultStatus == Solver.ResultStatus.OPTIMAL)
            {
                var tmpStringSolution = "Initial solution found: ";
                for (int p = 0; p < param.NumberOfNodes; p++)
                    for (int i = 0; i < param.NumberOfNodes; i++)
                        if (x[i].SolutionValue() == 1.0 && pos[i].SolutionValue() == p)
                        {
                            if (i != param.StartDepot)
                            {
                                tmpStringSolution += i.ToString() + ",";
                                solution.Add(i);
                            }

                        }
                SeqLogger.Info(tmpStringSolution.Remove(tmpStringSolution.Length - 1), nameof(ORToolsPreSolverWrapper));
            }
            else
            {
                SeqLogger.Info("Solver stopped with status code: " + DecodeStatusCode(resultStatus), nameof(ORToolsPreSolverWrapper));
                SeqLogger.Error("Can not find optimal initial solution!", nameof(ORToolsPreSolverWrapper));
                throw new SequencerException("Can not find optimal initial solution with MIP solver!");
            }
            // [END print_solution]

            // [START Statistics]
            SeqLogger.Info("Solver stopped with status code: " + DecodeStatusCode(resultStatus), nameof(ORToolsPreSolverWrapper));
            SeqLogger.Info("Problem solved in " + solver.WallTime() + " milliseconds", nameof(ORToolsPreSolverWrapper));
            SeqLogger.Info("Problem solved in " + solver.Nodes() + " branch-and-bound nodes", nameof(ORToolsPreSolverWrapper));
            // [END Statistics]

            SeqLogger.Indent--;
            SeqLogger.Info("ORTools building finished!", nameof(ORToolsPreSolverWrapper));
            return solution;
        }

        private LinearConstraint CreateLinearConstraint(GTSPPrecedenceConstraint item, Variable[] pos, Variable[] x, int numberOfDisjunctives)
        {
            return pos[item.Before.SequencingID] + 1 <= pos[item.After.SequencingID] + numberOfDisjunctives * (2 - x[item.Before.SequencingID] - x[item.After.SequencingID]);
        }

        private LinearConstraint CreateLinearConstraint(GTSPDisjointConstraint disjoint, Variable[] x)
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