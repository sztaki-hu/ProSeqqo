using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using SequencePlanner.Model.Hierarchy;
using System.Collections.Generic;

namespace SequencePlanner.Task
{
    public class CostManager
    {
        public IDistanceFunction DistanceFunction { get; set; }
        public IResourceFunction ResourceFunction { get; set; }
        public List<DetailedConfigCost> OverrideCost { get; set; }
        public double IdlePenalty { get; set; }
        public bool AddMotionLengthToCost { get; set; }
        public bool AddInMotionChangeoverToCost { get; set; }

        public CostManager()
        {
            DistanceFunction = new EuclidianDistanceFunction();
            ResourceFunction = new NoResourceFunction();
            IdlePenalty = 0;
            OverrideCost = new List<DetailedConfigCost>();
        }

        public DetailedConfigCost ComputeCostBetweenMotionConfigs(Config From, Config To)
        {
            var cost = new DetailedConfigCost()
            {
                A = From,
                B = To
            };
            if (From is null || To is null)
                return cost;
            var weight = 0.0;
            var overWeight = GetOverrideCost(From, To);
            if(overWeight is not null)
            {
                cost.OverrideCost = (double)overWeight;
                weight = (double) overWeight;
            }
            else
            {
                if (From.Virtual || To.Virtual)
                    weight = 0.0;
                else
                    weight = DistanceFunction.ComputeDistance(From, To);
                cost.DistanceFunctionCost = weight;
            }
            if (weight > 0)
            {
                weight += GetPenaltyCost(weight);
                cost.Penalty = GetPenaltyCost(weight);
            }
            weight = ResourceFunction.ComputeResourceCost(From, To, weight);
            cost.ResourceChangeoverCost = ResourceFunction.GetResourceCost(From, To);
            cost.FinalCost = weight;
            return cost;
        }

        public DetailedConfigCost ComputeCostInMotion(Config From, Config To)
        {
            var cost = new DetailedConfigCost()
            {
                A = From,
                B = To
            };
            if (AddMotionLengthToCost)
            {
                var weight = 0.0;
                var overWeight = GetOverrideCost(From, To);
                if (overWeight is not null)
                {
                    cost.OverrideCost = (double)overWeight;
                    weight = (double)overWeight;
                }
                else
                {
                    if (From.Virtual || To.Virtual)
                        weight = 0.0;
                    else
                        weight = DistanceFunction.ComputeDistance(From, To);
                    cost.DistanceFunctionCost = weight;
                }
                //In motion No Penalty
                //if (weight > 0)
                //{
                //    weight += GetPenaltyCost(weight);
                //    cost.Penalty = GetPenaltyCost(weight);
                //}
                //Always add resource cost in motion
                //if (AddInMotionChangeoverToCost)
                //{
                    weight = ResourceFunction.ComputeResourceCost(From, To, weight);
                    cost.ResourceChangeoverCost = ResourceFunction.GetResourceCost(From, To);
                //}
                cost.FinalCost = weight;
            }
            return cost;
        }

        public DetailedMotionCost ComputeCost(Motion From, Motion To)
        {
            var betweenMotions = ComputeCostBetweenMotionConfigs(From.LastConfig, To.FirstConfig);
            DetailedMotionCost detailedMotionCost = new DetailedMotionCost()
            {
                A = From,
                B = To,
                FinalCost = betweenMotions.FinalCost,
                DistanceFunctionCost = betweenMotions.DistanceFunctionCost,
                OverrideCost = betweenMotions.OverrideCost,
                Penalty = betweenMotions.Penalty,
                ResourceChangeoverCost = betweenMotions.ResourceChangeoverCost
            };

            if (!To.isShortcut)
            {
                var inMotion = ComputeCost(From);
                detailedMotionCost.FinalCost += inMotion.FinalCost;
                detailedMotionCost.PreviousMotionDetails = inMotion;
                //System.Console.WriteLine(From + "-" + To+": "+ betweenMotions.FinalCost);
                return detailedMotionCost;
            }
            else
            {
                detailedMotionCost.FinalCost += To.ShortcutCost;
            }
            return detailedMotionCost;

            //if (From.Virtual || From.Virtual)
            //    return 0.0;

            //double weight = ComputeCostBetweenMotions(From.LastConfig, To.FirstConfig).FinalCost;
            //weight = ResourceFunction.ComputeResourceCost(From.LastConfig, To.FirstConfig, weight);
            //if (AddMotionLengthToCost)
            //    weight += To.Cost;


            //if (From.OverrideWeightOut > 0)
            //   return From.OverrideWeightOut;
            //if (To.OverrideWeightIn > 0)
            //    return To.OverrideWeightOut;

            //double weight;
            //List<DetailedConfigCost> strict = new List<DetailedConfigCost>();
            //if (From.Configs.Count>0 && To.Configs.Count>0)
            //    strict = GetCostOverrides(From.LastConfig, To.FirstConfig);
            //if (strict is not null && strict.Count>0)
            //    weight = strict[0].OverrideCost;
            //else
            //    weight = DistanceFunction.ComputeDistance(From.LastConfig, To.FirstConfig);
            //weight = ResourceFunction.ComputeResourceCost(From.LastConfig, To.FirstConfig, weight);

            //if (From.AdditionalWeightOut > 0)
            //    weight += From.AdditionalWeightOut;
            //if (To.AdditionalWeightIn > 0)
            //    weight += To.AdditionalWeightIn;
        }

        public DetailedConfigCost ComputeCost(Motion motion)
        {
            motion.DetailedCost = new DetailedConfigCost();
            for (int i = 1; i < motion.Configs.Count; i++)
            {
                motion.DetailedCost = motion.DetailedCost.Add(ComputeCostInMotion(motion.Configs[i - 1], motion.Configs[i]));
            }
            return motion.DetailedCost;
        }

        public double? GetOverrideCost(Config A, Config B)
        {
            List<DetailedConfigCost> costs = new List<DetailedConfigCost>();
            foreach (var cost in OverrideCost)
            {
                if (cost.A.ID == A.ID && cost.B.ID == B.ID)
                    costs.Add(cost);
                if (cost.Bidirectional && cost.A.ID == B.ID && cost.B.ID == A.ID)
                    costs.Add(cost);
            }
            if (costs.Count > 1)
                throw new SeqException("Multiple override cost for configs: " + A.ID + "-" + B.ID);
            if (costs.Count == 0)
                return null;
            else
                return costs[0].OverrideCost;
        }

        public double GetPenaltyCost(double weight)
        {
            if (weight > 0)
                return IdlePenalty;
            else
                return 0;
        }

        public DetailedMotionCost GetDetailedMotionCost(Motion from, Motion to)
        {
            var motionCost = new DetailedMotionCost()
            {
                A = from,
                B = to,
               // FinalCost = ComputeCost(from, to)
            };
            motionCost.DistanceFunctionCost = DistanceFunction.ComputeDistance(from.LastConfig, to.FirstConfig);
            motionCost.ResourceChangeoverCost = ResourceFunction.GetResourceCost(from.LastConfig, to.FirstConfig);
            //motionCost.InMotionCost = from.Cost;
            //motionCost.ResourceChangeoverCostInMotionCost = from.ResourceChangeoverCostInCost;
            //var overrideCost = GetCostOverrides(from, to);
            //if (overrideCost.Count > 0)
            //    motionCost.OverrideCost = overrideCost[0].OverrideCost;
            return motionCost;
        }
    }
}