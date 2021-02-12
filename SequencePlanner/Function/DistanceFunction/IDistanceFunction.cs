using SequencePlanner.Helper;
using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.Function.DistanceFunction
{
    public interface IDistanceFunction
    {
        public string FunctionName { get; }
        public StrictEdgeWeightSet StrictUserEdgeWeights {get;set;}
        public StrictEdgeWeightSet StrictSystemEdgeWeights {get;set;}
        public double ComputeDistance(Position A, Position B);
        public void Validate();
        public void ToLog(LogLevel level);
    }
}