using SequencePlanner.GeneralModels.Result;
using System;

namespace SequencePlanner.GeneralModels
{
    public  class GeneralShortcutMapper: ITaskProcessor
    {
        public GeneralTask Task { get; set; }
        public GeneralShortcutMapper(GeneralTask task)
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