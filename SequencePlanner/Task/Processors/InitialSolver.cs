using SequencePlanner.Model.Hierarchy;
using SequencePlanner.OR_Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SequencePlanner.Task.Processors
{
    public class InitialSolver
    {
        public TimeSpan Time { get; set; }

        private Stopwatch Timer;
        private GeneralTask Task;

        public InitialSolver(GeneralTask newGeneralTask)
        {
            Task = newGeneralTask;
            Timer = new Stopwatch();
            Time = new TimeSpan();
        }

        public void CreateInitialSolution()
        {
            if (Task.SolverSettings.UseMIPprecedenceSolver)
            {
                Timer.Restart();
                Timer.Stop();
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
                Task.GTSPRepresentation.InitialSolution = PhraseSolution(result);
                Time = Timer.Elapsed;
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