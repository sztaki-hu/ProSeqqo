using SequencePlanner.Helper;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSPTask.Serialization.SerializationObject
{
    public class StrictEdgeWeightSetSerializationObject
    {
        public List<StrictEdgeWeightSerializationObject> Weights {get;set;}

        public StrictEdgeWeightSetSerializationObject()
        {
            Weights = new List<StrictEdgeWeightSerializationObject>();
        }
        public StrictEdgeWeightSetSerializationObject(StrictEdgeWeightSet set)
        {
            Weights = new List<StrictEdgeWeightSerializationObject>();
            foreach (var item in set.GetAll())
            {
                Weights.Add(new StrictEdgeWeightSerializationObject()
                {
                    A = item.A.SequencingID,
                    B = item.B.UserID,
                    Weight = item.Weight,
                    Bidirectional = item.Bidirectional
                });
            }
        }

        public StrictEdgeWeightSet ToStrictEdgeWeightSet(List<Position> positions)
        {
            var tmp = new StrictEdgeWeightSet();
            foreach (var item in Weights)
            {
                Position a = null;
                Position b = null;
                foreach (var pos in positions)
                {
                    if (pos.UserID == item.A)
                        a = pos;
                    if (pos.UserID == item.B)
                        b = pos;
                }
                if (a == null)
                    SeqLogger.Error("StrictEdgeWeight not find position with user id " + item.A, nameof(StrictEdgeWeightSetSerializationObject));

                if (b == null)
                    SeqLogger.Error("StrictEdgeWeight not find position with user id " + item.B, nameof(StrictEdgeWeightSetSerializationObject));
                tmp.Add(new StrictEdgeWeight(a, b, item.Weight, item.Bidirectional));
            }
            return tmp;
        }

        public string ToSEQ()
        {
            string seq = "";
            foreach (var item in Weights)
            {
                seq += item.A + ";" + item.B + ";" + item.Weight + ";" + item.Bidirectional + "\n";
            }
            return seq;
        }
    }
}
