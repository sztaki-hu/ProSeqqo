using SequencePlanner.OR_Tools;
using System;
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
                    DisjointConstraints = Task.PCGTSPRepresentation.DisjointSets,
                    //StrictOrderPrecedenceHierarchy = Task.PCGTSPRepresentation.CreateProcessPrecedenceFull(),
                    OrderPrecedenceConstraints = Task.PCGTSPRepresentation.MotionPrecedences,
                    StartDepot = Task.PCGTSPRepresentation.StartDepot.SequenceMatrixID,
                    FinishDepot = Task.PCGTSPRepresentation.FinishDepot.SequenceMatrixID,
                    //Processes = Processes
                };
                var solver = new ORToolsGeneralPreSolverWrapper(task);
                var result = solver.Solve();
                PhraseSolution(result);
                Time = Timer.Elapsed;
            }
        }

        private void PhraseSolution(ORToolsResult result)
        {
            //var ORPreSolver = new ORToolsGeneralPreSolverWrapper(task);

            //var result = ORPreSolver.Solve();
            //MIPRunTime = ORPreSolver.RunTime;
            //if (result.Count > 0)
            //{
            //    long[][] initialSolution = new long[1][];
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