using SequencePlanner.GTSP;
using SequencePlanner.GTSPTask.Task.Base;
using SequencePlanner.GTSPTask.Task.PointLike;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Task.LineLike
{
    internal class BaseTaskValidator
    {
        public BaseTaskValidator()
        {

        }

        public void Validate(BaseTask baseTask)
        {
            SeqLogger.Info("Validation started!", nameof(BaseTaskValidator));
            SeqLogger.Indent++;
            CheckTaskType(baseTask);
            if (baseTask.PositionMatrix.DistanceFunction.FunctionName!="MatrixDistance")
                CheckDimension(baseTask.Dimension);
            CheckDistaceFunction(baseTask);
            CheckWeightMultiplier(baseTask);
            CheckStartFinsihDepot(baseTask);
            CheckTimeLimit(baseTask);
            CheckMIPpresolver(baseTask);
            CheckResourceFunction(baseTask);
            CheckLocalSearchStrategy(baseTask);
            CheckPositionList(baseTask);
            CheckPositionMatrix(baseTask.PositionMatrix);
            CheckStrictEdgeWeights(baseTask);
            CheckCycle(baseTask);
            SeqLogger.Indent--;
            SeqLogger.Info("Validation finished!", nameof(BaseTaskValidator));
        }

        //Q13
        private void CheckStrictEdgeWeights(BaseTask task)
        {
            if (task.PositionMatrix.DistanceFunction.StrictUserEdgeWeights is null)
                throw new SeqException("PositionMatrix.DistanceFunction.StrictUserEdgeWeights are not initialized.");
            foreach (var weights in task.PositionMatrix.DistanceFunction.StrictUserEdgeWeights.GetAll())
            {
                var findA = false;
                var findB = false;
                foreach (var position in task.PositionMatrix.Positions)
                {
                    if (weights.A.GlobalID == position.Node.GlobalID)
                        findA = true;
                    if (weights.B.GlobalID == position.Node.GlobalID)
                        findB = true;
                }
                if(!findA)
                    throw new SeqException("PositionMatrix.DistanceFunction.StrictUserEdgeWeights contains position with wrong userID: "+ weights.A.GlobalID);

                if(!findB)
                    throw new SeqException("PositionMatrix.DistanceFunction.StrictUserEdgeWeights contains position with wrong userID: "+ weights.B.GlobalID);
            }
        }

        //Q12
        private void CheckPositionMatrix(PositionMatrix positionMatrix)
        {
            if (positionMatrix == null)
                throw new SeqException("PositionMatrix.PositionMatrix not given.");
            else
            {
                positionMatrix.Validate();
            }
            SeqLogger.Trace("CheckPositionMatrix validated!", nameof(BaseTaskValidator));
        }

        //Q11
        private void CheckPositionList(BaseTask task)
        {
            if (task.PositionMatrix is null)
                throw new SeqException("Position hierarchy, PositionMatrix is null.", "Please construct it.");
            if (task.PositionMatrix.Positions is null)
                throw new SeqException("Position matrix is null.", "Please construct it.");
            if (task.PositionMatrix.Positions.Count < 1)
                throw new SeqException("PositionMatrix.Positions not contain positions.", "Please add them.");
            var posList = task.PositionMatrix.Positions;
            for (int i = 0; i < posList.Count; i++)
            {
                for (int j = 0; j < posList.Count; j++)
                {
                    if (i != j)
                    {
                        if (posList[i].Node.GlobalID == posList[j].Node.GlobalID)
                            throw new SeqException("PositionMatrix.Positions contains position multiple times with GlobalID: " + posList[i].Node.GlobalID, "Remove duplicated positions.");
                        if (posList[i].Node.UserID == posList[j].Node.UserID)
                            throw new SeqException("PositionMatrix.Positions contains position multiple times with UserID: " + posList[i].Node.UserID, "Remove duplicated positions.");
                        //if (posList[i].Node.SequencingID == posList[j].Node.SequencingID)
                        //    throw new SeqException("PositionMatrix.Positions contains position multiple times times with SequencingID: " + posList[i].Node.SequencingID, "Remove duplicated positions.");
                    }
                }
                if(posList[i].In.Vector.Length != task.Dimension || posList[i].In.Vector.Length != posList[i].Out.Vector.Length)
                    throw new SeqException("Position with UserID: " + posList[i].In.UserID + " has dimension mismatch.");
            }
            SeqLogger.Info("PositionList: " + task.PositionMatrix.Positions.Count, nameof(BaseTaskValidator));
        }

        //Q10
        private void CheckLocalSearchStrategy(BaseTask task)
        {
            SeqLogger.Info("LocalSearchStrategy: " + task.LocalSearchStrategy.ToString(), nameof(BaseTaskValidator));
        }

        //O9
        private void CheckResourceFunction(BaseTask task)
        {
            if (task.PositionMatrix.ResourceFunction is null)
                throw new SeqException("Resource function is null.", "Please construct it.");
            task.PositionMatrix.ResourceFunction.Validate();
        }

        //O8
        private void CheckMIPpresolver(BaseTask task)
        {
            SeqLogger.Info("UseMIPPrecedenceSolver: "+ task.UseMIPprecedenceSolver, nameof(BaseTaskValidator));
        }

        //O7
        private void CheckTimeLimit(BaseTask task)
        {
            if (task.TimeLimit <= 0)
            {
                SeqLogger.Info("TimeLimit: 0 - Automatic based on solver", nameof(BaseTaskValidator));
                SeqLogger.Warning("Time limit not given, running can take a long time", nameof(BaseTaskValidator));
            }

            if (task.TimeLimit > 0)
                SeqLogger.Info("TimeLimit: "+task.TimeLimit+"ms - "+ System.TimeSpan.FromMilliseconds(task.TimeLimit).ToString(), nameof(BaseTaskValidator));
            if(task.LocalSearchStrategy == OR_Tools.LocalSearchStrategyEnum.Metaheuristics.GuidedLocalSearch && task.TimeLimit<=0)
                throw new SeqException("TimeLimit needed in case of "+ OR_Tools.LocalSearchStrategyEnum.Metaheuristics.GuidedLocalSearch.ToString() + " metaheuristic.");
        }

        //O6
        private void CheckStartFinsihDepot(BaseTask task)
        {
            var checkStartDepot =false;
            var checkFinishDepot =false;
            if (task.CyclicSequence)//if cyclic start depot needed!
            {
                checkStartDepot = true;
                if (task.FinishDepot is not null)
                    throw new SeqException("Can not use FinishDepot in cyclic tasks!");
            }
            else // not needed, optional
            {
                if (task.StartDepot is null)
                {
                    SeqLogger.Info("StartDepot: -", nameof(BaseTaskValidator));
                }
                else
                {
                    checkStartDepot = true;
                    SeqLogger.Info("StartDepot: "+ task.StartDepot.UserID, nameof(BaseTaskValidator));
                }

                if (task.FinishDepot is null)
                {
                    SeqLogger.Info("FinshDepot: -", nameof(BaseTaskValidator));
                }
                else
                {
                    SeqLogger.Info("FinshDepot: "+ task.FinishDepot.UserID, nameof(BaseTaskValidator));
                    checkFinishDepot = true;
                }
            }

            if (checkStartDepot || checkFinishDepot)
            {
                if (checkStartDepot && checkFinishDepot)
                    if (task.StartDepot.GlobalID == task.FinishDepot.GlobalID)
                        throw new SeqException("Start and finish depot can not be the same.", "Select other positions or use cyclic sequence.");
                //StartDepot needed
                if (task.StartDepot is null)
                    throw new SeqException("If task is cyclic start depot needed!");
                SeqLogger.Info("StartDepot: " + task.StartDepot.UserID, nameof(BaseTaskValidator));
                var findStart = false;
                var findFinish = false;
                //Positions must contain StartDepot
                foreach (var position in task.PositionMatrix.Positions)
                {
                    if (checkStartDepot && position.Node.GlobalID == task.StartDepot.GlobalID)
                        findStart = true;
                    if (checkFinishDepot && position.Node.GlobalID == task.FinishDepot.GlobalID)
                        findFinish = true;
                }
                if(checkStartDepot && !findStart)
                   throw new SeqException("Positions should contain StartDepot!");
                if (checkFinishDepot && !findFinish)
                    throw new SeqException("Positions should contain FinishDepot!");
            }
        }

        //O5
        private void CheckWeightMultiplier(BaseTask task)
        {
            if (task.WeightMultipier == 0)
                throw new SeqException("WeightMultipier should be greater then 0.");
            SeqLogger.Info("WeightMultipier: " + task.WeightMultipier, nameof(BaseTaskValidator));
        }

        //Q4
        private void CheckCycle(BaseTask task)
        {
            if (task.CyclicSequence)
            {
                if (task.StartDepot == null)
                    throw new SeqException("In case of CyclicSequence StartDepot needed!");
                if (task.FinishDepot != null)
                    throw new SeqException("In case of CyclicSequence FinishDepot not useable!");
            }
            SeqLogger.Trace("CheckCycle validated!", nameof(BaseTaskValidator));
        }

        //O3
        private void CheckDistaceFunction(BaseTask task)
        {
            if(task.PositionMatrix.DistanceFunction is null)
                throw new SeqException("Distance function is null.", "Please construct it.");
            SeqLogger.Info("DistanceFunction:"+ task.PositionMatrix.DistanceFunction.FunctionName, nameof(BaseTaskValidator));
            task.PositionMatrix.DistanceFunction.Validate();
        }

        //O2
        private void CheckDimension(int dimension)
        {
            if (dimension <= 0)
                throw new SeqException("Dimension should be > 0!");
        }

        //O1
        private void CheckTaskType(BaseTask task)
        {
            if (task.PositionMatrix is null)
                throw new SeqException("Position hierarchy, PositionMatrix is null.", "Please construct it.");
            if (task.PositionMatrix.Positions is null)
                throw new SeqException("Position matrix is null.", "Please construct it.");
            if (task.PositionMatrix.Positions.Count < 1)
                throw new SeqException("PositionMatrix.Positions not contain positions.", "Please add them.");
        }

        private void ListContainsPrecedenceItems(GTSPPrecedenceConstraint precedence, IEnumerable<BaseNode> nodes)
        {
            var findAfter = false;
            var findBefore = false;
            foreach (var node in nodes)
            {
                if (node.GlobalID == precedence.Before.GlobalID)
                    findBefore = true;
                if (node.GlobalID == precedence.After.GlobalID)
                    findAfter = true;
                if (findBefore && findAfter)
                    return;
            }
            if (!findBefore || !findAfter)
                throw new SeqException("Precedence list item.Before/After not found in Line/Contour list.");
        }
    }
}