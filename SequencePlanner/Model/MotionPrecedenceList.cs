using System.Collections.Generic;

namespace SequencePlanner.GeneralModels
{
    public class MotionPrecedenceList: Precedence<List<Motion>>
    {
        public MotionPrecedenceList(List<Motion> before, List<Motion> after) : base(before, after) { }
    }
}