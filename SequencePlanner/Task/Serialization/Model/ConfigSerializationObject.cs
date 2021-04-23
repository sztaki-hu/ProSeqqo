using SequencePlanner.Model.Hierarchy;
using System.Collections.Generic;

namespace SequencePlanner.Task.Serialization.Model
{
    public class ConfigSerializationObject
    {
        public int ID { get; set; }
        public double[] Config { get; set; }
        public string Name { get; set; }
        public int ResourceID { get; set; }


        public ConfigSerializationObject() { }
        public ConfigSerializationObject(Config config)
        {
            ID = config.ID;
            Config = config.Configuration.ToArray();
            Name = config.Name;
            ResourceID = config.Resource.ID;
        }

        
        public Config ToConfig()
        {
            return new Config(ID, new List<double>(Config), Name, new Resource(ResourceID, Name));
        }

        public string ToSEQ()
        {
            string separator = ";";
            string newline = "\n";
            string open = "[";
            string close = "]";
            string seq = "";
            seq += ID + separator;
            seq += open;
            for (int i = 0; i < Config.Length; i++)
            {
                seq += Config[i].ToString("0.####");
                if (i < Config.Length - 1)
                    seq += separator;
            }
            seq += close + separator;
            seq += Name + separator;
            seq += ResourceID + newline;
            return seq;
        }
    }
}
