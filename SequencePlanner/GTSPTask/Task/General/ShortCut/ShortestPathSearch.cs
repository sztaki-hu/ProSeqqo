using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Task.General.ShortCut
{
    public class ShortestPathSearch
    {
        private int Level { get; set; } 
        private Dictionary<int, double> Values { get; set; }
        private List<BaseNode> List { get; set; }
        private List<Edge> Edges { get; set; }
        private IDistanceFunction DistanceFunction { get; set; }
        private IResourceFunction ResourceFunction { get; set; }
        private List<Model.Task> Tasks { get; set; }

        public ShortestPathSearch(List<Model.Task> tasks, IDistanceFunction distanceFunction, IResourceFunction resourceFunction)
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

        public List<ShortestPath> CalculateCriticalRoute(GTSPNode openPos, List<GTSPNode> positions)
        {
            foreach (var pos in Tasks[0].Positions)
            {
                Values[pos.Node.SequencingID] = 0;
            }

            foreach (var pos in Tasks[1].Positions)
            {
                Values[pos.Node.SequencingID] = FindEdge(openPos.Node, pos.Node);
            }

            for (int i = 1; i < Tasks.Count-1; i++)
            {
                GTSPNode minPos = null;
                var min = double.MaxValue;
                for (int j = 0; j < Tasks[i].Positions.Count; j++)
                {
                    if (Values[Tasks[i].Positions[j].Node.SequencingID] < min)
                    {
                        min = Values[Tasks[i].Positions[j].Node.SequencingID];
                        minPos = Tasks[i].Positions[j];
                    }
                }
                foreach (var posNext in Tasks[i+1].Positions)
                {
                    var cost = Values[minPos.Node.SequencingID] + FindEdge(minPos.Out, posNext.In);
                    if (cost < Values[posNext.Node.SequencingID])
                        Values[posNext.Node.SequencingID] = Values[minPos.Node.SequencingID] + FindEdge(minPos.Out, posNext.In);
                }
                System.Console.WriteLine(minPos);
            }
            //SeqLogger.Indent++;
            var tmp = new List<ShortestPath>();
            foreach (var pos in positions)
            {
                tmp.Add(RollbackSolution(openPos, pos));
            }
            return tmp;
        }

        private ShortestPath RollbackSolution(GTSPNode from, GTSPNode to)
        {
            var path = new ShortestPath(from, to, Values.GetValueOrDefault(from.Node.SequencingID));
            var list = new List<GTSPNode>();
            GTSPNode akt = to;
            list.Add(akt);
            for (int i = 0; i < Level-2; i++)
            {
                akt = PreviousNode(akt);
                list.Add(akt);
            }
            list.Add(from);
            list.Reverse();
            path.Cut = list;
            path.Cost = Values[to.Node.SequencingID];
            for (int i = 0; i < path.Cut.Count-1; i++)
            {
                //path.Costs.Add(FindEdge(path.Cut[i].Out, path.Cut[i+1].In));
                path.Costs.Add(CalculateWeight(path.Cut[i], path.Cut[i + 1]));
            }
            return path;
        }

        private GTSPNode PreviousNode(GTSPNode B)
        {
            foreach (var edge in Edges)
            {
                if (edge.B.Node.SequencingID == B.Node.SequencingID)
                    return edge.A;
            }
            throw new SeqException("CPM error!");
        }

        private void Build()
        {
            int j = 0;
            foreach (var task in Tasks)
            {
                foreach (var position in task.Positions)
                {
                    position.Node.SequencingID = j++;
                    List.Add(position.Node);
                    Values.Add(position.Node.SequencingID, double.MaxValue);
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

        private double CalculateWeight(GTSPNode A, GTSPNode B)
        {
            if (A.Node.Virtual || B.Node.Virtual)
                return 0.0;
            if (A.OverrideWeightOut > 0)
                return A.OverrideWeightOut;
            if (B.OverrideWeightIn > 0)
                return B.OverrideWeightIn;
            double weight = DistanceFunction.ComputeDistance(A.Out, B.In);
            weight = ResourceFunction.ComputeResourceCost(A.Out, B.In, weight);
            if (A.AdditionalWeightOut > 0)
                weight += A.AdditionalWeightOut;
            if (B.AdditionalWeightIn > 0)
                weight += B.AdditionalWeightIn;
            return weight;
        }

        public double FindEdge(BaseNode A, BaseNode B)
        {
            foreach (var item in Edges)
            {
                if (item.A.Node.GlobalID == A.GlobalID && item.B.Node.GlobalID == B.GlobalID)
                    return item.Weight;
            }
            throw new SeqException("Edge not found in CPM!");
        }
    }

    internal class Edge
    {
        public GTSPNode A { get; set; }
        public GTSPNode B { get; set; }
        public double Weight { get; set; }
    }
}
