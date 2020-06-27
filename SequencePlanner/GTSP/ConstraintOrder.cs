namespace SequencePlanner.GTSP
{
    public class ConstraintOrder
    {
        public Position Before { get; set; }
        public Position After { get; set; }

        public ConstraintOrder(Position before, Position after)
        {
            Before = before;
            After = after;
        }
    }
}