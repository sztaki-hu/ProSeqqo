using System.Collections.Generic;

namespace SequencerTest.Benchmark
{
    public class BenchmarkTask
    {
        public string TemplateDir { get; set; }
        public string Dir { get; set; }
        public List<Dictionary<string, string>> Parameters { get; set; }

    }
}
