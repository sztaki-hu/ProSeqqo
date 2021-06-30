using ProSeqqoLib.Helper;
using ProSeqqoLib.Model.Hierarchy;

namespace ProSeqqoLib.Model
{
    public class DetailedMotionCost : DetailedCost<Motion>
    {
        public double PreviousMotionCost { get { if (PreviousMotionDetails is not null) return PreviousMotionDetails.FinalCost; else return 0; } }
        public double InMotion { get; set; }
        public DetailedConfigCost PreviousMotionDetails { get; set; }

        public DetailedMotionCost()
        {
            PreviousMotionDetails = new DetailedConfigCost();
        }


        public string ToStringShort()
        {
            return A?.ToString() + " - " + B?.ToString() + " Cost: " + FinalCost.ToString("N2");
        }

        public override string ToString()
        {
            return A?.ToString() + " - " + B?.ToString() + " Cost: " + FinalCost.ToString("N2") + " (Details: Distance: " + DistanceFunctionCost.ToString("N2") + " ResourceChangCost: " + ResourceChangeoverCost.ToString("N2") + " OverrideCost: " + OverrideCost.ToString("N2") + " Penalty: " + Penalty.ToString("N2") + " PreviousMotionCost: " + PreviousMotionCost.ToString("N2") + " InMotion: " + InMotion.ToString("N2") + ")";
        }
    }
}