using SequencePlanner.GeneralModels.Result;

namespace SequencePlanner.GeneralModels
{
    public interface ITaskProcessor
    {
        public void Change();
        public void ChangeBack();
        public GeneralTaskResult ResolveSolution(GeneralTaskResult generalTaskReult);
    }
}
