using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Helper
{
    public class CPM
    {
        private int Level { get; set; }
        private Dictionary<int, double> Values { get; set; }
        private List<BaseNode> List { get; set; }
        private List<Edge> Edges { get; set; }

        public CPM(List<Model.Task> tasks, IDistanceFunction distanceFunction, IResourceFunction resourceFunction)
        {
            Level = tasks.Count;
        }

        public List<CriticalPath> CalculateCriticelRoute(Position openPos, List<Position> positions)
        {
            throw new NotImplementedException();
        }

        public CriticalPath RollbackSolution(Position from, Position to)
        {
            var path = new CriticalPath(from, to, Values.GetValueOrDefault(from.SequencingID));
            var list = new List<BaseNode>();
            BaseNode akt = to;
            list.Add(akt);
            for (int i = 0; i < Level; i++)
            {
                akt = PreviousNode(akt);
                list.Add(akt);
            }
            list.Reverse();
            path.Cut = list;
            return path;
        }

        public BaseNode PreviousNode(BaseNode B)
        {
            foreach (var edge in Edges)
            {
                if (edge.B.SequencingID == B.SequencingID)
                    return edge.A;
            }
            throw new SequencerException("CPM error!");
        }

    }

    public class Edge{
        public BaseNode A { get; set; }
        public BaseNode B { get; set; }
    }
}
