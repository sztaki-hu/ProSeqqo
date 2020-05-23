using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser
{
    public class ValidationResult
    {
        public List<string> NewInclude { get; set; }
        public bool Validated { get; set; }

        public ValidationResult()
        {
            NewInclude = new List<string>();
            Validated = false;
        }
    }
}