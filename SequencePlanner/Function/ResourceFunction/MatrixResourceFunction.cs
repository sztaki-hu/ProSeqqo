using SequencePlanner.Helper;
using SequencePlanner.Model;
using SequencePlanner.Function.ResourceFunction.ResourceDistanceLink;
using System.Collections.Generic;

namespace SequencePlanner.Function.ResourceFunction
{
    public class MatrixResourceFunction : IResourceFunction
    {
        public string FunctionName { get { return "MatrixResource"; } }
        //Header of the cost matrix with the ResourceIDs
        public List<int> CostMatrixIDHeader { get; set; }
        public List<List<double>> CostMatrix { get;}
        public IResourceDistanceLinkFunction LinkingFunction { get; set; }

        public double ComputeResourceCost(Position A, Position B, double distance)
        {
            if (A.ResourceID == -1)
                throw new SeqException("Position with UserID: " + A.UserID + " has no ResourceID: -1");
            if (B.ResourceID == -1)
                throw new SeqException("Position with UserID: " + B.UserID + " has no ResourceID: -1");
            var ida = -1;
            var idb = -1;
            for (int i = 0; i < CostMatrixIDHeader.Count; i++)
            {
                if (CostMatrixIDHeader[i] == A.ResourceID)
                    ida = i;
                if (CostMatrixIDHeader[i] == B.ResourceID)
                    idb = i;
                if(ida!=-1 && idb!=-1)
                    return LinkingFunction.ComputeResourceDistanceCost(CostMatrix[ida][idb], distance);
            }
            if (ida == -1)
                throw new SeqException("Position with ResourceID: "+ A.ResourceID + " not contained by CostMatrixIDHeader");
            if (idb == -1)
                throw new SeqException("Position with ResourceID: "+ B.ResourceID + " not contained by CostMatrixIDHeader");
            return LinkingFunction.ComputeResourceDistanceCost(CostMatrix[ida][idb], distance);
        }

        public MatrixResourceFunction(List<List<double>> costMatrix, List<int> resourceIDList, IResourceDistanceLinkFunction link)
        {
            CostMatrix = costMatrix;
            CostMatrixIDHeader = resourceIDList;
            LinkingFunction = link;
            Validate();
        }

        public void Validate()
        {
            if(CostMatrixIDHeader.Count != CostMatrix.Count)
                throw new SeqException("Resource cost matrix size not equal with the header.");

            foreach (var mx1 in CostMatrix)
            {
                foreach (var mx2 in CostMatrix)
                {
                    if (mx1.Count != mx2.Count)
                        throw new SeqException("Size of resource cost matrix should be n x n");
                }
            }

            if (LinkingFunction == null)
            {
                throw new SeqException("ConstantResourceFunction.LinkingFunction not initalized.");
            }
            SeqLogger.Debug("ResourceFunction: " + FunctionName, nameof(MatrixResourceFunction));
            SeqLogger.Debug("MatrixResource: " + CostMatrix.Count+"x"+CostMatrix.Count, nameof(MatrixResourceFunction));
            SeqLogger.Debug("LinkingFunction: " + LinkingFunction.FunctionName, nameof(MatrixResourceFunction));
        }
        public void ToLog(LogLevel level)
        {
            SeqLogger.Indent++;
            SeqLogger.WriteLog(level, "ResourceFunction: " + FunctionName, nameof(MatrixResourceFunction));
            SeqLogger.Indent--;
        }
    }
}