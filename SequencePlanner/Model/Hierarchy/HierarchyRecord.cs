namespace SequencePlanner.Model.Hierarchy
{
    public class HierarchyRecord
    {
        public Process Process { get; set; }
        public Alternative Alternative { get; set; }
        public Task Task { get; set; }
        public Motion Motion { get; set; }

        public override string ToString()
        {
            return "Process: " + Process.ID + " Alternative: " + Alternative.ID + " Task: " + Task.ID + " Motion: " + Motion.ID;
        }
    }
}
