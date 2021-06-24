namespace SequencePlanner.Model.Hierarchy
{
    public class Node
    {
        private static int NextMaxGlobalID = 0;
        public int ID { get; set; }
        public int GlobalID { get; private set; }
        public string Name { get; set; }
        public bool Virtual { get; set; }

        protected Node()
        {
            GlobalID = NextMaxGlobalID++;
            Name = "Node";
            Virtual = false;
        }

        public Node(int id, string name) : this()
        {
            ID = id;
            Name = name;
            Virtual = false;
        }
    }
}
