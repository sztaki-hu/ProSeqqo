using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.Helper;
using System.Collections.Generic;

namespace SequencePlanner.GeneralModels
{
    public class CostManager
    {
        public IDistanceFunction DistanceFunction { get; set; }
        public IResourceFunction ResourceFunction { get; set; }
        public List<DetailedConfigCost> OverrideCost { get; set; }
        public double IdlePenalty{ get; set; }
        public bool AddMotionLengthToCost { get; set; }
        public bool AddInMotionChangeoverToCost { get; set; }


        public CostManager()
        {
            DistanceFunction = new EuclidianDistanceFunction();
            ResourceFunction = new NoResourceFunction();
            IdlePenalty = 0;
            OverrideCost = new List<DetailedConfigCost>();
        }

        public double ComputeCost(Config From, Config To)
        {
            var weight = 0.0;
            var cost = GetCostOverrides(From, To);
            if (cost.Count>0)
                weight = cost[0].OverrideCost;
            else
                weight = DistanceFunction.ComputeDistance(From, To);
            weight = ResourceFunction.ComputeResourceCost(From, To, weight);
            return weight;
        }

        public double ComputeCost(Motion From, Motion To)
        {
            if (From.Virtual || From.Virtual)
                return 0.0;

            //if (From.OverrideWeightOut > 0)
            //   return From.OverrideWeightOut;
            //if (To.OverrideWeightIn > 0)
            //    return To.OverrideWeightOut;
            
            double weight;
            List<DetailedConfigCost> strict = new List<DetailedConfigCost>();
            if (From.Configs.Count>0 && To.Configs.Count>0)
                strict = GetCostOverrides(From.LastConfig, To.FirstConfig);
            if (strict is not null && strict.Count>0)
                weight = strict[0].OverrideCost;
            else
                weight = DistanceFunction.ComputeDistance(From.LastConfig, To.FirstConfig);
            weight = ResourceFunction.ComputeResourceCost(From.LastConfig, To.FirstConfig, weight);
            
            //if (From.AdditionalWeightOut > 0)
            //    weight += From.AdditionalWeightOut;
            //if (To.AdditionalWeightIn > 0)
            //    weight += To.AdditionalWeightIn;

            return weight;
        }

        public void ComputeLength(Motion motion)
        {
            var cost = 0.0;
            for (int i = 1; i < motion.Configs.Count; i++)
            {
                cost+=ComputeCost(motion.Configs[i - 1], motion.Configs[i]);
            }
            motion.Length = cost;
        }

        public List<DetailedConfigCost> GetCostOverrides(Config A, Config B)
        {
            List<DetailedConfigCost> costs = new List<DetailedConfigCost>();
            foreach (var cost in OverrideCost)
            {
                if (cost.A.ID == A.ID && cost.B.ID == B.ID)
                    costs.Add(cost);
                if(cost.Bidirectional && cost.A.ID == B.ID && cost.B.ID == A.ID)
                    costs.Add(cost);
            }
            if (costs.Count > 0)
                throw new SeqException("Multiple override cost for configs: "+A.ID+"-"+B.ID);
            return costs;
        }

        public DetailedConfigCost GetDetailedConfigCost(Config from, Config to)
        {
            var configCost = new DetailedConfigCost()
            {
                A = from, 
                B = to
            };
            configCost.DistanceFunctionCost = DistanceFunction.ComputeDistance(from, to);
            configCost.ResourceChangeoverCost = ResourceFunction.ComputeResourceCost(from, to, configCost.DistanceFunctionCost);
            var overrideCost = GetCostOverrides(from, to);
            if (overrideCost.Count > 0)
                configCost.OverrideCost = overrideCost[0].OverrideCost;
            configCost.FinalCost = ComputeCost(from, to);
            return configCost;
        }
    }
}