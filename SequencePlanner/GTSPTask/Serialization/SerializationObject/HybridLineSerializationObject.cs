using SequencePlanner.Model;

namespace SequencePlanner.GTSPTask.Serialization.SerializationObject
{
    public class HybridLineSerializationObject
    {
        public int LineID { get; set; }
        public int PositionIDA { get; set; }
        public int PositionIDB { get; set; }
        public string Name { get; set; }
        public int ResourceID { get; set; }
        public bool Bidirectional { get; set; }


        public HybridLineSerializationObject()
        {
            Bidirectional = Line.BIDIRECTIONAL_DEFAULT;
        }

        public HybridLineSerializationObject(Line line)
        {
            LineID = line.UserID;
            //ContourID = line.
            PositionIDA = line.NodeA.UserID;
            PositionIDB = line.NodeB.UserID;
            Name = line.Name;
            ResourceID = line.ResourceID;
            Bidirectional = line.Bidirectional;
        }


        public string ToSEQ()
        {
            string separator = ";";
            string newline = "\n";
            string seq = "";
            seq += LineID+separator;
            seq += PositionIDA+separator;
            seq += PositionIDB+separator;
            seq += Name+separator;
            seq += ResourceID+separator;
            seq += Bidirectional+separator+newline;
            return seq;
        }
    }
}
