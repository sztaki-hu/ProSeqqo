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
        private Variable[] x;                 
        private Variable[] position; 
        private int[] alternativeID;
        private int[] processID;

        public ORToolsPreSolverWrapper(ORToolsPreSolverTask parameters)
        {
            param = parameters;
        }

        public List<int> Solve()
        {
            // Init
            SeqLogger.Info("ORTools building started!", nameof(ORToolsSequencerWrapper));
            SeqLogger.Indent++;
            Solver solver = Solver.CreateSolver("CBC_MIXED_INTEGER_PROGRAMMING");
            x = solver.MakeIntVarArray(param.NumberOfNodes, 0.0, 1.0, "x");                                                     // Boolean, indicates if node is selected
            position = solver.MakeIntVarArray(param.NumberOfNodes, 0.0, param.DisjointConstraints.Count - 1, "pos");            // Int, indicates order of nodes
            alternativeID = new int[param.NumberOfNodes];                                                                       // Alternative ID of Node
            processID = new int[param.NumberOfNodes];                                                                           // Process ID of Node
            FillAlternativesAndProcesses(solver, param.Processes);                                                              // Fill ProcessID and AlternativeID

            // Precedences
            if (param.StartDepot > -1)                                                                                          //If Start depo exist, position = 0
                solver.Add(position[param.StartDepot] == 0.0);
            AddDisjointConstraints(solver, param.DisjointConstraints)    ;                                                      //Add disjoint sets of alternative nodes
            AddPrecedenceConstraints(solver, param.PrecedenceConstraints);                                                      //Add order precedences, node1 should be before node2 in the solution if both are selected
            AddStrictPrecedenceConstraints(solver, param.PrecedenceHierarchy);                                                  //Add strct order precedences, node1 must be followed by node2 directly
            AddAlternativeDenyConstraints(solver, param.Processes);
            SeqLogger.Info("Number of variables = " + solver.NumVariables(), nameof(ORToolsPreSolverWrapper));
            SeqLogger.Info("Number of constraints = " + solver.NumConstraints(), nameof(ORToolsPreSolverWrapper));

            // Solve
            Solver.ResultStatus resultStatus = RunSolver(solver);
            return ProcessSolution2(solver, resultStatus);
        }

        private Solver.ResultStatus RunSolver(Solver solver)
        {
            //Solve            
            SeqLogger.Info("Solver running!", nameof(ORToolsPreSolverWrapper));
            Solver.ResultStatus resultStatus = solver.Solve();
            SeqLogger.Info("Solver finished!", nameof(ORToolsPreSolverWrapper));
            return resultStatus;
        }

        private void AddAlternativeDenyConstraints(Solver solver, List<Model.Process> processes)
        {
            for (int i = 0; i < processes.Count; i++)
            {
                if (processes[i].Alternatives.Count > 2)
                {
                    for (int j = 0; j < processes[i].Alternatives.Count - 1; j++)
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
                                            solver.Add(x[ap.SequencingID] != x[bp.SequencingID]);                               //If ap node and bp node are in the same process and in different alternative, only one alternative must be selected.
                                        }                                                                                       //Must not to step across form a alternative to b alternative 
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AddPrecedenceConstraints(Solver solver, List<GTSPPrecedenceConstraint> precedenceConstraints)
        {
            SeqLogger.Trace("Precedences: ", nameof(ORToolsPreSolverWrapper)); SeqLogger.Indent++;
            foreach (var item in param.PrecedenceConstraints)
            {
                solver.Add(position[item.Before.SequencingID] +1 <= position[item.After.SequencingID] );
                SeqLogger.Trace(item.ToString(), nameof(ORToolsPreSolverWrapper));
            }
            SeqLogger.Indent--;
        }

        private void AddStrictPrecedenceConstraints(Solver solver, List<GTSPPrecedenceConstraint> precedenceHierarchy)
        {
            SeqLogger.Trace("StrictPrecedences: ", nameof(ORToolsPreSolverWrapper)); SeqLogger.Indent++;
            foreach (var item in param.PrecedenceHierarchy)
            {
                solver.Add(position[item.Before.SequencingID] + 1 == position[item.After.SequencingID]);
                SeqLogger.Trace(item.ToString(), nameof(ORToolsPreSolverWrapper));
            }
            SeqLogger.Indent--;
        }

        private void AddDisjointConstraints(Solver solver, List<GTSPDisjointConstraint> disjointConstraints)
        {
            SeqLogger.Trace("DisjointSets: ", nameof(ORToolsPreSolverWrapper)); SeqLogger.Indent++;
            foreach (var item in param.DisjointConstraints)
            {
                solver.Add(CreateDisjointConstraint(item, x));
                SeqLogger.Trace(item.ToString(), nameof(ORToolsPreSolverWrapper));
            }
            SeqLogger.Indent--;
        }

        private void FillAlternativesAndProcesses(Solver solver, List<Model.Process> processes)
        {
            for (int i = 0; i < processes.Count; i++)
            {
                for (int j = 0; j < processes[i].Alternatives.Count; j++)
                {
                    for (int k = 0; k < processes[i].Alternatives[j].Tasks.Count; k++)
                    {
                        for (int m = 0; m < processes[i].Alternatives[j].Tasks[k].Positions.Count; m++)
                        {
                            processID[processes[i].Alternatives[j].Tasks[k].Positions[m].SequencingID] = i;         // i is the index of process; processID[n] n is the number of node
                            alternativeID[processes[i].Alternatives[j].Tasks[k].Positions[m].SequencingID] = j;     // j is the index of alternative in i process; processID[n] n is the number of node
                        }
                    }
                }
            }
        }

        private List<int> ProcessSolution2(Solver solver, Solver.ResultStatus resultStatus)
        {
            List<Process> processes = new List<Process>();
            var solution = new List<int>();
            var solutionString = "Initial solution found: ";
            
            for (int i = 0; i < param.Processes.Count; i++)
            {
                processes.Add(new Process());
            }

            if (resultStatus == Solver.ResultStatus.OPTIMAL)
            {
                for (int i = 0; i < param.NumberOfNodes; i++)
                {
                    if (x[i].SolutionValue() == 1) {                                                    //If node selected
                        int aktProcessID = (int)processID[i];
                        processes[aktProcessID].posKey.Add(i);                                          //Add node index to process
                        processes[aktProcessID].posOrder.Add((int)position[i].SolutionValue());         //Add node position to process
                        if (position[i].SolutionValue() < processes[aktProcessID].min)                  
                            processes[aktProcessID].min = position[i].SolutionValue();                  //Find the first position in process
                        if (position[i].SolutionValue() > processes[aktProcessID].max)
                            processes[aktProcessID].max = position[i].SolutionValue();                  //Find the last position in process
                    }
                }

                List<Process> SortedList = processes.OrderBy(o => o.min).ToList();                      //Order the processes by the minimum
                foreach (var item in SortedList)
                    solution.AddRange(item.GetResult());                                                //Add the nodes of the process
                solution.Remove(param.StartDepot);                                                      //Remove the first "start depot" because OR-Tools routing requires the initial solution without depot.
                foreach (var item in solution)
                    solutionString += item.ToString() + ",";

                for (int p = 0; p < param.NumberOfNodes; p++)                                           //Trace the selected nodes
                {
                    if (x[p].SolutionValue() == 1)
                        SeqLogger.Trace("i: " + p + " X = " + x[p].SolutionValue() + ", Position = " + this.position[p].SolutionValue() + ", Alternative = " + this.alternativeID[p] + ", Process = " + this.processID[p], nameof(OR_Tools.ORToolsPreSolverWrapper));
                }   
                SeqLogger.Info(solutionString, nameof(ORToolsPreSolverWrapper));
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
    
        private class Process
        {
            public double min { get; set; }
            public double max { get; set; }
            public List<int> posKey { get; set; }
            public List<int> posOrder { get; set; }
            public int akt { get; set; }

            public Process()
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

//private List<int> ProcessSolution(Solver solver, Solver.ResultStatus resultStatus)
//{
//    var solution = new List<int>();
//    if (resultStatus == Solver.ResultStatus.OPTIMAL)
//    {
//        var tmpStringSolution = "Initial solution found: ";
//        for (int p = 0; p < param.NumberOfNodes; p++)
//        {
//            for (int i = 0; i < param.NumberOfNodes; i++)
//            {
//                if (x[i].SolutionValue() == 1.0 && position[i].SolutionValue() == p)
//                {
//                    if (i != param.StartDepot)
//                    {
//                        tmpStringSolution += i.ToString() + ",";
//                        solution.Add(i);
//                    }
//                }
//            }
//            if (x[p].SolutionValue() == 1)
//                SeqLogger.Trace("i: " + p + " X = " + x[p].SolutionValue() + ", Position = " + position[p].SolutionValue() + ", Alternative = " + alternativeID[p] + ", Process = " + processID[p], nameof(ORToolsPreSolverWrapper));
//        }
//        SeqLogger.Info(tmpStringSolution.Remove(tmpStringSolution.Length - 1), nameof(ORToolsPreSolverWrapper));
//    }
//    else
//    {
//        SeqLogger.Info("Solver stopped with status code: " + DecodeStatusCode(resultStatus), nameof(ORToolsPreSolverWrapper));
//        SeqLogger.Error("Can not find optimal initial solution!", nameof(ORToolsPreSolverWrapper));
//        throw new SequencerException("Can not find optimal initial solution with MIP solver!");
//    }
//    SeqLogger.Info("Solver stopped with status code: " + DecodeStatusCode(resultStatus), nameof(ORToolsPreSolverWrapper));
//    SeqLogger.Info("Problem solved in " + solver.WallTime() + " milliseconds", nameof(ORToolsPreSolverWrapper));
//    SeqLogger.Info("Problem solved in " + solver.Nodes() + " branch-and-bound nodes", nameof(ORToolsPreSolverWrapper));
//    SeqLogger.Indent--;
//    SeqLogger.Info("ORTools building finished!", nameof(ORToolsPreSolverWrapper));
//    return solution;
//}