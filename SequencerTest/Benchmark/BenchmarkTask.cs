using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencerTest.Benchmark
{
    public class BenchmarkTask
    {
        public string TemplateDir { get; set; }
        public string Dir { get; set; }
        public List<Dictionary<string, string>> Parameters { get; set; }

    }
}
