using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSPTask.Serialization.SerializationObject
{
    public class StrictEdgeWeightSerializationObject
    {
        public int A { get; set; }
        public int B { get; set; }
        public double Weight { get; set; }
        public bool Bidirectional { get; set; }
    }
}
