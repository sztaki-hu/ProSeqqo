using System.Collections.Generic;
using SequencePlanner.Model;
using SequencePlanner.Helper;

//TODO: MotionPrecedence only between processes, no inside alternatives and between alternatives in one process.
namespace SequencePlanner.GeneralModels
{
    public class NewGeneralTaskValidator
    {
        public static void Validate(NewGeneralTask pointLikeTask)
        {
            SeqLogger.Debug("Validation started!", nameof(NewGeneralTaskValidator));
            SeqLogger.Indent++;
            //CheckDistaceFunction(pointLikeTask);
            //CheckWeightMultiplier(pointLikeTask);
            //CheckStartFinsihDepot(pointLikeTask);
            //CheckTimeLimit(pointLikeTask);
            //CheckMIPpresolver(pointLikeTask);
            //CheckResourceFunction(pointLikeTask);
            //CheckLocalSearchStrategy(pointLikeTask);
            //CheckConfigList(pointLikeTask);
            //CheckPositionMatrix(pointLikeTask.PositionMatrix);
            //CheckStrictEdgeWeights(pointLikeTask);
            //CheckCycle(pointLikeTask);
            //CheckTaskType(pointLikeTask);
            //CheckProcessHierarchy(pointLikeTask);
            //CheckProcessPrecedence(pointLikeTask);
            //CheckUseAlternativeShortcuts(pointLikeTask);
            
            SeqLogger.Indent--;
            SeqLogger.Debug("Validation finished!", nameof(NewGeneralTaskValidator));
        }

        ////Q17
        //private static void CheckUseAlternativeShortcuts(NewGeneralTask task)
        //{
        //    if (task.UseShortcutInAlternatives)
        //        foreach (var process in task.Processes)
        //        {
        //            foreach (var alternative in process.Alternatives)
        //            {
        //                for (int i = 0; i < alternative.Tasks.Count; i++)
        //                {
        //                    if (i != 0 && i != alternative.Tasks.Count - 1) //Not the first or the last task of the alternative.
        //                        foreach (var position in alternative.Tasks[i].Positions)
        //                        {
        //                            foreach (var precedence in task.MotionPrecedence)
        //                            {
        //                                if (precedence.Before.GlobalID == position.Node.GlobalID || precedence.After.GlobalID == position.Node.GlobalID)
        //                                    throw new SeqException("In case of alternative shortcuts, position precedences available only for the positions of the first and last task of alternatives. Position's userID: " + position.Node.UserID);
        //                            }
        //                        }
        //                }
        //            }
        //        }
        //}

        ////Q16
        //private static void CheckMotionPrecedence(NewGeneralTask task)
        //{
        //    if ((task.StartDepot is not null || task.FinishDepot is not null) && task.MotionPrecedence is not null && task.MotionPrecedence.Count > 0)
        //        foreach (var position in task.PositionMatrix.Positions)
        //        {
        //            foreach (var precedence in task.ProcessPrecedence)
        //            {
        //                if (task.StartDepot is not null && (precedence.Before.GlobalID == task.StartDepot.GlobalID || precedence.After.GlobalID != task.StartDepot.GlobalID))
        //                    throw new SeqException("Position precedence should not contain StartDepo's position, UserID: " + task.StartDepot.GlobalID);
        //                if (task.FinishDepot is not null && (precedence.Before.GlobalID == task.FinishDepot.GlobalID || precedence.After.GlobalID != task.FinishDepot.GlobalID))
        //                    throw new SeqException("Position precedence should not contain FinishDepo's position, UserID: " + task.FinishDepot.GlobalID);
        //            }
        //        }
        //    SeqLogger.Debug("MotionPrecedence: " + task.ProcessPrecedence.Count + " precedences", nameof(NewGeneralTaskValidator));
        //}

        ////Q15
        //private static void CheckProcessPrecedence(NewGeneralTask task)
        //{
        //    Process a = null;
        //    Process b = null;
        //    if ((task.StartDepot is not null || task.FinishDepot is not null) && task.ProcessPrecedence is not null && task.ProcessPrecedence.Count > 0)
        //        foreach (var process in task.Processes)
        //        {
        //            foreach (var alternative in process.Alternatives)
        //            {
        //                foreach (var t in alternative.Tasks)
        //                {
        //                    foreach (var position in t.Positions)
        //                    {

        //                        if (task.StartDepot is not null && task.StartDepot.GlobalID == position.Node.GlobalID)
        //                            a = process;
        //                        if (task.FinishDepot is not null && task.FinishDepot.GlobalID == position.Node.GlobalID)
        //                            b = process;
        //                        if ((task.StartDepot != null || a is not null) && (task.FinishDepot != null || b is not null))
        //                            goto Finish;
        //                    }
        //                }
        //            }
        //        }
        //    Finish:
        //    foreach (var precedence in task.ProcessPrecedence)
        //    {
        //        if (a is not null && (precedence.Before.GlobalID == a.GlobalID || precedence.After.GlobalID == a.GlobalID))
        //            throw new SeqException("Process precedence should not contain StartDepo's process, UserID: " + a.UserID);
        //        if (b is not null && (precedence.Before.GlobalID == b.GlobalID || precedence.After.GlobalID == b.UserID))
        //            throw new SeqException("Process precedence should not contain FinishDepo's process, UserID: " + a.GlobalID);
        //    }
        //    SeqLogger.Debug("ProcessPrecedence: " + task.ProcessPrecedence.Count + " precedences", nameof(NewGeneralTaskValidator));
        //}

        ////Q14
        //private static void CheckProcessHierarchy(NewGeneralTask task)
        //{
        //    for (int i = 0; i < task.Processes.Count; i++)
        //    {
        //        for (int j = 0; j < task.Processes.Count; j++)
        //        {
        //            if (i != j && task.Processes[i].GlobalID == task.Processes[j].GlobalID)
        //                throw new SeqException("Process list contains process with UserID:" + task.Processes[j] + " multiple times.");
        //        }
        //    }

        //    for (int i = 0; i < task.Alternatives.Count; i++)
        //    {
        //        for (int j = 0; j < task.Alternatives.Count; j++)
        //        {
        //            if (i != j && task.Alternatives[i].GlobalID == task.Alternatives[j].GlobalID)
        //                throw new SeqException("Alternative list contains alternative with UserID:" + task.Alternatives[j] + " multiple times.");
        //        }
        //    }

        //    for (int i = 0; i < task.Tasks.Count; i++)
        //    {
        //        for (int j = 0; j < task.Tasks.Count; j++)
        //        {
        //            if (i != j && task.Tasks[i].GlobalID == task.Tasks[j].GlobalID)
        //                throw new SeqException("Tasks list contains task with UserID:" + task.Alternatives[j] + " multiple times.");
        //        }
        //    }
        //}

        ////Q13
        //private static void CheckStrictEdgeWeights(NewGeneralTask task)
        //{
        //    if (task.PositionMatrix.StrictUserEdgeWeights is null)
        //        throw new SeqException("PositionMatrix.DistanceFunction.StrictUserEdgeWeights are not initialized.");
        //    foreach (var weights in task.PositionMatrix.StrictUserEdgeWeights.GetAll())
        //    {
        //        var findA = false;
        //        var findB = false;
        //        foreach (var position in task.PositionMatrix.Positions)
        //        {
        //            if (weights.A.GlobalID == position.Node.GlobalID)
        //                findA = true;
        //            if (weights.B.GlobalID == position.Node.GlobalID)
        //                findB = true;
        //        }
        //        if (!findA)
        //            throw new SeqException("PositionMatrix.DistanceFunction.StrictUserEdgeWeights contains position with wrong userID: " + weights.A.GlobalID);

        //        if (!findB)
        //            throw new SeqException("PositionMatrix.DistanceFunction.StrictUserEdgeWeights contains position with wrong userID: " + weights.B.GlobalID);
        //    }
        //}

        ////Q12
        //private static void CheckPositionMatrix(PositionMatrix positionMatrix)
        //{
        //    if (positionMatrix == null)
        //        throw new SeqException("PositionMatrix.PositionMatrix not given.");
        //    else
        //    {
        //        positionMatrix.Validate();
        //    }
        //    SeqLogger.Trace("CheckPositionMatrix validated!", nameof(NewGeneralTaskValidator));
        //}

        ////Q11
        //private static void CheckConfigList(NewGeneralTask task)
        //{
        //    if (task.PositionMatrix is null)
        //        throw new SeqException("Position hierarchy, PositionMatrix is null.", "Please construct it.");
        //    if (task.PositionMatrix.Positions is null)
        //        throw new SeqException("Position matrix is null.", "Please construct it.");
        //    if (task.PositionMatrix.Positions.Count < 1)
        //        throw new SeqException("PositionMatrix.Positions not contain positions.", "Please add them.");
        //    var posList = task.PositionMatrix.Positions;
        //    for (int i = 0; i < posList.Count; i++)
        //    {
        //        for (int j = 0; j < posList.Count; j++)
        //        {
        //            //if (i != j && !posList[i].Bidirectional && !posList[i].Bidirectional)
        //            if (i != j)
        //            {
        //                if (posList[i].Node.GlobalID == posList[j].Node.GlobalID)
        //                    throw new SeqException("PositionMatrix.Positions contains position multiple times with GlobalID: " + posList[i].Node.GlobalID, "Remove duplicated positions.");
        //                if (posList[i].Node.UserID == posList[j].Node.UserID && !posList[i].Bidirectional && !posList[i].Bidirectional)
        //                    throw new SeqException("PositionMatrix.Positions contains position multiple times with UserID: " + posList[i].Node.UserID, "Remove duplicated positions.");
        //                //if (posList[i].Node.SequencingID == posList[j].Node.SequencingID)
        //                //    throw new SeqException("PositionMatrix.Positions contains position multiple times times with SequencingID: " + posList[i].Node.SequencingID, "Remove duplicated positions.");
        //            }
        //        }
        //        //if (posList[i].In.Vector.Length != task.Dimension)
        //        //    throw new SeqException("Position with UserID: " + posList[i].In.UserID + " has dimension mismatch. Dimension != Position.Vector (" + task.Dimension + "!=" + posList[i].In.Vector.Length + ")");
        //        //if (posList[i].In.Vector.Length != posList[i].Out.Vector.Length)
        //        //    throw new SeqException("Position with UserID: " + posList[i].In.UserID + " has dimension mismatch. Dimension != Position.Vector (" + task.Dimension + "!=" + posList[i].Out.Vector.Length + ")");
        //    }
        //    SeqLogger.Debug("ConfigList: " + task.PositionMatrix.Positions.Count, nameof(NewGeneralTaskValidator));
        //}

        ////Q10
        //private static void CheckLocalSearchStrategy(NewGeneralTask task)
        //{
        //    SeqLogger.Debug("LocalSearchStrategy: " + task.LocalSearchStrategy.ToString(), nameof(NewGeneralTaskValidator));
        //}

        ////Q9
        //private static void CheckResourceFunction(NewGeneralTask task)
        //{
        //    if (task.PositionMatrix.ResourceFunction is null)
        //        throw new SeqException("Resource function is null.", "Please construct it.");
        //    task.PositionMatrix.ResourceFunction.Validate();
        //}

        ////Q8
        //private static void CheckMIPpresolver(NewGeneralTask task)
        //{
        //    SeqLogger.Debug("UseMIPPrecedenceSolver: " + task.UseMIPprecedenceSolver, nameof(NewGeneralTaskValidator));
        //}

        ////Q7
        //private static void CheckTimeLimit(NewGeneralTask task)
        //{
        //    if (task.TimeLimit <= 0)
        //    {
        //        SeqLogger.Debug("TimeLimit: 0 - Automatic based on solver", nameof(NewGeneralTaskValidator));
        //        SeqLogger.Warning("Time limit not given, running can take a long time", nameof(NewGeneralTaskValidator));
        //    }

        //    if (task.TimeLimit > 0)
        //        SeqLogger.Debug("TimeLimit: " + task.TimeLimit + "ms - " + System.TimeSpan.FromMilliseconds(task.TimeLimit).ToString(), nameof(NewGeneralTaskValidator));
        //    if (task.LocalSearchStrategy == OR_Tools.LocalSearchStrategyEnum.Metaheuristics.GuidedLocalSearch && task.TimeLimit <= 0)
        //        throw new SeqException("TimeLimit needed in case of " + OR_Tools.LocalSearchStrategyEnum.Metaheuristics.GuidedLocalSearch.ToString() + " metaheuristic.");
        //}

        ////Q6
        //private static void CheckStartFinsihDepot(NewGeneralTask task)
        //{
        //    var checkStartDepot = false;
        //    var checkFinishDepot = false;
        //    if (task.Cyclic)//if cyclic start depot needed!
        //    {
        //        checkStartDepot = true;
        //        if (task.FinishDepot is not null)
        //            throw new SeqException("Can not use FinishDepot in cyclic tasks!");
        //    }
        //    else // not needed, optional
        //    {
        //        if (task.StartDepot is not null)
        //        {
        //            checkStartDepot = true;
        //            SeqLogger.Debug("StartDepot: " + task.StartDepot.UserID, nameof(NewGeneralTaskValidator));
        //        }

        //        if (task.FinishDepot is not null)
        //        {
        //            checkFinishDepot = true;
        //        }
        //    }

        //    if (checkStartDepot || checkFinishDepot)
        //    {
        //        if (checkStartDepot && checkFinishDepot)
        //            if (task.StartDepot.GlobalID == task.FinishDepot.GlobalID)
        //                throw new SeqException("Start and finish depot can not be the same.", "Select other positions or use cyclic sequence.");
        //        //StartDepot needed
        //        if (task.Cyclic && task.StartDepot is null)
        //            throw new SeqException("If task is cyclic start depot needed!");
        //        //SeqLogger.Debug("StartDepot: " + task.StartDepot.UserID, nameof(PointLikeTaskValidator));
        //        var findStart = false;
        //        var findFinish = false;
        //        //Positions must contain StartDepot
        //        foreach (var position in task.PositionMatrix.Positions)
        //        {

        //            if (checkStartDepot && position.Node.GlobalID == task.StartDepot.GlobalID)
        //                findStart = true;
        //            if (checkFinishDepot && position.Node.GlobalID == task.FinishDepot.GlobalID)
        //                findFinish = true;
        //        }
        //        if (checkStartDepot && !findStart)
        //            throw new SeqException("Positions should contain StartDepot!");
        //        if (checkFinishDepot && !findFinish)
        //            throw new SeqException("Positions should contain FinishDepot!");
        //        //StartDepot have to be contain by a task and it should be alone in it.
        //        findStart = false;
        //        findFinish = false;
        //        foreach (var process in task.Processes)
        //        {
        //            foreach (var alternative in process.Alternatives)
        //            {
        //                foreach (var t in alternative.Tasks)
        //                {
        //                    foreach (var position in t.Positions)
        //                    {
        //                        if (checkStartDepot && position.Node.GlobalID == task.StartDepot.GlobalID)
        //                        {
        //                            if (t.Positions.Count > 1)
        //                            {
        //                                throw new SeqException("StartDepot position shold be alone in containing task, because the others ignored.");
        //                            }
        //                            if (process.Alternatives.Count > 1)
        //                            {
        //                                throw new SeqException("StartDepot alternetive shold be alone in containing process, because the others ignored.");
        //                            }
        //                            findStart = true;
        //                        }
        //                        if (checkFinishDepot && position.Node.GlobalID == task.FinishDepot.GlobalID)
        //                        {
        //                            if (t.Positions.Count > 1)
        //                            {
        //                                throw new SeqException("FinsihDepot position shold be alone in containing task, because the others ignored.");
        //                            }
        //                            if (process.Alternatives.Count > 1)
        //                            {
        //                                throw new SeqException("FinsihDepot process shold be alone in containing process, because the others ignored.");
        //                            }
        //                            findFinish = true;
        //                        }
        //                        if ((!checkStartDepot || findStart) && (!checkFinishDepot || findFinish))
        //                            goto AfterBreak;
        //                    }
        //                }
        //            }
        //        }
        //    AfterBreak:
        //        if (checkStartDepot && !findStart)
        //        {
        //            throw new SeqException("StartDepot should contain by a process, alternative and a task.");
        //        }
        //        if (checkFinishDepot && !findFinish)
        //        {
        //            throw new SeqException("Finish should contain by a process, alternative and a task.");
        //        }
        //    }
        //}

        ////Q5
        //private static void CheckWeightMultiplier(NewGeneralTask task)
        //{
        //    if (task.WeightMultipier == 0)
        //        throw new SeqException("WeightMultipier should be greater then 0.");
        //    SeqLogger.Debug("WeightMultipier: " + task.WeightMultipier, nameof(NewGeneralTaskValidator));
        //}

        ////Q4
        //private static void CheckCycle(NewGeneralTask task)
        //{
        //    if (task.Cyclic)
        //    {
        //        if (task.StartDepot == null)
        //            throw new SeqException("In case of Cyclic StartDepot needed!");
        //        if (task.FinishDepot != null)
        //            throw new SeqException("In case of Cyclic FinishDepot not useable!");
        //    }
        //    SeqLogger.Trace("CheckCycle validated!", nameof(NewGeneralTaskValidator));
        //}

        ////Q3
        //private static void CheckDistaceFunction(NewGeneralTask task)
        //{
        //    if (task.PositionMatrix.DistanceFunction is null)
        //        throw new SeqException("Distance function is null.", "Please construct it.");
        //    SeqLogger.Debug("DistanceFunction:" + task.PositionMatrix.DistanceFunction.FunctionName, nameof(NewGeneralTaskValidator));
        //    task.PositionMatrix.DistanceFunction.Validate();
        //}

        ////O2
        //private static void CheckDimension(int dimension)
        //{
        //    if (dimension <= 0)
        //        throw new SeqException("Dimension should be > 0!");
        //}

        ////Q1
        //private static void CheckTaskType(NewGeneralTask task)
        //{
        //    if (task.PositionMatrix is null)
        //        throw new SeqException("Position hierarchy, PositionMatrix is null.", "Please construct it.");
        //    if (task.PositionMatrix.Positions is null)
        //        throw new SeqException("Position matrix is null.", "Please construct it.");
        //    if (task.PositionMatrix.Positions.Count < 1)
        //        throw new SeqException("PositionMatrix.Positions not contain positions.", "Please add them.");
        //    if (task.MotionPrecedence is null)
        //        throw new SeqException("MotionPrecedence is null.", "Please construct it.");
        //    if (task.ProcessPrecedence is null)
        //        throw new SeqException("ProcessPrecedence is null.", "Please construct it.");
        //    if (task.MotionPrecedence.Count < 1)
        //        SeqLogger.Warning("No position precedence found.", nameof(NewGeneralTaskValidator));
        //    else
        //        SeqLogger.Debug(task.MotionPrecedence.Count + " position precedence found.", nameof(NewGeneralTaskValidator));

        //    if (task.ProcessPrecedence.Count < 1)
        //        SeqLogger.Warning("No process precedence found.", nameof(NewGeneralTaskValidator));
        //    else
        //        SeqLogger.Debug(task.ProcessPrecedence.Count + " process precedence found.", nameof(NewGeneralTaskValidator));
        //}

        //private static void ListContainsPrecedenceItems(GTSPPrecedenceConstraint precedence, IEnumerable<BaseNode> nodes)
        //{
        //    var findAfter = false;
        //    var findBefore = false;
        //    foreach (var node in nodes)
        //    {
        //        if (node.GlobalID == precedence.Before.GlobalID)
        //            findBefore = true;
        //        if (node.GlobalID == precedence.After.GlobalID)
        //            findAfter = true;
        //        if (findBefore && findAfter)
        //            return;
        //    }
        //    if (!findBefore || !findAfter)
        //        throw new SeqException("Precedence list item.Before/After not found in Line/Contour list.");
        //}
    }
}