using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;
using SequencePlanner.Helper;
using SequencePlanner.Task.Serialization.Token;

namespace SequencePlanner.Task.Serialization.Model
{
    public class ResourceFunctionSerializationObject
    {
        public string ResourceDistanceFunction { get; set; }
        public string ResourceSource { get; set; }
        public double ResourceCostConstant { get; set; }
        public ResourceMatrixSerializationObject ResourceCostMatrix { get; set; }


        public ResourceFunctionSerializationObject() { }
        public ResourceFunctionSerializationObject(IResourceFunction resFunc)
        {
            if (resFunc.LinkingFunction != null)
                ResourceDistanceFunction = resFunc.LinkingFunction.FunctionName;
            ResourceSource = resFunc.FunctionName;

            if (ResourceSource == "Off")
            {

            }
            if (ResourceSource == "Constant")
            {
                ResourceCostConstant = ((ConstantResourceFunction)resFunc).Cost;
            }
            if (ResourceSource == "Matrix")
            {
                ResourceCostMatrix = new ResourceMatrixSerializationObject
                {
                    ResourceCostMatrix = ((MatrixResourceFunction)resFunc).CostMatrix,
                    IDHeader = ((MatrixResourceFunction)resFunc).CostMatrixIDHeader
                };
            }
        }


        public string ToSEQ()
        {
            string open = "[";
            string close = "]";
            string separator = ";";
            string newline = "\n";
            string seq = "";
            seq += "ResourceChangeover: " + ResourceDistanceFunction + newline;
            if (ResourceSource != "Off")
                seq += "ResourceSource: " + ResourceSource + newline;
            if (ResourceSource == "Constant")
                seq += "ResourceCostConstant: " + ResourceCostConstant + newline;
            if (ResourceSource == "Matrix")
            {
                //TODO: Create ResourceCostMatrix2.ToSEQ;
                seq += "ResourceCostMatrix: " + newline;
                for (int i = 0; i < ResourceCostMatrix.ResourceCostMatrix.Count; i++)
                {
                    seq += open;
                    for (int j = 0; j < ResourceCostMatrix.ResourceCostMatrix[i].Count; j++)
                    {
                        seq += ResourceCostMatrix.ResourceCostMatrix[i][j].ToString("0.####");
                        if (j < ResourceCostMatrix.ResourceCostMatrix[i].Count - 1)
                            seq += separator;
                    }
                    seq += close;
                    if (i < ResourceCostMatrix.ResourceCostMatrix.Count - 1)
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
                "Matrix" => new MatrixResourceFunction(ResourceCostMatrix.ResourceCostMatrix, ResourceCostMatrix.IDHeader, resourceDistanceLinkFunction),
                _ => throw new SeqException("ResourceFunction unknown!"),
            };
            return resourceFunction;
        }
        public void FillBySEQTokens(SEQTokenizer tokenizer)
        {

            ResourceDistanceFunction = tokenizer.GetStringByHeader("ResourceChangeoverFunction");
            ResourceSource = tokenizer.GetStringByHeader("ResourceChangeover");
            switch (ResourceSource)
            {
                case "Off":
                    break;
                case "Constant":
                    ResourceCostConstant = tokenizer.GetDoubleByHeader("ChangeoverConstant");
                    break;
                case "Matrix":
                    ResourceCostMatrix = new ResourceMatrixSerializationObject();
                    ResourceCostMatrix.FillBySEQTokens(tokenizer);
                    break;
                default:
                    throw new SeqException("ResourceFunction unknown!");
            }
        }
    }
}