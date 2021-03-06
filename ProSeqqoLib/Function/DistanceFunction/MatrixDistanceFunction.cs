using ProSeqqoLib.Helper;
using ProSeqqoLib.Model.Hierarchy;
using System.Collections.Generic;

namespace ProSeqqoLib.Function.DistanceFunction
{
    public class MatrixDistanceFunction : DistanceFunction
    {
        public override string FunctionName { get { return "Matrix"; } }
        public List<int> IDHeader { get; set; }
        public List<List<double>> CostMatrix { get; }


        public MatrixDistanceFunction(List<List<double>> costMatrix, List<int> IDheader) : base()
        {
            CostMatrix = costMatrix;
            IDHeader = IDheader;
            Validate();
        }


        public override double ComputeDistance(Config A, Config B)
        {
            if (A == null || B == null)
                throw new SeqException("MatrixDistanceFunction A/B configuration is null!");
            if (A.Configuration.Count != B.Configuration.Count)
                throw new SeqException("MatrixDistanceFunction found dimendion mismatch!", "Check dimension of configurations with " + A.ID + ", " + B.ID+ "IDs");

            var aid = -1;
            var bid = -1;
            for (int i = 0; i < IDHeader.Count; i++)
            {
                if (IDHeader[i] == A.ID)
                    aid = i;
                if (IDHeader[i] == B.ID)
                    bid = i;
            }

            if (aid == -1)
                throw new SeqException("Matrix distance function can not find user configuration ID: " + A.ID + " - " + A.Name);
            if (bid == -1)
                throw new SeqException("Matrix distance function can not find user configuration ID: " + B.ID + " - " + B.Name);
            return CostMatrix[aid][bid];
        }

        public override void Validate()
        {
            if (CostMatrix == null || IDHeader == null)
                throw new SeqException("MatrixDistanceFunction contains null property, ResourceIDList/CostMatrix.");
            if (CostMatrix.Count != CostMatrix[0].Count)
                throw new SeqException("MatrixDistanceFunction.CostMatrix size should be n x n.");
            if (CostMatrix.Count != IDHeader.Count)
                throw new SeqException("MatrixDistanceFunction.CostMatrix and ResourceIDList dimension should be equal.");
        }
    }
}