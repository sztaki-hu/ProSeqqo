namespace SequencePlanner.Task.Processors
{
    public interface ITaskProcessor
    {
        public void Change();
        public void ChangeBack();
        public GeneralTaskResult ResolveSolution(GeneralTaskResult generalTaskReult);
    }
}
