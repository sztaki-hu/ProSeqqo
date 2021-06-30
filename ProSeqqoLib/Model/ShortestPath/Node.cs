using SequencePlanner.Model.Hierarchy;

namespace SequencePlanner.Model.ShortestPath
{
    public class Node
    {
        public int ID { get; set; }
        public Motion Motion { get; set; }

        public Node(int Id, Motion motion)
        {
            ID = Id;
            Motion = motion;
        }
    }
}
