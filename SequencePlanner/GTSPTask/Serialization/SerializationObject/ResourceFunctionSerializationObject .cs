using SequencePlanner.Function.ResourceFunction;
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
        public ResourceMatrixSerializationObject ResourceCostMatrix2 { get; set; }

        public ResourceFunctionSerializationObject() { }
        public ResourceFunctionSerializationObject(PositionMatrix positionMatrix)
        {
            if(positionMatrix.ResourceFunction.LinkingFunction!=null)
                ResourceDistanceFunction = positionMatrix.ResourceFunction.LinkingFunction.FunctionName;
            ResourceSource = positionMatrix.ResourceFunction.FunctionName;

            if (ResourceSource == "Off")
            {
                
            }
            if (ResourceSource == "Constant")
            {
                ResourceCostConstant = ((ConstantResourceFunction)positionMatrix.ResourceFunction).Cost;
            }
            if (ResourceSource == "Matrix")
            {
                ResourceCostMatrix2 = new ResourceMatrixSerializationObject();
                ResourceCostMatrix2.ResourceCostMatrix = ((MatrixResourceFunction)positionMatrix.ResourceFunction).CostMatrix;
                ResourceCostMatrix2.IDHeader = ((MatrixResourceFunction)positionMatrix.ResourceFunction).CostMatrixIDHeader;
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
            if(ResourceSource != "Off")
                seq += "ResourceSource: " + ResourceSource + newline;
            if(ResourceSource == "Constant")
                seq += "ResourceCostConstant: " + ResourceCostConstant + newline;
            if(ResourceSource == "Matrix")
            {
                //TODO: Create ResourceCostMatrix2.ToSEQ;
                seq += "ResourceCostMatrix: " + newline;
                for (int i = 0; i < ResourceCostMatrix2.ResourceCostMatrix.Count; i++)
                {
                    seq += open;
                    for (int j = 0; j < ResourceCostMatrix2.ResourceCostMatrix[i].Count; j++)
                    {
                        seq += ResourceCostMatrix2.ResourceCostMatrix[i][j].ToString("0.####");
                        if (j < ResourceCostMatrix2.ResourceCostMatrix[i].Count - 1)
                            seq += separator;
                    }
                    seq += close;
                    if (i < ResourceCostMatrix2.ResourceCostMatrix.Count - 1)
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
                _ => null,
            };
            IResourceFunction resourceFunction = ResourceSource switch
            {
                "Off" => new NoResourceFunction(),
                "Constant" => new ConstantResourceFunction(ResourceCostConstant, resourceDistanceLinkFunction),
                "Matrix" => new MatrixResourceFunction(ResourceCostMatrix2.ResourceCostMatrix, ResourceCostMatrix2.IDHeader, resourceDistanceLinkFunction),
                "PositionBasedResource" => throw new SeqException("PositionBasedResource not implemented yet!"),
                _ => throw new SeqException("ResourceFunction unknown!"),
            };
            return resourceFunction;
        }
        public void FillBySEQTokens(SEQTokenizer tokenizer)
        {

            ResourceDistanceFunction = tokenizer.GetStringByHeader("ResourceDistanceFunction");
            ResourceSource = tokenizer.GetStringByHeader("ResourceSource");
            switch (ResourceSource)
            {
                case "Off":
                    break;
                case "Constant":
                    ResourceCostConstant = tokenizer.GetDoubleByHeader("ResourceCostConstant");
                    break;
                case "Matrix":
                    ResourceCostMatrix2 = new ResourceMatrixSerializationObject();
                    ResourceCostMatrix2.FillBySEQTokens(tokenizer);
                    break;
                case "PositionBasedResource":
                    throw new SeqException("PositionBasedResource not implemented yet!");
                default:
                    throw new SeqException("ResourceFunction unknown!");
            }
        }
    }
}