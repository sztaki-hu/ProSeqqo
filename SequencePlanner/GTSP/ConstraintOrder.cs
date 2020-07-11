namespace SequencePlanner.GTSP
{
    public class ConstraintOrder
    {        
        public int BeforeID { get; set; }
        public int AfterID { get; set; }

        public ConstraintOrder(int before, int after)
        {
            BeforeID = before;
            AfterID = after;
        }
    }
}