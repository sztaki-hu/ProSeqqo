using System.Collections.Generic;

namespace SequencePlanner.GTSP
{
    public class ConstraintDisjoint
    {
        public List<NodeBase> NodeList;

        public ConstraintDisjoint()
        {
            NodeList = new List<NodeBase>();
        }

        public void addConstraint(NodeBase node)
        {
            NodeList.Add(node);
        }

    }
}