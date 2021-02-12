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
    }
}
