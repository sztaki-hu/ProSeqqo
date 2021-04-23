using SequencePlanner.Model.Hierarchy;
using System.Collections.Generic;

namespace SequencePlanner.Model
{
    public class ProcessPrecedence : Precedence<Process>
    {
        public ProcessPrecedence(Process before, Process after) : base(before, after)
        {

        }

        public static bool IsCyclic(List<ProcessPrecedence> precedences)
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
                map.TryGetValue(precedence.After.GlobalID, out int a);
                map.TryGetValue(precedence.Before.GlobalID, out int b);
                graph.AddEdge(a, b);
            }
            return graph.IsCyclic();
        }
    }
}
