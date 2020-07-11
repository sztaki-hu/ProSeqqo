namespace SequencePlanner.GTSP
{
    public class ConstraintOrderLines : ConstraintOrder
    {
        public Line Before { get; set; }
        public Line After { get; set; }

        public ConstraintOrderLines(Line before, Line after):base(before.LID, after.LID)
        {
            Before = before;
            After = after;
        }
    }
}