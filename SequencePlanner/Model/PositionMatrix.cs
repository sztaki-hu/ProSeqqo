using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.Helper;
using System.Collections.Generic;

namespace SequencePlanner.Model
{
    public class PositionMatrix
    {
        public List<Position> Positions { get; set; }
        public double[,] Matrix { get; set; }
        public IDistanceFunction DistanceFunction { get; set; }
        public IResourceFunction ResourceFunction { get; set; }

        public PositionMatrix(List<Position> positions, IDistanceFunction distanceFunction, IResourceFunction resourceFunction)
        {
            Positions = positions;
            DistanceFunction = distanceFunction;
            ResourceFunction = resourceFunction;
            Validate();
            UpdateMatrixFromPositions();
        }

        public PositionMatrix()
        {
            Positions = new List<Position>();
        }

        public void UpdateMatrixFromPositions()
        { 
            Matrix = new double[Positions.Count, Positions.Count];
            for (int i = 0; i < Positions.Count; i++)
            {
                for (int j = 0; j < Positions.Count; j++)
                {
                    Matrix[i, j] = DistanceFunction.ComputeDistance(Positions[i], Positions[j]);
                    Matrix[i, j] = ResourceFunction.ComputeResourceCost(Positions[i], Positions[j], Matrix[i, j]);
                }
            }
        }

        public void Validate()
        {
            if (DistanceFunction == null)
                throw new SequencerException("PositionMatrix.DistanceFunction not given.");
            else
            {
                DistanceFunction.Validate();
            }

            if (ResourceFunction == null)
                throw new SequencerException("PositionMatrix.ResourceFunction not given.");
            else
            {
                ResourceFunction.Validate();
            }

            if (Positions == null)
                throw new SequencerException("PositionMatrix.Positions not given.");
        }


        //private void CheckMatrix(double[,] matrix)
        //{
        //    if (matrix.GetLength(0) != matrix.GetLength(1))
        //        throw new SequencerException("Matrix dimension mismatch, it should be n x n.", "Check input parameters of PositionMatrix!");
        //}

    }
}