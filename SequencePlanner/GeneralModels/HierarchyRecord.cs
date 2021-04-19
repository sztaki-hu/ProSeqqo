using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePlanner.GeneralModels
{
    public class HierarchyRecord
    {
        public Process Process { get; set; }
        public Alternative Alternative { get; set; }
        public Task Task { get; set; }
        public Motion Motion { get; set; }
    }
}
