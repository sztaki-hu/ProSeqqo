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
        private IDistanceFunction DistanceFunction { get; set; }
        private IResourceFunction ResourceFunction { get; set; }
        private List<Model.Task> Tasks { get; set; }


        public CPM(List<Model.Task> tasks, IDistanceFunction distanceFunction, IResourceFunction resourceFunction)
        {
            Level = tasks.Count;
            List = new List<BaseNode>();
            Edges = new List<Edge>();
            Values = new Dictionary<int, double>();
            DistanceFunction = distanceFunction;
            ResourceFunction = resourceFunction;
            Tasks = tasks;
            Build();
        }

        public List<CriticalPath> CalculateCriticalRoute(Position openPos, List<Position> positions)
        {
            foreach (var pos in Tasks[0].Positions)
            {
                Values[pos.SequencingID] = 0;
            }

            foreach (var pos in Tasks[1].Positions)
            {
                Values[pos.SequencingID] = FindEdge(openPos, pos);
            }

            for (int i = 1; i < Tasks.Count-1; i++)
            {
                Position minPos = null;
                var min = double.MaxValue;
                for (int j = 0; j < Tasks[i].Positions.Count; j++)
                {
                    if (Values[Tasks[i].Positions[j].SequencingID] < min)
                    {
                        min = Values[Tasks[i].Positions[j].SequencingID];
                        minPos = Tasks[i].Positions[j];
                    }
                }
                foreach (var posNext in Tasks[i+1].Positions)
                {
                    var cost = Values[minPos.SequencingID] + FindEdge(minPos, posNext);
                    if (cost < Values[posNext.SequencingID])
                        Values[posNext.SequencingID] = Values[minPos.SequencingID] + FindEdge(minPos, posNext);
                }
            }
            //SeqLogger.Indent++;
            var tmp = new List<CriticalPath>();
            foreach (var pos in positions)
            {
                tmp.Add(RollbackSolution(openPos, pos));
                //SeqLogger.Trace("Shortcut between "+openPos+" and "+ pos+" wit cost: "+tmp[tmp.Count-1].Cost, nameof(CPM));
            }
            //SeqLogger.Indent--;
            return tmp;
        }

        public CriticalPath RollbackSolution(Position from, Position to)
        {
            var path = new CriticalPath(from, to, Values.GetValueOrDefault(from.SequencingID));
            var list = new List<BaseNode>();
            BaseNode akt = to;
            list.Add(akt);
            for (int i = 0; i < Level-1; i++)
            {
                akt = PreviousNode(akt);
                list.Add(akt);
            }
            list.Reverse();
            path.Cut = list;
            path.Cost = Values[to.SequencingID];

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

        private void Build()
        {
            int j = 0;
            foreach (var task in Tasks)
            {
                foreach (var position in task.Positions)
                {
                    position.SequencingID = j++;
                    List.Add(position);
                    Values.Add(position.SequencingID, double.MaxValue);
                }
            }
            for (int i = 0; i < Tasks.Count-1; i++)
            {
                foreach (var posPrev in Tasks[i].Positions)
                {
                    foreach (var posNext in Tasks[i + 1].Positions)
                    {
                        Edges.Add(new Edge() { A = posPrev, B = posNext, Weight = CalculateWeight(posPrev, posNext) });
                    }
                }
            }
        }

        private double CalculateWeight(Position A, Position B)
        {
            if (A.Virtual || B.Virtual)
                return 0.0;
            double weight = DistanceFunction.ComputeDistance(A, B);
            weight = ResourceFunction.ComputeResourceCost(A, B, weight);
            return weight;
        }

        public double FindEdge(Position A, Position B)
        {
            foreach (var item in Edges)
            {
                if (item.A.GlobalID == A.GlobalID && item.B.GlobalID == B.GlobalID)
                    return item.Weight;
            }
            throw new SequencerException("Edge not found in CPM!");
        }
    }

    internal class Edge
    {
        public BaseNode A { get; set; }
        public BaseNode B { get; set; }
        public double Weight { get; set; }
    }
}
