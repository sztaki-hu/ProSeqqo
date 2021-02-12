using System.Collections.Generic;

namespace SequencePlanner.Model
{
    public class Process : BaseNode
    {
        public List<Alternative> Alternatives { get; set; }
        public Process(): base()
        {
            Name = UserID + "_Process_" + GlobalID;
            Alternatives = new List<Alternative>();
        }
    }
}
