using SequencePlanner.Helper;
using SequencePlanner.Model.Hierarchy;

//TODO: MotionPrecedence only between processes, no inside alternatives and between alternatives in one process.
namespace SequencePlanner.Task.Processors
{
    public static class TaskValidator
    {
        public static void Validate(GeneralTask task)
        {
            SeqLogger.Debug("Validation started!", nameof(TaskValidator));
            SeqLogger.Indent++;
            CheckDistaceFunction(task);
            CheckStartFinsihDepot(task);
            CheckTimeLimit(task);
            CheckMIPpresolver(task);
            CheckResourceFunction(task);
            CheckLocalSearchStrategy(task);
            CheckConfigList(task);
            CheckStrictEdgeWeights(task);
            CheckCycle(task);
            CheckTaskType(task);
            //CheckProcessHierarchy(task);
            CheckProcessPrecedence(task);
            CheckUseAlternativeShortcuts(task);

            SeqLogger.Indent--;
            SeqLogger.Debug("Validation finished!", nameof(TaskValidator));
        }

        //Q17
        private static void CheckUseAlternativeShortcuts(GeneralTask task)
        {
            if (task.SolverSettings.UseShortcutInAlternatives)
                foreach (var process in task.Hierarchy.GetProcesses())
                {
                    foreach (var alternative in task.Hierarchy.GetAlternativesOf(process))
                    {
                        for (int i = 0; i < task.Hierarchy.GetTasksOf(alternative).Count; i++)
                        {
                            if (i != 0 && i != task.Hierarchy.GetTasksOf(alternative).Count - 1) //Not the first or the last task of the alternative.
                                foreach (var motion in task.Hierarchy.GetMotionsOf(task.Hierarchy.GetTasksOf(alternative)[i]))
                                {
                                    foreach (var precedence in task.MotionPrecedences)
                                    {
                                        if (precedence.Before.GlobalID == motion.GlobalID || precedence.After.GlobalID == motion.GlobalID)
                                            throw new SeqException("In case of alternative shortcuts, precedences available only for the configs of the first and last task of alternatives. Configuration's ID: " + motion.ID);
                                    }
                                }
                        }
                    }
                }
        }

        //Q16
        private static void CheckMotionPrecedence(GeneralTask task)
        {
            if ((task.StartDepot is not null || task.FinishDepot is not null) && task.MotionPrecedences is not null && task.MotionPrecedences.Count > 0)
                foreach (var config in task.Hierarchy.Configs)
                {
                    foreach (var precedence in task.ProcessPrecedences)
                    {
                        if (task.StartDepot is not null && (precedence.Before.GlobalID == task.StartDepot.GlobalID || precedence.After.GlobalID != task.StartDepot.GlobalID))
                            throw new SeqException("Configuration precedence should not contain StartDepo's configuration, ID: " + task.StartDepot.GlobalID);
                        if (task.FinishDepot is not null && (precedence.Before.GlobalID == task.FinishDepot.GlobalID || precedence.After.GlobalID != task.FinishDepot.GlobalID))
                            throw new SeqException("Configuration precedence should not contain FinishDepo's configuration, ID: " + task.FinishDepot.GlobalID);
                    }
                }
            SeqLogger.Debug("MotionPrecedence: " + task.ProcessPrecedences.Count + " precedences", nameof(TaskValidator));
        }

        //Q15
        private static void CheckProcessPrecedence(GeneralTask task)
        {
            Process a = null;
            Process b = null;
            if ((task.StartDepot is not null || task.FinishDepot is not null) && task.ProcessPrecedences is not null && task.ProcessPrecedences.Count > 0)
                foreach (var process in task.Hierarchy.GetProcesses())
                {
                    foreach (var alternative in task.Hierarchy.GetAlternativesOf(process))
                    {
                        foreach (var t in task.Hierarchy.GetTasksOf(alternative))
                        {
                            foreach (var motion in task.Hierarchy.GetMotionsOf(t))
                            {

                                if (task.StartDepot is not null && task.StartDepot.GlobalID == motion.GlobalID)
                                    a = process;
                                if (task.FinishDepot is not null && task.FinishDepot.GlobalID == motion.GlobalID)
                                    b = process;
                                if ((task.StartDepot != null || a is not null) && (task.FinishDepot != null || b is not null))
                                    goto Finish;
                            }
                        }
                    }
                }
            Finish:
            foreach (var precedence in task.ProcessPrecedences)
            {
                if (a is not null && (precedence.Before.GlobalID == a.GlobalID || precedence.After.GlobalID == a.GlobalID))
                    throw new SeqException("Process precedence should not contain StartDepo's process, UserID: " + a.ID);
                if (b is not null && (precedence.Before.GlobalID == b.GlobalID || precedence.After.GlobalID == b.ID))
                    throw new SeqException("Process precedence should not contain FinishDepo's process, UserID: " + a.GlobalID);
            }
            SeqLogger.Debug("ProcessPrecedence: " + task.ProcessPrecedences.Count + " precedences", nameof(TaskValidator));
        }

        //Q14
        //private static void CheckProcessHierarchy(GeneralTask task)
        //{
        //    for (int i = 0; i < task.Hierarchy.Count; i++)
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

        //Q13
        private static void CheckStrictEdgeWeights(GeneralTask task)
        {
            if (task.CostManager.OverrideCost is null)
                throw new SeqException("ConfigMatrix.DistanceFunction.StrictUserEdgeWeights are not initialized.");
            foreach (var weights in task.CostManager.OverrideCost)
            {
                var findA = false;
                var findB = false;
                foreach (var config in task.Hierarchy.Configs)
                {
                    if (weights.A.GlobalID == config.GlobalID)
                        findA = true;
                    if (weights.B.GlobalID == config.GlobalID)
                        findB = true;
                }
                if (!findA)
                    throw new SeqException("ConfigMatrix.DistanceFunction.StrictUserEdgeWeights contains configuration with wrong userID: " + weights.A.GlobalID);

                if (!findB)
                    throw new SeqException("ConfigMatrix.DistanceFunction.StrictUserEdgeWeights contains configuration with wrong userID: " + weights.B.GlobalID);
            }
        }

        //Q12
        //private static void CheckPositionMatrix(PositionMatrix positionMatrix)
        //{
        //    if (positionMatrix == null)
        //        throw new SeqException("PositionMatrix.PositionMatrix not given.");
        //    else
        //    {
        //        positionMatrix.Validate();
        //    }
        //    SeqLogger.Trace("CheckPositionMatrix validated!", nameof(TaskValidator));
        //}

        //Q11
        private static void CheckConfigList(GeneralTask task)
        {
            if (task.Hierarchy.Configs is null)
                throw new SeqException("Hierarchy, ConfigMatrix is null.", "Please construct it.");
            //if (task.Hierarchy.Configs.Count < 1)
            //    throw new SeqException("ConfigMatrix.Configurations not contain configs.", "Please add them.");
            var posList = task.Hierarchy.Motions;
            for (int i = 0; i < posList.Count; i++)
            {
                for (int j = 0; j < posList.Count; j++)
                {
                    if (i != j)
                    {
                        if (posList[i].GlobalID == posList[j].GlobalID)
                            throw new SeqException("ConfigMatrix.Configs contains config multiple times with GlobalID: " + posList[i].GlobalID, "Remove duplicated configurations.");
                        if (posList[i].ID == posList[j].ID && !posList[i].Bidirectional && !posList[i].Bidirectional)
                            throw new SeqException("ConfigMatrix.Configs contains config multiple times with ID: " + posList[i].ID, "Remove duplicated configurations.");
                        //if (posList[i].Node.SequencingID == posList[j].Node.SequencingID)
                        //    throw new SeqException("ConfigMatrix.Configs contains config multiple times times with SequencingID: " + posList[i].Node.SequencingID, "Remove duplicated configs.");
                    }
                }
                //if (posList[i].In.Vector.Length != task.Dimension)
                //    throw new SeqException("Config with ID: " + posList[i].In.ID + " has dimension mismatch. Dimension != Configuration.Config (" + task.Dimension + "!=" + posList[i].In.Vector.Length + ")");
                //if (posList[i].In.Vector.Length != posList[i].Out.Vector.Length)
                //    throw new SeqException("Config with ID: " + posList[i].In.ID + " has dimension mismatch. Dimension != Configuration.Config (" + task.Dimension + "!=" + posList[i].Out.Vector.Length + ")");
            }
            SeqLogger.Debug("ConfigList: " + task.Hierarchy.Configs.Count, nameof(TaskValidator));
        }

        //Q10
        private static void CheckLocalSearchStrategy(GeneralTask task)
        {
            SeqLogger.Debug("LocalSearchStrategy: " + task.SolverSettings.Metaheuristics.ToString(), nameof(TaskValidator));
        }

        //Q9
        private static void CheckResourceFunction(GeneralTask task)
        {
            if (task.CostManager.ResourceFunction is null)
                throw new SeqException("Resource function is null.", "Please construct it.");
            task.CostManager.ResourceFunction.Validate();
        }

        //Q8
        private static void CheckMIPpresolver(GeneralTask task)
        {
            SeqLogger.Debug("UseMIPPrecedenceSolver: " + task.SolverSettings.UseMIPprecedenceSolver, nameof(TaskValidator));
        }

        //Q7
        private static void CheckTimeLimit(GeneralTask task)
        {
            if (task.SolverSettings.TimeLimit <= 0)
            {
                SeqLogger.Debug("TimeLimit: 0 - Automatic based on solver", nameof(TaskValidator));
                SeqLogger.Warning("Time limit not given, running can take a long time", nameof(TaskValidator));
            }

            if (task.SolverSettings.TimeLimit > 0)
                SeqLogger.Debug("TimeLimit: " + task.SolverSettings.TimeLimit + "ms - " + System.TimeSpan.FromMilliseconds(task.SolverSettings.TimeLimit).ToString(), nameof(TaskValidator));
            if (task.SolverSettings.Metaheuristics == Metaheuristics.GuidedLocalSearch && task.SolverSettings.TimeLimit <= 0)
                throw new SeqException("TimeLimit needed in case of " + Metaheuristics.GuidedLocalSearch.ToString() + " metaheuristic.");
        }

        //Q6
        private static void CheckStartFinsihDepot(GeneralTask task)
        {
            var checkStartDepot = false;
            var checkFinishDepot = false;
            if (task.Cyclic)//if cyclic start depot needed!
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
                    SeqLogger.Debug("StartDepot: " + task.StartDepot.ID, nameof(TaskValidator));
                }

                if (task.FinishDepot is not null)
                {
                    checkFinishDepot = true;
                }
            }

            if (checkStartDepot || checkFinishDepot)
            {
                if (checkStartDepot && checkFinishDepot)
                    if (task.StartDepot.GlobalID == task.FinishDepot.GlobalID)
                        throw new SeqException("Start and finish depot can not be the same.", "Select other configs or use cyclic sequence.");
                //StartDepot needed
                if (task.Cyclic && task.StartDepotConfig is null)
                    throw new SeqException("If task is cyclic start depot needed!");
                //SeqLogger.Debug("StartDepot: " + task.StartDepot.UserID, nameof(PointLikeTaskValidator));
                var findStart = false;
                var findFinish = false;
                //Configurations must contain StartDepot
                foreach (var config in task.Hierarchy.Configs)
                {
                    if (checkStartDepot && config.GlobalID == task.StartDepotConfig.GlobalID)
                        findStart = true;
                    if (checkFinishDepot && config.GlobalID == task.FinishDepotConfig.GlobalID)
                        findFinish = true;
                }
                if (checkStartDepot && !findStart)
                    throw new SeqException("Configs should contain StartDepot!");
                if (checkFinishDepot && !findFinish)
                    throw new SeqException("Configs should contain FinishDepot!");
            }
        }

        //Q4
        private static void CheckCycle(GeneralTask task)
        {
            if (task.Cyclic)
            {
                if (task.StartDepotConfig == null)
                    throw new SeqException("In case of Cyclic StartDepot needed!");
                if (task.FinishDepotConfig != null)
                    throw new SeqException("In case of Cyclic FinishDepot not useable!");
            }
            SeqLogger.Trace("CheckCycle validated!", nameof(TaskValidator));
        }

        //Q3
        private static void CheckDistaceFunction(GeneralTask task)
        {
            if (task.CostManager.DistanceFunction is null)
                throw new SeqException("Distance function is null.", "Please construct it.");
            SeqLogger.Debug("DistanceFunction:" + task.CostManager.DistanceFunction.FunctionName, nameof(TaskValidator));
            task.CostManager.DistanceFunction.Validate();
        }

        //Q1
        private static void CheckTaskType(GeneralTask task)
        {
            if (task.CostManager is null)
                throw new SeqException("CostManager is null.", "Please construct it.");
            if (task.CostManager.DistanceFunction is null)
                throw new SeqException("CostManager, DistanceFunction is null", "Please construct it.");
            if (task.CostManager.ResourceFunction is null)
                throw new SeqException("CostManager, ResourceFunction is null", "Please construct it.");

            if (task.Hierarchy is null)
                throw new SeqException("Hierarchy is null.", "Please construct it.");
            if (task.Hierarchy.Configs is null)
                throw new SeqException("Hierarchy, Configs is null.", "Please construct it.");
            //if (task.Hierarchy.Configs.Count==0)
            //    throw new SeqException("No configs given.", "Please add them.");
            if (task.Hierarchy.Motions is null)
                throw new SeqException("Hierarchy, Motions is null.", "Please construct it.");
            //if (task.Hierarchy.Motions.Count==0)
            //    throw new SeqException("No motions given.", "Please add them.");
            if (task.Hierarchy.HierarchyRecords is null)
                throw new SeqException("Hierarchy, HierarchyRecords is null.", "Please construct it.");
            if (task.Hierarchy.HierarchyRecords.Count==0)
                throw new SeqException("Hierarchy, HierarchyRecords not given.", "Please add them.");

            if (task.SolverSettings is null)
                throw new SeqException("SolverSettings is null.", "Please construct it.");
            if (task.GTSPRepresentation is null)
                throw new SeqException("GTSPRepresentation is null.", "Please construct it.");

            if (task.ProcessPrecedences is null)
                throw new SeqException("ProcessPrecedence is null.", "Please construct it.");
            if (task.ProcessPrecedences.Count==1)
                SeqLogger.Warning("No process precedence found.", nameof(TaskValidator));
            else
                SeqLogger.Debug(task.ProcessPrecedences.Count + " process precedence found.", nameof(TaskValidator));

            if (task.MotionPrecedences.Count==1)
                SeqLogger.Warning("No motion precedence found.", nameof(TaskValidator));
            else
                SeqLogger.Debug(task.MotionPrecedences.Count + " motion precedence found.", nameof(TaskValidator));
        }
    }
}