using System.Collections.Generic;
using SequencePlanner.Model;
using SequencePlanner.Helper;
using SequencePlanner.GTSPTask.Task.Base;
using SequencePlanner.GTSPTask.Task.LineTask;

//TODO: PositionPrecedence only between processes, no inside alternatives and between alternatives in one process.
namespace SequencePlanner.GTSPTask.Task.General
{
    public class GeneralTaskValidator
    {
        public static void Validate(GeneralTask pointLikeTask)
        {
            SeqLogger.Debug("Validation started!", nameof(GeneralTaskValidator));
            SeqLogger.Indent++;
            BaseTaskValidator.Validate((BaseTask)pointLikeTask);
            CheckTaskType(pointLikeTask);
            CheckStartFinsihDepot(pointLikeTask);
            CheckProcessHierarchy(pointLikeTask);
            CheckProcessPrecedence(pointLikeTask);
            CheckUseAlternativeShortcuts(pointLikeTask);
            SeqLogger.Indent--;
            SeqLogger.Debug("Validation finished!", nameof(GeneralTaskValidator));
        }


        //Q17
        private static void CheckUseAlternativeShortcuts(GeneralTask task)
        {
            if(task.UseShortcutInAlternatives)
                foreach (var process in task.Processes)
                {
                    foreach (var alternative in process.Alternatives)
                    {
                        for (int i = 0; i < alternative.Tasks.Count; i++)
                        {
                            if(i!=0 && i!=alternative.Tasks.Count-1) //Not the first or the last task of the alternative.
                                foreach (var position in alternative.Tasks[i].Positions)
                                {
                                    foreach (var precedence in task.PositionPrecedence)
                                    {
                                        if (precedence.Before.GlobalID == position.Node.GlobalID || precedence.After.GlobalID == position.Node.GlobalID)
                                            throw new SeqException("In case of alternative shortcuts, position precedences available only for the positions of the first and last task of alternatives. Position's userID: "+ position.Node.UserID);
                                    }
                                }
                        }
                    }
                }
        }

        //Q16
        private static void CheckPositionPrecedence(GeneralTask task)
        {
            if ((task.StartDepot is not null || task.FinishDepot is not null) && task.PositionPrecedence is not null && task.PositionPrecedence.Count > 0)
                foreach (var position in task.PositionMatrix.Positions)
                {
                    foreach (var precedence in task.ProcessPrecedence)
                    {
                        if (task.StartDepot is not null && (precedence.Before.GlobalID == task.StartDepot.GlobalID || precedence.After.GlobalID != task.StartDepot.GlobalID))
                            throw new SeqException("Position precedence should not contain StartDepo's position, UserID: " + task.StartDepot.GlobalID);
                        if (task.FinishDepot is not null && (precedence.Before.GlobalID == task.FinishDepot.GlobalID || precedence.After.GlobalID != task.FinishDepot.GlobalID))
                            throw new SeqException("Position precedence should not contain FinishDepo's position, UserID: " + task.FinishDepot.GlobalID);
                    }
                }
            SeqLogger.Debug("PositionPrecedence: " + task.ProcessPrecedence.Count+ " precedences", nameof(GeneralTaskValidator));
        }

        //Q15
        private static void CheckProcessPrecedence(GeneralTask task)
        {
            Process a = null;
            Process b = null;
            if((task.StartDepot is not null || task.FinishDepot is not null) && task.ProcessPrecedence is not null && task.ProcessPrecedence.Count>0)
                foreach (var process in task.Processes)
                {
                    foreach (var alternative in process.Alternatives)
                    {
                        foreach (var t in alternative.Tasks)
                        {
                            foreach (var position in t.Positions)
                            {

                                if (task.StartDepot is not null && task.StartDepot.GlobalID == position.Node.GlobalID)
                                    a = process;
                                if(task.FinishDepot is not null && task.FinishDepot.GlobalID == position.Node.GlobalID)
                                    b = process;
                                if ((task.StartDepot != null || a is not null) && (task.FinishDepot != null || b is not null))
                                    goto Finish;
                            }
                        }
                    }
                }
            Finish:
            foreach (var precedence in task.ProcessPrecedence)
            {
                if (a is not null && (precedence.Before.GlobalID == a.GlobalID || precedence.After.GlobalID == a.GlobalID))
                    throw new SeqException("Process precedence should not contain StartDepo's process, UserID: "+a.UserID);
                if (b is not null && (precedence.Before.GlobalID == b.GlobalID || precedence.After.GlobalID == b.UserID))
                    throw new SeqException("Process precedence should not contain FinishDepo's process, UserID: " + a.GlobalID);
            }
            SeqLogger.Debug("ProcessPrecedence: " + task.ProcessPrecedence.Count+" precedences", nameof(GeneralTaskValidator));
        }

        //Q14
        private static void CheckProcessHierarchy(GeneralTask task)
        {
            for (int i = 0; i < task.Processes.Count; i++)
            {
                for (int j = 0; j < task.Processes.Count; j++)
                {
                    if (i!=j && task.Processes[i].GlobalID == task.Processes[j].GlobalID)
                        throw new SeqException("Process list contains process with UserID:" + task.Processes[j] + " multiple times.");
                }
            }

            for (int i = 0; i < task.Alternatives.Count; i++)
            {
                for (int j = 0; j < task.Alternatives.Count; j++)
                {
                    if (i != j && task.Alternatives[i].GlobalID == task.Alternatives[j].GlobalID)
                        throw new SeqException("Alternative list contains alternative with UserID:" + task.Alternatives[j] + " multiple times.");
                }
            }

            for (int i = 0; i < task.Tasks.Count; i++)
            {
                for (int j = 0; j < task.Tasks.Count; j++)
                {
                    if (i != j && task.Tasks[i].GlobalID == task.Tasks[j].GlobalID)
                        throw new SeqException("Tasks list contains task with UserID:" + task.Alternatives[j] + " multiple times.");
                }
            }
        }

        //O6
        private static void CheckStartFinsihDepot(GeneralTask task)
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
                if (task.StartDepot is not null)
                {
                    checkStartDepot = true;
                    SeqLogger.Debug("StartDepot: "+ task.StartDepot.UserID, nameof(GeneralTaskValidator));
                }

                if (task.FinishDepot is  not null)
                {
                    checkFinishDepot = true;
                }
            }

            if (checkStartDepot || checkFinishDepot)
            {
                if (checkStartDepot && checkFinishDepot)
                    if (task.StartDepot.GlobalID == task.FinishDepot.GlobalID)
                        throw new SeqException("Start and finish depot can not be the same.", "Select other positions or use cyclic sequence.");
                //StartDepot needed
                if (task.CyclicSequence && task.StartDepot is null)
                    throw new SeqException("If task is cyclic start depot needed!");
                //SeqLogger.Debug("StartDepot: " + task.StartDepot.UserID, nameof(PointLikeTaskValidator));
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
                //StartDepot have to be contain by a task and it should be alone in it.
                findStart = false;
                findFinish = false;
                foreach (var process in task.Processes)
                {
                    foreach (var alternative in process.Alternatives)
                    {
                        foreach (var t in alternative.Tasks)
                        {
                            foreach (var position in t.Positions)
                            {
                                if (checkStartDepot && position.Node.GlobalID == task.StartDepot.GlobalID)
                                {
                                    if (t.Positions.Count > 1)
                                    {
                                        throw new SeqException("StartDepot position shold be alone in containing task, because the others ignored.");
                                    }
                                    if (process.Alternatives.Count > 1)
                                    {
                                        throw new SeqException("StartDepot alternetive shold be alone in containing process, because the others ignored.");
                                    }
                                    findStart = true;
                                }
                                if (checkFinishDepot && position.Node.GlobalID == task.FinishDepot.GlobalID)
                                {
                                    if (t.Positions.Count > 1)
                                    {
                                        throw new SeqException("FinsihDepot position shold be alone in containing task, because the others ignored.");
                                    }
                                    if (process.Alternatives.Count > 1)
                                    {
                                        throw new SeqException("FinsihDepot process shold be alone in containing process, because the others ignored.");
                                    }
                                    findFinish = true;
                                }
                                if ((!checkStartDepot || findStart) && (!checkFinishDepot || findFinish))
                                    goto AfterBreak;
                            }
                        }                      
                    }
                }
                AfterBreak:
                if (checkStartDepot && !findStart)
                {
                    throw new SeqException("StartDepot should contain by a process, alternative and a task.");
                }
                if (checkFinishDepot && !findFinish)
                {
                    throw new SeqException("Finish should contain by a process, alternative and a task.");
                }
            }
        }

        //O1
        private static void CheckTaskType(GeneralTask task)
        {
            if (task.PositionMatrix is null)
                throw new SeqException("Position hierarchy, PositionMatrix is null.", "Please construct it.");
            if (task.PositionMatrix.Positions is null)
                throw new SeqException("Position matrix is null.", "Please construct it.");
            if (task.PositionMatrix.Positions.Count < 1)
                throw new SeqException("PositionMatrix.Positions not contain positions.", "Please add them.");
            if (task.PositionPrecedence is null)
                throw new SeqException("PositionPrecedence is null.", "Please construct it.");
            if (task.ProcessPrecedence is null)
                throw new SeqException("ProcessPrecedence is null.", "Please construct it.");
            if (task.PositionPrecedence.Count < 1)
                SeqLogger.Warning("No position precedence found.", nameof(GeneralTaskValidator));
            else
                SeqLogger.Debug(task.PositionPrecedence.Count + " position precedence found.", nameof(GeneralTaskValidator));

            if (task.ProcessPrecedence.Count < 1)
                SeqLogger.Warning("No process precedence found.", nameof(GeneralTaskValidator));
            else
                SeqLogger.Debug(task.ProcessPrecedence.Count + " process precedence found.", nameof(GeneralTaskValidator));
        }

        private static void ListContainsPrecedenceItems(GTSPPrecedenceConstraint precedence, IEnumerable<BaseNode> nodes)
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