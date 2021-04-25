using SequencePlanner.Helper;

namespace SequencePlanner.Model
{
    public class Precedence<Base>
    {
        public Base Before { get; set; }
        public Base After { get; set; }


        public Precedence(Base before, Base after)
        {
            Before = before;
            After = after;
        }

        public void Validate()
        {
            if (Before == null)
                throw new SeqException("GTSPPrecedenceConstraint.Before is null!");
            if (After == null)
                throw new SeqException("GTSPPrecedenceConstraint.After is null!");
        }

        public override string ToString()
        {
            return "Precedence constraint before: " + Before.ToString() + " after: " + After.ToString();
        }
    }
}