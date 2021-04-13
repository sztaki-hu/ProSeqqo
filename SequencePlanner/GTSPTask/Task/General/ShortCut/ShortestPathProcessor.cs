using System.Collections.Generic;
using SequencePlanner.Model;
using SequencePlanner.Helper;
using SequencePlanner.GTSPTask.Result;

namespace SequencePlanner.GTSPTask.Task.General.ShortCut
{
    public class ShortestPathProcessor
    {
        private GeneralTask GeneralTask { get; set; }
        private List<GTSPNode> DeletedPositions { get; set; }
        private List<GTSPNode> PositionOfShortcuts { get; set; } 
        private List<Model.Task> DeletedTasks { get; set; }
        private List<Model.Task> TasksOfShortcuts { get; set; } 
        private List<GTSPPrecedenceConstraint> DeletedPositionPrecedencesOfShortcuts { get; set; }
        private List<GTSPPrecedenceConstraint> PositionPrecedencesOfShortcuts { get; set; }
        private List<AlternativeShortcut> AlternativeShortcuts { get; set; }


        public ShortestPathProcessor(GeneralTask pointTask) {
            GeneralTask = pointTask;
            DeletedPositions = new List<GTSPNode>();
            PositionOfShortcuts = new List<GTSPNode>();
            DeletedTasks = new List<Model.Task>();
            TasksOfShortcuts = new List<Model.Task>();
            AlternativeShortcuts = new List<AlternativeShortcut>();
            DeletedPositionPrecedencesOfShortcuts = new List<GTSPPrecedenceConstraint>();
            PositionPrecedencesOfShortcuts = new List<GTSPPrecedenceConstraint>();
        }


        public void Change()
        {
            SeqLogger.Debug("Alternative shortcut creation started!", nameof(GeneralTask));
            SeqLogger.Indent++;
            AlternativeShortcuts.Clear();
            for (int i = 0; i < GeneralTask.Alternatives.Count; i++)
            {
                var alternative = GeneralTask.Alternatives[i];
                if (alternative.Tasks.Count > 1)
                {
                    SeqLogger.Trace("Alternative " + alternative + " in process!", nameof(GeneralTask));
                    SeqLogger.Indent++;
                    var shortAlternative = new AlternativeShortcut();                                                                               //Create alternative with shortcut for every alternative
                    shortAlternative.Assimilate(alternative);                                                                     //Init with the original, it is saved inside the new one.
                    shortAlternative.CreateShortcut(GeneralTask.PositionMatrix.DistanceFunction, GeneralTask.PositionMatrix.ResourceFunction);  //Create shortcuts between every position of first and last tasks.

                    DeletedTasks.AddRange(alternative.Tasks);
                    foreach (var task in DeletedTasks)
                    {
                        DeletedPositions.AddRange(task.Positions);
                    }
                    TasksOfShortcuts.Add(shortAlternative.Proxy);
                    PositionOfShortcuts.AddRange(shortAlternative.Proxy.Positions);


                    GeneralTask.Alternatives[i] = shortAlternative;
                    AlternativeShortcuts.Add(shortAlternative);


                    SeqLogger.Indent--;
                }
            }

            foreach (var task in TasksOfShortcuts)
            {
                GeneralTask.Tasks.Add(task);
            }
            foreach (var positions in PositionOfShortcuts)
            {
                GeneralTask.PositionMatrix.Positions.Add(positions);
            }


            foreach (var task in DeletedTasks)
            {
                GeneralTask.Tasks.Remove(task);
            }
            foreach (var positions in DeletedPositions)
            {
                for (int i = 0; i < GeneralTask.PositionMatrix.Positions.Count; i++)
                {
                    if(positions.Node.GlobalID == GeneralTask.PositionMatrix.Positions[i].Node.GlobalID)
                    GeneralTask.PositionMatrix.Positions.RemoveAt(i);
                }
            }

            foreach (var shortcut in AlternativeShortcuts)                                                                                  //Check position precedences, if one of the positions part of shortcut,
            {                                                                                                                               //it have to be override with the first position of the cut
                for (int i = 0; i < GeneralTask.PositionPrecedence.Count; i++)
                {
                    var newPrec = shortcut.FindPrecedenceHeaderOfPositions(GeneralTask.PositionPrecedence[i]);
                    if (newPrec != null && newPrec.Count > 0)
                    {
                        DeletedPositionPrecedencesOfShortcuts.Add(GeneralTask.PositionPrecedence[i]);
                        PositionPrecedencesOfShortcuts.AddRange(newPrec);
                    }
                }
            }

            foreach (var item in DeletedPositionPrecedencesOfShortcuts)
            {
                GeneralTask.PositionPrecedence.Remove(item);
            }

            foreach (var item in PositionPrecedencesOfShortcuts)
            {
                GeneralTask.PositionPrecedence.Add(item);
            }

            for (int i = 0; i < GeneralTask.Processes.Count; i++)
            {
                for (int j = 0; j < GeneralTask.Processes[i].Alternatives.Count; j++)
                {
                    foreach (var shortcut in AlternativeShortcuts)
                    {
                        if (GeneralTask.Processes[i].Alternatives[j].GlobalID == shortcut.GlobalID)
                            GeneralTask.Processes[i].Alternatives[j] = shortcut;
                    }
                }
            }

            SeqLogger.Indent--;
            SeqLogger.Info("Alternative shortcut creation finished!", nameof(GeneralTask));
        }
        public void ChangeBack()
        {

            foreach (var shortcut in AlternativeShortcuts)
            {
                for (int i = 0; i < GeneralTask.Alternatives.Count; i++)                    //Change back alternatives in Alternatives collection to original.
                {
                    if (GeneralTask.Alternatives[i].GlobalID == shortcut.GlobalID)
                        GeneralTask.Alternatives[i] = shortcut.Original;
                }
                foreach (var process in GeneralTask.Processes)                              //Change back alternatives in Processes collection to original.
                {
                    for (int i = 0; i < process.Alternatives.Count; i++)
                    {
                        if (process.Alternatives[i].GlobalID == shortcut.GlobalID)
                            process.Alternatives[i] = shortcut.Original;
                    }
                }
            }

            if (GeneralTask.PositionMatrix != null && GeneralTask.PositionMatrix.Positions != null) {       //Add deleted positions of original alternatives to Positions collection.
                foreach (var position in DeletedPositions)
                {
                    GeneralTask.PositionMatrix.Positions.Add(position);
                }
                GeneralTask.PositionMatrix.StrictSystemEdgeWeights.DeleteAll();              //Delete the edge weight ovverides of shortcuts.
            }         

            for (int i = 0; i < GeneralTask.PositionPrecedence.Count; i++)                                    //Remove precedences of shortcuts
            {
                for (int j = 0; j < PositionPrecedencesOfShortcuts.Count; j++)
                {
                    if (GeneralTask.PositionPrecedence[i].Before.GlobalID == PositionPrecedencesOfShortcuts[j].Before.GlobalID && GeneralTask.PositionPrecedence[i].After.GlobalID == PositionPrecedencesOfShortcuts[j].After.GlobalID)
                        GeneralTask.PositionPrecedence.RemoveAt(i);
                }
            }

            foreach (var posPrec in DeletedPositionPrecedencesOfShortcuts)                                      //Add original precedences
            {
                GeneralTask.PositionPrecedence.Add(posPrec);
            }

            AlternativeShortcuts.Clear();
            DeletedPositions.Clear();
            DeletedPositionPrecedencesOfShortcuts.Clear();
            PositionPrecedencesOfShortcuts.Clear();
        }
        public GeneralTaskResult ResolveSolution(GeneralTaskResult taskResult, GeneralTask.CalculateWeightDelegate calculateWeightFunction)
        {
            SeqLogger.Debug("Solution update by alternative shortcuts started!", nameof(GeneralTask));
            SeqLogger.Indent++;
            foreach (var alternative in AlternativeShortcuts)
            {
                taskResult = alternative.ResolveSolution(taskResult);                                           //If AlternativeShortcut fits for result, replace it.
            }

            taskResult.Calculate();
            taskResult.ResolveHelperStruct();
            taskResult.ResolveCosts(calculateWeightFunction);
            SeqLogger.Indent--;
            SeqLogger.Info("Solution update by alternative shortcuts finished!", nameof(GeneralTask));
            return taskResult;
        }
    }
}