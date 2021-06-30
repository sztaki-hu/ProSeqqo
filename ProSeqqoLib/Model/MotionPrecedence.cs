using ProSeqqoLib.Model.Hierarchy;

namespace ProSeqqoLib.Model
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
