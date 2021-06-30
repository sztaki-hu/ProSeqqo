using System;
using System.Collections.Generic;

namespace SequencePlanner.OR_Tools
{
    public class ORToolsResult
    {
        public List<int> Solution { get; set; }
        public TimeSpan Time { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
    }
}