using SequencePlanner.GTSPTask.Serialization.SerializationObject.Token;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace SequencePlanner.GTSPTask.Serialization.SerializationObject
{
    public class DistanceMatrixSerializationObject
    {
        public List<int> IDHeader { get; set; }
        public List<List<double>> DistanceMatrix { get; set; }
        public List<string> NameFooter { get; set; }
        public List<int> ResourceFooter { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public List<PositionSerializationObject> PositionList { get; set; }

        public DistanceMatrixSerializationObject()
        {
            IDHeader = new List<int>();
            NameFooter = new List<string>();
            ResourceFooter = new List<int>();
        }

        public string ToSEQ()
        {
            string separator = ";";
            string newline = "\n";
            string seq = "";
            if (IDHeader != null)
            {
                for (int i = 0; i < IDHeader.Count; i++)
                {
                    seq += IDHeader[i];
                    if (i < DistanceMatrix[i].Count - 1)
                        seq += separator;
                }
                seq += newline;
            }
            if (DistanceMatrix != null)
            {
                //seq += open;
                for (int i = 0; i < DistanceMatrix.Count; i++)
                {
                    //seq += open;
                    for (int j = 0; j < DistanceMatrix[i].Count; j++)
                    {
                        seq += DistanceMatrix[i][j].ToString("0.####");
                        if (j < DistanceMatrix[i].Count - 1)
                            seq += separator;
                    }
                    //seq += close;
                    if (i < DistanceMatrix.Count - 1)
                        //seq += separator;
                    seq += newline;
                }
                seq += newline;
            }
            if (NameFooter != null)
            {
                for (int i = 0; i < IDHeader.Count; i++)
                {
                    seq += NameFooter[i];
                    if (i < DistanceMatrix[i].Count - 1)
                        seq += separator;
                }
                seq += newline;
            }
            if (ResourceFooter != null)
            {
                for (int i = 0; i < IDHeader.Count; i++)
                {
                    seq += ResourceFooter[i];
                    if (i < DistanceMatrix[i].Count - 1)
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
            ResourceFooter = new List<int>();
            DistanceMatrix = new List<List<double>>();
            double[,] distanceMatrix;
            var matrix = tokenizer.FindTokenByHeader("PositionMatrix");
            string[] tmp = matrix.Lines[0].Line.Split(';');     //1;2;3;4--->[1][2][3][4]
            foreach (var item in tmp)
            {
                IDHeader.Add(int.Parse(item));
            }
            int dim = IDHeader.Count;
            distanceMatrix = new double[dim, dim];
            for (int i = 1; i <= dim; i++)
            {
                tmp = matrix.Lines[i].Line.Split(';');  //10;20;30;40;Name--->[10][20][30][40][Name]
                for (int j = 0; j < dim; j++)
                {
                    distanceMatrix[i - 1,j] = double.Parse(tmp[j]);
                }
            }
            if (matrix.Lines.Count > dim + 1)
            {
                tmp = matrix.Lines[dim + 1].Line.Split(';');//-1
                for (int i = 0; i < tmp.Length; i++)
                {
                    NameFooter.Add(tmp[i]);
                }
            }
            if (matrix.Lines.Count > dim + 2)
            {
                tmp = matrix.Lines[dim+2].Line.Split(';');
                for (int i = 0; i < tmp.Length; i++)
                {
                    ResourceFooter.Add(Int32.Parse(tmp[i]));
                }
            }
            for (int i = 0; i < dim; i++)
            {
                DistanceMatrix.Add(new List<double>());
                for (int j = 0; j < dim; j++)
                {
                    DistanceMatrix[i].Add(distanceMatrix[i, j]);
                }
            }
            CreatePositionList();
        }

        private void CreatePositionList()
        {
            PositionList = new List<PositionSerializationObject>();
            for (int i = 0; i < IDHeader.Count; i++)
            {
                var position = new PositionSerializationObject
                {
                    ID = IDHeader[i]
                };
                if (NameFooter!=null && NameFooter.Count!=0)
                    position.Name = NameFooter[i];
                if (ResourceFooter != null && ResourceFooter.Count != 0)
                    position.ResourceID = ResourceFooter[i];
                position.Position = new double[0];
                PositionList.Add(position);
            }
        }
    }
}