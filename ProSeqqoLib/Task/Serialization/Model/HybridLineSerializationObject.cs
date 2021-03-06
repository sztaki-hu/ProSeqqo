using ProSeqqoLib.Model.Hierarchy;

namespace ProSeqqoLib.Task.Serialization.Model
{
    public class HybridLineSerializationObject
    {
        public int LineID { get; set; }
        public int ConfigIDA { get; set; }
        public int ConfigIDB { get; set; }
        public string Name { get; set; }
        public int ResourceID { get; set; }
        public bool Bidirectional { get; set; }


        public HybridLineSerializationObject()
        {
            Bidirectional = Line.BIDIRECTIONAL_DEFAULT;
        }

        public HybridLineSerializationObject(Line line)
        {
            //LineID = line.UserID;
            //ContourID = line.
            ConfigIDA = line.NodeA.UserID;
            ConfigIDB = line.NodeB.UserID;
            //Name = line.Name;
            //ResourceID = line.ResourceID;
            Bidirectional = line.Bidirectional;
        }


        public string ToSEQ()
        {
            string separator = ";";
            string newline = "\n";
            string seq = "";
            seq += LineID + separator;
            seq += ConfigIDA + separator;
            seq += ConfigIDB + separator;
            seq += Name + separator;
            seq += ResourceID + separator;
            seq += Bidirectional + separator + newline;
            return seq;
        }
    }
}
