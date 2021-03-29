using SequencerTest.Benchmark;
using System;
using System.Collections.Generic;

namespace SequencerBenchmarkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Runner runner = new Runner();
            //runner.InitWithTasks(GetBenchmarkTasks());
            runner.Init();
            runner.RunBenchmark();
        }

        private static List<BenchmarkTask> GetBenchmarkTasks()
        {
            var benchmarkTasks = new List<BenchmarkTask>();
            //benchmarkTasks.Add(new BenchmarkTask()
            //{
            //    TemplateDir = "Resources/Benchmark/Kockapakolas/Templates",
            //    Dir = "Resources/Benchmark/Kockapakolas",
            //    Parameters = new List<Dictionary<string, string>>() {
            //        //new Dictionary<string, string>() { ["T"] = "10000", ["MIP"] = "True", ["LSS"] = "Automatic", ["USIA"] = "True", },
            //        //new Dictionary<string, string>() { ["T"] = "0", ["MIP"] = "False", ["LSS"] = "Automatic", ["USIA"] = "False", },
            //        new Dictionary<string, string>() { ["T"] = "0", ["MIP"] = "True", ["LSS"] = "Automatic", ["USIA"] = "False", },
            //        //new Dictionary<string, string>() { ["T"] = "10000", ["MIP"] = "False", ["LSS"] = "Automatic", ["USIA"] = "True", },
            //    }
            //});

            //benchmarkTasks.Add(new BenchmarkTask()
            //{
            //    TemplateDir = "Resources/Benchmark/PickAndPlace/Templates",
            //    Dir = "Resources/Benchmark/PickAndPlace",
            //    Parameters = new List<Dictionary<string, string>>() {
            //        new Dictionary<string, string>() { ["T"] = "1000",["MIP"] = "False", ["LSS"] = "GreedyDescent", ["USIA"] = "False", },
            //        new Dictionary<string, string>() { ["T"] = "1000",["MIP"] = "True",  ["LSS"] = "GreedyDescent", ["USIA"] = "False", },
            //        new Dictionary<string, string>() { ["T"] = "1000",["MIP"] = "True",  ["LSS"] = "GreedyDescent", ["USIA"] = "True", },
            //        new Dictionary<string, string>() { ["T"] = "1000",["MIP"] = "False", ["LSS"] = "GreedyDescent", ["USIA"] = "True", }
            //    }
            //});

            //benchmarkTasks.Add(new BenchmarkTask()
            //{
            //    TemplateDir = "Resources/Benchmark/CSOPA/Templates",
            //    Dir = "Resources/Benchmark/CSOPA",
            //    Parameters = new List<Dictionary<string, string>>() {
            //    }
            //});

            //benchmarkTasks.Add(new BenchmarkTask()
            //{
            //    TemplateDir = "Resources/Benchmark/Celta/Templates",
            //    Dir = "Resources/Benchmark/Celta",
            //    Parameters = new List<Dictionary<string, string>>() {
            //    }
            //});

            //benchmarkTasks.Add(new BenchmarkTask()
            //{
            //    TemplateDir = "Resources/Benchmark/Kubik/Templates",
            //    Dir = "Resources/Benchmark/Kubik",
            //    Parameters = new List<Dictionary<string, string>>()
            //    {
            //    }
            //});

            benchmarkTasks.Add(new BenchmarkTask()
            {
                TemplateDir = "Resources/Benchmark/SeqTest/Templates",
                Dir = "Resources/Benchmark/SeqTest",
                Parameters = new List<Dictionary<string, string>>()
                {
                    //new Dictionary<string, string>() { ["T"] = "1200000", ["MIP"] = "False", ["Strategy"] = "Automatic" },
                    //new Dictionary<string, string>() { ["T"] = "0",       ["MIP"] = "True",  ["Strategy"] = "Automatic" },
                    //new Dictionary<string, string>() { ["T"] = "1000",    ["MIP"] = "True",  ["Strategy"] = "Automatic" },
                    //new Dictionary<string, string>() { ["T"] = "5000",    ["MIP"] = "True",  ["Strategy"] = "Automatic" },
                    //new Dictionary<string, string>() { ["T"] = "1200000", ["MIP"] = "False", ["Strategy"] = "GuidedLocalSearch" },
                    new Dictionary<string, string>() { ["T"] = "10",    ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch" },
                    new Dictionary<string, string>() { ["T"] = "20",    ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch" },
                    new Dictionary<string, string>() { ["T"] = "50",   ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch" },
                    new Dictionary<string, string>() { ["T"] = "100",  ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch" },
                    new Dictionary<string, string>() { ["T"] = "150",  ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch" },
                    new Dictionary<string, string>() { ["T"] = "200",  ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch" },
                    new Dictionary<string, string>() { ["T"] = "300",  ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch" },
                    new Dictionary<string, string>() { ["T"] = "400",  ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch" },
                    new Dictionary<string, string>() { ["T"] = "500",  ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch" },
                    new Dictionary<string, string>() { ["T"] = "750",  ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch" },
                }
            });

            return benchmarkTasks;
        }
    }
}
