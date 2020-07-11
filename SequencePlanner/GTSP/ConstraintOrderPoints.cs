namespace SequencePlanner.GTSP
{
    public class ConstraintOrderPoints : ConstraintOrder
    {
        public Position Before { get; set; }
        public Position After { get; set; }

        public ConstraintOrderPoints(Position before, Position after):base(before.PID, after.PID)
        {
            Before = before;
            After = after;
        }
    }
}