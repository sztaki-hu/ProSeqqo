using SequencePlanner.Model;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Task.General;

namespace SequencePlanner.Helper
{
    public interface IDepotMapper
    {
        public BaseNode ORToolsStartDepot { get; set; }
        public BaseNode ORToolsFinishDepot { get; set; }
        public int ORToolsStartDepotSequenceID { get; }
        public int ORToolsFinishDepotSequenceID { get; }


        public void Change(GeneralTask task);
        public void ChangeBack(GeneralTask task);
        public GeneralTaskResult ResolveSolution(GeneralTaskResult result);
        public void OverrideWeights(GeneralGTSPRepresentation task);
    }
}
