using SequencePlanner.GTSPTask.Result;
using SequencePlanner.Model;
using SequencePlanner.OR_Tools;

namespace SequencePlanner.GTSPTask.Task.Base
{
    public interface IBaseTask
    {
        public int Dimension { get; set; }
        public bool CyclicSequence { get; set; }
        public int WeightMultipier { get; set; }
        public Position StartDepot { get; set; }
        public Position FinishDepot { get; set; }
        public PositionMatrix PositionMatrix { get; set; }
        public int TimeLimit { get; set; }
        public LocalSearchStrategyEnum.Metaheuristics LocalSearchStrategy { get; set; }

        public delegate void TaskCompleted(int ID, ITaskResult result);
        public event TaskCompleted SequencingTaskCompleted;

        //public void GenerateModel();
        public void ValidateModel();
        //public ITaskResult RunModel();
        //public Task<ITaskResult> RunModelAsync(int taskID, CancellationToken cancellationToken);
    }
}