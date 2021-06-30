using ProSeqqoLib.Helper;
using ProSeqqoLib.Model.Hierarchy;
using System.Collections.Generic;

namespace ProSeqqoLib.Model
{
    public class MotionPrecedenceList : Precedence<List<Motion>>
    {
        public MotionPrecedenceList(List<Motion> before, List<Motion> after) : base(before, after) { }
        public MotionPrecedenceList() : base(new List<Motion>(), new List<Motion>()) { }

        public override string ToString()
        {
            return "Precedence constraint before: " + Before.ToIDListString() + " after: " + After.ToIDListString();
        }
    }
}