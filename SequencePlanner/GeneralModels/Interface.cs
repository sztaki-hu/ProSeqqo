using SequencePlanner.GeneralModels.Result;
using SequencePlanner.GTSPTask.Result;

namespace SequencePlanner.GeneralModels
{
    public interface ITaskSolverProcess
    {
        public void Change();
        public void ChangeBack();
        public TaskResult ResolveSolution(TaskResult generalTaskReult);
    }
}
