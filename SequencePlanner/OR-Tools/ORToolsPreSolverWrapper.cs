using Google.OrTools.LinearSolver;
using SequencePlanner.GTSP;
using SequencePlanner.Helper;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
            Variable[] alter = solver.MakeIntVarArray(param.NumberOfNodes, 0.0, param.NumberOfNodes - 1, "alter");
            Variable[] proc = solver.MakeIntVarArray(param.NumberOfNodes, 0.0, param.Processes.Count - 1, "proc");
            

            // Precedences
            if (param.StartDepot > -1)
                solver.Add(pos[param.StartDepot] == 0.0);
            AddDisjointConstraints(solver, param.DisjointConstraints, x);
            AddPrecedenceConstraints(solver, param.PrecedenceConstraints, x, pos, param.DisjointConstraints.Count);
            AddStrictPrecedenceConstraints(solver, param.PrecedenceHierarchy, x, pos, param.DisjointConstraints.Count);
            AddAlterAndProc(solver, param.Processes, alter, proc, x);
            //AddDisjoinOrderConstraint(solver, param.DisjointConstraints, x, pos, param.DisjointConstraints.Count);
            SeqLogger.Info("Number of variables = " + solver.NumVariables(), nameof(ORToolsPreSolverWrapper));
            SeqLogger.Info("Number of constraints = " + solver.NumConstraints(), nameof(ORToolsPreSolverWrapper));

            Solver.ResultStatus resultStatus = RunSolver(solver);
            return ProcessSolution2(solver, resultStatus, x, pos, alter, proc);
        }

        private void AddAlterAndProc(Solver solver, List<Model.Process> processes, Variable[] alter, Variable[] proc, Variable[] x)
        {
            for (int i = 0; i < processes.Count; i++)
            {
                for (int j = 0; j < processes[i].Alternatives.Count; j++)
                {
                    for (int k = 0; k < processes[i].Alternatives[j].Tasks.Count; k++)
                    {
                        for (int m = 0; m < processes[i].Alternatives[j].Tasks[k].Positions.Count; m++)
                        {
                            solver.Add(proc[processes[i].Alternatives[j].Tasks[k].Positions[m].SequencingID] == i);
                            solver.Add(alter[processes[i].Alternatives[j].Tasks[k].Positions[m].SequencingID] == j);
                        }
                    }
                }
            }

            for (int i = 0; i < processes.Count; i++)
            {
                if (processes[i].Alternatives.Count > 2)
                {
                    for (int j = 0; j < processes[i].Alternatives.Count-1; j++)
                    {
                        for (int k = j; k < processes[i].Alternatives.Count; k++)
                        {
                            foreach (var a in processes[i].Alternatives[j].Tasks)
                            {
                                foreach (var b in processes[i].Alternatives[k].Tasks)
                                {
                                    foreach (var ap in a.Positions)
                                    {
                                        foreach (var bp in b.Positions)
                                        {
                                            solver.Add(x[ap.SequencingID] != x[bp.SequencingID]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                for (int j = 0; j < processes[i].Alternatives.Count; j++)
                {
                    for (int k = 0; k < processes[i].Alternatives[j].Tasks.Count; k++)
                    {
                        for (int m = 0; m < processes[i].Alternatives[j].Tasks[k].Positions.Count; m++)
                        {
                            solver.Add(proc[processes[i].Alternatives[j].Tasks[k].Positions[m].SequencingID] == i);
                            solver.Add(alter[processes[i].Alternatives[j].Tasks[k].Positions[m].SequencingID] == j);
                        }
                    }
                }
            }
        }

        private void AddDisjoinOrderConstraint(Solver solver, List<GTSPDisjointConstraint> disjointConstraints, Variable[] x, Variable[] pos, int count)
        {
            for (int i = 0; i < disjointConstraints.Count-1; i++)
            {
                for (int j = i+1; j < disjointConstraints.Count; j++)
                {
                    foreach (var m in disjointConstraints[i].DisjointSet)
                    {
                        foreach ( var n in disjointConstraints[j].DisjointSet)
                        {
                            solver.Add(pos[n] != pos[m]);
                            System.Console.WriteLine("pos["+n+"]!=pos["+m+"]");
                        }
                    }
                }
            }
        }

        private List<int> ProcessSolution(Solver solver, Solver.ResultStatus resultStatus, Variable[] x, Variable[] pos, Variable[] alter, Variable[] proc)
        {
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
                    if (x[p].SolutionValue() == 1)
                        SeqLogger.Trace("X[" + p + "]=" + x[p].SolutionValue() + ", POS[" + p + "]= " + pos[p].SolutionValue() + ", Alter[" + p + "]= " + alter[p].SolutionValue()+ ", Proc[" + p + "]= " + proc[p].SolutionValue(), nameof(ORToolsPreSolverWrapper));
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

        private List<int> ProcessSolution2(Solver solver, Solver.ResultStatus resultStatus, Variable[] x, Variable[] pos, Variable[] alter, Variable[] proc)
        {
            List<Proc> procs = new List<Proc>();
            for (int i = 0; i < param.Processes.Count; i++)
            {
                procs.Add(new Proc());
            }
            var solution = new List<int>();
            var tmpStringSolution = "Initial solution found: ";
            if (resultStatus == Solver.ResultStatus.OPTIMAL)
            {
                for (int i = 0; i < param.NumberOfNodes; i++)
                {
                    if (x[i].SolutionValue() == 1) {
                        int processID = (int)proc[i].SolutionValue();
                        procs[processID].posKey.Add(i);
                        procs[processID].posOrder.Add((int)pos[i].SolutionValue());
                        if (pos[i].SolutionValue() < procs[processID].min)
                            procs[processID].min = pos[i].SolutionValue();
                        if (pos[i].SolutionValue() > procs[processID].max)
                            procs[processID].max = pos[i].SolutionValue();
                    }
                }

                List<Proc> SortedList = procs.OrderBy(o => o.min).ToList();
                foreach (var item in SortedList)
                {
                    solution.AddRange(item.GetResult());
                }

                solution.Remove(param.StartDepot);

                foreach (var item in solution)
                {
                    tmpStringSolution += item.ToString() + ",";
                }


                for (int p = 0; p < param.NumberOfNodes; p++)
                {
                    if(x[p].SolutionValue()==1)
                        SeqLogger.Trace("X[" + p + "]=" + x[p].SolutionValue() + ", POS[" + p + "]= " + pos[p].SolutionValue() + ", A[" + p + "]= " + alter[p].SolutionValue() + ", P[" + p + "]= " + proc[p].SolutionValue(), nameof(ORToolsPreSolverWrapper));
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

        private Solver.ResultStatus RunSolver(Solver solver)
        {
            //Solve            
            SeqLogger.Info("Solver running!", nameof(ORToolsPreSolverWrapper));
            Solver.ResultStatus resultStatus = solver.Solve();
            SeqLogger.Info("Solver finished!", nameof(ORToolsPreSolverWrapper));
            return resultStatus;
        }

        private void AddPrecedenceConstraints(Solver solver, List<GTSPPrecedenceConstraint> precedenceConstraints, Variable[] x, Variable[] pos, int count)
        {
            SeqLogger.Trace("Precedences: ", nameof(ORToolsPreSolverWrapper)); SeqLogger.Indent++;
            foreach (var item in param.PrecedenceConstraints)
            {
                //solver.Add(pos[item.Before.SequencingID] + x[item.Before.SequencingID] <= pos[item.After.SequencingID] + x[item.After.SequencingID]);
                solver.Add(pos[item.Before.SequencingID] +1 <= pos[item.After.SequencingID] );
                SeqLogger.Trace(item.ToString(), nameof(ORToolsPreSolverWrapper));
            }
            SeqLogger.Indent--;
        }

        private void AddStrictPrecedenceConstraints(Solver solver, List<GTSPPrecedenceConstraint> precedenceHierarchy, Variable[] x, Variable[] pos, int count)
        {
            SeqLogger.Trace("StrictPrecedences: ", nameof(ORToolsPreSolverWrapper)); SeqLogger.Indent++;
            foreach (var item in param.PrecedenceHierarchy)
            {
                solver.Add(pos[item.Before.SequencingID] + 1 == pos[item.After.SequencingID]);
                //solver.Add(pos[item.Before.SequencingID] + 1 == pos[item.After.SequencingID]);
                SeqLogger.Trace(item.ToString(), nameof(ORToolsPreSolverWrapper));
            }
            SeqLogger.Indent--;
        }

        private void AddDisjointConstraints(Solver solver, List<GTSPDisjointConstraint> disjointConstraints, Variable[] x)
        {
            SeqLogger.Trace("DisjointSets: ", nameof(ORToolsPreSolverWrapper)); SeqLogger.Indent++;
            foreach (var item in param.DisjointConstraints)
            {
                solver.Add(CreateDisjointConstraint(item, x));
                SeqLogger.Trace(item.ToString(), nameof(ORToolsPreSolverWrapper));
            }
            SeqLogger.Indent--;
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
    
        private class Proc
        {
            public double min { get; set; }
            public double max { get; set; }
            public List<int> posKey { get; set; }
            public List<int> posOrder { get; set; }
            public int akt { get; set; }

            public Proc()
            {
                akt = -1;
                min = double.MaxValue;
                max = -1;
                posKey = new List<int>();
                posOrder = new List<int>();
            }

            public List<int> GetResult()
            {
                var result = new List<int>();
                var akt = min;
                for (int i = (int)min; i <= max; i++)
                {
                    for (int j = 0; j < posOrder.Count; j++)
                    {
                        if (posOrder[j] == i)
                            result.Add(posKey[j]);

                    }
                }
                return result;
            }
        }
    }

}