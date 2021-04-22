namespace SequencePlanner.GeneralModels
{
    public class DetailedMotionCost: DetailedCost<Motion>
    {
        public double MotionCost { get; set; }
        public double ResourceChangeoverCostInMotionCost { get; set; }

        public override string ToString()
        {
            return A.ToString() + " - " + B.ToString() + " Cost: " + FinalCost + " (Details: Distance: " + DistanceFunctionCost + " ResourceChangCost: " + ResourceChangeoverCost + " OverrideCost: " + OverrideCost + " MotionCost: " + MotionCost + " ResourceChangCostInMotion: "+ ResourceChangeoverCostInMotionCost;
        }
    }
}