using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePlanner.GeneralModels
{
    public class Config: Node
    {
        public List<double> Configuration { get; set; }
        public Resource Resource { get; set; }


        public Config(int id, List<double> configuration) : base(id, id+"-Config")
        {
            Configuration = configuration;
        }

        public Config(int id, List<double> configuration, string name): base(id, name)
        {
            Configuration = configuration;
        }

        public Config(int id, List<double> configuration, string name, Resource resource): this(id, configuration, name)
        {
            Configuration = configuration;
            Resource = resource;
        }

        public Config(int id, List<double> configuration, Resource resource) : this(id, configuration, id + "-Config")
        {
            Configuration = configuration;
            Resource = resource;
        }
    }
}
