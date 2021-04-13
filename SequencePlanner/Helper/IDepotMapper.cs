using SequencePlanner.Model;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Task.Base;

namespace SequencePlanner.Helper
{
    public interface IDepotMapper
    {
        public BaseNode ORToolsStartDepot { get; set; }
        public BaseNode ORToolsFinishDepot { get; set; }
        public int ORToolsStartDepotSequenceID { get; }
        public int ORToolsFinishDepotSequenceID { get; }


        public void Map(BaseTask task);
        public void ReverseMap(BaseTask task);
        public TaskResult ResolveSolution(TaskResult result);
        public void OverrideWeights(IGTSPRepresentation task);
    }
}
