using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.Helper;
using System;
using System.Collections.Generic;

namespace SequencePlanner.Model
{
    public class PositionMatrix
    {
        private int MAX_SEQUENCING_ID = 0;
        public List<GTSPNode> Positions { get; set; }
        public double[,] Matrix { get; set; }
        public IDistanceFunction DistanceFunction { get; set; }
        public IResourceFunction ResourceFunction { get; set; }
        public bool UseLineLengthInWeight { get; set; }
        public bool UseResourceInLineLength { get; set; }

        public PositionMatrix(List<GTSPNode> positions, IDistanceFunction distanceFunction, IResourceFunction resourceFunction)
        {
            Positions = positions;
            DistanceFunction = distanceFunction;
            ResourceFunction = resourceFunction;
            Validate();
            Init();
        }

        public PositionMatrix()
        {
            Positions = new List<GTSPNode>();
        }

        public void Init()
        {
            foreach (var position in Positions)
            {
                position.Node.SequencingID = MAX_SEQUENCING_ID++;

                if (UseLineLengthInWeight)
                {
                    position.Weight = CalculateWeight(position.In, position.Out);
                    position.AdditionalWeightIn += position.Weight;
                }
            }
            Matrix = new double[MAX_SEQUENCING_ID, MAX_SEQUENCING_ID];
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    Matrix[i, j] = int.MaxValue / 1000;
                }
            }
        }

        public double CalculateWeight(GTSPNode A, GTSPNode B)
        {
            //if (A.Node.Virtual || B.Node.Virtual)
            //    return 0.0;
            if (A.OverrideWeightOut > 0)
            {
                Matrix[A.Node.SequencingID, B.Node.SequencingID] = A.OverrideWeightOut;
                return A.OverrideWeightOut;
            }
            if (B.OverrideWeightIn > 0)
            {
                Matrix[A.Node.SequencingID, B.Node.SequencingID] = B.OverrideWeightOut;
                return B.OverrideWeightOut;
            }
            double weight = 0;
            if (A.Node.Virtual || B.Node.Virtual)
                weight = 0;
            else
            {
                weight = DistanceFunction.ComputeDistance(A.Out, B.In);
                weight = ResourceFunction.ComputeResourceCost(A.Out, B.In, weight);
            }
            if (A.AdditionalWeightOut > 0)
                weight += A.AdditionalWeightOut;
            if (B.AdditionalWeightIn > 0)
                weight += B.AdditionalWeightIn;
            Matrix[A.Node.SequencingID, B.Node.SequencingID] = weight;
            return weight;
        }

        public double CalculateWeight(Position A, Position B)
        {
            double weight = 0;
            if (A.Virtual || B.Virtual)
                weight = 0;
            else
            {
                weight = DistanceFunction.ComputeDistance(A, B);
                if(UseResourceInLineLength)
                    weight = ResourceFunction.ComputeResourceCost(A, B, weight);
            }
            return weight;
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