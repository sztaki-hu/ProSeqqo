using Google.OrTools.LinearSolver;
using SequencePlanner.GTSP;
using SequencePlanner.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SequencePlanner.OR_Tools
{
    public class ORToolsPointLikePreSolverWrapper
    {
        public TimeSpan RunTime { get; private set; }
        private readonly ORToolsPointLikePreSolverTask parameters;
        private Stopwatch timer;
        private Variable[] x;                 
        private Variable[] position; 
        private int[] alternativeID;
        private int[] processID;

        private List<string> constraints =  new List<string>(); //DEBUG

        public ORToolsPointLikePreSolverWrapper(ORToolsPointLikePreSolverTask parameters)
        {
            this.parameters = parameters;
            timer = new Stopwatch();
            RunTime = new TimeSpan();
        }

        public List<int> Solve()
        {

            // Init
            timer.Reset();
            timer.Start();
            SeqLogger.Info("ORTools building started!", nameof(ORToolsSequencerWrapper));
            SeqLogger.Indent++;
            Solver solver = Solver.CreateSolver("CBC_MIXED_INTEGER_PROGRAMMING");
            x = solver.MakeIntVarArray(parameters.NumberOfNodes, 0.0, 1.0, "x");                                                    // Boolean, indicates if node is selected
            position = solver.MakeIntVarArray(parameters.NumberOfNodes, 0.0, parameters.DisjointConstraints.Count, "pos");      // Int, indicates order of nodes
            //position = solver.MakeIntVarArray(parameters.NumberOfNodes, 0.0, parameters.DisjointConstraints.Count*1000, "pos");      // Int, indicates order of nodes
            //position = solver.MakeIntVarArray(parameters.NumberOfNodes, 0.0, 9.0, "pos");      // Int, indicates order of nodes
            alternativeID = new int[parameters.NumberOfNodes];                                                                      // Alternative ID of Node
            processID = new int[parameters.NumberOfNodes];                                                                          // Process ID of Node
            FillAlternativesAndProcesses(solver, parameters.Processes);                                                             // Fill ProcessID and AlternativeID

            // Precedences
            AddStartDepotConstraints(solver);
            AddDisjointConstraints(solver, parameters.DisjointConstraints);                                                      //Add disjoint sets of alternative nodes
            AddPrecedenceConstraints(solver, parameters.OrderPrecedenceConstraints);                                                 //Add order precedences, node1 should be before node2 in the solution if both are selected
            AddStrictPrecedenceConstraints(solver, parameters.StrictOrderPrecedenceHierarchy);                                       //Add strct order precedences, node1 must be followed by node2 directly
            AddStrictOrderPrecedenceConstraints(solver, parameters.StrictOrderPrecedenceHierarchy);                                       //Add strct order precedences, node1 must be followed by node2 directly
            //AddAlternativeDenyConstraints(solver, parameters.Processes);
            //foreach (var item in constraints)
            //{
            //    SeqLogger.Critical(item);
            //}
            SeqLogger.Info("Number of variables = " + solver.NumVariables(), nameof(ORToolsPointLikePreSolverWrapper));
            SeqLogger.Info("Number of constraints = " + solver.NumConstraints(), nameof(ORToolsPointLikePreSolverWrapper));

            // Solve
            Solver.ResultStatus resultStatus = RunSolver(solver);
            timer.Stop();
            RunTime = timer.Elapsed;
            return ProcessSolution1(solver, resultStatus);
        }
        private Solver.ResultStatus RunSolver(Solver solver)
        {
            //Solve            
            SeqLogger.Info("Solver running!", nameof(ORToolsPointLikePreSolverWrapper));
            Solver.ResultStatus resultStatus = solver.Solve();
            SeqLogger.Info("Solver finished!", nameof(ORToolsPointLikePreSolverWrapper));
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

        private void AddPrecedenceConstraints(Solver solver, List<GTSPPrecedenceConstraint> precedenceConstraints)
        {
            SeqLogger.Trace("Precedences: ", nameof(ORToolsPointLikePreSolverWrapper)); SeqLogger.Indent++;
            foreach (var item in parameters.OrderPrecedenceConstraints)
            {
                solver.Add(position[item.Before.SequencingID] +1 <= position[item.After.SequencingID]+parameters.DisjointConstraints.Count*(2-x[item.Before.SequencingID]-x[item.After.SequencingID]));
                //solver.Add(position[item.Before.SequencingID] +1 <= position[item.After.SequencingID]+9*(2-x[item.Before.SequencingID]-x[item.After.SequencingID]));
                constraints.Add((position[item.Before.SequencingID] + 1 <= position[item.After.SequencingID] + parameters.DisjointConstraints.Count * (2 - x[item.Before.SequencingID] - x[item.After.SequencingID])).ToString());
                SeqLogger.Trace(item.ToString(), nameof(ORToolsPointLikePreSolverWrapper));
            }
            SeqLogger.Indent--;
        }        
        private void AddStrictPrecedenceConstraints(Solver solver, List<GTSPPrecedenceConstraintList> precedenceHierarchy)
        {
            SeqLogger.Trace("StrictPrecedences: ", nameof(ORToolsPointLikePreSolverWrapper)); SeqLogger.Indent++;
            foreach (var item in precedenceHierarchy)
            {
                solver.Add(CreateStrictOrderPrecedence(item.Before, item.After));
                SeqLogger.Trace(item.ToString(), nameof(ORToolsPointLikePreSolverWrapper));
            }
            SeqLogger.Indent--;
        }
        private void AddStrictOrderPrecedenceConstraints(Solver solver, List<GTSPPrecedenceConstraintList> strictOrderPrecedenceHierarchy)
        {
            SeqLogger.Trace("StrictOrderPrecedences: ", nameof(ORToolsPointLikePreSolverWrapper)); SeqLogger.Indent++;
            foreach (var item in strictOrderPrecedenceHierarchy)
            {
                foreach (var b in item.Before)
                {
                    foreach (var a in item.After)
                    {
                        solver.Add(position[b.SequencingID]+1 == position[a.SequencingID]);
                        constraints.Add((position[b.SequencingID] + 1 == position[a.SequencingID]).ToString());
                        SeqLogger.Trace(new GTSPPrecedenceConstraint(b,a).ToString(), nameof(ORToolsPointLikePreSolverWrapper));
                    }
                }     
            }
            SeqLogger.Indent--;
        }
        private void AddDisjointConstraints(Solver solver, List<GTSPDisjointConstraint> disjointConstraints)
        {
            SeqLogger.Trace("DisjointSets: ", nameof(ORToolsPointLikePreSolverWrapper)); SeqLogger.Indent++;
            foreach (var item in parameters.DisjointConstraints)
            {
                solver.Add(CreateDisjointConstraint(item));
                SeqLogger.Trace(item.ToString(), nameof(ORToolsPointLikePreSolverWrapper));
            }
            SeqLogger.Indent--;
        }

        private LinearConstraint CreateStrictOrderPrecedence(List<Model.BaseNode> before, List<Model.BaseNode> after)
        {
            LinearExpr beforExpr = new LinearExpr();
            foreach (var item in before)
            {
                beforExpr += x[item.SequencingID];
            }
            LinearExpr afterExpr = new LinearExpr();
            foreach (var item in after)
            {
                afterExpr += x[item.SequencingID];
            }
            constraints.Add((beforExpr == afterExpr).ToString());
            return beforExpr == afterExpr;
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
                            processID[processes[i].Alternatives[j].Tasks[k].Positions[m].Node.SequencingID] = i;         // i is the index of process; processID[n] n is the number of node
                            alternativeID[processes[i].Alternatives[j].Tasks[k].Positions[m].Node.SequencingID] = j;     // j is the index of alternative in i process; processID[n] n is the number of node
                        }
                    }
                }
            }
        }
        private List<int> ProcessSolution1(Solver solver, Solver.ResultStatus resultStatus)
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
                solution.Remove(parameters.StartDepot);                                                      //Remove the first "start depot" because OR-Tools routing requires the initial solution without depot.
                foreach (var item in solution)
                    solutionString += item.ToString() + ",";

                for (int p = 0; p < parameters.NumberOfNodes; p++)                                           //Trace the selected nodes
                {
                    if (x[p].SolutionValue() == 1)
                        SeqLogger.Trace("i: " + p + " X = " + x[p].SolutionValue() + ", Position = " + this.position[p].SolutionValue() + ", Alternative = " + this.alternativeID[p] + ", Process = " + this.processID[p], nameof(OR_Tools.ORToolsPointLikePreSolverWrapper));
                }   
                SeqLogger.Info(solutionString, nameof(ORToolsPointLikePreSolverWrapper));
            }
            else
            {
                SeqLogger.Info("Solver stopped with status code: " + DecodeStatusCode(resultStatus), nameof(ORToolsPointLikePreSolverWrapper));
                SeqLogger.Error("Can not find optimal initial solution!", nameof(ORToolsPointLikePreSolverWrapper));
                throw new SeqException("Can not find optimal initial solution with MIP solver!");
            }
            SeqLogger.Info("Solver stopped with status code: " + DecodeStatusCode(resultStatus), nameof(ORToolsPointLikePreSolverWrapper));
            SeqLogger.Info("Problem solved in " + solver.WallTime() + " milliseconds", nameof(ORToolsPointLikePreSolverWrapper));
            SeqLogger.Info("Problem solved in " + solver.Nodes() + " branch-and-bound nodes", nameof(ORToolsPointLikePreSolverWrapper));
            SeqLogger.Indent--;
            SeqLogger.Info("ORTools building finished!", nameof(ORToolsPointLikePreSolverWrapper));
            return solution;
        }
        private LinearConstraint CreateDisjointConstraint(GTSPDisjointConstraint disjoint)
        {
            LinearExpr constraintExpr = new LinearExpr();
            foreach (var item in disjoint.DisjointSetSeq)
            {
                constraintExpr += x[item];
            }
            LinearConstraint contraint = constraintExpr == 1.0;
            constraints.Add(contraint.ToString());
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