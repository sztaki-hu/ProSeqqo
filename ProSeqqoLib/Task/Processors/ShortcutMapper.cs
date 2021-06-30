using ProSeqqoLib.Helper;
using ProSeqqoLib.Model.Hierarchy;
using ProSeqqoLib.Model.ShortestPath;
using System.Collections.Generic;

namespace ProSeqqoLib.Task.Processors
{
    public class ShortcutMapper : ITaskProcessor
    {
        private static int NewTaskIDMax = 9000000;
        public GeneralTask Task { get; set; }
        public List<HierarchyRecord> OldRecords { get; set; }
        public List<HierarchyRecord> NewRecords { get; set; }
        public List<ShortAlernative> ShortAlternatives { get; set; }

        public ShortcutMapper(GeneralTask task)
        {
            Task = task;
            OldRecords = new List<HierarchyRecord>();
            NewRecords = new List<HierarchyRecord>();
            ShortAlternatives = new List<ShortAlernative>();
        }

        public void Change()
        {
            if (!Task.SolverSettings.UseShortcutInAlternatives)
                return;
            else
            {
                CreateShortAlternatives();
                ChangeRecords();
                foreach (var item in ShortAlternatives)
                {
                    item.GetShortestPath();
                }
                foreach (var item in ShortAlternatives)
                {
                    foreach (var a in item.GetShortestPath())
                        SeqLogger.Trace(a.ProxyMotion + " Cost: " + a.ProxyMotion.ShortcutCost);
                }
            }
        }

        public void ChangeBack()
        {
            if (!Task.SolverSettings.UseShortcutInAlternatives)
                return;
            else
            {
                ChangeBackRecords();
            }
        }

        public GeneralTaskResult ResolveSolution(GeneralTaskResult generalTaskResult)
        {
            if (!Task.SolverSettings.UseShortcutInAlternatives)
                return generalTaskResult;
            else
            {
                for (int i = 0; i < generalTaskResult.SolutionMotion.Count; i++)
                {
                    for (int j = 0; j < ShortAlternatives.Count; j++)
                    {
                        foreach (var path in ShortAlternatives[j].GetShortestPath())
                        {
                            if (generalTaskResult.SolutionMotion[i].GlobalID == path.ProxyMotion.GlobalID)
                            {
                                generalTaskResult.SolutionMotion.Remove(path.ProxyMotion);
                                SeqLogger.Trace("Removed from solution: " + path.ProxyMotion);
                                var offset = 0;
                                foreach (var item in path.Path)
                                {
                                    generalTaskResult.SolutionMotion.Insert(i+offset, item);
                                    offset++;
                                    SeqLogger.Trace("Added to solution: " + item);
                                }
                            }
                        }
                    }
                }
                return generalTaskResult;
            }
        }

        private void CreateShortAlternatives()
        {
            var lastAlterID = -1;
            var lastTaskID = -1;
            var numberOfTasks = 0;
            Alternative lastAlter = null;
            List<HierarchyRecord> records = new List<HierarchyRecord>();
            foreach (var record in Task.Hierarchy.HierarchyRecords)
            {
                if (lastAlterID != record.Alternative.GlobalID)
                {
                    if (lastAlterID != -1)
                    {
                        if (records.Count > 1 && numberOfTasks > 1)
                            ShortAlternatives.Add(new ShortAlernative(lastAlter, records, Task.CostManager));
                    }
                    lastTaskID = record.Task.GlobalID;
                    lastAlter = record.Alternative;
                    lastAlterID = record.Alternative.GlobalID;
                    records = new List<HierarchyRecord>();
                    records.Add(record);
                    numberOfTasks = 1;
                }
                else
                {
                    if (lastTaskID != record.Task.GlobalID)
                        numberOfTasks++;
                    records.Add(record);
                }
            }
        }

        private void ChangeRecords()
        {
            var find = false;
            foreach (var alternative in ShortAlternatives)
            {
                find = false;
                foreach (var record in Task.Hierarchy.HierarchyRecords)
                {
                    if (alternative.Alternative.GlobalID == record.Alternative.GlobalID)
                    {
                        if (!find)
                        {
                            find = true;
                            var t = new Model.Hierarchy.Task() { ID = NewTaskIDMax++, Name = "ShortcutProxyTask" };
                            foreach (var path in alternative.GetShortestPath())
                            {
                                NewRecords.Add(new HierarchyRecord()
                                {
                                    Process = record.Process,
                                    Alternative = alternative,
                                    Task = t,
                                    Motion = path.ProxyMotion
                                });
                                SeqLogger.Trace("New Record: " + NewRecords[^1]);
                            }
                        }
                        OldRecords.Add(record);
                        SeqLogger.Trace("Old Record: " + OldRecords[^1]);
                    }
                }
            }

            //Delete replaced records and motions
            foreach (var oldRecord in OldRecords)
            {
                Task.Hierarchy.HierarchyRecords.Remove(oldRecord);
                Task.Hierarchy.Motions.Remove(oldRecord.Motion);
            }

            //Add new records and proxy motions
            foreach (var newRecord in NewRecords)
            {
                Task.Hierarchy.HierarchyRecords.Add(newRecord);
                Task.Hierarchy.Motions.Add(newRecord.Motion);
            }
            //Rebuild hierarchy
            Task.Hierarchy.Build();
        }

        private void ChangeBackRecords()
        {
            //Remove proxy records and motions
            foreach (var newRecord in NewRecords)
            {
                Task.Hierarchy.HierarchyRecords.Remove(newRecord);
                Task.Hierarchy.Motions.Remove(newRecord.Motion);
            }

            //Put back original records and motions
            foreach (var oldRecord in OldRecords)
            {
                Task.Hierarchy.HierarchyRecords.Add(oldRecord);
                Task.Hierarchy.Motions.Add(oldRecord.Motion);
            }

            //Rebuild hierarchy
            Task.Hierarchy.Build();
        }
    }
}