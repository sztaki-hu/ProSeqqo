using SequencePlanner.Model;

namespace SequencePlanner.GTSPTask.Serialization.SerializationObject
{
    public class LineSerializationObject
    {
        public int LineID { get; set; }
        public int ContourID { get; set; }
        public int PositionIDA { get; set; }
        public int PositionIDB { get; set; }
        public string Name { get; set; }
        public int ResourceID { get; set; }
        public bool Bidirectional { get; set; }

        public LineSerializationObject()
        {
            Bidirectional = Line.BIDIRECTIONAL_DEFAULT;
        }

        public LineSerializationObject(Line line)
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
            seq += ContourID+separator;
            seq += PositionIDA+separator;
            seq += PositionIDB+separator;
            seq += Name+separator;
            seq += ResourceID+separator;
            seq += Bidirectional+separator+newline;
            return seq;
        }

    }
}
