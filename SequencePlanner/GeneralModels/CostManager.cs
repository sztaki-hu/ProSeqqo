using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Function.ResourceFunction;
using System;
using System.Collections.Generic;

namespace SequencePlanner.GeneralModels
{
    public class CostManager
    {
        public IDistanceFunction DistanceFunction { get; set; }
        public IResourceFunction ResourceFunction { get; set; }
        public List<ConfigCost> OverrideCost { get; set; }
        public double IdlePenalty{ get; set; }

        public CostManager()
        {
            DistanceFunction = new EuclidianDistanceFunction();
            ResourceFunction = new NoResourceFunction();
            IdlePenalty = 0;
            OverrideCost = new List<ConfigCost>();
        }

        public double ComputeCost(Config From, Config To)
        {
            throw new NotImplementedException();
        }

        public double ComputeCost(Motion From, Motion To)
        {
            throw new NotImplementedException();
        }

        public void ComputeLength(Motion motion)
        {

        }
    }
}