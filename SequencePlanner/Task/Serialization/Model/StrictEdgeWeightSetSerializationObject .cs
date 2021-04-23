using System.Collections.Generic;
using SequencePlanner.GeneralModels;
using SequencePlanner.Helper;

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
