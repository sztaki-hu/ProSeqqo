using System.Collections.Generic;

namespace ProSeqqoLib.Model.Hierarchy
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
        public OverrideCost OverrideCostIn { get; set; }
        public OverrideCost OverrideCostOut { get; set; }
        public double ShortcutCost { get; set; }
        public bool isShortcut { get; set; }


        public Motion() : base()
        {
            DetailedCost = new DetailedConfigCost();
            OverrideCostIn = new OverrideCost();
            OverrideCostOut = new OverrideCost();
            Configs = new List<Config>();
        }

        public Motion(int id, string name) : base(id, name)
        {
            DetailedCost = new DetailedConfigCost();
            Configs = new List<Config>();
        }

        public Motion(int id, Config a, Config b) : base(id, id + "-Motion")
        {
            DetailedCost = new DetailedConfigCost();
            Configs = new List<Config>();
            Configs.Add(a);
            Configs.Add(b);
        }

        public Motion(int id, Config config) : base(id, id + "-Motion")
        {
            DetailedCost = new DetailedConfigCost();
            Configs = new List<Config>();
            Configs.Add(config);
        }

        public Motion(int id, List<Config> motion) : base(id, id + "-Motion")
        {
            DetailedCost = new DetailedConfigCost();
            Configs = motion;
        }

        public Motion GetReverse()
        {
            var configs = new List<Config>();
            foreach (var config in Configs)
            {
                configs.Add(config);
            }
            configs.Reverse();

            return new Motion()
            {
                ID = -ID,
                Name = Name + "_Reverse",
                Configs = configs,
                Bidirectional = false,
            };
        }

        public override string ToString()
        {
            string temp = ID.ToString();
            if (Name is not null && !Name.Equals(""))
                temp += "-" + Name;
            //if (FirstConfig is not null && !FirstConfig.Name.Equals(""))
            //    temp += "-" + FirstConfig.Name;
            return temp;
        }
    }
}