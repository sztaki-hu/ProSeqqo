using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.Helper;
using System;
using System.Collections.Generic;

namespace SequencePlanner.Model
{
    public class PositionMatrix
    {
        public List<GTSPNode> Positions { get; set; }
        public double[,] Matrix { get; set; }
        public IDistanceFunction DistanceFunction { get; set; }
        public IResourceFunction ResourceFunction { get; set; }

        public PositionMatrix(List<GTSPNode> positions, IDistanceFunction distanceFunction, IResourceFunction resourceFunction)
        {
            Positions = positions;
            DistanceFunction = distanceFunction;
            ResourceFunction = resourceFunction;
            Validate();
            UpdateMatrixFromPositions();
        }

        public PositionMatrix()
        {
            Positions = new List<GTSPNode>();
        }

        public void UpdateMatrixFromPositions()
        { 
            Matrix = new double[Positions.Count, Positions.Count];
            for (int i = 0; i < Positions.Count; i++)
            {
                for (int j = 0; j < Positions.Count; j++)
                {
                    Matrix[i, j] = DistanceFunction.ComputeDistance(Positions[i].Out, Positions[j].In);
                    Matrix[i, j] = ResourceFunction.ComputeResourceCost(Positions[i].Out, Positions[j].In, Matrix[i, j]);
                }
            }
        }

        public void Validate()
        {
            if (DistanceFunction == null)
                throw new SeqException("PositionMatrix.DistanceFunction not given.");
            
            if (ResourceFunction == null)
                throw new SeqException("PositionMatrix.ResourceFunction not given.");
            
            if (Positions == null)
                throw new SeqException("PositionMatrix.Positions not given.");
        }

        public void ToLog(LogLevel level)
        {
            DistanceFunction.ToLog(level);
            ResourceFunction.ToLog(level);
            SeqLogger.WriteLog(level, "Positions:", nameof(PositionMatrix));
            SeqLogger.Indent++;
            foreach (var position in Positions)
            {
                SeqLogger.WriteLog(level, position.ToString(), nameof(PositionMatrix));
            }
            SeqLogger.Indent--;
            if(Matrix is not null)
            {
                SeqLogger.WriteLog(level, "Matrix[SeqID,SeqID] = ["+Matrix.GetLength(0)+";"+Matrix.GetLength(1)+"]", nameof(PositionMatrix));
                SeqLogger.Indent++;
                for (int i = 0; i < Matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < Matrix.GetLength(1); j++)
                    {
                        SeqLogger.Trace("Matrix[" + i + ";" + j + "]=" + Matrix[i, j], nameof(PositionMatrix));
                    }
                }
                SeqLogger.Indent--;
            }
            foreach (var position in Positions)
            {
                SeqLogger.WriteLog(level, position.ToString(), nameof(PositionMatrix));
            }
        }

        //private void CheckMatrix(double[,] matrix)
        //{
        //    if (matrix.GetLength(0) != matrix.GetLength(1))
        //        throw new SequencerException("Matrix dimension mismatch, it should be n x n.", "Check input parameters of PositionMatrix!");
        //}
    }
}