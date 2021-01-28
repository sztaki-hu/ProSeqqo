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
            Solver solver = Solver.CreateSolver("CBC_MIXED_INTEGER_PROGRAMMING");

            Variable[] x = solver.MakeIntVarArray(param.NumberOfNodes, 0.0, 1.0, "x");  // Boolean, indicates if node is selected
            Variable[] pos = solver.MakeIntVarArray(param.NumberOfNodes, 0.0, param.DisjointConstraints.Count - 1, "pos");  // Boolean, indicates if node is selected

            // Precedences
            if (param.StartDepot > -1)
                solver.Add(pos[param.StartDepot] == 0.0);
            AddDisjointConstraints(solver, param.DisjointConstraints, x);
            AddPrecedenceConstraints(solver, param.PrecedenceConstraints, x, pos, param.DisjointConstraints.Count);
            AddStrictPrecedenceConstraints(solver, param.PrecedenceHierarchy, x, pos, param.DisjointConstraints.Count);

            //Solve            
            SeqLogger.Info("Solver running!", nameof(ORToolsPreSolverWrapper));
            Solver.ResultStatus resultStatus = solver.Solve();
            SeqLogger.Info("Solver finished!", nameof(ORToolsPreSolverWrapper));

            //Solution
            var solution = new List<int>();
            if (resultStatus == Solver.ResultStatus.OPTIMAL)
            {
                var tmpStringSolution = "Initial solution found: ";
                for (int p = 0; p < param.NumberOfNodes; p++)
                {
                    for (int i = 0; i < param.NumberOfNodes; i++)
                    {
                        if (x[i].SolutionValue() == 1.0 && pos[i].SolutionValue() == p)
                        {
                            if (i != param.StartDepot)
                            {
                                tmpStringSolution += i.ToString() + ",";
                                solution.Add(i);
                            }
                        }
                    }
                    SeqLogger.Trace("X[" + p + "]=" + x[p].SolutionValue() + ", POS[" + p + "]= " + pos[p].SolutionValue(), nameof(ORToolsPreSolverWrapper));
                }
                SeqLogger.Info(tmpStringSolution.Remove(tmpStringSolution.Length - 1), nameof(ORToolsPreSolverWrapper));
            }
            else
            {
                SeqLogger.Info("Solver stopped with status code: " + DecodeStatusCode(resultStatus), nameof(ORToolsPreSolverWrapper));
                SeqLogger.Error("Can not find optimal initial solution!", nameof(ORToolsPreSolverWrapper));
                throw new SequencerException("Can not find optimal initial solution with MIP solver!");
            }
            SeqLogger.Info("Solver stopped with status code: " + DecodeStatusCode(resultStatus), nameof(ORToolsPreSolverWrapper));
            SeqLogger.Info("Problem solved in " + solver.WallTime() + " milliseconds", nameof(ORToolsPreSolverWrapper));
            SeqLogger.Info("Problem solved in " + solver.Nodes() + " branch-and-bound nodes", nameof(ORToolsPreSolverWrapper));
            SeqLogger.Indent--;
            SeqLogger.Info("ORTools building finished!", nameof(ORToolsPreSolverWrapper));
            return solution;
        }

        private void AddStrictPrecedenceConstraints(Solver solver, List<GTSPPrecedenceConstraint> precedenceHierarchy, Variable[] x, Variable[] pos, int count)
        {
            foreach (var item in param.PrecedenceHierarchy)
            {
                solver.Add(pos[item.Before.SequencingID] + 1 == pos[item.After.SequencingID] + count * (2 - x[item.Before.SequencingID] - x[item.After.SequencingID]));
                solver.Add(pos[item.Before.SequencingID] + 1 >= pos[item.After.SequencingID]);
                SeqLogger.Trace(item.ToString(), nameof(ORToolsPreSolverWrapper));
            }
            SeqLogger.Info("Number of variables = " + solver.NumVariables(), nameof(ORToolsPreSolverWrapper));
            SeqLogger.Info("Number of constraints = " + solver.NumConstraints(), nameof(ORToolsPreSolverWrapper));
        }

        private void AddPrecedenceConstraints(Solver solver, List<GTSPPrecedenceConstraint> precedenceConstraints, Variable[] x, Variable[] pos, int count)
        {
            foreach (var item in param.PrecedenceConstraints)
            {
                solver.Add(pos[item.Before.SequencingID] + 1 <= pos[item.After.SequencingID] + count * (2 - x[item.Before.SequencingID] - x[item.After.SequencingID]));
                SeqLogger.Trace(item.ToString(), nameof(ORToolsPreSolverWrapper));
            }
            SeqLogger.Info("Number of variables = " + solver.NumVariables(), nameof(ORToolsPreSolverWrapper));
            SeqLogger.Info("Number of constraints = " + solver.NumConstraints(), nameof(ORToolsPreSolverWrapper));
        }

        private void AddDisjointConstraints(Solver solver, List<GTSPDisjointConstraint> disjointConstraints, Variable[] x)
        {
            foreach (var item in param.DisjointConstraints)
            {
                solver.Add(CreateDisjointConstraint(item, x));
                SeqLogger.Trace(item.ToString(), nameof(ORToolsPreSolverWrapper));
            }
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