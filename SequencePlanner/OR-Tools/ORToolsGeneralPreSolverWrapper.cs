using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Google.OrTools.LinearSolver;
using SequencePlanner.Model;
using SequencePlanner.Helper;
using SequencePlanner.Model.Hierarchy;

namespace SequencePlanner.OR_Tools
{
    public class ORToolsGeneralPreSolverWrapper
    {
        public TimeSpan RunTime { get; private set; }
        private readonly ORToolsGeneralPreSolverTask parameters;
        private readonly Stopwatch timer;
        private Variable[] x;                 
        private Variable[] position; 
        private int[] alternativeID;
        private int[] processID;
        private readonly List<string> constraints =  new List<string>(); 

        public ORToolsGeneralPreSolverWrapper(ORToolsGeneralPreSolverTask parameters)
        {
            this.parameters = parameters;
            timer = new Stopwatch();
            RunTime = new TimeSpan();
        }

        public ORToolsResult Solve()
        {
            // Init
            timer.Reset();
            timer.Start();
            SeqLogger.Debug("ORTools building started!", nameof(ORToolsSequencerWrapper));
            SeqLogger.Indent++;
            Solver solver = Solver.CreateSolver("CBC_MIXED_INTEGER_PROGRAMMING");
            x = solver.MakeIntVarArray(parameters.NumberOfNodes, 0.0, 1.0, "x");                                                    // Boolean, indicates if node is selected
            position = solver.MakeIntVarArray(parameters.NumberOfNodes, 0.0, parameters.DisjointConstraints.Count, "pos");      // Int, indicates order of nodes
            //position = solver.MakeIntVarArray(parameters.NumberOfNodes, 0.0, parameters.DisjointConstraints.Count*1000, "pos");      // Int, indicates order of nodes
            //position = solver.MakeIntVarArray(parameters.NumberOfNodes, 0.0, 9.0, "pos");      // Int, indicates order of nodes
            alternativeID = new int[parameters.NumberOfNodes];                                                                      // Alternative ID of Node
            processID = new int[parameters.NumberOfNodes];                                                                          // Process ID of Node
            //FillAlternativesAndProcesses(parameters.Processes);                                                             // Fill ProcessID and AlternativeID

            // Precedences
            AddStartDepotConstraints(solver);
            AddFinishDepotConstraints(solver);
            AddDisjointConstraints(solver, parameters.DisjointConstraints);                                                      //Add disjoint sets of alternative nodes
            AddPrecedenceConstraints(solver, parameters.OrderPrecedenceConstraints);                                                 //Add order precedences, node1 should be before node2 in the solution if both are selected
            AddStrictPrecedenceConstraints(solver, parameters.StrictOrderPrecedenceHierarchy);                                       //Add strct order precedences, node1 must be followed by node2 directly
            AddStrictOrderPrecedenceConstraints(solver, parameters.StrictOrderPrecedenceHierarchy);                                       //Add strct order precedences, node1 must be followed by node2 directly
            //AddAlternativeDenyConstraints(solver, parameters.Processes);
            //foreach (var item in constraints)
            //{
            //    SeqLogger.Critical(item);
            //}
            SeqLogger.Debug("Number of variables = " + solver.NumVariables(), nameof(ORToolsGeneralPreSolverWrapper));
            SeqLogger.Debug("Number of constraints = " + solver.NumConstraints(), nameof(ORToolsGeneralPreSolverWrapper));

            // Solve
            Solver.ResultStatus resultStatus = RunSolver(solver);
            timer.Stop();
            RunTime = timer.Elapsed;
            return ProcessSolution(solver, resultStatus);
        }
        private static Solver.ResultStatus RunSolver(Solver solver)
        {
            //Solve            
            SeqLogger.Info("Solver running!", nameof(ORToolsGeneralPreSolverWrapper));
            Solver.ResultStatus resultStatus = solver.Solve();
            SeqLogger.Debug("Solver finished!", nameof(ORToolsGeneralPreSolverWrapper));
            return resultStatus;
        }
        
        private void AddStartDepotConstraints(Solver solver)
        {
            if (parameters.StartDepot > -1)                                                                                         //If Start depo exist, position = 0 and selected x = 1
            {
                solver.Add(position[parameters.StartDepot] == 0.0);
                constraints.Add((position[parameters.StartDepot] == 0.0).ToString());
                solver.Add(x[parameters.StartDepot] == 1.0);
                constraints.Add((x[parameters.StartDepot] == 1.0).ToString());
            }
        }
        private void AddFinishDepotConstraints(Solver solver)
        {
            if (parameters.FinishDepot > -1)                                                                                         //If Start depo exist, position = 0 and selected x = 1
            {
                solver.Add(position[parameters.FinishDepot] == parameters.DisjointConstraints.Count);
                constraints.Add((position[parameters.FinishDepot] == parameters.DisjointConstraints.Count).ToString());
                solver.Add(x[parameters.FinishDepot] == 1.0);
                constraints.Add((x[parameters.FinishDepot] == 1.0).ToString());
            }
        }
        private void AddPrecedenceConstraints(Solver solver, List<MotionPrecedence> precedenceConstraints)
        {
            SeqLogger.Trace("Precedences: ", nameof(ORToolsGeneralPreSolverWrapper)); SeqLogger.Indent++;
            foreach (var item in parameters.OrderPrecedenceConstraints)
            {
                solver.Add(position[item.Before.SequenceMatrixID] +1 <= position[item.After.SequenceMatrixID] +parameters.DisjointConstraints.Count*(2-x[item.Before.SequenceMatrixID] -x[item.After.SequenceMatrixID]));
                //solver.Add(position[item.Before.SequencingID] +1 <= position[item.After.SequencingID]+9*(2-x[item.Before.SequencingID]-x[item.After.SequencingID]));
                constraints.Add((position[item.Before.SequenceMatrixID] + 1 <= position[item.After.SequenceMatrixID] + parameters.DisjointConstraints.Count * (2 - x[item.Before.SequenceMatrixID] - x[item.After.SequenceMatrixID])).ToString());
                SeqLogger.Trace(item.ToString(), nameof(ORToolsGeneralPreSolverWrapper));
            }
            SeqLogger.Indent--;
        }        
        private void AddStrictPrecedenceConstraints(Solver solver, List<MotionPrecedenceList> precedenceHierarchy)
        {
            SeqLogger.Trace("StrictPrecedences: ", nameof(ORToolsGeneralPreSolverWrapper)); SeqLogger.Indent++;
            foreach (var item in precedenceHierarchy)
            {
                solver.Add(CreateStrictOrderPrecedence(item.Before, item.After));
                SeqLogger.Trace(item.ToString(), nameof(ORToolsGeneralPreSolverWrapper));
            }
            SeqLogger.Indent--;
        }
        private void AddStrictOrderPrecedenceConstraints(Solver solver, List<MotionPrecedenceList> strictOrderPrecedenceHierarchy)
        {
            SeqLogger.Trace("StrictOrderPrecedences: ", nameof(ORToolsGeneralPreSolverWrapper)); SeqLogger.Indent++;
            foreach (var item in strictOrderPrecedenceHierarchy)
            {
                foreach (var b in item.Before)
                {
                    foreach (var a in item.After)
                    {
                        solver.Add(position[b.SequenceMatrixID]+1 == position[a.SequenceMatrixID]);
                        constraints.Add((position[b.SequenceMatrixID] + 1 == position[a.SequenceMatrixID]).ToString());
                        SeqLogger.Trace(new MotionPrecedence(b,a).ToString(), nameof(ORToolsGeneralPreSolverWrapper));
                    }
                }     
            }
            SeqLogger.Indent--;
        }
        private void AddDisjointConstraints(Solver solver, List<MotionDisjointSet> disjointConstraints)
        {
            SeqLogger.Trace("DisjointSets: ", nameof(ORToolsGeneralPreSolverWrapper)); SeqLogger.Indent++;
            foreach (var item in parameters.DisjointConstraints)
            {
                solver.Add(CreateDisjointConstraint(item));
                SeqLogger.Trace(item.ToString(), nameof(ORToolsGeneralPreSolverWrapper));
            }
            SeqLogger.Indent--;
        }

        private void FillAlternativesAndProcesses(List<Process> processes)
        {
            //for (int i = 0; i < processes.Count; i++)
            //{
            //    for (int j = 0; j < processes[i].Alternatives.Count; j++)
            //    {
            //        for (int k = 0; k < processes[i].Alternatives[j].Tasks.Count; k++)
            //        {
            //            for (int m = 0; m < processes[i].Alternatives[j].Tasks[k].Positions.Count; m++)
            //            {
            //                processID[processes[i].Alternatives[j].Tasks[k].Positions[m].Node.SequencingID] = i;         // i is the index of process; processID[n] n is the number of node
            //                alternativeID[processes[i].Alternatives[j].Tasks[k].Positions[m].Node.SequencingID] = j;     // j is the index of alternative in i process; processID[n] n is the number of node
            //            }
            //        }
            //    }
            //}
        }
        private LinearConstraint CreateStrictOrderPrecedence(List<Motion> before, List<Motion> after)
        {
            LinearExpr beforExpr = new LinearExpr();
            foreach (var item in before)
            {
                beforExpr += x[item.SequenceMatrixID];
            }
            LinearExpr afterExpr = new LinearExpr();
            foreach (var item in after)
            {
                afterExpr += x[item.SequenceMatrixID];
            }
            constraints.Add((beforExpr == afterExpr).ToString());
            return beforExpr == afterExpr;
        }
        private LinearConstraint CreateDisjointConstraint(MotionDisjointSet disjoint)
        {
            LinearExpr constraintExpr = new LinearExpr();
            foreach (var item in disjoint.SequencMatrixcIDs)
            {
                constraintExpr += x[item];
            }
            LinearConstraint contraint = constraintExpr == 1.0;
            constraints.Add(contraint.ToString());
            return contraint;
        }
        private ORToolsResult ProcessSolution(Solver solver, Solver.ResultStatus resultStatus)
        {
            List<Process> processes = new List<Process>();
            var solution = new List<int>();
            var solutionString = "Initial solution found: ";
            
            for (int i = 0; i < parameters.Processes.Count; i++)
            {
                processes.Add(new Process());
            }

            if (resultStatus == Solver.ResultStatus.OPTIMAL)
            {
                for (int i = 0; i < parameters.NumberOfNodes; i++)
                {
                    if (x[i].SolutionValue() == 1) {                                                    //If node selected
                        int aktProcessID = (int)processID[i];
                        processes[aktProcessID].PosKey.Add(i);                                          //Add node index to process
                        processes[aktProcessID].PosOrder.Add((int)position[i].SolutionValue());         //Add node position to process
                        if (position[i].SolutionValue() < processes[aktProcessID].Min)                  
                            processes[aktProcessID].Min = position[i].SolutionValue();                  //Find the first position in process
                        if (position[i].SolutionValue() > processes[aktProcessID].Max)
                            processes[aktProcessID].Max = position[i].SolutionValue();                  //Find the last position in process
                    }
                }

                List<Process> SortedList = processes.OrderBy(o => o.Min).ToList();                      //Order the processes by the minimum
                foreach (var item in SortedList)
                    solution.AddRange(item.GetResult());                                                //Add the nodes of the process
                solution.Remove(parameters.StartDepot);                                                      //Remove the first "start depot" because OR-Tools routing requires the initial solution without depot.
                foreach (var item in solution)
                    solutionString += item.ToString() + ",";

                for (int p = 0; p < parameters.NumberOfNodes; p++)                                           //Trace the selected nodes
                {
                    if (x[p].SolutionValue() == 1)
                        SeqLogger.Trace("i: " + p + " X = " + x[p].SolutionValue() + ", Position = " + this.position[p].SolutionValue() + ", Alternative = " + this.alternativeID[p] + ", Process = " + this.processID[p], nameof(OR_Tools.ORToolsGeneralPreSolverWrapper));
                }   
                SeqLogger.Info(solutionString, nameof(ORToolsGeneralPreSolverWrapper));
            }
            else
            {
                SeqLogger.Debug("Solver stopped with status code: " + DecodeStatusCode(resultStatus), nameof(ORToolsGeneralPreSolverWrapper));
                SeqLogger.Error("Can not find optimal initial solution!", nameof(ORToolsGeneralPreSolverWrapper));
                throw new SeqException("Can not find optimal initial solution with MIP solver!");
            }
            SeqLogger.Debug("Solver stopped with status code: " + DecodeStatusCode(resultStatus), nameof(ORToolsGeneralPreSolverWrapper));
            SeqLogger.Debug("Problem solved in " + solver.WallTime() + " milliseconds", nameof(ORToolsGeneralPreSolverWrapper));
            SeqLogger.Debug("Problem solved in " + solver.Nodes() + " branch-and-bound nodes", nameof(ORToolsGeneralPreSolverWrapper));
            SeqLogger.Indent--;
            SeqLogger.Debug("ORTools building finished!", nameof(ORToolsGeneralPreSolverWrapper));
            var result = new ORToolsResult()
            {
                Solution = solution,
                StatusCode = DecodeStatusCodeInt(resultStatus),
                StatusMessage = DecodeStatusCode(resultStatus),
                Time = timer.Elapsed
            };
            return result;
        }
        private static string DecodeStatusCode(Solver.ResultStatus status)
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

        private static int DecodeStatusCodeInt(Solver.ResultStatus status)
        {
            return status switch
            {
                Solver.ResultStatus.OPTIMAL => 0,
                Solver.ResultStatus.FEASIBLE => 1,
                Solver.ResultStatus.INFEASIBLE => 2,
                Solver.ResultStatus.UNBOUNDED => 3,
                Solver.ResultStatus.ABNORMAL => 4,
                Solver.ResultStatus.NOT_SOLVED => 6,
                _ => -1,
            };
        }

        private class Process
        {
            public double Min { get; set; }
            public double Max { get; set; }
            public List<int> PosKey { get; set; }
            public List<int> PosOrder { get; set; }
            public int Akt { get; set; }

            public Process()
            {
                Akt = -1;
                Min = double.MaxValue;
                Max = -1;
                PosKey = new List<int>();
                PosOrder = new List<int>();
            }
            public List<int> GetResult()
            {
                var result = new List<int>();
                for (int i = (int)Min; i <= Max; i++)
                {
                    for (int j = 0; j < PosOrder.Count; j++)
                    {
                        if (PosOrder[j] == i)
                            result.Add(PosKey[j]);

                    }
                }
                return result;
            }
        }
    }
}