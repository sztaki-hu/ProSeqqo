using System.Collections.Generic;

namespace SequencePlanner.Model
{
    public class Task: BaseNode
    {
        public List<GTSPNode> Positions { get; set; }

        public Task():base()
        {
            Name = UserID + "_Task_" + GlobalID;
            Positions = new List<GTSPNode>();
        }
    }
}
