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
                if(user!=null)
                    SeqLogger.Warning("System generated edge weight ovveride user given, between positions with " + A.UserID + ", " + B.UserID + " user id.", nameof(DistanceFunction));
                return system;
            }
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public void ToLog(LogLevel level)
        {
            SeqLogger.WriteLog(level, "DistanceFunction: "+FunctionName, nameof(DistanceFunction));
            SeqLogger.Indent++;
            SeqLogger.WriteLog(level, "FunctionName: "+FunctionName, nameof(DistanceFunction));
            SeqLogger.WriteLog(level, "User defined strict edge weights: ", nameof(DistanceFunction));
            StrictUserEdgeWeights.ToLog(level);
            SeqLogger.WriteLog(level, "System defined strict edge weights: ", nameof(DistanceFunction));
            StrictSystemEdgeWeights.ToLog(level);
            SeqLogger.Indent--;
        }
    }
}