﻿using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.GTSPTask.Serialization.SerializationObject.Token;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Serialization.SerializationObject
{
    public class DistanceFunctionSerializationObject
    {
        public string Function { get; set; }
        public DistanceMatrixSerializationObject DistanceMatrix { get; set; }
        public double[] TrapezoidAcceleration { get; set; }
        public double[] TrapezoidSpeed { get; set; }
        public StrictEdgeWeightSetSerializationObject StrictUserEdgeWeights { get; set; }


        public DistanceFunctionSerializationObject()
        {
        }

        public DistanceFunctionSerializationObject(PositionMatrix positionMatrix)
        {
            Function = positionMatrix.DistanceFunction.FunctionName;
            StrictUserEdgeWeights = new StrictEdgeWeightSetSerializationObject(positionMatrix.DistanceFunction.StrictUserEdgeWeights);

            if (Function == "MatrixDistance")
            {
                DistanceMatrix = new DistanceMatrixSerializationObject
                {
                    DistanceMatrix = ((MatrixDistanceFunction)positionMatrix.DistanceFunction).CostMatrix
                };
                foreach (var position in positionMatrix.Positions)
                {
                    if (!position.Node.Virtual)
                    {
                        DistanceMatrix.IDHeader.Add(position.Node.UserID);
                        DistanceMatrix.NameFooter.Add(position.Node.Name);
                        DistanceMatrix.ResourceFooter.Add(position.Node.ResourceID);
                    }
                        
                }
            }

            if (Function == "TrapezoidTime")
            {
                TrapezoidAcceleration = ((TrapezoidTimeDistanceFunction)positionMatrix.DistanceFunction).MaxAcceleration;
                TrapezoidSpeed = ((TrapezoidTimeDistanceFunction)positionMatrix.DistanceFunction).MaxSpeed;
            }

            if (Function == "TrapezoidTimeWithTimeBreaker")
            {
                TrapezoidAcceleration = ((TrapezoidTimeDistanceFunction)positionMatrix.DistanceFunction).MaxAcceleration;
                TrapezoidSpeed = ((TrapezoidTimeDistanceFunction)positionMatrix.DistanceFunction).MaxSpeed;
            }


        }

        public string ToSEQShort()
        {
            string open = "[";
            string close = "]";
            string separator = ";";
            string newline = "\n";
            string seq = "";
            seq += "DistanceFunction: " + Function + newline;
            if (TrapezoidAcceleration != null)
            {
                seq += "TrapezoidAcceleration: ";
                seq += open;
                for (int i = 0; i < TrapezoidAcceleration.Length; i++)
                {
                    seq += TrapezoidAcceleration[i];
                    if (i < TrapezoidAcceleration.Length - 1)
                        seq += separator;
                }
                seq += close + newline;
            }

            if (TrapezoidSpeed != null)
            {
                seq += "TrapezoidSpeed: ";
                seq += open;
                for (int i = 0; i < TrapezoidSpeed.Length; i++)
                {
                    seq += TrapezoidSpeed[i];
                    if (i < TrapezoidSpeed.Length - 1)
                        seq += separator;
                }
                seq += close + newline;
            }
            return seq;
        }

        public string ToSEQLong()
        {
            string newline = "\n";
            string seq = "";
            if (DistanceMatrix != null)
            {
                seq += "PositionMatrix: " + newline;
                seq += DistanceMatrix.ToSEQ();
            }
            if (StrictUserEdgeWeights != null && StrictUserEdgeWeights.Weights.Count>0)
            {
                seq += "StrictEdgeWeights: " + newline;
                seq += StrictUserEdgeWeights.ToSEQ();
                
            }
            return seq;
        }

        public IDistanceFunction ToDistanceFunction(List<GTSPNode> positions)
        {
            IDistanceFunction newDistanceFunction = Function switch
            {
                "EuclidianDistance" => new EuclidianDistanceFunction(),
                "ManhattanDistance" => new ManhattanDistanceFunction(),
                "MaxDistance" => new MaxDistanceFunction(),
                "TrapezoidTimeDistance" => new TrapezoidTimeDistanceFunction(TrapezoidAcceleration, TrapezoidSpeed),
                "TrapezoidTimeDistanceWithTimeBreaker" => new TrapezoidTimeWithTimeBreakerDistanceFunction(TrapezoidAcceleration, TrapezoidSpeed),
                "MatrixDistance" => new MatrixDistanceFunction(DistanceMatrix.DistanceMatrix, DistanceMatrix.IDHeader),
                _ => throw new SeqException("DistanceFunction is unknown!"),
            };
            newDistanceFunction.StrictUserEdgeWeights = StrictUserEdgeWeights.ToStrictEdgeWeightSet(positions);
            return newDistanceFunction;
        }
        public void FillBySEQTokens(SEQTokenizer tokenizer)
        {
            Function = TokenConverter.GetStringByHeader("DistanceFunction", tokenizer);
            StrictUserEdgeWeights = TokenConverter.GetStrictEdgeWeightSet("StrictEdgeWeights", tokenizer);
            if (Function != null)
            {
                if (Function == "MatrixDistance")
                {
                    DistanceMatrix = new DistanceMatrixSerializationObject();
                    DistanceMatrix.FillBySEQTokens(tokenizer);
                }
                if (Function == "TrapezoidTimeDistance" || Function == "TrapezoidTimeDistanceWithTimeBreaker")
                {
                    TrapezoidAcceleration = TokenConverter.GetDoubleVectorByHeader("TrapezoidAcceleration", tokenizer);
                    TrapezoidSpeed = TokenConverter.GetDoubleVectorByHeader("TrapezoidSpeed", tokenizer);
                }
            }
        }   
    }
}