using SequencePlanner.Model.Hierarchy;

namespace SequencePlanner.Model
{
    public class MotionPrecedence : Precedence<Motion>
    {
        public MotionPrecedence(Motion before, Motion after) : base(before, after)
        {

        }

        public override string ToString()
        {
            return base.ToString()+" SeqID: ["+Before.SequenceMatrixID+"-"+After.SequenceMatrixID+"]";
        }
    }
}
