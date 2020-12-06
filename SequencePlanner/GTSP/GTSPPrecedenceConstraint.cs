using SequencePlanner.Helper;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSP
{
    public class GTSPPrecedenceConstraint
    {
        public BaseNode Before { get; set; }
        public BaseNode After { get; set; }

        public GTSPPrecedenceConstraint(BaseNode before, BaseNode after)
        {
            Before = before;
            After = after;
        }

        public GTSPPrecedenceConstraint()
        {

        }

        public override string ToString()
        {
            return "Precedence constraint before: " + Before.ToString() + " after: " + After.ToString();
        }

        public void Validate()
        {
            if (Before == null)
                throw new SequencerException("GTSPPrecedenceConstraint.Before is null!");
            if (After == null)
                throw new SequencerException("GTSPPrecedenceConstraint.After is null!");
        }

        public static bool isCyclic(List<GTSPPrecedenceConstraint> precedences)
        {
            var map = new Dictionary<int, int>();
            var i = 0;
            foreach (var item in precedences)
            {
                if (!map.ContainsKey(item.Before.GlobalID))
                    map.Add(item.Before.GlobalID, i++);
                if (!map.ContainsKey(item.After.GlobalID))
                    map.Add(item.After.GlobalID, i++);
            }
            Graph graph = new Graph(i);
            foreach (var precedence in precedences)
            {
                int a;
                map.TryGetValue(precedence.After.GlobalID,out a);
                int b;
                map.TryGetValue(precedence.Before.GlobalID, out b);
                graph.addEdge(a,b);
            }
            return graph.isCyclic();
        }
    }

    public class Graph
    {
        private readonly int V;
        private readonly List<List<int>> adj;

        public Graph(int V)
        {
            this.V = V;
            adj = new List<List<int>>(V);

            for (int i = 0; i < V; i++)
                adj.Add(new List<int>());
        }

        private bool isCyclicUtil(int i, bool[] visited, bool[] recStack)
        {
            if (recStack[i])
                return true;

            if (visited[i])
                return false;

            visited[i] = true;

            recStack[i] = true;
            List<int> children = adj[i];

            foreach (int c in children)
                if (isCyclicUtil(c, visited, recStack))
                    return true;

            recStack[i] = false;
            return false;
        }

        public void addEdge(int sou, int dest)
        {
            adj[sou].Add(dest);
        }

        public bool isCyclic()
        {
            bool[] visited = new bool[V];
            bool[] recStack = new bool[V];

            for (int i = 0; i < V; i++)
                if (isCyclicUtil(i, visited, recStack))
                    return true;

            return false;
        }
    }
}