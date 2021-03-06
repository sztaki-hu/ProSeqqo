using ProSeqqoLib.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProSeqqoLib.Model.Hierarchy
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

        public void Build()
        {
            FillConfigsMotions();
            GenerateBidirectionals();
            HierarchyRecords.Sort(new HierarchySorter());
            SetFirstLast();
            //foreach (var item in HierarchyRecords)
            //{
            //    var s = item.ToString();
            //    if (item.FirstTaskOfAlternative)
            //        s += "First!";
            //    if (item.LastTaskOfAlternative)
            //        s += "Last!";
            //    Console.WriteLine(s);
            //}
        }

        private void SetFirstLast()
        {
            int aktProcess = -1;
            int aktAlternative = -1;
            int minTaskIDInAlternative = int.MaxValue;
            int maxTaskIDInAlternative = -1;
            HierarchyRecord r = null;
            for (int i = 0; i < HierarchyRecords.Count; i++)
            {
                r = HierarchyRecords[i];
                if (aktAlternative != r.Alternative.GlobalID && aktAlternative != -1)
                {
                    GoBackAndSetMinMax(i-1, aktProcess, aktAlternative, minTaskIDInAlternative, maxTaskIDInAlternative);
                    aktAlternative = -1;
                    maxTaskIDInAlternative = -1;
                    minTaskIDInAlternative = int.MaxValue;
                }

                if (aktProcess != r.Process.ID)
                {
                    aktProcess = -1;
                }

                if (aktProcess == -1)
                    aktProcess = r.Process.ID;
                if (aktAlternative == -1)
                    aktAlternative = r.Alternative.GlobalID;
                if (minTaskIDInAlternative > r.Task.ID)
                    minTaskIDInAlternative = r.Task.ID;
                if (maxTaskIDInAlternative < r.Task.ID)
                    maxTaskIDInAlternative = r.Task.ID;
            }
            GoBackAndSetMinMax(HierarchyRecords.Count - 1, aktProcess, aktAlternative, minTaskIDInAlternative, maxTaskIDInAlternative);
        }

        private void GoBackAndSetMinMax(int index, int aktProcess, int aktAlternative, int minTaskIDInAlternative, int maxTaskIDInAlternative)
        {
            for (int i = index; i >= 0; i--)
            {
                var r = HierarchyRecords[i];
                if (r.Process.ID != aktProcess || r.Alternative.GlobalID != aktAlternative)
                    return;
                if (r.Task.ID == minTaskIDInAlternative)
                    r.FirstTaskOfAlternative = true;
                if (r.Task.ID == maxTaskIDInAlternative)
                    r.LastTaskOfAlternative = true;
            }
        }

        private void GenerateBidirectionals()
        {
            for (int i = 0; i < Motions.Count; i++)
            {
                if (Motions[i].Bidirectional)
                {
                    var bid = Motions[i].GetReverse();
                    Motions.Add(bid);
                    var record = GetRecordByMotion(Motions[i]).Copy();
                    record.Motion = bid;
                    HierarchyRecords.Add(record);
                }
            }
        }

        private void FillConfigsMotions()
        {
            Motions = new List<Motion>();
            Configs = new List<Config>();
            foreach (var r in HierarchyRecords)
            {
                Motions.Add(r.Motion);
                Configs.AddRange(r.Motion.Configs);
            }
        }

        public HierarchyRecord GetRecordByMotion(Motion motion)
        {
            return HierarchyRecords.Where(r => r.Motion.ID == motion.ID).FirstOrDefault();
        }

        public List<HierarchyRecord> GetRecordsByTask(Task task)
        {
            return HierarchyRecords.Where(r => r.Task.ID == task.ID).ToList();
        }

        public List<HierarchyRecord> GetRecordsByAlternative(Alternative alternative)
        {
            return HierarchyRecords.Where(r => r.Alternative.ID == alternative.ID).ToList();
        }

        public List<HierarchyRecord> GetRecordByProcess(Process process)
        {
            return HierarchyRecords.Where(r => r.Process.ID == process.ID).ToList();
        }

        //Process
        public List<Process> GetProcesses()
        {
            return HierarchyRecords.Select(r => r.Process).Distinct().ToList();
        }

        public Process GetProcessByID(int id)
        {
            return HierarchyRecords.Where(r => r.Process.ID == id).Select(r => r.Process).FirstOrDefault();
        }

        public List<Alternative> GetAlternativesOf(Process process)
        {
            return HierarchyRecords.Where(r => r.Process.GlobalID == process.GlobalID)
                                   .Select(r => r.Alternative)
                                   .Distinct()
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
        public bool IsAlternativeInProcess(int alternativeID, Process process)
        {
            return HierarchyRecords.Where(r => r.Process.GlobalID == process.GlobalID && r.Alternative.ID == alternativeID).Count() > 0;
        }

        public Alternative GetAlternativeInProcess(int alternativeID, Process proc)
        {
            return HierarchyRecords.Where(r => r.Process.GlobalID == proc.GlobalID && r.Alternative.ID == alternativeID)
                                   .Select(r => r.Alternative)
                                   .FirstOrDefault();
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
            return HierarchyRecords.Where(r => r.Alternative.ID == id)
                                   .Select(r => r.Alternative)
                                   .FirstOrDefault();
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
                                   .Distinct()
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

        public bool IsTaskInAlternative(int taskID, Alternative alternative)
        {
            return HierarchyRecords.Where(r => r.Alternative.GlobalID == alternative.GlobalID && r.Task.ID == taskID).Count() > 0;
        }

        public bool IsMotionInAlternative(Motion motion, Alternative alternative)
        {
            return HierarchyRecords.Where(r => r.Alternative.GlobalID == alternative.GlobalID && r.Motion.GlobalID == motion.GlobalID).Count() > 0;
        }

        public Task GetTaskInProcessAlternative(int taskID, Process proc, Alternative alternative)
        {
            return HierarchyRecords.Where(r => r.Process.GlobalID == proc.GlobalID && r.Alternative.GlobalID == alternative.GlobalID && r.Task.ID == taskID)
                                   .Select(r => r.Task)
                                   .FirstOrDefault();
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
                                   .Distinct()
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
                    motions = GetMotionsOf(tasks[tasks.Count - 1]);
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