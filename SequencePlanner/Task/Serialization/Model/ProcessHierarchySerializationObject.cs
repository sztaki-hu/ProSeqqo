using System.Collections.Generic;

namespace SequencePlanner.Task.Serialization.Model
{
    public class ProcessHierarchySerializationObject
    {
        public int ProcessID { get; set; }
        public int AlternativeID { get; set; }
        public int TaskID { get; set; }
        public int MotionID { get; set; }
        public List<int> ConfigIDs { get; set; }
        public bool? Bidirectional { get; set; }
        public string Name { get; set; }

        public ProcessHierarchySerializationObject()
        {
            ConfigIDs = new List<int>();
        }

        public override string ToString()
        {
            return " ProcessID: " + ProcessID + " AlternativeID: " + AlternativeID + " TaskID: " + TaskID + "MotionID: " + MotionID;
        }

        public string ToSEQ()
        {
            string separator = ";";
            string newline = "\n";
            string seq = "";
            seq += ProcessID + separator + newline;
            seq += AlternativeID + separator;
            seq += TaskID + separator;
            seq += MotionID + separator;
            seq += "[";
            foreach (var configID in ConfigIDs)
            {
                seq += configID + separator;
            }
            seq += "]" + separator;
            seq += Bidirectional + separator;
            seq += Name + separator;

            return seq;
        }
    }
}
