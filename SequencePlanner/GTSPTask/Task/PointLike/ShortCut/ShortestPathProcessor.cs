using SequencePlanner.GTSP;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePlanner.GTSPTask.Task.PointLike.ShortCut
{

    public class ShortestPathProcessor
    {
        private PointLikeTask PointLikeTask { get; set; }
        private List<GTSPNode> DeletedPositionsOfShortcuts { get; set; }
        private List<GTSPPrecedenceConstraint> DeletedPositionPrecedencesOfShortcuts { get; set; }
        private List<GTSPPrecedenceConstraint> PositionPrecedencesOfShortcuts { get; set; }
        private List<AlternativeShortcut> AlternativeShortcuts { get; set; }


        public ShortestPathProcessor(PointLikeTask pointTask) {
            PointLikeTask = pointTask;
            DeletedPositionsOfShortcuts = new List<GTSPNode>();
            AlternativeShortcuts = new List<AlternativeShortcut>();
            DeletedPositionPrecedencesOfShortcuts = new List<GTSPPrecedenceConstraint>();
            PositionPrecedencesOfShortcuts = new List<GTSPPrecedenceConstraint>();
        }

        public void Change()
        {
                SeqLogger.Debug("Alternative shortcut creation started!", nameof(PointLikeTask));
                SeqLogger.Indent++;
                AlternativeShortcuts.Clear();
                for (int i = 0; i < PointLikeTask.Alternatives.Count; i++)
                {
                    if (PointLikeTask.Alternatives[i].Tasks.Count > 2)
                    {
                        SeqLogger.Trace("Alternative " + PointLikeTask.Alternatives[i] + " in process!", nameof(PointLikeTask));
                        SeqLogger.Indent++;
                        var shortcut = new AlternativeShortcut();                                                                               //Create alternative with shortcut for every alternative
                        shortcut.Assimilate(PointLikeTask.Alternatives[i]);                                                                     //Init with the original, it is saved inside the new one.
                        shortcut.CreateShortcut(PointLikeTask.PositionMatrix.DistanceFunction, PointLikeTask.PositionMatrix.ResourceFunction);  //Create shortcuts between every position of first and last tasks.
                        for (int j = 1; j < PointLikeTask.Alternatives[i].Tasks.Count - 1; j++)
                        {
                            foreach (var position in PointLikeTask.Alternatives[i].Tasks[j].Positions)
                            {
                            PointLikeTask.PositionMatrix.Positions.Remove(position);
                                DeletedPositionsOfShortcuts.Add(position);
                                SeqLogger.Trace("Deleted position:" + position, nameof(PointLikeTask));
                            }
                        }
                        AlternativeShortcuts.Add(shortcut);
                        PointLikeTask.Alternatives[i] = shortcut;                                                                               //Change the alternatives for the new ones
                        foreach (var process in PointLikeTask.Processes)                                                                        //also in processes.
                        {
                            for (int j = 0; j < process.Alternatives.Count; j++)
                            {
                                if (process.Alternatives[j].GlobalID == shortcut.GlobalID)
                                {
                                    process.Alternatives[j] = shortcut;
                                }
                            }
                        }
                        SeqLogger.Indent--;
                    }
                }
                foreach (var shortcut in AlternativeShortcuts)                                                                                  //Check position precedences, if one of the positions part of shortcut,
                {                                                                                                                               //it have to be override with the first position of the cut
                    for (int i = 0; i < PointLikeTask.PositionPrecedence.Count; i++)
                    {
                        var newPrec = shortcut.FindPrecedenceHeaderOfPositions(PointLikeTask.PositionPrecedence[i]);
                        if (newPrec != null && newPrec.Count > 0)
                        {
                            DeletedPositionPrecedencesOfShortcuts.Add(PointLikeTask.PositionPrecedence[i]);
                            PositionPrecedencesOfShortcuts.AddRange(newPrec);
                            //PositionPrecedence.RemoveAt(i);
                            //PositionPrecedence.AddRange(newPrec);
                        }
                    }
                }
                foreach (var item in DeletedPositionPrecedencesOfShortcuts)
                {
                    PointLikeTask.PositionPrecedence.Remove(item);
                }

                foreach (var item in PositionPrecedencesOfShortcuts)
                {
                    PointLikeTask.PositionPrecedence.Add(item);
                }

                SeqLogger.Indent--;
                SeqLogger.Info("Alternative shortcut creation finished!", nameof(PointLikeTask));
        }

        public void ChangeBack()
        {

            foreach (var shortcut in AlternativeShortcuts)
            {
                for (int i = 0; i < PointLikeTask.Alternatives.Count; i++)                    //Change back alternatives in Alternatives collection to original.
                {
                    if (PointLikeTask.Alternatives[i].GlobalID == shortcut.GlobalID)
                        PointLikeTask.Alternatives[i] = shortcut.Original;
                }
                foreach (var process in PointLikeTask.Processes)                              //Change back alternatives in Processes collection to original.
                {
                    for (int i = 0; i < process.Alternatives.Count; i++)
                    {
                        if (process.Alternatives[i].GlobalID == shortcut.GlobalID)
                            process.Alternatives[i] = shortcut.Original;
                    }
                }
            }

            if (PointLikeTask.PositionMatrix != null && PointLikeTask.PositionMatrix.Positions != null) {       //Add deleted positions of original alternatives to Positions collection.
                foreach (var position in DeletedPositionsOfShortcuts)
                {
                    PointLikeTask.PositionMatrix.Positions.Add(position);
                }
                PointLikeTask.PositionMatrix.DistanceFunction.StrictSystemEdgeWeights.DeleteAll();              //Delete the edge weight ovverides of shortcuts.
            }         

            for (int i = 0; i < PointLikeTask.PositionPrecedence.Count; i++)                                    //Remove precedences of shortcuts
            {
                for (int j = 0; j < PositionPrecedencesOfShortcuts.Count; j++)
                {
                    if (PointLikeTask.PositionPrecedence[i].Before.GlobalID == PositionPrecedencesOfShortcuts[j].Before.GlobalID && PointLikeTask.PositionPrecedence[i].After.GlobalID == PositionPrecedencesOfShortcuts[j].After.GlobalID)
                        PointLikeTask.PositionPrecedence.RemoveAt(i);
                }
            }

            foreach (var posPrec in DeletedPositionPrecedencesOfShortcuts)                                      //Add original precedences
            {
                PointLikeTask.PositionPrecedence.Add(posPrec);
            }

            AlternativeShortcuts.Clear();
            DeletedPositionsOfShortcuts.Clear();
            DeletedPositionPrecedencesOfShortcuts.Clear();
            PositionPrecedencesOfShortcuts.Clear();
        }

        public PointTaskResult ResolveSolution(PointTaskResult taskResult)
        {
            SeqLogger.Debug("Solution update by alternative shortcuts started!", nameof(PointLikeTask));
            SeqLogger.Indent++;
            foreach (var alternative in AlternativeShortcuts)
            {
                taskResult = alternative.ResolveSolution(taskResult);                                           //If AlternativeShortcut fits for result, replace it.
            }
            SeqLogger.Indent--;
            SeqLogger.Info("Solution update by alternative shortcuts finished!", nameof(PointLikeTask));
            return taskResult;
        }
    }
}
