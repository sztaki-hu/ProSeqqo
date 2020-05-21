using SequencePlanner.GTSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner
{
    public class SequencerTask
    {
        public TaskType TaskType { get; set; }
        public EdgeWeightSource EdgeWeightSource { get; set; }
        public DistanceFunction DistanceFunction{get;set;}
        public int Dimension { get; set; }
        public int TimeLimit { get; set; }
        public bool CyclicSequence { get; set; }
        public int StartDepotID { get; set; }
        public int FinishDepotID { get; set; }
        public bool WeightMultiplyerAuto { get; set; }
        public int WeightMulitplyerAuto { get; set; }
        public GraphRepresentation Graph { get; set; }



    }

    public enum TaskType
    {
        LineLike,
        PointLike
    }
    public enum EdgeWeightSource
    {
        FullMatrix,
        CalculateFromPositions
    }
    public enum DistanceFunction
    {
        Euclidian_Distance,
        Max_Distance,
        Trapezoid_Time,
        Manhattan_Distance
    }
}
