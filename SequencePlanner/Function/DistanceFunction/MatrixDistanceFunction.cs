using SequencePlanner.Helper;
using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.Function.DistanceFunction
{
    public class MatrixDistanceFunction : DistanceFunction
    {
        public override string FunctionName { get { return "Matrix"; } }
        public List<int> IDHeader { get; set; }
        public List<List<double>> CostMatrix { get; }


        public MatrixDistanceFunction(List<List<double>> costMatrix, List<int> resourceIDList) : base()
        {
            CostMatrix = costMatrix;
            IDHeader = resourceIDList;
            Validate();
        }


        public override double ComputeDistance(Position A, Position B)
        {
            if (A == null || B == null)
                throw new SeqException("MatrixDistanceFunction A/B position null!");
            if (A.Dimension != B.Dimension)
                throw new SeqException("MatrixDistanceFunction found dimendion mismatch!", "Check dimension of Positions with " + A.UserID + ", " + B.UserID);

            var aid = -1;
            var bid = -1;
            for (int i = 0; i < IDHeader.Count; i++)
            {
                if (IDHeader[i] == A.UserID)
                    aid = i;
                if (IDHeader[i] == B.UserID)
                    bid = i;
            }

            if (aid == -1)
                throw new SeqException("Matrix distance function can not find user position ID. PositionA: [G:" + A.UserID + "] " + A.Name);
            if (bid == -1)
                throw new SeqException("Matrix distance function can not find user position ID. PositionA: [G:" + B.UserID + "] " + B.Name);

            return CostMatrix[aid][bid];
        }

        public override void Validate()
        {
            if (CostMatrix==null || IDHeader==null)
                throw new SeqException("MatrixDistanceFunction contains null property, ResourceIDList/CostMatrix.");
            if (CostMatrix.Count != CostMatrix[0].Count)
                throw new SeqException("MatrixDistanceFunction.CostMatrix size should be n x n.");
            if (CostMatrix.Count != IDHeader.Count)
                throw new SeqException("MatrixDistanceFunction.CostMatrix and ResourceIDList dimension should be equal.");
        }
    }
}