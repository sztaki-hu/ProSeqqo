using SequencePlanner.GTSPTask.Task.PointLike;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Result
{
    public class PointTaskResult : TaskResult
    {
        public List<GTSPNode> PositionResult { get; set; }
        public List<ResolveStruct> ResolveHelper { get; set; }

        public PointTaskResult(TaskResult baseTask) : base(baseTask)
        {
            PositionResult = new List<GTSPNode>();
            ResolveHelper = new List<ResolveStruct>();
        }

        public PointTaskResult()
        {
            PositionResult = new List<GTSPNode>();
            ResolveHelper = new List<ResolveStruct>();
        }

        public void CreateRawGeneralIDStruct()
        {
            foreach (var item in SolutionRaw)
            {
                ResolveHelper.Add(new ResolveStruct() { SeqID = (int)item }) ;
            }
        }

        public void ResolveHelperStruct()
        {
            SolutionRaw = new List<long>();
            CostsRaw = new List<double>();
            PositionResult = new List<GTSPNode>();
            foreach (var item in ResolveHelper)
            {
                if (item.Resolved)
                {
                    foreach (var resolveItem in item.Resolve)
                    {
                        SolutionRaw.Add((long)(resolveItem.Node.UserID));
                        PositionResult.Add(resolveItem);
                    }
                    foreach (var c in item.ResolveCost)
                    {
                        CostsRaw.Add(c);
                    }
                    CostsRaw.Add(item.Cost);
                }
                else
                {
                    SolutionRaw.Add((long)(item.Node.Node.UserID));
                    CostsRaw.Add(item.Cost);
                    PositionResult.Add(item.Node);
                }
            }
        }

        public void ResolveCosts(PointLikeTask.CalculateWeightDelegate calculateWeightFunction)
        {
            CostsRaw = new List<double>();
            CostSum = 0;
            for (int i = 0; i < PositionResult.Count-1; i++)
            {
                CostsRaw.Add(calculateWeightFunction(PositionResult[i], PositionResult[i + 1]));
                CostSum += CostsRaw[i];
            }
        }

        public void ToLog(LogLevel lvl)
        {
            SeqLogger.Info("Result: ");
            SeqLogger.Indent++;
            base.ToLog(lvl);
            if (StatusCode == 1)
            {
                SeqLogger.Info("Solution: ");
                SeqLogger.Indent++;
                if(PositionResult!=null && PositionResult.Count > 0)
                {
                    for (int i = 0; i < PositionResult.Count - 1; i++)
                    {
                        SeqLogger.Info(PositionResult[i].ToString());
                        SeqLogger.Info("--" + CostsRaw[i].ToString());
                    }
                    SeqLogger.Info(PositionResult[PositionResult.Count - 1].ToString());
                }
                SeqLogger.Indent--;
            }
            SeqLogger.Indent--;
        }

        public static string ToCSVHeader() => TaskResult.ToCSVHeader();
        public string ToCSV() => base.ToCSV();
    }

    public class ResolveStruct
    {
        public int SeqID { get; set; }
        public int GID { get; set; }
        public GTSPNode Node { get; set; }
        public double Cost { get; set; }
        public List<GTSPNode> Resolve { get; set; }
        public List<double> ResolveCost { get; set; }
        public bool Resolved { get; set; }

        public ResolveStruct()
        {
            Resolve = new List<GTSPNode>();
            ResolveCost = new List<double>();
        }
    }
}