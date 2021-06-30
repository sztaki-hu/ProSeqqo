using System.Collections.Generic;

namespace SequencePlanner.Task.Serialization.Token
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

        public override string ToString()
        {
            return Header;
        }
    }
}
