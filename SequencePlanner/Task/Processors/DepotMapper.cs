using SequencePlanner.Helper;
using SequencePlanner.Model.Hierarchy;
using System.Collections.Generic;

namespace SequencePlanner.Task.Processors
{
    public class DepotMapper : ITaskProcessor
    {
        private HierarchyRecord Record { get; set; }

        private GeneralTask Task { get; set; }
        private GeneralTaskResult Result { get; set; }
        private DepotChangeType DepotChangeType { get; set; }
        private Config StartDepotConfig { get; set; }
        private Config FinishDepotConfig { get; set; }
        private Motion StartDepot { get; set; }
        private Motion FinishDepot { get; set; }
        public Motion ORToolsStartDepot { get; set; }
        public Motion ORToolsFinishDepot { get; set; }

        public int ORToolsStartDepotSequenceID { get { if (ORToolsStartDepot is not null) return ORToolsStartDepot.SequenceMatrixID; else return -1; } }
        public int ORToolsFinishDepotSequenceID { get { if (ORToolsFinishDepot is not null) return ORToolsFinishDepot.SequenceMatrixID; else return -1; } }

        public DepotMapper(GeneralTask task)
        {
            Task = task;
        }

        public void Change()
        {
            CreateMotionForDepos();
            if (Task.Cyclic)
                DepotChangeType = DepotChangeType.CyclicStartDepot;
            else
            {
                if (StartDepot is null && FinishDepot is null)
                    DepotChangeType = DepotChangeType.NotCyclicNoDepot;
                if (StartDepot is not null && FinishDepot is null)
                    DepotChangeType = DepotChangeType.NotCyclicOnlyStartDepot;
                if (StartDepot is null && FinishDepot is not null)
                    DepotChangeType = DepotChangeType.NotCyclicOnlyFinishDepot;
                if (StartDepot is not null && FinishDepot is not null)
                    DepotChangeType = DepotChangeType.NotCyclicStartFinishDepot;
            }

            switch (DepotChangeType)
            {
                case DepotChangeType.CyclicStartDepot: CyclicStartDepot(); break;
                case DepotChangeType.NotCyclicNoDepot: NotCyclicNoDepot(); break;
                case DepotChangeType.NotCyclicOnlyStartDepot: NotCyclicOnlyStartDepot(); break;
                case DepotChangeType.NotCyclicOnlyFinishDepot: NotCyclicOnlyFinishDepot(); break;
                case DepotChangeType.NotCyclicStartFinishDepot: NotCyclicStartFinishDepot(); break;
            }
            Task.StartDepot = ORToolsStartDepot;
            Task.FinishDepot = ORToolsFinishDepot;
        }



        public void ChangeBack()
        {
            RemoveMotionForDepots();
            switch (DepotChangeType)
            {
                case DepotChangeType.CyclicStartDepot: CyclicStartDepotReverse(); break;
                case DepotChangeType.NotCyclicNoDepot: NotCyclicNoDepotReverse(); break;
                case DepotChangeType.NotCyclicOnlyStartDepot: NotCyclicOnlyStartDepotReverse(); break;
                case DepotChangeType.NotCyclicOnlyFinishDepot: NotCyclicOnlyFinishDepotReverse(); break;
                case DepotChangeType.NotCyclicStartFinishDepot: NotCyclicStartFinishDepotReverse(); break;
            }
        }

        private void CreateMotionForDepos()
        {
            StartDepotConfig = Task.StartDepotConfig;
            FinishDepotConfig = Task.FinishDepotConfig;
            StartDepot = Task.StartDepot;
            FinishDepot = Task.FinishDepot;
            if (StartDepotConfig != null)
            {
                StartDepot = CreateMotion(StartDepotConfig, "StartDepot");
            }
            if (FinishDepotConfig != null)
            {
                FinishDepot = CreateMotion(FinishDepotConfig, "FinishDepot");
            }
        }

        private void RemoveMotionForDepots()
        {
            if (StartDepot != null)
                Task.Hierarchy.DeleteMotion(StartDepot);
            if (FinishDepot != null)
                Task.Hierarchy.DeleteMotion(FinishDepot);
        }

        public GeneralTaskResult ResolveSolution(GeneralTaskResult result)
        {
            Result = result;
            switch (DepotChangeType)
            {
                case DepotChangeType.CyclicStartDepot: CyclicStartDepotResolve(); break;
                case DepotChangeType.NotCyclicNoDepot: NotCyclicNoDepotResolve(); break;
                case DepotChangeType.NotCyclicOnlyStartDepot: NotCyclicOnlyStartDepotResolve(); break;
                case DepotChangeType.NotCyclicOnlyFinishDepot: NotCyclicOnlyFinishDepotResolve(); break;
                case DepotChangeType.NotCyclicStartFinishDepot: NotCyclicStartFinishDepotResolve(); break;
                default:
                    break;
            }
            return Result;
        }

        private void CyclicStartDepot()
        {
            ORToolsStartDepot = StartDepot;
            ORToolsFinishDepot = null;
        }
        private void NotCyclicNoDepot()
        {
            ORToolsStartDepot = CreateVirtualNode(Task, "VirtualStartAndFinish");
        }
        private void NotCyclicOnlyStartDepot()
        {
            ORToolsStartDepot = StartDepot;
            ORToolsFinishDepot = CreateVirtualNode(Task, "VirtualFinish"); ;
        }
        private void NotCyclicOnlyFinishDepot()
        {
            ORToolsStartDepot = CreateVirtualNode(Task, "VirtualStart");
            ORToolsFinishDepot = StartDepot;
        }
        private void NotCyclicStartFinishDepot()
        {
            ORToolsStartDepot = StartDepot;
            Task.StartDepot = StartDepot;
            ORToolsFinishDepot = FinishDepot;
            Task.FinishDepot = FinishDepot;
        }

        //REVERSE
        private void CyclicStartDepotReverse() { }
        private void NotCyclicNoDepotReverse()
        {
            DeleteVirualNode(Task);
        }
        private void NotCyclicOnlyStartDepotReverse()
        {
            DeleteVirualNode(Task);
        }
        private void NotCyclicOnlyFinishDepotReverse()
        {
            DeleteVirualNode(Task);
        }
        private void NotCyclicStartFinishDepotReverse() { }

        //RESOLVE
        private void CyclicStartDepotResolve() { }
        private void NotCyclicNoDepotResolve()
        {
            //Result.Delete(Result.Solution.Count - 1);
            // Result.Delete(0);
        }
        private void NotCyclicOnlyStartDepotResolve()
        {
            //Result.Delete(Result.SolutionRaw.Count - 1);
        }
        private void NotCyclicOnlyFinishDepotResolve()
        {
            //Result.Delete(0);
        }
        private void NotCyclicStartFinishDepotResolve() { }

        private Motion CreateVirtualNode(GeneralTask task, string name)
        {
            Motion motion = new Motion()
            {
                ID = 999999,
                Name = name,
                Virtual = true
            };

            Model.Hierarchy.Task t = new Model.Hierarchy.Task()
            {
                ID = 999999,
                Name = name,
                Virtual = true
            };

            Alternative alternative = new Alternative()
            {
                ID = 999999,
                Name = name,
                Virtual = true
            };

            Process process = new Process()
            {
                ID = 999999,
                Name = name,
                Virtual = true
            };

            Record = new HierarchyRecord() { Process = process, Alternative = alternative, Task = t, Motion = motion };
            task.Hierarchy.HierarchyRecords.Add(Record);
            task.Hierarchy.Motions.Add(motion);

            return motion;
        }

        private Motion CreateMotion(Config config, string name)
        {
            Motion motion = new Motion()
            {
                ID = 999999,
                Name = name,
                Virtual = true,
                Configs = new List<Config>() { config }
            };

            Model.Hierarchy.Task t = new Model.Hierarchy.Task()
            {
                ID = 999999,
                Name = name,
                Virtual = true
            };

            Alternative alternative = new Alternative()
            {
                ID = 999999,
                Name = name,
                Virtual = true
            };

            Process process = new Process()
            {
                ID = 999999,
                Name = name,
                Virtual = true
            };

            Record = new HierarchyRecord() { Process = process, Alternative = alternative, Task = t, Motion = motion };
            Task.Hierarchy.HierarchyRecords.Add(Record);
            Task.Hierarchy.Motions.Add(motion);
            return motion;
        }

        private void DeleteVirualNode(GeneralTask task)
        {
            task.Hierarchy.HierarchyRecords.Remove(Record);
            task.Hierarchy.Motions.Remove(Record.Motion);
        }
    }
}