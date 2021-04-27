using System;

namespace SequencePlanner.Task.Processors
{
    public class ShortcutMapper : ITaskProcessor
    {
        public GeneralTask Task { get; set; }
        public ShortcutMapper(GeneralTask task)
        {
            Task = task;
        }

        public void Change()
        {
            if (!Task.SolverSettings.UseShortcutInAlternatives)
                return;
            else
                throw new NotImplementedException();
        }

        public void ChangeBack()
        {
            if (!Task.SolverSettings.UseShortcutInAlternatives)
                return;
            else
                throw new NotImplementedException();
        }

        public GeneralTaskResult ResolveSolution(GeneralTaskResult generalTaskReult)
        {
            if (!Task.SolverSettings.UseShortcutInAlternatives)
                return generalTaskReult;
            else
                throw new NotImplementedException();
        }
    }
}