using ProSeqqoLib.Helper;
using ProSeqqoLib.Model.Hierarchy;
using ProSeqqoLib.OR_Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProSeqqoLib.Task.Processors
{
    public class InitialSolver
    {
        public TimeSpan Time { get; set; }
        private Stopwatch Timer;
        private GeneralTask Task;

        public List<Motion> InitialSolution { get; set; }
        public List<int> InitialSolutionIDs { get; set; }

        public InitialSolver(GeneralTask newGeneralTask)
        {
            Task = newGeneralTask;
            Timer = new Stopwatch();
            Time = new TimeSpan();
            InitialSolution = new List<Motion>();
            InitialSolutionIDs = new List<int>();
        }

        public void CreateInitialSolution()
        {
            if (InitialSolutionIDs is not null && InitialSolutionIDs.Count > 0)
                FillByIDs();

            if (InitialSolution is not null && InitialSolution.Count > 0) 
            {
                SeqLogger.Info("Initial solution given by the user.");
                if (Task.StartDepot is not null)
                {
                    if (Task.StartDepot.ID == InitialSolution[0].ID)
                        SeqLogger.Error("Start depot not needed in initial solution.");
                    //else
                        //InitialSolution.Insert(0, Task.StartDepot);
                }
                if (Task.FinishDepot is not null)
                {
                    if (Task.FinishDepot.ID == InitialSolution[^1].ID)
                        SeqLogger.Error("Finish depot not needed in initial solution.");
                    //else
                        //InitialSolution.Add(Task.FinishDepot);
                }
            }
            else
            {
                if (Task.SolverSettings.UseMIPprecedenceSolver)
                {
                    Timer.Restart();
                    Timer.Start();
                    var task = new ORToolsGeneralPreSolverTask()
                    {
                        NumberOfNodes = Task.Hierarchy.Motions.Count,
                        DisjointConstraints = Task.GTSPRepresentation.DisjointSets,
                        StrictOrderPrecedenceHierarchy = Task.GTSPRepresentation.CreatePrecedenceHierarchiesForInitialSolution(),
                        OrderPrecedenceConstraints = Task.GTSPRepresentation.MotionPrecedences,
                        Hierarchy = Task.Hierarchy
                    };
                    if (Task.GTSPRepresentation.StartDepot is not null)
                        task.StartDepot = Task.GTSPRepresentation.StartDepot.SequenceMatrixID;
                    else
                        task.StartDepot = -1;
                    if (Task.GTSPRepresentation.FinishDepot is not null)
                        task.FinishDepot = Task.GTSPRepresentation.FinishDepot.SequenceMatrixID;
                    else
                        task.FinishDepot = -1;
                    var solver = new ORToolsGeneralPreSolverWrapper(task);
                    var result = solver.Solve();
                    InitialSolution = PhraseSolution(result);
                    Timer.Stop();
                    Time = Timer.Elapsed;
                }
            }
        }

        private void FillByIDs()
        {
            foreach (var item in InitialSolutionIDs)
            {
                var result = Task.Hierarchy.GetMotionByID(item);
                if (result is not null)
                    InitialSolution.Add(result);
                else
                    SeqLogger.Error("Initial solution ID not found.");
            }
        }

        private List<Motion> PhraseSolution(ORToolsResult result)
        {
            var motions = new List<Motion>();
            if(result.StatusCode == 1 || result.StatusCode == 0)
            {
                foreach (var item in result.Solution)
                {
                    motions.Add(Task.Hierarchy.GetMotionBySeqID(item));
                }
            }

            return motions;
            //var ORPreSolver = new ORToolsGeneralPreSolverWrapper(task);

           // var result = ORPreSolver.Solve();
            //MIPRunTime = ORPreSolver.RunTime;
            //if (result.Count > 0)
           // {
            //   long[][] initialSolution = new long[1][];
            //    initialSolution[0] = new long[result.Count];
            //    for (int i = 0; i < result.Count; i++)
            //    {
            //        initialSolution[0][i] = result[i];
            //    }
            //    ResolveInitialSolution(initialSolution);
            //    return initialSolution;
            //}
            //return null;
        }
    }
}