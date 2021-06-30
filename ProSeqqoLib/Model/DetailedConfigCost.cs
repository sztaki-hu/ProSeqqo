using ProSeqqoLib.Model.Hierarchy;

namespace ProSeqqoLib.Model
{
    public class DetailedConfigCost : DetailedCost<Config>
    {
        
        public string ToStringShort()
        {
            return A?.ToString() + " - " + B?.ToString() + " Cost: " + FinalCost.ToString("0.##");
        }

        public override string ToString()
        {
            return A?.ToString() + " - " + B?.ToString() + " Cost: " + FinalCost.ToString("0.##") + " (Details: Distance: " + DistanceFunctionCost.ToString("0.##") + " ResourceChangCost: " + ResourceChangeoverCost.ToString("0.##") + " OverrideCost: " + OverrideCost.ToString("0.##") + " Penalty: " + Penalty.ToString("0.##") + ")";
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