using SequencePlanner.GTSPTask.Task.General;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Result
{
    public class GeneralTaskResult
    {
        public TimeSpan FullTime { get; set; }          //Run time of build and execute task
        public TimeSpan SolverTime { get; set; }        //Run time of OR-Tools VRP solver
        public TimeSpan PreSolverTime { get; set; }     //Run time of MIP presolver
        public List<long> SolutionRaw { get; set; }     //Solution of 
        public List<double> CostsRaw { get; set; }      //Costs of solution between the steps
        public double CostSum { get; set; }             //Sum of solution costs (CostsRaw)
        public int StatusCode { get; set; }             //OR-Tools exit status code
        public string StatusMessage { get; set; }       //OR-Tools exit message
        public List<string> Log { get; set; }           //Console log of task running
        public List<string> ErrorMessage { get; set; }  //Console log of task running
        public List<GTSPNode> PositionResult { get; set; }
        public List<ResolveStruct> ResolveHelper { get; set; }

        public virtual void Calculate()
        {
            CostSum = 0;
            foreach (var cost in CostsRaw)
            {
                CostSum += cost;
            }
        }

        public GeneralTaskResult()
        {
            FullTime = new TimeSpan();
            SolverTime = new TimeSpan();
            SolutionRaw = new List<long>();
            CostsRaw = new List<double>();
            CostSum = 0;
            StatusCode = -1;
            StatusMessage = "Not filled yet!";
            Log = new List<string>();
            ErrorMessage = new List<string>();
            PositionResult = new List<GTSPNode>();
            ResolveHelper = new List<ResolveStruct>();
        }

        public GeneralTaskResult(GeneralTaskResult task)
        {
            FullTime = task.FullTime;
            SolverTime = task.SolverTime;
            SolutionRaw = task.SolutionRaw;
            CostsRaw = task.CostsRaw;
            CostSum = task.CostSum;
            StatusCode = task.StatusCode;
            StatusMessage = task.StatusMessage;
            Log = task.Log;
            ErrorMessage = task.ErrorMessage;
            PositionResult = new List<GTSPNode>();
            ResolveHelper = new List<ResolveStruct>();
            foreach (var item in SolutionRaw)
            {
                ResolveHelper.Add(new ResolveStruct() { SeqID = (int)item });
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

        public void Delete(int index)
        {
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

        public virtual void DeleteFirst()
        {
            Delete(0);
        }
        public virtual void DeleteLast()
        {
            Delete(SolutionRaw.Count - 1);
        }

        public static string ToCSVHeader()
        {
            var s = ";";
            return nameof(FullTime) + "[ms]" + s + nameof(SolverTime) + "[ms]" + s + nameof(PreSolverTime) + "[ms]" + s + nameof(StatusCode) + s + nameof(StatusMessage) + s + nameof(CostSum) + s + nameof(CostsRaw) + s + nameof(SolutionRaw);
        }

        public string ToCSV()
        {
            var s = ";";
            return FullTime.TotalMilliseconds.ToString() + s + SolverTime.TotalMilliseconds.ToString() + s + PreSolverTime.TotalMilliseconds.ToString() + s + StatusCode + s + StatusMessage + s + CostSum + s + CostsRaw.ToListString() + s + SolutionRaw.ToListString();
        }

        public void ToLog(LogLevel lvl)
        {
            SeqLogger.Info("Result: ");
            SeqLogger.Indent++;
            if (StatusCode == 1)
            {
                SeqLogger.WriteLog(lvl, "StatusCode: " + StatusCode);
                SeqLogger.WriteLog(lvl, "StatusMessage: " + StatusMessage);
                SeqLogger.WriteLog(lvl, "FullTime: " + FullTime);
                SeqLogger.WriteLog(lvl, "SolverTime: " + SolverTime);
                SeqLogger.WriteLog(lvl, "PreSolverTime: " + PreSolverTime);
                SeqLogger.WriteLog(lvl, "SolutionRaw: " + SolutionRaw.ToListString());
                SeqLogger.WriteLog(lvl, "CostsRaw: " + CostsRaw.ToListString());
                SeqLogger.WriteLog(lvl, "CostSum: " + CostSum);
                SeqLogger.WriteLog(lvl, "Log size: " + Log.Count + " lines");
                var error = "";
                foreach (var item in ErrorMessage)
                {
                    error = item + "\n";
                }
                SeqLogger.WriteLog(lvl, "Error messages: " + error);
            }
            else
            {
                SeqLogger.WriteLog(lvl, "StatusCode: " + StatusCode);
                SeqLogger.WriteLog(lvl, "StatusMessage: " + StatusMessage);
                SeqLogger.WriteLog(lvl, "FullTime: " + FullTime);
                SeqLogger.WriteLog(lvl, "SolverTime: " + SolverTime);
                SeqLogger.WriteLog(lvl, "Log size: " + Log.Count + " lines");
                var error = "";
                foreach (var item in ErrorMessage)
                {
                    error = item + "\n";
                }
                SeqLogger.WriteLog(lvl, "Error messages: " + error);
            }
            if (StatusCode == 1)
            {
                SeqLogger.Info("Solution: ");
                SeqLogger.Indent++;
                if (PositionResult != null && PositionResult.Count > 0)
                {
                    for (int i = 0; i < PositionResult.Count - 1; i++)
                    {
                        SeqLogger.Info(PositionResult[i].ToString());
                        SeqLogger.Info("--" + CostsRaw[i].ToString());
                    }
                    SeqLogger.Info(PositionResult[^1].ToString());
                }
                SeqLogger.Indent--;
            }
            SeqLogger.Indent--;
        }

        public void Validate(List<GTSPDisjointConstraint> disjointConstraints, List<GTSPPrecedenceConstraint> motionPrecedence)
        {
            ValidateDisjoint(disjointConstraints);
            ValidateMotionPrec(motionPrecedence);
            //ValidateProcessPrec(processPrecedence);
        }

        private void ValidateMotionPrec(List<GTSPPrecedenceConstraint> motionPrecedence)
        {
            if(motionPrecedence is not null &&  PositionResult is not null && PositionResult.Count > 0 && motionPrecedence.Count > 0)
            {
                var findFirst = false;
                var first = -1;
                var findSecond = false;
                var second = -1;
                foreach (var prec in motionPrecedence)
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
                    if (PositionResult[0].Node.GlobalID != PositionResult[^1].Node.GlobalID)
                    {
                        findFirst = false;
                        first = -1;
                        findSecond = false;
                        second = -1;
                        if (PositionResult[^1].Node.UserID == prec.Before.UserID)
                        {
                            findFirst = true;
                            first = PositionResult.Count;
                        }
                        if (PositionResult[^1].Node.UserID == prec.After.UserID)
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

        //private void ValidateProcessPrec(List<GTSPPrecedenceConstraint> processPrecedence)
        //{
        //    foreach (var prec in processPrecedence)
        //    {
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
        // }
        //}
    }
}