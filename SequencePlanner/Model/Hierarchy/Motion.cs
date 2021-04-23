using System.Collections.Generic;

namespace SequencePlanner.Model.Hierarchy
{
    public class Motion : Node
    {
        public int SequenceMatrixID { get; set; }
        public List<Config> Configs { get; set; }
        public bool Bidirectional { get; set; }
        public double Cost { get { return DetailedCost.FinalCost; } }
        public DetailedConfigCost DetailedCost { get; set; }
        public double ResourceChangeoverCostInCost { get; set; }
        public double StricLength { get; set; }
        public Config FirstConfig { get { if (Configs.Count > 0) return Configs[0]; else return null; } }
        public Config LastConfig { get { if (Configs.Count > 0) return Configs[^1]; else return null; } }


        public Motion() : base()
        {
            DetailedCost = new DetailedConfigCost();
            Configs = new List<Config>();
        }

        public Motion(int id, string name) : base(id, name)
        {
            DetailedCost = new DetailedConfigCost();
            Configs = new List<Config>();
        }

        public Motion(int id, Config a, Config b) : base(id, id + "-Motion")
        {

        }

        public Motion(int id, Config config) : base(id, id + "-Motion")
        {

        }

        public Motion(int id, List<Config> motion) : base(id, id + "-Motion")
        {

        }

        public override string ToString()
        {
            string temp = ID.ToString();
            if (Name is not null && !Name.Equals(""))
                temp += "-" + Name;
            return temp;
        }
    }
}