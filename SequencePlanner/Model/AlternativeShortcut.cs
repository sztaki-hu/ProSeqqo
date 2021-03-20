using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.GTSP;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Task.PointLike.ShortCut;
using SequencePlanner.Helper;
using System.Collections.Generic;

namespace SequencePlanner.Model
{
    public class AlternativeShortcut : Alternative
    {
        public Alternative Original { get; private set; }
        public Task FrontProxy { get; set; }
        public Task BackProxy { get; set; }
        public Task Proxy { get; set; }
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
            Original = alternative;
        }

        public void CreateShortcut(IDistanceFunction distanceFunction, IResourceFunction resourceFunction)
        {
            if(Original is not null && Original.Tasks is not null && Original.Tasks.Count > 1)
            {
                FrontProxy = Original.Tasks[0];
                BackProxy = Original.Tasks[Original.Tasks.Count - 1];
                FindShortcuts(distanceFunction, resourceFunction);
                SeqLogger.Trace(CriticalPaths.Count + " shortcut created in [UID:" + Original.UserID + "] alternative, between: " + FrontProxy.ToString() + " and " + BackProxy.ToString(), nameof(AlternativeShortcut));
                Tasks.Add(new Task() { Name = "ShortcutPathOf" + Name });
                foreach (var path in CriticalPaths)
                {
                    Tasks[0].Positions.Add(path.Representer);
                    SeqLogger.Trace("Contains " + path.Cut.Count + " in cut with " + path.Cost + " cost, between: " + path.Front.ToString() + " and " + path.Back.ToString(), nameof(AlternativeShortcut));
                }
                Proxy = Tasks[0];
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
                    if (position.Node.GlobalID == gTSPPrecedenceConstraint.Before.GlobalID)
                        findBefore = true;
                    if (position.Node.GlobalID == gTSPPrecedenceConstraint.After.GlobalID)
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
                        prec.Before = path.Front.Node;
                    else
                        prec.Before = gTSPPrecedenceConstraint.Before;
                    if (findAfter)
                        prec.After = path.Front.Node;
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
                for (int i = 0; i < taskResult.ResolveHelper.Count; i++)
                {
                    if (taskResult.ResolveHelper[i].Node.Node.GlobalID == path.Representer.Node.GlobalID)
                    {
                        SeqLogger.Trace("Cut found [" + path.Front.Node.ToString() + ";" + path.Back.Node.ToString() + "] and changed to [" + SeqLogger.ToList(path.Cut) + "]");
                        for (int j = 0; j < path.Cut.Count-1; j++)
                        {
                            taskResult.ResolveHelper[i].Resolve.Add(path.Cut[j]);
                            taskResult.ResolveHelper[i].ResolveCost.Add(path.Costs[j]);
                        }
                        taskResult.ResolveHelper[i].Resolved = true;
                        taskResult.ResolveHelper[i].Resolve.Add(path.Cut[path.Cut.Count-1]);
                    }
                }
            }
            return taskResult;
        }
    }
}