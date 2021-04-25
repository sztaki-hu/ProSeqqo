using SequencePlanner.Model.Hierarchy;
using System.Collections.Generic;

namespace SequencePlanner.Model
{
    public class MotionPrecedenceList : Precedence<List<Motion>>
    {
        public MotionPrecedenceList(List<Motion> before, List<Motion> after) : base(before, after) { }
        public MotionPrecedenceList() : base(new List<Motion>(), new List<Motion>()) { }
    }
}