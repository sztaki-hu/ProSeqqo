using ProSeqqoLib.Helper;

namespace ProSeqqoLib.Model
{
    public class DetailedCost<Base>
    {
        public Base A { get; set; }
        public Base B { get; set; }
        public double FinalCost { get; set; }
        public double ResourceChangeoverCost { get; set; }
        public double DistanceFunctionCost { get; set; }
        public double OverrideCost { get; set; }
        public double Penalty { get; set; }
        public bool Bidirectional { get; set; }

        public override string ToString()
        {
            return A.ToString() + "-" + B.ToString() + " Cost: " + FinalCost + " (Details: Distance: " + DistanceFunctionCost + " ResourceChangCost: " + ResourceChangeoverCost + " OverrideCost: " + OverrideCost + " Penalty: " + Penalty;
        }

        public void ToLog(LogLevel logLevel)
        {
            SeqLogger.WriteLog(logLevel, "");
        }
    }
}