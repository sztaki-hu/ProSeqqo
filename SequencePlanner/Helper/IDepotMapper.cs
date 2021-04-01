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
        public Position ORToolsStartDepot { get; set; }
        public Position ORToolsFinishDepot { get; set; }
        public int ORToolsStartDepotSequenceID { get { if (ORToolsStartDepot is not null) return ORToolsStartDepot.SequencingID; else return -1; } }
        public int ORToolsFinishDepotSequenceID { get { if (ORToolsFinishDepot is not null) return ORToolsFinishDepot.SequencingID; else return -1; } }

        public void Map(BaseTask task);

        public void ReverseMap(BaseTask task);

        public TaskResult ResolveSolution(TaskResult result);
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
