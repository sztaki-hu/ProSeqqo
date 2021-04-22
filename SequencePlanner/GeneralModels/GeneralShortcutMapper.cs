using SequencePlanner.GeneralModels.Result;
using SequencePlanner.GTSPTask.Result;
using System;

namespace SequencePlanner.GeneralModels
{
    public  class GeneralShortcutMapper: ITaskSolverProcess
    {
        public NewGeneralTask Task { get; set; }
        public GeneralShortcutMapper(NewGeneralTask task)
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

        public TaskResult ResolveSolution(TaskResult generalTaskReult)
        {
            if (!Task.SolverSettings.UseShortcutInAlternatives)
                return generalTaskReult;
            else
                throw new NotImplementedException();
        }
    }
}