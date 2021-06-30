using ProSeqqoLib.Model;
using System;

namespace ProSeqqoLib.Task.Serialization.Model
{
    public class StrictEdgeWeightSerializationObject
    {
        public int A { get; set; }
        public int B { get; set; }
        public double Weight { get; set; }
        public bool Bidirectional { get; set; }
    }
}
