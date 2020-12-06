﻿using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;
using SequencePlanner.GTSPTask.Serialization.SerializationObject.Token;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Serialization.SerializationObject
{
    public class ResourceFunctionSerializationObject
    {
        public string ResourceDistanceFunction { get; set; }
        public string ResourceSource { get; set; }
        public double ResourceCostConstant { get; set; }
        public List<int> ResourceCostMatrixIDHeader { get; set; }
        public List<List<double>> ResourceCostMatrix { get; set; }
        public ResourceMatrixSerializationObject ResourceCostMatrix2 { get; set; }

        public ResourceFunctionSerializationObject() { }
        public ResourceFunctionSerializationObject(PositionMatrix positionMatrix)
        {
            ResourceDistanceFunction = positionMatrix.ResourceFunction.LinkingFunction.FunctionName;
            ResourceSource = positionMatrix.ResourceFunction.FunctionName;

            if (ResourceSource == "ConstantResource")
            {
                ResourceCostConstant = ((ConstantResourceFunction)positionMatrix.ResourceFunction).Cost;
            }
            if (ResourceSource == "MatrixResource")
            {
                ResourceCostMatrix = ((MatrixResourceFunction)positionMatrix.ResourceFunction).CostMatrix;
                ResourceCostMatrixIDHeader = ((MatrixResourceFunction)positionMatrix.ResourceFunction).CostMatrixIDHeader;
            }
        }

        public string ToSEQ()
        {
            string open = "[";
            string close = "]";
            string separator = ";";
            string newline = "\n";
            string seq = "";
            seq += "ResourceDistanceFunction: " + ResourceDistanceFunction + newline;
            seq += "ResourceSource: " + ResourceSource + newline;
            if(ResourceSource == "ConstantResource")
                seq += "ResourceCostConstant: " + ResourceCostConstant + newline;
            if(ResourceSource == "MatrixResource")
            {
                seq += "ResourceCostConstant: " + ResourceCostConstant + newline;
                seq += "ResourceCostMatrix: " + newline;
                for (int i = 0; i < ResourceCostMatrix.Count; i++)
                {
                    seq += open;
                    for (int j = 0; j < ResourceCostMatrix[i].Count; j++)
                    {
                        seq += ResourceCostMatrix[i][j].ToString("0.####");
                        if (j < ResourceCostMatrix[i].Count - 1)
                            seq += separator;
                    }
                    seq += close;
                    if (i < ResourceCostMatrix.Count - 1)
                        seq += separator;
                }
                seq += newline;
            }
            return seq;
        }
        public IResourceFunction ToResourceFunction()
        {
            IResourceDistanceLinkFunction resourceDistanceLinkFunction = ResourceDistanceFunction switch
            {
                "Add" => new AddResourceDistanceLinkFunction(),
                "Max" => new MaxResourceDistanceLinkFunction(),
                _ => throw new SequencerException("ResourceDistanceLinkFunction unkown!"),
            };
            IResourceFunction resourceFunction = ResourceSource switch
            {
                "ConstantResource" => new ConstantResourceFunction(ResourceCostConstant, resourceDistanceLinkFunction),
                "MatrixResource" => new MatrixResourceFunction(ResourceCostMatrix, ResourceCostMatrixIDHeader, resourceDistanceLinkFunction),
                "PositionBasedResource" => throw new SequencerException("PositionBasedResource not implemented yet!"),
                _ => throw new SequencerException("ResourceFunction unknown!"),
            };
            return resourceFunction;
        }
        public void FillBySEQTokens(SEQTokenizer tokenizer)
        {

            ResourceDistanceFunction = TokenConverter.GetStringByHeader("ResourceDistanceFunction", tokenizer);
            ResourceSource = TokenConverter.GetStringByHeader("ResourceSource", tokenizer);
            switch (ResourceSource)
            {
                case "ConstantResource":
                    ResourceCostConstant = TokenConverter.GetDoubleByHeader("ResourceCostConstant", tokenizer);
                    break;
                case "MatrixResource":
                    ResourceCostMatrix2 = new ResourceMatrixSerializationObject();
                    ResourceCostMatrix2.FillBySEQTokens(tokenizer);
                    break;
                case "PositionBasedResource":
                    throw new SequencerException("PositionBasedResource not implemented yet!");
                default:
                    throw new SequencerException("ResourceFunction unknown!");
            }
        }
    }
}