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
        public Line ORToolsStartDepot { get; set; }
        public Line ORToolsFinishDepot { get; set; }
        public int ORToolsStartDepotSequenceID { get { if (ORToolsStartDepot is not null) return ORToolsStartDepot.SequencingID; else return -1; } }
        public int ORToolsFinishDepotSequenceID { get { if (ORToolsFinishDepot is not null) return ORToolsFinishDepot.SequencingID; else return -1; } }
        private DepotChangeType DepotChangeType { get; set; }
        private LineLikeTask Task { get; set; }
        
        public void Map(BaseTask task)
        {
            Task = (LineLikeTask)task;
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
            Task = (LineLikeTask)task;
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
            //switch (DepotChangeType)
            //{
            //    case DepotChangeType.CyclicStartDepot: return CyclicStartDepotResolve(result);
            //    case DepotChangeType.NotCyclicNoDepot: return NotCyclicNoDepotResolve(result); 
            //    case DepotChangeType.NotCyclicOnlyStartDepot: return NotCyclicOnlyStartDepotResolve(result); 
            //    case DepotChangeType.NotCyclicOnlyFinishDepot: return NotCyclicOnlyFinishDepotResolve(result); 
            //    case DepotChangeType.NotCyclicStartFinishDepot: return NotCyclicStartFinishDepotResolve(result); 
            //}
            //return null;
            result.DeleteFirst();
            result.DeleteLast();
            return result;
        }

        private void CyclicStartDepot()
        {
            StartLine = CreateVirtualLine(StartDepot, StartDepot, "VirtualStart");
            Task.Lines.Add(StartLine);
            ORToolsStartDepot = StartLine;
            ORToolsFinishDepot = null;
        }

        private void NotCyclicNoDepot()
        {
            StartLine = CreateVirtualLine(StartDepot, StartDepot, "VirtualStartAndFinish");
            Task.Lines.Add(StartLine);
            ORToolsStartDepot = StartLine;
            ORToolsFinishDepot = null;
        }
        private void NotCyclicOnlyStartDepot()
        {
            StartLine = CreateVirtualLine(StartDepot, StartDepot, "VirtualStartAndFinish");
            Task.Lines.Add(StartLine);
            ORToolsStartDepot = StartLine;
            ORToolsFinishDepot = null;
        }
        private void NotCyclicOnlyFinishDepot()
        {
            FinishLine = CreateVirtualLine(FinishDepot, FinishDepot, "VirtualStartAndFinish");
            Task.Lines.Add(FinishLine);
            ORToolsStartDepot = null;
            ORToolsFinishDepot = FinishLine;
        }
        private void NotCyclicStartFinishDepot()
        {
            StartLine = CreateVirtualLine(StartDepot, FinishDepot, "VirtualStartAndFinish");
            Task.Lines.Add(StartLine);
            ORToolsStartDepot = StartLine;
            ORToolsFinishDepot = null;
        }

        //REVERSE
        private void CyclicStartDepotReverse()
        {
            Task.Lines.Remove(StartLine);
        }
        private void NotCyclicNoDepotReverse(LineLikeTask task)
        {
            Task.Lines.Remove(StartLine);
        }
        private void NotCyclicOnlyStartDepotReverse(LineLikeTask task)
        {
            Task.Lines.Remove(StartLine);
        }
        private void NotCyclicOnlyFinishDepotReverse(LineLikeTask task)
        {
            Task.Lines.Remove(FinishLine);
        }
        private void NotCyclicStartFinishDepotReverse()
        {
            Task.Lines.Remove(StartLine);
        }

        ////RESOLVE
        //private TaskResult CyclicStartDepotResolve(TaskResult result)
        //{
        //    result.DeleteFirst();
        //    result.DeleteLast();
        //    return result;
        //}

        //private TaskResult NotCyclicNoDepotResolve(TaskResult result)
        //{
        //    result.DeleteFirst();
        //    result.DeleteLast();
        //    return result;
        //}

        //private TaskResult NotCyclicOnlyStartDepotResolve(TaskResult result)
        //{
        //    result.DeleteFirst();
        //    result.DeleteLast();
        //    return result;
        //}

        //private TaskResult NotCyclicOnlyFinishDepotResolve(TaskResult result)
        //{
        //    result.DeleteFirst();
        //    result.DeleteLast();
        //    return result;
        //}

        //private TaskResult NotCyclicStartFinishDepotResolve(TaskResult result)
        //{
        //    result.DeleteFirst();
        //    result.DeleteLast();
        //    return result;
        //}

        private Position Position;
        private GTSPNode GTSPNode;
        private Model.Task TaskNode;
        private Alternative Alternative;
        private Process Process;
        private Line StartLine;
        private Line FinishLine;
        
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

        private Line CreateVirtualLine(Position start, Position finish, string name)
        {
           return new Line()
            {
                UserID = 99999,
                Name = name,
                Bidirectional = false,
                NodeA = start,
                NodeB = finish,
                ResourceID = start.ResourceID
            };
        }

        private void DeleteVirualNode(LineLikeTask task)
        {
            //task.Processes.Remove(Process);
            //task.Alternatives.Remove(Alternative);
            //task.Tasks.Remove(TaskNode);
            task.PositionMatrix.Positions.Remove(GTSPNode);
        }

        public void OverrideWeights(BaseTask task)
        {
            Task = (LineLikeTask)task;
            switch (DepotChangeType)
            {
                case DepotChangeType.CyclicStartDepot:break;
                case DepotChangeType.NotCyclicNoDepot:
                    for (int i = 0; i < Task.Lines.Count; i++)
                    {
                        Task.PositionMatrix.Matrix[i, ORToolsStartDepotSequenceID] = 0;
                        Task.PositionMatrix.Matrix[ORToolsStartDepotSequenceID,i] = 0;
                    }
                    break;
                case DepotChangeType.NotCyclicOnlyStartDepot:
                    for (int i = 0; i < Task.Lines.Count; i++)
                    {
                        Task.PositionMatrix.Matrix[i, ORToolsStartDepotSequenceID] = 0;
                    }
                    break;
                case DepotChangeType.NotCyclicOnlyFinishDepot:
                    for (int i = 0; i < Task.Lines.Count; i++)
                    {
                        Task.PositionMatrix.Matrix[ORToolsStartDepotSequenceID,i] = 0;
                    }
                    break;
                case DepotChangeType.NotCyclicStartFinishDepot:
                    break;
            }
        }
    }
}
