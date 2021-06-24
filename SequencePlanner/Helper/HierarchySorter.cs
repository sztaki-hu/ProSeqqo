using SequencePlanner.Model.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePlanner.Helper
{
    public class HierarchySorter : IComparer<HierarchyRecord>
    {
        public int Compare(HierarchyRecord x, HierarchyRecord y)
        {
            if (x.Process.ID > y.Process.ID) return 1;
            if (x.Process.ID < y.Process.ID) return -1;
            if (x.Alternative.ID > y.Alternative.ID) return 1;
            if (x.Alternative.ID < y.Alternative.ID) return -1;
            if (x.Task.ID > y.Task.ID) return 1;
            if (x.Task.ID < y.Task.ID) return -1;
            if (x.Motion.ID > y.Motion.ID) return 1;
            if (x.Motion.ID < y.Motion.ID) return -1;
            return 0;
        }
    }
}
