using SequencePlanner.Helper;
using SequencePlanner.Model;

namespace SequencePlanner.Function.DistanceFunction
{
    public abstract class DistanceFunction: IDistanceFunction
    {
        public string FunctionName { get; protected set; }
        public StrictEdgeWeightSet StrictUserEdgeWeights {get;set;}
        public StrictEdgeWeightSet StrictSystemEdgeWeights {get;set;}

        public DistanceFunction()
        {
            StrictUserEdgeWeights = new StrictEdgeWeightSet();
            StrictSystemEdgeWeights = new StrictEdgeWeightSet();
        }
        public abstract double ComputeDistance(Position A, Position B);
        public abstract void Validate();
        public StrictEdgeWeight GetStrictEdgeWeight(Position A, Position B)
        {
            var user = StrictUserEdgeWeights.Get(A, B);
            var system = StrictSystemEdgeWeights.Get(A, B);
            if (system != null)
            {
                SeqLogger.Warning("System generated edge weight ovveride user given, between positions with " + A.GlobalID + ", " + B.GlobalID + "global id.", nameof(DistanceFunction));
                return system;
            }
            if (user != null)
            {
                return user;
            }
            return null;
        }
    }
}