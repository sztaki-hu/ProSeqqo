using SequencePlanner.Model.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePlanner.Model
{
    public class MotionPrecedence: Precedence<Motion>
    {
        public MotionPrecedence(Motion before, Motion after): base(before, after){

        }
    }
}
