using SequencePlanner.Model.Hierarchy;

namespace SequencePlanner.Task.Serialization.Model
{
    public class LineSerializationObject
    {
        public int LineID { get; set; }
        public int ContourID { get; set; }
        public int ConfigIDA { get; set; }
        public int ConfigIDB { get; set; }
        public string Name { get; set; }
        public int ResourceID { get; set; }
        public bool Bidirectional { get; set; }


        public LineSerializationObject()
        {
            Bidirectional = Line.BIDIRECTIONAL_DEFAULT;
        }

        public LineSerializationObject(Line line)
        {
            //LineID = line.;
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
            seq += ContourID + separator;
            seq += ConfigIDA + separator;
            seq += ConfigIDB + separator;
            seq += Name + separator;
            seq += ResourceID + separator;
            seq += Bidirectional + separator + newline;
            return seq;
        }

    }
}
