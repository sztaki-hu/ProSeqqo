using System.Collections.Generic;
using SequencePlanner.GeneralModels;
using SequencePlanner.Helper;
using SequencePlanner.Model;

namespace SequencePlanner.GTSPTask.Serialization.SerializationObject
{
    public class StrictEdgeWeightSetSerializationObject
    {
        public List<StrictEdgeWeightSerializationObject> Weights {get;set;}


        public StrictEdgeWeightSetSerializationObject()
        {
            Weights = new List<StrictEdgeWeightSerializationObject>();
        }
        public StrictEdgeWeightSetSerializationObject(List<DetailedConfigCost> costs)
        {
            Weights = new List<StrictEdgeWeightSerializationObject>();
            foreach (var item in costs)
            {
                Weights.Add(new StrictEdgeWeightSerializationObject()
                {
                    A = item.A.ID,
                    B = item.B.ID,
                    Weight = item.OverrideCost,
                    Bidirectional = item.Bidirectional
                });
            }
        }


        public StrictEdgeWeightSet ToStrictEdgeWeightSet(List<GTSPNode> positions)
        {
            var tmp = new StrictEdgeWeightSet();
            foreach (var item in Weights)
            {
                Position a = null;
                Position b = null;
                foreach (var pos in positions)
                {
                    if (pos.Node.UserID == item.A)
                        a = pos.Out;
                    if (pos.Node.UserID == item.B)
                        b = pos.Out;
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
