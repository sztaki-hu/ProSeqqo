namespace SequencePlanner.Model.Hierarchy
{
    public class MatirxConfig : Node
    {
        public Resource Resource { get; set; }


        public MatirxConfig(int id) : base(id, id + "-Config")
        {
        }

        public MatirxConfig(int id, string name) : base(id, name)
        {
        }

        public MatirxConfig(int id, Resource resource) : this(id, id + "-Config")
        {
            Resource = resource;
        }

        public MatirxConfig(int id, string name, Resource resource) : this(id, name)
        {
            Resource = resource;
        }
    }
}