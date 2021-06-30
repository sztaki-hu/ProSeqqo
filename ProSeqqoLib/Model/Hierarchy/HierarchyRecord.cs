using SequencePlanner.Helper;

namespace SequencePlanner.Model.Hierarchy
{
    public class HierarchyRecord
    {
        public Process Process { get; set; }
        public Alternative Alternative { get; set; }
        public Task Task { get; set; }
        public Motion Motion { get; set; }
        public bool FirstTaskOfAlternative { get; set; }
        public bool LastTaskOfAlternative { get; set; }

        public HierarchyRecord Copy()
        {
            return new HierarchyRecord()
            {
                Process = Process,
                Alternative = Alternative,
                Task = Task,
                Motion = Motion,
                FirstTaskOfAlternative = FirstTaskOfAlternative,
                LastTaskOfAlternative = LastTaskOfAlternative
            };
        }

        public override string ToString()
        {
            return "Process: " + Process.ID + " Alternative: " + Alternative.ID + " Task: " + Task.ID + " Motion: " + Motion.ID + " Configs: [" + Motion.Configs.ToIDListString() +"]";
        }
    }
}
