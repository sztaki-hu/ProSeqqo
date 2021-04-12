using SequencePlanner.Helper;
using System.Collections.Generic;

namespace SequencePlanner.Model
{
    public class Contour: BaseNode
    {
        public List<Line> Lines { get; set; }

        public Contour(): base()
        {
            Name = UserID + "_Contour_" + GlobalID;
            Lines = new List<Line>();
        }

        public void Validate()
        {
            if (Lines == null)
                throw new SeqException("Contour with UserID: " + UserID + " Lines should not be null.");
            if(Lines.Count<1)
                throw new SeqException("Contour with UserID: " + UserID + " is empty.");
        }

        //DEFAULT
        public Contour GetCopy()
        {
            var copy = (Contour)base.GetCopy(new Contour());
            copy.Lines = Lines;
            return copy;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Contour contour = (Contour)obj;
                base.Equals((BaseNode)contour);
                if (!contour.Lines.Equals(Lines))
                    return false;
                return true;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() & Lines.GetHashCode();
        }
    }
}
