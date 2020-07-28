namespace SequencePlanner.GTSP
{
    public class ConstraintOrder
    {        
        public NodeBase Before { get; set; }
        public NodeBase After { get; set; }

        public ConstraintOrder(NodeBase before, NodeBase after)
        {
            Before = before;
            After = after;
        }

        public override string ToString()
        {
            return "Before: " + Before.UID + " After: " + After.UID;
        }
    }
}