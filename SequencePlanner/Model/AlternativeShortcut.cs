using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.GTSP;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Task.PointLike.ShortCut;
using SequencePlanner.Helper;
using System;
using System.Collections.Generic;

namespace SequencePlanner.Model
{
    public class AlternativeShortcut : Alternative
    {
        public Alternative Original { get; private set; }
        public Task FrontProxy { get; set; }
        public Task BackProxy { get; set; }
        public List<ShortestPath> CriticalPaths{get;set;}
        public StrictEdgeWeightSet StrictSystemEdgeWeightSet { get; set; }

        public AlternativeShortcut():base()
        {
            Name = UserID + "_Alternative_" + GlobalID;
            Tasks = new List<Task>();
            CriticalPaths = new List<ShortestPath>();
            StrictSystemEdgeWeightSet = new StrictEdgeWeightSet();
        }
        
        public void Assimilate(Alternative alternative)
        {
            this.GlobalID = alternative.GlobalID;
            this.UserID = alternative.UserID;
            this.SequencingID = alternative.SequencingID;
            this.ResourceID = alternative.ResourceID;
            this.Name = alternative.Name;
            this.Virtual = alternative.Virtual;
            //this.Tasks = alternative.Tasks;
            Original = alternative;
        }

        public void CreateShortcut(IDistanceFunction distanceFunction, IResourceFunction resourceFunction)
        {
            if(Original is not null && Original.Tasks is not null && Original.Tasks.Count > 1)
            {
                FrontProxy = Original.Tasks[0];
                BackProxy = Original.Tasks[Original.Tasks.Count - 1];
                Tasks.Add(FrontProxy);
                Tasks.Add(BackProxy);
                FindShortcuts(distanceFunction, resourceFunction);
                
                SeqLogger.Trace(CriticalPaths.Count + " shortcut created in [UID:" + Original.UserID + "] alternative, between: " + FrontProxy.ToString() + " and " + BackProxy.ToString(), nameof(AlternativeShortcut));
                foreach (var path in CriticalPaths)
                {
                    distanceFunction.StrictSystemEdgeWeights.Add(new StrictEdgeWeight(path.Front, path.Back, path.Cost));
                    SeqLogger.Trace("Contains " + path.Cut.Count + " in cut with " + path.Cost + " cost, between: " + path.Front.ToString() + " and " + path.Back.ToString(), nameof(AlternativeShortcut));
                }
            }
            
        }

        private void FindShortcuts(IDistanceFunction distanceFunction, IResourceFunction resourceFunction)
        {
            ShortestPathSearch cpm = new ShortestPathSearch(Original.Tasks, distanceFunction, resourceFunction);
            foreach (var openPos in FrontProxy.Positions)
            {
                CriticalPaths.AddRange(cpm.CalculateCriticalRoute(openPos, BackProxy.Positions));
            }
        }

        public List<GTSPPrecedenceConstraint> FindPrecedenceHeaderOfPositions(GTSPPrecedenceConstraint gTSPPrecedenceConstraint)
        {
            var newPrecedences = new List<GTSPPrecedenceConstraint>();
            var findBefore = false;
            var findAfter = false;
            foreach (var path in CriticalPaths)
            {
                foreach (var position in path.Cut)
                {
                    if (position.GlobalID == gTSPPrecedenceConstraint.Before.GlobalID)
                        findBefore = true;
                    if (position.GlobalID == gTSPPrecedenceConstraint.After.GlobalID)
                        findAfter = true;
                }
                if (findBefore && findAfter)
                {
                    SeqLogger.Error("Position precedence found where before and after is in the same alternative.");
                    throw new SeqException("Position precedence inside the an alternative!", "Remove precedence with these userids: [" + gTSPPrecedenceConstraint.Before.UserID + ";" + gTSPPrecedenceConstraint.After.UserID + "]");
                }

                if (findBefore || findAfter)
                {
                    var prec = new GTSPPrecedenceConstraint();
                    if (findBefore)
                        prec.Before = path.Front;
                    else
                        prec.Before = gTSPPrecedenceConstraint.Before;
                    if (findAfter)
                        prec.After = path.Front;
                    else
                        prec.After = gTSPPrecedenceConstraint.After;

                    var containsNewPrec = false;
                    foreach (var precedence in newPrecedences)
                    {
                        if ((precedence.Before.GlobalID == prec.Before.GlobalID) && (precedence.After.GlobalID == prec.After.GlobalID))
                            containsNewPrec = true;
                    }
                    if (!containsNewPrec)
                    {
                        newPrecedences.Add(prec);
                        SeqLogger.Trace("Position precendence "+gTSPPrecedenceConstraint +" changed to "+ prec, nameof(AlternativeShortcut) );
                        containsNewPrec = false;
                    }
                }
                findBefore = false;
                findAfter = false;
            }
            return newPrecedences;
        }

        public PointTaskResult ResolveSolution(PointTaskResult taskResult)
        {
            foreach (var path in CriticalPaths)
            {
                for (int i = 0; i < taskResult.PositionResult.Count-1; i++)
                {
                    if ((path.Front.GlobalID == taskResult.PositionResult[i].GlobalID) && (path.Back.GlobalID == taskResult.PositionResult[i + 1].GlobalID))
                    {
                        SeqLogger.Trace("Cut found [" + path.Front.UserID + ";" + path.Back.UserID+"] and changed to ["+SeqLogger.ToList(path.Cut)+"]");
                        for (int j = 1; j < path.Cut.Count-1; j++)
                        {
                            taskResult.PositionResult.Insert(i+j, (Position) path.Cut[j]);
                            taskResult.SolutionRaw.Insert(i+j, (long) path.Cut[j].UserID);
                        }
                        SeqLogger.Trace("Costs from " + taskResult.CostsRaw[i] + " hanged to [" + SeqLogger.ToList(path.Costs)+"]" );
                        taskResult.CostsRaw.RemoveAt(i);
                        for (int j = 0; j < path.Costs.Count; j++)
                        {
                            taskResult.CostsRaw.Insert(i + j, (long)path.Costs[j]);
                        }
                        taskResult.Calculate();
                    }
                }
            }
            return taskResult;
        }
    }
}