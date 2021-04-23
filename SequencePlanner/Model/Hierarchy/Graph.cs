using System.Collections.Generic;

namespace SequencePlanner.Model.Hierarchy
{
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

        private bool IsCyclicUtil(int i, bool[] visited, bool[] recStack)
        {
            if (recStack[i])
                return true;

            if (visited[i])
                return false;

            visited[i] = true;

            recStack[i] = true;
            List<int> children = adj[i];

            foreach (int c in children)
                if (IsCyclicUtil(c, visited, recStack))
                    return true;

            recStack[i] = false;
            return false;
        }

        public void AddEdge(int sou, int dest)
        {
            adj[sou].Add(dest);
        }

        public bool IsCyclic()
        {
            bool[] visited = new bool[V];
            bool[] recStack = new bool[V];

            for (int i = 0; i < V; i++)
                if (IsCyclicUtil(i, visited, recStack))
                    return true;

            return false;
        }
    }
}