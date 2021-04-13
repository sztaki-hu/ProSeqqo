namespace SequencePlanner.GTSPTask.Serialization.SerializationObject
{
    public class ProcessHierarchySerializationObject
    {
        public int PositionID { get; set; }
        public int TaskID { get; set; }
        public int AlternativeID { get; set; }
        public int ProcessID { get; set; }


        public override string ToString()
        {
            return "PositionID: " + PositionID + " TaskID: " + TaskID + " AlternativeID: " + AlternativeID + " ProcessID: " + ProcessID;
        }
        public string ToSEQ()
        {
            string separator = ";";
            string newline = "\n";
            string seq = "";
            seq += PositionID + separator;
            seq += TaskID + separator;
            seq += AlternativeID + separator;
            seq += ProcessID + separator+ newline;
            return seq;
        }
    }
}
