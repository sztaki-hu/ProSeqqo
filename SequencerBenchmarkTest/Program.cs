using SequencerTest.Benchmark;
using System;

namespace SequencerBenchmarkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Runner runner = new Runner();
            runner.Init();
            runner.RunBenchmark();
        }
    }
}
