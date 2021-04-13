using SequencePlanner.Helper;
using System.Collections.Generic;

namespace SequencePlanner.Model
{
    public class GTSPPrecedenceConstraint
    {
        public BaseNode Before { get; set; }
        public BaseNode After { get; set; }


        public GTSPPrecedenceConstraint() { }
        public GTSPPrecedenceConstraint(BaseNode before, BaseNode after)
        {
            Before = before;
            After = after;
        }


        public void Validate()
        {
            if (Before == null)
                throw new SeqException("GTSPPrecedenceConstraint.Before is null!");
            if (After == null)
                throw new SeqException("GTSPPrecedenceConstraint.After is null!");
        }
        public static bool IsCyclic(List<GTSPPrecedenceConstraint> precedences)
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
                graph.AddEdge(a,b);
            }
            return graph.IsCyclic();
        }

        public override string ToString()
        {
            return "Precedence constraint before: " + Before.ToString() + " after: " + After.ToString();
        }
    }
}