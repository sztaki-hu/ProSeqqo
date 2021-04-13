using System.Collections.Generic;
using SequencePlanner.GTSPTask.Serialization.SerializationObject.Token;

namespace SequencePlanner.GTSPTask.Serialization.SerializationObject
{
    public class ResourceMatrixSerializationObject
    {
        public List<int> IDHeader { get; set; }
        public List<string> NameFooter { get; set; }
        public List<List<double>> ResourceCostMatrix { get; set; }


        public string ToSEQ()
        {
            string open = "[";
            string close = "]";
            string separator = ";";
            string newline = "\n";
            string seq = "";
            if (IDHeader != null)
            {
                seq += open;
                for (int i = 0; i < IDHeader.Count; i++)
                {
                    //seq += open;
                    for (int j = 0; j < ResourceCostMatrix[i].Count; j++)
                    {
                        seq += IDHeader[i];
                        if (j < ResourceCostMatrix[i].Count - 1)
                            seq += separator;
                    }
                    //seq += close;
                    if (i < IDHeader.Count - 1)
                        seq += separator;
                }
                seq += close;
            }
            if (ResourceCostMatrix != null)
            {
                seq += open;
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
        public void FillBySEQTokens(SEQTokenizer tokenizer)
        {
            IDHeader = new List<int>();
            NameFooter = new List<string>();
            ResourceCostMatrix = new List<List<double>>();
            double[,] resourceMatrix;
            var matrix = tokenizer.FindTokenByHeader("ResourceMatrix");
            string[] tmp = matrix.Lines[0].Line.Split(';');     //1;2;3;4--->[1][2][3][4]
            foreach (var item in tmp)
            {
                IDHeader.Add(int.Parse(item));
            }
            int dim = IDHeader.Count;
            resourceMatrix = new double[dim, dim];
            for (int i = 1; i <= dim; i++)
            {
                tmp = matrix.Lines[i].Line.Split(';');  //10;20;30;40--->[10][20][30][40]
                for (int j = 0; j < dim; j++)
                {
                    resourceMatrix[i - 1, j] = double.Parse(tmp[j]);
                }
            }
            if (matrix.Lines.Count > dim + 1)
            {
                tmp = matrix.Lines[dim + 1].Line.Split(';');
                for (int i = 0; i < tmp.Length; i++)
                {
                    NameFooter.Add(tmp[i]);
                }
            }

            for (int i = 0; i < dim; i++)
            {
                ResourceCostMatrix.Add(new List<double>());
                for (int j = 0; j < dim; j++)
                {
                    ResourceCostMatrix[i].Add(resourceMatrix[i, j]);
                }
            }
        }
    }
}
