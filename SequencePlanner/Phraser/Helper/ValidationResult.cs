using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Helper
{
    public class ValidationResult
    {
        public List<string> NewIncludeNeed { get; set; }
        public List<string> NewIncludeOptional { get; set; }
        public bool Validated { get; set; }

        public ValidationResult()
        {
            NewIncludeOptional = new List<string>();
            NewIncludeNeed = new List<string>();
            Validated = false;
        }
    }
}