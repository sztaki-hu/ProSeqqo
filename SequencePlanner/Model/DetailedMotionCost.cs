using SequencePlanner.Helper;
using SequencePlanner.Model.Hierarchy;

namespace SequencePlanner.Model
{
    public class DetailedMotionCost : DetailedCost<Motion>
    {
        public double PreviousMotionCost { get { return PreviousMotionDetails.FinalCost; } }
        public DetailedConfigCost PreviousMotionDetails { get; set; }

        public DetailedMotionCost()
        {
            PreviousMotionDetails = new DetailedConfigCost();
        }


        public override string ToString()
        {
            return (A.ToString() + " - " + B.ToString() + " Cost: " + FinalCost).FitFor(50) + " (Details: Distance: " + DistanceFunctionCost + " ResourceChangCost: " + ResourceChangeoverCost + " OverrideCost: " + OverrideCost + " Penalty: " + Penalty + " PreviousMotionCost: " + PreviousMotionCost + ")";
        }
    }
}