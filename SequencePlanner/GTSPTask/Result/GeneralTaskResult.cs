using SequencePlanner.GTSP;
using SequencePlanner.GTSPTask.Task.PointLike;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Result
{
    public class GeneralTaskResult : TaskResult
    {
        public List<GTSPNode> PositionResult { get; set; }
        public List<ResolveStruct> ResolveHelper { get; set; }

        public GeneralTaskResult(TaskResult baseTask) : base(baseTask)
        {
            PositionResult = new List<GTSPNode>();
            ResolveHelper = new List<ResolveStruct>();
        }

        public GeneralTaskResult()
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

        public void ResolveCosts(GeneralTask.CalculateWeightDelegate calculateWeightFunction)
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

        public override void Delete(int index)
        {
            base.Delete(index);
            if (SolutionRaw.Count <= index)
                throw new SeqException("Result delete failed: index > Solution.Length" + index + ">" + SolutionRaw.Count);
            if (SolutionRaw.Count > 0)
            {
                if (index != 0)
                {
                    CostSum -= CostsRaw[index-1];
                    CostsRaw.RemoveAt(index-1);
                }
                else
                {
                    CostSum -= CostsRaw[index];
                    CostsRaw.RemoveAt(index);
                }
                SolutionRaw.RemoveAt(index);
                PositionResult.RemoveAt(index);
            }
        }

        public void Validate(List<GTSPDisjointConstraint> disjointConstraints, List<GTSPPrecedenceConstraint> positionPrecedence, List<GTSPPrecedenceConstraint> processPrecedence)
        {
            ValidateDisjoint(disjointConstraints);
            ValidatePositionPrec(positionPrecedence);
            ValidateProcessPrec(processPrecedence);
        }

        private void ValidateProcessPrec(List<GTSPPrecedenceConstraint> processPrecedence)
        {
            //foreach (var prec in processPrecedence)
            //{
            //    var findFirst = false;
            //    var first = -1;

            //    var findSecond = false;
            //    var second = -1;
            //    for (int i = 0; i < PositionResult.Count; i++)
            //    {
            //        if (PositionResult[i].Node.UserID == prec.Before.UserID)
            //        {
            //            findFirst = true;
            //            first = i;
            //        }
            //        if (PositionResult[i].Node.UserID == prec.After.UserID)
            //        {
            //            findSecond = true;
            //            second = i;
            //        }
            //    }
            //    if (findSecond && findFirst && first > second)
            //        throw new SeqException("Result violates position precedence: " + prec);
            //}
        }

        private void ValidatePositionPrec(List<GTSPPrecedenceConstraint> positionPrecedence)
        {
            if(positionPrecedence is not null &&  PositionResult is not null && PositionResult.Count > 0 && positionPrecedence.Count > 0)
            {
                var findFirst = false;
                var first = -1;
                var findSecond = false;
                var second = -1;
                foreach (var prec in positionPrecedence)
                {
                    findFirst = false;
                    first = -1;
                    findSecond = false;
                    second = -1;
                    for (int i = 0; i < PositionResult.Count - 1; i++)
                    {
                        if (PositionResult[i].Node.UserID == prec.Before.UserID)
                        {
                            findFirst = true;
                            first = i;
                        }
                        if (PositionResult[i].Node.UserID == prec.After.UserID)
                        {
                            findSecond = true;
                            second = i;
                        }
                    }

                    if (findSecond && findFirst && first > second)
                            throw new SeqException("Result violates position precedence: " + prec);
                
                    //Check last result item, if not equal with the start depot
                    if (PositionResult[0].Node.GlobalID != PositionResult[PositionResult.Count-1].Node.GlobalID)
                    {
                        findFirst = false;
                        first = -1;
                        findSecond = false;
                        second = -1;
                        if (PositionResult[PositionResult.Count-1].Node.UserID == prec.Before.UserID)
                        {
                            findFirst = true;
                            first = PositionResult.Count;
                        }
                        if (PositionResult[PositionResult.Count-1].Node.UserID == prec.After.UserID)
                        {
                            findSecond = true;
                            second = PositionResult.Count;
                        }
                    } 

                    if (findSecond && findFirst && first > second)
                        throw new SeqException("Result violates position precedence: " + prec);
                }
            }
        }

        private void ValidateDisjoint(List<GTSPDisjointConstraint> disjointConstraints)
        {
            foreach (var disj in disjointConstraints)
            {
                foreach (var d in disj.DisjointSetUser)
                {
                    var findOne = false;
                    for (int i = 0; i < PositionResult.Count-1; i++)
                    {
                        if (d == PositionResult[i].Node.UserID)
                            if (findOne == true)
                                throw new SeqException("Result contains more than one element of disjoint set.");
                            else
                                findOne = true;
                    }
                }
            };
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