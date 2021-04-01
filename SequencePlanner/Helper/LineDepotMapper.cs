using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Task.Base;
using SequencePlanner.GTSPTask.Task.LineLike;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePlanner.Helper
{
    public class LineDepotMapper: IDepotMapper
    {
        private Position StartDepot { get; set; }
        private Position FinishDepot { get; set; }
        public Position ORToolsStartDepot { get; set; }
        public Position ORToolsFinishDepot { get; set; }
        public int ORToolsStartDepotSequenceID { get { if (ORToolsStartDepot is not null) return ORToolsStartDepot.SequencingID; else return -1; } }
        public int ORToolsFinishDepotSequenceID { get { if (ORToolsFinishDepot is not null) return ORToolsFinishDepot.SequencingID; else return -1; } }
        private DepotChangeType DepotChangeType { get; set; }
        private IBaseTask Task { get; set; }
        
        public void Map(BaseTask task)
        {
            Task = task;
            StartDepot = task.StartDepot;
            FinishDepot = task.FinishDepot;
            if (task.CyclicSequence)
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

        public void ReverseMap(BaseTask task)
        {
            Task = task;
            switch (DepotChangeType)
            {
                case DepotChangeType.CyclicStartDepot: CyclicStartDepotReverse(); break;
                case DepotChangeType.NotCyclicNoDepot: NotCyclicNoDepotReverse((LineLikeTask)task); break;
                case DepotChangeType.NotCyclicOnlyStartDepot: NotCyclicOnlyStartDepotReverse((LineLikeTask)task); break;
                case DepotChangeType.NotCyclicOnlyFinishDepot: NotCyclicOnlyFinishDepotReverse((LineLikeTask)task); break;
                case DepotChangeType.NotCyclicStartFinishDepot: NotCyclicStartFinishDepotReverse(); break;
            }
        }

        public TaskResult ResolveSolution(TaskResult result)
        {
            switch (DepotChangeType)
            {
                case DepotChangeType.CyclicStartDepot: return CyclicStartDepotResolve(result);
                case DepotChangeType.NotCyclicNoDepot: return NotCyclicNoDepotResolve(result); 
                case DepotChangeType.NotCyclicOnlyStartDepot: return NotCyclicOnlyStartDepotResolve(result); 
                case DepotChangeType.NotCyclicOnlyFinishDepot: return NotCyclicOnlyFinishDepotResolve(result); 
                case DepotChangeType.NotCyclicStartFinishDepot: return NotCyclicStartFinishDepotResolve(result); 
            }
            return null;
        }

        private void CyclicStartDepot()
        {
            ORToolsStartDepot = StartDepot;
            ORToolsFinishDepot = null;
        }

        private void NotCyclicNoDepot()
        {
            ORToolsStartDepot = CreateVirtualNode((LineLikeTask)Task, "VirtualStartAndFinish");
        }
        private void NotCyclicOnlyStartDepot()
        {
            ORToolsStartDepot = StartDepot;
            ORToolsFinishDepot = CreateVirtualNode((LineLikeTask)Task, "VirtualFinish"); ;
        }
        private void NotCyclicOnlyFinishDepot()
        {
            ORToolsStartDepot = CreateVirtualNode((LineLikeTask)Task, "VirtualStart");
            ORToolsFinishDepot = StartDepot;
        }
        private void NotCyclicStartFinishDepot()
        {
            ORToolsStartDepot = StartDepot;
            ORToolsFinishDepot = FinishDepot;
        }

        //REVERSE
        private void CyclicStartDepotReverse()
        {
        }

        private void NotCyclicNoDepotReverse(LineLikeTask task)
        {
            DeleteVirualNode(task);
        }
        private void NotCyclicOnlyStartDepotReverse(LineLikeTask task)
        {
            DeleteVirualNode(task);
        }
        private void NotCyclicOnlyFinishDepotReverse(LineLikeTask task)
        {
            DeleteVirualNode(task);
        }
        private void NotCyclicStartFinishDepotReverse()
        {

        }

        //RESOLVE
        private TaskResult CyclicStartDepotResolve(TaskResult result)
        {

            return result;
        }

        private TaskResult NotCyclicNoDepotResolve(TaskResult result)
        {
            result.Delete(result.SolutionRaw.Count - 1);
            result.Delete(0);
            return result;
        }

        private TaskResult NotCyclicOnlyStartDepotResolve(TaskResult result)
        {
            result.Delete(result.SolutionRaw.Count - 1);
            return result;
        }

        private TaskResult NotCyclicOnlyFinishDepotResolve(TaskResult result)
        {
            result.Delete(0);
            return result;
        }

        private TaskResult NotCyclicStartFinishDepotResolve(TaskResult result)
        {
            return result;
        }

        private Position Position;
        private GTSPNode GTSPNode;
        private Model.Task TaskNode;
        private Alternative Alternative;
        private Process Process;
        
        private Position CreateVirtualNode(LineLikeTask task,string name)
        {
            Position = new Position()
            {
                UserID = 999999,
                Name = name,
                Virtual = true,
            };
            GTSPNode = new GTSPNode(Position);
            GTSPNode.OverrideWeightIn = 0;
            GTSPNode.OverrideWeightOut = 0;

            TaskNode = new Model.Task()
            {
                Name = name,
                Virtual = true,
                Positions = new List<GTSPNode>() { GTSPNode }
            };

            Alternative = new Alternative()
            {
                Name = name,
                Virtual = true,
                Tasks = new List<Model.Task>() { TaskNode }
            };

            Process = new Process()
            {
                Name = name,
                Virtual = true,
                Alternatives = new List<Alternative>() { Alternative }
            };


            //task.Processes.Add(Process);
            //task.Alternatives.Add(Alternative);
            //task.Tasks.Add(TaskNode);
            task.PositionMatrix.Positions.Add(GTSPNode);

            return Position;
        }

        private void DeleteVirualNode(LineLikeTask task)
        {
            //task.Processes.Remove(Process);
            //task.Alternatives.Remove(Alternative);
            //task.Tasks.Remove(TaskNode);
            task.PositionMatrix.Positions.Remove(GTSPNode);
        }
    }
}
