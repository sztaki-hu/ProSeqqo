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
            var ida = -1;
            var idb = -1;
            for (int i = 0; i < CostMatrixIDHeader.Count; i++)
            {
                if (CostMatrixIDHeader[i] == A.ResourceID)
                    ida = i;
                if (CostMatrixIDHeader[i] == B.ResourceID)
                    idb = i;
            }
            return CostMatrix[ida][idb];
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
            if (LinkingFunction == null)
            {
                throw new SeqException("ConstantResourceFunction.LinkingFunction not given - NULL.");
            }
        }
        public void ToLog(LogLevel level)
        {
            SeqLogger.Indent++;
            SeqLogger.WriteLog(level, "ResourceFunction: " + FunctionName, nameof(DistanceFunction));
            SeqLogger.Indent--;
        }
    }
}