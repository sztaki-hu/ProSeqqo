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

        public DepotMapper(GeneralTask task)
        {
            Task = task;
        }

        public void Change()
        {
            StartDepotConfig = Task.StartDepotConfig;
            FinishDepotConfig = Task.FinishDepotConfig;
            if (Task.Cyclic)
                DepotChangeType = DepotChangeType.CyclicStartDepot;
            else
            {
                if (StartDepotConfig is null && FinishDepotConfig is null)
                    DepotChangeType = DepotChangeType.NotCyclicNoDepot;
                if (StartDepotConfig is not null && FinishDepotConfig is null)
                    DepotChangeType = DepotChangeType.NotCyclicOnlyStartDepot;
                if (StartDepotConfig is null && FinishDepotConfig is not null)
                    DepotChangeType = DepotChangeType.NotCyclicOnlyFinishDepot;
                if (StartDepotConfig is not null && FinishDepotConfig is not null)
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
            Task.StartDepot = StartDepot;
            Task.FinishDepot = FinishDepot;
        }

        public void ChangeBack()
        {
            switch (DepotChangeType)
            {
                case DepotChangeType.CyclicStartDepot: CyclicStartDepotReverse(); break;
                case DepotChangeType.NotCyclicNoDepot: NotCyclicNoDepotReverse(); break;
                case DepotChangeType.NotCyclicOnlyStartDepot: NotCyclicOnlyStartDepotReverse(); break;
                case DepotChangeType.NotCyclicOnlyFinishDepot: NotCyclicOnlyFinishDepotReverse(); break;
                case DepotChangeType.NotCyclicStartFinishDepot: NotCyclicStartFinishDepotReverse(); break;
            }
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

        //CHANGE
        private void CyclicStartDepot()
        {
            StartDepot = CreateMotion(StartDepotConfig, 9999999, "StartDepot");
            FinishDepot = null;
        }

        private void NotCyclicNoDepot()
        {
            StartDepot = CreateVirtualMotion(9999999, "VirtualStartDepot");
            FinishDepot = CreateVirtualMotion(8888888, "VirtualFinishDepot");
        }

        private void NotCyclicOnlyStartDepot()
        {
            StartDepot = CreateMotion(StartDepotConfig, 9999999, "StartDepot");
            FinishDepot = CreateVirtualMotion(8888888, "VirtualFinishDepot");
        }
        private void NotCyclicOnlyFinishDepot()
        {
            StartDepot = CreateVirtualMotion(9999999, "VirtualStartDepot");
            FinishDepot = CreateMotion(FinishDepotConfig, 8888888, "FinishDepot");
        }
        private void NotCyclicStartFinishDepot()
        {
            StartDepot = CreateMotion(StartDepotConfig, 9999999, "StartDepot");
            FinishDepot = CreateMotion(FinishDepotConfig, 8888888, "FinishDepot");
            
        }

        //REVERSE
        private void CyclicStartDepotReverse() {
            
        }
        private void NotCyclicNoDepotReverse()
        {
            
        }
        private void NotCyclicOnlyStartDepotReverse()
        {
        }
        private void NotCyclicOnlyFinishDepotReverse()
        {
        }
        private void NotCyclicStartFinishDepotReverse()
        {
        }

        //RESOLVE
        private void CyclicStartDepotResolve(){
        
        }
        private void NotCyclicNoDepotResolve()
        {

        }
        private void NotCyclicOnlyStartDepotResolve()
        {

        }
        private void NotCyclicOnlyFinishDepotResolve()
        {

        }
        private void NotCyclicStartFinishDepotResolve()
        {

        }

        private Motion CreateMotion(Config config, int id, string name)
        {
            Motion motion = new Motion()
            {
                ID = id,
                Name = name,
                Virtual = true,
                Configs = new List<Config>() { config }
            };

            Model.Hierarchy.Task t = new Model.Hierarchy.Task()
            {
                ID = id,
                Name = name,
                Virtual = true
            };

            Alternative alternative = new Alternative()
            {
                ID = id,
                Name = name,
                Virtual = true
            };

            Process process = new Process()
            {
                ID = id,
                Name = name,
                Virtual = true
            };

            Record = new HierarchyRecord() { Process = process, Alternative = alternative, Task = t, Motion = motion };
            Task.Hierarchy.HierarchyRecords.Add(Record);
            Task.Hierarchy.Motions.Add(motion);
            return motion;
        }

        private Motion CreateVirtualMotion(int id, string name)
        {
            Config c = new Config(id, new List<double>())
            {
                Virtual = true,
                Name = name,
            };

            Motion motion = new Motion()
            {
                ID = id,
                Name = name,
                Virtual = true,
                Configs = new List<Config>() { c }
            };

            Model.Hierarchy.Task t = new Model.Hierarchy.Task()
            {
                ID = id,
                Name = name,
                Virtual = true
            };

            Alternative alternative = new Alternative()
            {
                ID = id,
                Name = name,
                Virtual = true
            };

            Process process = new Process()
            {
                ID = id,
                Name = name,
                Virtual = true
            };

            Record = new HierarchyRecord() { Process = process, Alternative = alternative, Task = t, Motion = motion };
            Task.Hierarchy.HierarchyRecords.Add(Record);
            Task.Hierarchy.Motions.Add(motion);
            return motion;
        }
    }
}