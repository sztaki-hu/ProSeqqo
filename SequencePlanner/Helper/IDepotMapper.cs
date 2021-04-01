using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Task.Base;
using SequencePlanner.GTSPTask.Task.PointLike;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePlanner.Helper
{
    public interface IDepotMapper
    {
        public int ORToolsStartDepotSequenceID { get; }
        public int ORToolsFinishDepotSequenceID { get; }

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
