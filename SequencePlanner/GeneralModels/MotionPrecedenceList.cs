using System.Collections.Generic;

namespace SequencePlanner.GeneralModels
{
    public class MotionPrecedenceList
    {
        public List<Motion> Before { get; set; }
        public List<Motion> After { get; set; }

        public MotionPrecedenceList(List<Motion> before, List<Motion> after){
            Before = before;
            After = after;
        }
    }
}