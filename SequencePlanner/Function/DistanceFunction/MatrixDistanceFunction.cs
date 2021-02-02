using SequencePlanner.Helper;
using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.Function.DistanceFunction
{
    public class MatrixDistanceFunction : DistanceFunction
    {
        //Header of the cost matrix with the ResourceIDs
        public List<int> IDList { get; set; }
        public List<List<double>> CostMatrix { get; }

        public MatrixDistanceFunction(List<List<double>> costMatrix, List<int> resourceIDList) : base()
        {
            FunctionName = "MatrixDistance";
            CostMatrix = costMatrix;
            IDList = resourceIDList;
            Validate();
        }

        public override double ComputeDistance(Position A, Position B)
        {
            if (A == null || B == null)
                throw new SequencerException("MatrixDistanceFunction A/B position null!");
            if (A.Dimension != B.Dimension)
                throw new SequencerException("MatrixDistanceFunction found dimendion mismatch!", "Check dimension of Positions with " + A.UserID + ", " + B.UserID);

            var givenDistance = GetStrictEdgeWeight(A, B);
            if (givenDistance != null)
                return givenDistance.Weight;
            else
            {
                var aid = -1;
                var bid = -1;
                for (int i = 0; i < IDList.Count; i++)
                {
                    if (IDList[i] == A.UserID)
                        aid = i;
                    if (IDList[i] == B.UserID)
                        bid = i;
                }
                if (aid == -1)
                    throw new SequencerException("Matrix distance function can not find user position ID. PositionA: [G:" + A.UserID + "] " + A.Name);
                if (bid == -1)
                    throw new SequencerException("Matrix distance function can not find user position ID. PositionA: [G:" + B.UserID + "] " + B.Name);
                return CostMatrix[aid][bid];
            }
        }

        public override void Validate()
        {
            if (CostMatrix==null || IDList==null)
                throw new SequencerException("MatrixDistanceFunction contains null property, ResourceIDList/CostMatrix.");
            if (CostMatrix.Count != CostMatrix[0].Count)
                throw new SequencerException("MatrixDistanceFunction.CostMatrix size should be n x n.");
            if (CostMatrix.Count != IDList.Count)
                throw new SequencerException("MatrixDistanceFunction.CostMatrix and ResourceIDList dimension should be equal.");
        }
    }
}
