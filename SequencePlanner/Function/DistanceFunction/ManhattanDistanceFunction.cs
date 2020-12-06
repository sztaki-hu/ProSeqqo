using SequencePlanner.Helper;
using SequencePlanner.Model;

namespace SequencePlanner.Function.DistanceFunction
{
    public class ManhattanDistanceFunction : IDistanceFunction
    {
        public string FunctionName { get { return "ManhattanDistance"; } }
        public double ComputeDistance(Position A, Position B)
        {
            if (A == null || B == null)
                throw new SequencerException("ManhattanDistanceFunction A/B position null!");
            if (A.Dimension != B.Dimension)
                throw new SequencerException("ManhattanDistanceFunction found dimendion mismatch!", "Check dimension of Positions with " + A.UserID + ", " + B.UserID);
            return 3;
        }

        public void Validate()
        {
            //Nothing to Validate
        }
    }
}
