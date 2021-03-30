using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Task.Base;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePlanner.Helper
{
    public class DepotMapper
    {
        private Position StartDepot { get; set; }
        private Position FinishDepot { get; set; }
        public Position ORToolsStartDepot { get; set; }
        private DepotChangeType DepotChangeType { get; set; }
        private BaseTask Task { get; set; }
        
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
                case DepotChangeType.NotCyclicNoDepot: NotCyclicNoDepotReverse(); break;
                case DepotChangeType.NotCyclicOnlyStartDepot: NotCyclicOnlyStartDepotReverse(); break;
                case DepotChangeType.NotCyclicOnlyFinishDepot: NotCyclicOnlyFinishDepotReverse(); break;
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
        }

        private void NotCyclicNoDepot()
        {
            //Create virual position, in a task, alternative, process.
        }
        private void NotCyclicOnlyStartDepot()
        {
            //Create virual position, in a task, alternative, process.
            //Set this virtual position as startup position
            //Set matrix edge weights to zero, from any last task's positions to virtual position
            //Set matrix edge weights to zero, from virtual position to any first task's positions 
            //Set other matrix edge weights to infinity
        }
        private void NotCyclicOnlyFinishDepot()
        {
            //Create virual position, in a task, alternative, process.
            //Set this virtual position as startup position
            //Set matrix edge weights to zero, from any last task's positions to virtual position
            //Set matrix edge weights to zero, from virtual position to any first task's positions 
            //Set other matrix edge weights to infinity

        }
        private void NotCyclicStartFinishDepot()
        {

        }

        //REVERSE
        private void CyclicStartDepotReverse()
        {
            //Do nothing
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
        private TaskResult CyclicStartDepotResolve(TaskResult result)
        {
            return result;
        }

        private TaskResult NotCyclicNoDepotResolve(TaskResult result)
        {
            return result;
        }
        private TaskResult NotCyclicOnlyStartDepotResolve(TaskResult result)
        {
            return result;
        }
        private TaskResult NotCyclicOnlyFinishDepotResolve(TaskResult result)
        {
            return result;
        }
        private TaskResult NotCyclicStartFinishDepotResolve(TaskResult result)
        {
            return result;
        }
    }

    enum DepotChangeType
    {
        CyclicStartDepot,
        NotCyclicNoDepot,
        NotCyclicOnlyStartDepot,
        NotCyclicOnlyFinishDepot,
        NotCyclicStartFinishDepot
    }
}
