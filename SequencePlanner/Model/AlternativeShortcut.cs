﻿using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.Function.ResourceFunction;
using SequencePlanner.Helper;
using System.Collections.Generic;

namespace SequencePlanner.Model
{
    public class AlternativeShortcut : Alternative
    {
        public Alternative Original { get; set; }
        public Task FrontProxy { get; set; }
        public List<CriticalPath> CriticalPaths{get;set;}
        public Task BackProxy { get; set; }

        public AlternativeShortcut():base()
        {
            Name = UserID + "_Alternative_" + GlobalID;
            Tasks = new List<Task>();
            CriticalPaths = new List<CriticalPath>();
        }
        
        public void Assimilate(Alternative alternative)
        {
            this.GlobalID = alternative.GlobalID;
            this.UserID = alternative.UserID;
            this.SequencingID = alternative.SequencingID;
            this.ResourceID = alternative.ResourceID;
            this.Name = alternative.Name;
            this.Virtual = alternative.Virtual;
            this.Tasks = alternative.Tasks;
            Original = alternative;
        }

        public void CreateShortcut(IDistanceFunction distanceFunction, IResourceFunction resourceFunction)
        {
            if(Original.Tasks.Count > 1)
            {
                FrontProxy = Original.Tasks[0];
                BackProxy = Original.Tasks[Original.Tasks.Count - 1];
                FindShortcuts(distanceFunction,resourceFunction);
            }
            SeqLogger.Trace(CriticalPaths.Count+" shortcut created in [UID:"+Original.UserID+"] alternative, between: "+FrontProxy.ToString()+" and "+ BackProxy.ToString(), nameof(AlternativeShortcut));
        }

        private void FindShortcuts(IDistanceFunction distanceFunction, IResourceFunction resourceFunction)
        {
            CPM cpm = new CPM(Original.Tasks, distanceFunction, resourceFunction);
            foreach (var openPos in FrontProxy.Positions)
            {
                CriticalPaths.AddRange(cpm.CalculateCriticalRoute(openPos, BackProxy.Positions));
            }

        }

        public double[,] OverrideWeights(double[,] matrix)
        {
            return matrix;
        }
    }
}