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
                Time = Timer.Elapsed;
            }
        }
    }
}