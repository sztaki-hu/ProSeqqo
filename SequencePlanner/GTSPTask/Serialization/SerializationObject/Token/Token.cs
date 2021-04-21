using SequencePlanner.Helper;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Serialization.SerializationObject.Token
{
    public class Token
    {
        public string Header { get; set; }
        public List<TokenLineDeserializationObject> Lines { get; set; }
        public bool Phrased { get; set; }


        public Token()
        {
            Lines = new List<TokenLineDeserializationObject>();
        }
    }
}
