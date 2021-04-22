using System.Collections.Generic;
using System.Linq;

namespace SequencePlanner.GeneralModels
{
    public class Hierarchy
    {
        public List<Motion> Motions { get; set; }
        public List<Config> Configs { get; set; }
        public List<HierarchyRecord> HierarchyRecords { get; set; }
        public bool BidirectionalMotionDefault { get; set; }

        public Hierarchy()
        {
            Motions = new List<Motion>();
            Configs = new List<Config>();
            HierarchyRecords = new List<HierarchyRecord>();
            BidirectionalMotionDefault = false;
        }

        public HierarchyRecord GetRecordByMotionID(int MotionID)
        {
            return HierarchyRecords.Where(r => r.Motion.ID == MotionID).FirstOrDefault();
        }

        //Process
        public List<Process> GetProcesses()
        {
            return HierarchyRecords.Select(r => r.Process).Distinct().ToList();
        }

        public Process GetProcessByID(int id)
        {
            return HierarchyRecords.Where(r => r.Process.ID==id).Select(r => r.Process).FirstOrDefault();
        }

        public List<Alternative> GetAlternativesOf(Process process)
        {
            return HierarchyRecords.Where(r => r.Process.GlobalID==process.GlobalID)
                                   .Select(r => r.Alternative)
                                   .ToList();
        }

        public List<Task> GetTasksOf(Process process)
        {
            return HierarchyRecords.Where(r => r.Process.GlobalID == process.GlobalID)
                                   .Select(r => r.Task)
                                   .OrderBy(r => r.ID)
                                   .ToList();
        }

        public List<Task> GetFirstTasksOfAlternativesInProcesses(Process process)
        {
            var tasks = new List<Task>();
            foreach (var alternative in GetAlternativesOf(process))
            {
                tasks.Add(GetOrderedTasksOf(alternative)[0]);
            }
            return tasks;
        }

        public List<Task> GetLastTasksOfAlternativesInProcess(Process process)
        {
            var tasks = new List<Task>();
            foreach (var alternative in GetAlternativesOf(process))
            {
                tasks.Add(GetOrderedTasksOf(alternative)[^-1]);
            }
            return tasks;
        }

        public List<Motion> GetMotionsOf(Process process)
        {
            return HierarchyRecords.Where(r => r.Process.GlobalID == process.GlobalID)
                                   .Select(r => r.Motion)
                                   .ToList();
        }

        public bool IsAlternativeInProcess(Alternative alternative, Process process)
        {
            return HierarchyRecords.Where(r => r.Process.GlobalID == process.GlobalID && r.Alternative.GlobalID == alternative.GlobalID).Count() > 0;
        }

        public bool IsTaskInProcess(Task task, Process process)
        {
            return HierarchyRecords.Where(r => r.Process.GlobalID == process.GlobalID && r.Task.GlobalID == task.GlobalID).Count() > 0;
        }

        public bool IsMotionInProcess(Motion motion, Process process)
        {
            return HierarchyRecords.Where(r => r.Process.GlobalID == process.GlobalID && r.Motion.GlobalID == motion.GlobalID).Count() > 0;
        }


        //Alternative
        public List<Alternative> GetAlternatives()
        {
            return HierarchyRecords.Select(r => r.Alternative).Distinct().ToList();
        }

        public Alternative GetAlternativeByID(int id)
        {
            return HierarchyRecords.Where(r => r.Alternative.ID == id).Select(r => r.Alternative).FirstOrDefault();
        }

        public Process GetProcessOf(Alternative alternative)
        {
            return HierarchyRecords.Where(r => r.Alternative.GlobalID == alternative.GlobalID)
                                   .Select(r => r.Process)
                                   .FirstOrDefault();
        }

        public List<Alternative> GetNeighboursOf(Alternative alternative)
        {
           return GetAlternativesOf(GetProcessOf(alternative));
        }

        public List<Task> GetTasksOf(Alternative alternative)
        {
            return HierarchyRecords.Where(r => r.Alternative.GlobalID == alternative.GlobalID)
                                   .Select(r => r.Task)
                                   .ToList();
        }

        public List<Task> GetOrderedTasksOf(Alternative alternative)
        {
            return HierarchyRecords.Where(r => r.Alternative.GlobalID == alternative.GlobalID)
                                   .Select(r => r.Task)
                                   .OrderBy(r => r.ID)
                                   .Distinct()
                                   .ToList();
        }

        public List<Motion> GetMotionsOf(Alternative alternative)
        {
            return HierarchyRecords.Where(r => r.Alternative.GlobalID == alternative.GlobalID)
                                   .Select(r => r.Motion)
                                   .ToList();
        }

        public bool IsTaskInAlternative(Task task, Alternative alternative)
        {
            return HierarchyRecords.Where(r => r.Alternative.GlobalID == alternative.GlobalID && r.Task.GlobalID == task.GlobalID).Count() > 0;
        }

        public bool IsMotionInAlternative(Motion motion, Alternative alternative)
        {
            return HierarchyRecords.Where(r => r.Alternative.GlobalID == alternative.GlobalID && r.Motion.GlobalID == motion.GlobalID).Count() > 0;
        }


        //Tasks
        public List<Task> GetTasks()
        {
            return HierarchyRecords.Select(r => r.Task).Distinct().ToList();
        }

        public Task GetTaskByID(int id)
        {
            return HierarchyRecords.Where(r => r.Task.ID == id).Select(r => r.Task).FirstOrDefault();
        }

        public Process GetProcessOf(Task task)
        {
            return HierarchyRecords.Where(r => r.Task.GlobalID == task.GlobalID)
                                   .Select(r => r.Process)
                                   .FirstOrDefault();
        }

        public Alternative GetAlternativeOf(Task task)
        {
            return HierarchyRecords.Where(r => r.Task.GlobalID == task.GlobalID)
                                   .Select(r => r.Alternative)
                                   .FirstOrDefault();
        }

        public List<Task> GetNeighbours(Task task)
        {
            return GetTasksOf(GetAlternativeOf(task));
        }

        public List<Motion> GetMotionsOf(Task task)
        {
            return HierarchyRecords.Where(r => r.Task.GlobalID == task.GlobalID)
                                   .Select(r => r.Motion)
                                   .ToList();
        }

        public bool IsMotionInTask(Motion motion, Task task)
        {
            return HierarchyRecords.Where(r => r.Task.GlobalID == task.GlobalID && r.Motion.GlobalID == motion.GlobalID).Count() > 0;
        }

        //Motion
        public List<Motion> GetMotions()
        {
            return HierarchyRecords.Select(r => r.Motion).Distinct().ToList();
        }

        public Motion GetMotionByID(int id)
        {
            return HierarchyRecords.Where(r => r.Motion.ID == id).Select(r => r.Motion).FirstOrDefault();
        }
        public Motion GetMotionBySeqID(int id)
        {
            return HierarchyRecords.Where(r => r.Motion.SequenceMatrixID == id).Select(r => r.Motion).FirstOrDefault();
        }

        public Process GetProcessOf(Motion motion)
        {
            return HierarchyRecords.Where(r => r.Motion.GlobalID == motion.GlobalID)
                                   .Select(r => r.Process)
                                   .FirstOrDefault();
        }

        public Alternative GetAlternativeOf(Motion motion)
        {
            return HierarchyRecords.Where(r => r.Motion.GlobalID == motion.GlobalID)
                                   .Select(r => r.Alternative)
                                   .FirstOrDefault();
        }

        public Task GetTaskOf(Motion motion)
        {
            return HierarchyRecords.Where(r => r.Motion.GlobalID == motion.GlobalID)
                                   .Select(r => r.Task)
                                   .FirstOrDefault();
        }

        public List<Motion> GetNeighbours(Motion motion)
        {
            return GetMotionsOf(GetTaskOf(motion));
        }

        public List<Motion> GetMotionsOf(Motion motion)
        {
            return HierarchyRecords.Where(r => r.Motion.GlobalID == motion.GlobalID)
                                   .Select(r => r.Motion)
                                   .ToList();
        }

        public List<Motion> GetMotionsOfFirstTasksOfAlternativesInProcesses(Process process)
        {
            var motions = new List<Motion>();
            foreach (var alternative in GetAlternativesOf(process))
            {
                foreach (var item in GetMotionsOf(GetOrderedTasksOf(alternative)[0]))
                {
                    motions.Add(item);
                }
            }
            return motions;
        }

        public List<Motion> GetMotionsOfLastTasksOfAlternativesInProcesses(Process process)
        {
            var motions = new List<Motion>();
            foreach (var alternative in GetAlternativesOf(process))
            {
                if (GetTasksOf(alternative).Count > 0)
                {
                    List<Task> tasks = GetOrderedTasksOf(alternative);
                    motions = GetMotionsOf(tasks[tasks.Count-1]);
                }
            }
            return motions;
        }

        public void DeleteMotion(Motion motion)
        {
            Motions.Remove(motion);
            HierarchyRecords.Remove(HierarchyRecords.Where(r => r.Motion.GlobalID == motion.GlobalID).FirstOrDefault());
        }

        public Config GetConfigByID(int id)
        {
            return Configs.Where(r => r.ID == id).FirstOrDefault();
        }
    }
}