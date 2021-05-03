using SequencePlanner.Helper;
using System.Collections.Generic;

namespace SequencePlanner.Model.Hierarchy
{
    public class Config : Node
    {
        public List<double> Configuration { get; set; }
        public Resource Resource { get; set; }


        public Config(int id, List<double> configuration) : base(id, id + "-Config")
        {
            Configuration = configuration;
            Resource = new Resource() { ID = -1 };
        }

        public Config(int id, List<double> configuration, string name) : base(id, name)
        {
            Configuration = configuration;
            Resource = new Resource() { ID = -1};
        }

        public Config(int id, List<double> configuration, string name, Resource resource) : this(id, configuration, name)
        {
            Configuration = configuration;
            Resource = resource;
        }

        public Config(int id, List<double> configuration, Resource resource) : this(id, configuration, id + "-Config")
        {
            Configuration = configuration;
            Resource = resource;
        }

        public override string ToString()
        {
            string temp = ID.ToString();
            if (Name is not null && !Name.Equals(""))
                temp += "-" + Name;
            if (Configuration is not null && Configuration.Count > 0)
                temp += " [" + Configuration.ToListString() + "] ";
            if (Virtual)
                temp += " !Virtual";
            return temp;
        }
    }
}
