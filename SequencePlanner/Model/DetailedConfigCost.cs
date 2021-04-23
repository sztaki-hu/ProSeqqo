using SequencePlanner.Model.Hierarchy;

namespace SequencePlanner.Model
{
    public class DetailedConfigCost : DetailedCost<Config>
    {
        public override string ToString()
        {
            return A.ToString() + " - " + B.ToString() + " Cost: " + FinalCost + " (Details: Distance: " + DistanceFunctionCost + " ResourceChangCost: " + ResourceChangeoverCost + " OverrideCost: " + OverrideCost + " Penalty: " + Penalty + ")";
        }

        public DetailedConfigCost Add(DetailedConfigCost config)
        {
            return new DetailedConfigCost()
            {
                A = null,
                B = null,
                FinalCost = FinalCost + config.FinalCost,
                DistanceFunctionCost = DistanceFunctionCost + config.DistanceFunctionCost,
                ResourceChangeoverCost = ResourceChangeoverCost + config.ResourceChangeoverCost,
                OverrideCost = OverrideCost + config.OverrideCost,
                Penalty = Penalty + config.Penalty,
                Bidirectional = false
            };
        }
    }
}