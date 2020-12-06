using SequencePlanner.Model;

namespace SequencePlanner.Function.DistanceFunction
{
    public interface IDistanceFunction
    {
        public string FunctionName { get; }
        public double ComputeDistance(Position A, Position B);
        public void Validate();
    }
}