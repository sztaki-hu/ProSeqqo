namespace SequencePlanner.GTSPTask.Serialization.SerializationObject
{
    public class OrderConstraintSerializationObject
    {
        public int BeforeID { get; set; }
        public int AfterID { get; set; }


        public string ToSEQ()
        {
            string separator = ";";
            string newline = "\n";
            string seq = "";
            seq += BeforeID + separator + AfterID + newline;
            return seq;
        }
    }
}