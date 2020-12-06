using SequencePlanner.Helper;
using SequencePlanner.Model;
namespace SequencePlanner.Function.DistanceFunction
{
    public class MaxDistanceFunction : IDistanceFunction
    {
        public string FunctionName { get { return "MaxDistance"; } }
        public double ComputeDistance(Position A, Position B)
        {
            if (A == null || B == null)
                throw new SequencerException("MaxDistanceFunction A/B position null!");
            if (A.Dimension != B.Dimension)
                throw new SequencerException("MaxDistanceFunction found dimendion mismatch!", "Check dimension of Positions with " + A.UserID + ", " + B.UserID);
            return 2;
        }

        public void Validate()
        {
            //Nothing to validate
        }
    }
}
