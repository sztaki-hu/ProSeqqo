using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Task.Base;
using SequencePlanner.Model;

namespace SequencePlanner.Helper
{
    public interface IDepotMapper
    {
        public int ORToolsStartDepotSequenceID { get; }
        public int ORToolsFinishDepotSequenceID { get; }
        public BaseNode ORToolsStartDepot { get; set; }
        public BaseNode ORToolsFinishDepot { get; set; }

        public void Map(BaseTask task);

        public void ReverseMap(BaseTask task);

        public TaskResult ResolveSolution(TaskResult result);

        public void OverrideWeights(IGTSPRepresentation task);
    }

    enum DepotChangeType
    {
        CyclicStartDepot,
        NotCyclicNoDepot,
        NotCyclicOnlyStartDepot,
        NotCyclicOnlyFinishDepot,
        NotCyclicStartFinishDepot
    }
}
