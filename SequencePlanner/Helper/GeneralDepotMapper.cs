using System;
using System.Collections.Generic;
using SequencePlanner.Model;
using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Task.General;

namespace SequencePlanner.Helper
{
    public class GeneraDepotMapper : IDepotMapper
    {
        private Position Position;
        private GTSPNode GTSPNode;
        private Model.Task TaskNode;
        private Alternative Alternative;
        private Process Process;
  
        private GeneralTask Task { get; set; }
        private GeneralTaskResult Result { get; set; }
        private DepotChangeType DepotChangeType { get; set; }
        private Position StartDepot { get; set; }
        private Position FinishDepot { get; set; }
        public BaseNode ORToolsStartDepot { get; set; }
        public BaseNode ORToolsFinishDepot { get; set; }

        public int ORToolsStartDepotSequenceID { get { if (ORToolsStartDepot is not null) return ORToolsStartDepot.SequencingID; else return -1; } }
        public int ORToolsFinishDepotSequenceID { get { if (ORToolsFinishDepot is not null) return ORToolsFinishDepot.SequencingID; else return -1; } }
        

        public void Change(GeneralTask task)
        {
            Task = (GeneralTask)task;
            StartDepot = task.StartDepot;
            FinishDepot = task.FinishDepot;
            if (task.Cyclic)
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
        }
        public void ChangeBack(GeneralTask task)
        {
            Task = (GeneralTask)task;
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

        private void CyclicStartDepot()
        {
            ORToolsStartDepot = StartDepot;
            ORToolsFinishDepot = null;
        }
        private void NotCyclicNoDepot()
        {
            ORToolsStartDepot = CreateVirtualNode((GeneralTask)Task, "VirtualStartAndFinish");
        }
        private void NotCyclicOnlyStartDepot()
        {
            ORToolsStartDepot = StartDepot;
            ORToolsFinishDepot = CreateVirtualNode((GeneralTask)Task, "VirtualFinish"); ;
        }
        private void NotCyclicOnlyFinishDepot()
        {
            ORToolsStartDepot = CreateVirtualNode((GeneralTask)Task, "VirtualStart");
            ORToolsFinishDepot = StartDepot;
        }
        private void NotCyclicStartFinishDepot()
        {
            ORToolsStartDepot = StartDepot;
            ORToolsFinishDepot = FinishDepot;
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
            Result.Delete(Result.SolutionRaw.Count - 1);
            Result.Delete(0);
        }
        private void NotCyclicOnlyStartDepotResolve()
        {
            Result.Delete(Result.SolutionRaw.Count - 1);
        }
        private void NotCyclicOnlyFinishDepotResolve()
        {
            Result.Delete(0);
        }
        private void NotCyclicStartFinishDepotResolve() { }
        
        private Position CreateVirtualNode(GeneralTask task,string name)
        {
            Position = new Position()
            {
                UserID = 999999,
                Name = name,
                Virtual = true,
            };
            GTSPNode = new GTSPNode(Position)
            {
                OverrideWeightIn = 0,
                OverrideWeightOut = 0
            };

            TaskNode = new Model.Task()
            {
                Name = name,
                Virtual = true,
                Positions = new List<GTSPNode>() { GTSPNode }
            };

            Alternative = new Model.Alternative()
            {
                Name = name,
                Virtual = true,
                Tasks = new List<Model.Task>() { TaskNode }
            };

            Process = new Model.Process()
            {
                Name = name,
                Virtual = true,
                Alternatives = new List<Alternative>() { Alternative }
            };

            task.Processes.Add(Process);
            task.Alternatives.Add(Alternative);
            task.Tasks.Add(TaskNode);
            task.PositionMatrix.Positions.Add(GTSPNode);

            return Position;
        }
        private void DeleteVirualNode(GeneralTask task)
        {
            task.Processes.Remove(Process);
            task.Alternatives.Remove(Alternative);
            task.Tasks.Remove(TaskNode);
            task.PositionMatrix.Positions.Remove(GTSPNode);
        }
        public void OverrideWeights(GeneralGTSPRepresentation task)
        {
            throw new NotImplementedException();
        }
    }
}