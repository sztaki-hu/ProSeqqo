using System.Collections.Generic;

namespace SequencePlanner.GeneralModels
{
    public class Motion: Node
    {
        public int SequenceMatrixID { get; set; }
        public List<Config> Configs { get; set; }
        public bool Bidirectional { get; set; }
        public double Length { get; set; }
        public double StricLength { get; set; }


        public Motion():base()
        {
            Configs = new List<Config>();
        }

        public Motion(int id, string name): base(id, name) {
            Configs = new List<Config>();
        }

        public Motion(int id, Config a, Config b): base(id, id+"-Motion") {
        
        }
        public Motion(int id, Config config) : base(id, id+"-Motion")
        {

        }

        public Motion(int id, List<Config> motion): base(id, id+"-Motion")
        {

        }
    }
}