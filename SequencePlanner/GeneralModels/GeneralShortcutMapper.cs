using SequencePlanner.GeneralModels.Result;
using SequencePlanner.GTSPTask.Result;

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
            throw new System.NotImplementedException();
        }

        public void ChangeBack()
        {
            throw new System.NotImplementedException();
        }

        public TaskResult ResolveSolution(TaskResult generalTaskReult)
        {
            return generalTaskReult;
        }
    }
}