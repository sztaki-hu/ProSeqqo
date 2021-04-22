using System;
using System.Diagnostics;

namespace SequencePlanner.GeneralModels
{
    public class InitialSolver
    {
        public TimeSpan Time { get; set; }


        private Stopwatch Timer;
        private NewGeneralTask Task;

        public InitialSolver(NewGeneralTask newGeneralTask)
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