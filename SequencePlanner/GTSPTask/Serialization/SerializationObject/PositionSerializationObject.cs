using SequencePlanner.Model;


namespace SequencePlanner.GTSPTask.Serialization.SerializationObject
{
    public class PositionSerializationObject
    {
        public int ID { get; set; }
        public double[] Position { get; set; }
        public string Name { get; set; }
        public int ResourceID { get; set; }

        public PositionSerializationObject() { }
        public PositionSerializationObject(Position position)
        {
            ID = position.UserID;
            Position = position.Vector;
            Name = position.Name;
            ResourceID = position.ResourceID;
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
            for (int i = 0; i < Position.Length; i++)
            {
                seq += Position[i].ToString("0.####");
                if (i < Position.Length - 1)
                    seq += separator;
            }
            seq += close+separator;
            seq += Name + separator;
            seq += ResourceID + newline;
            return seq;
        }
        public Position ToPosition()
        {
           return new Position() { UserID = ID, Name = Name, Vector = Position, ResourceID = ResourceID };
        }
    }
}
