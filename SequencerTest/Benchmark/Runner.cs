using Microsoft.VisualStudio.TestTools.UnitTesting;
using SequencePlanner.Helper;
using System;
using System.Collections.Generic;
using System.IO;

namespace SequencerTest.Benchmark
{
    [TestClass]
    public class Runner
    {
        List<Template> templates = new List<Template>();
        List<BenchmarkTask> benchmarkTasks = new List<BenchmarkTask>();
        string generationDir;
        int firstNTemplate = -1;

        public void Run()
        {
            //benchmarkTasks = GetCICDBenchmarks();
            var t = DateTime.Now;
            var d = "_";
            generationDir = "Benchmark_" + t.Year + d + t.Month + d + t.Day + d + t.Hour + d + t.Minute + d + t.Second;
            GenerateInstances();
            RunBenchmark();
        }

        public void InitWithTasks(List<BenchmarkTask> benchmarks = null)
        {
            if (benchmarks is null)
                throw new SeqException("No benchmars!");
            else
                benchmarkTasks = benchmarks;
            var t = DateTime.Now;
            var d = "_";
            generationDir = "Benchmark_" + t.Year + d + t.Month + d + t.Day + d + t.Hour + d + t.Minute + d + t.Second;
            GenerateInstances();
        }

        private void GenerateInstances()
        {
            for (int i = 0; i < benchmarkTasks.Count; i++)
            {
                var genDir = Path.Combine(benchmarkTasks[i].Dir, generationDir);
                Directory.CreateDirectory(genDir);
                var outDir = Path.Combine(genDir, "Out");
                Directory.CreateDirectory(outDir);
                var files = Directory.GetFiles(benchmarkTasks[i].TemplateDir);
                for (int j = 0; j < files.Length; j++)
                {
                    var tmp = new Template(files[j], genDir, outDir, benchmarkTasks[i].Parameters);
                    templates.Add(tmp);
                }
            }
        }

        public void RunBenchmark()
        {
            SeqLogger.LogLevel = LogLevel.Info;
            foreach (var item in templates)
            {
                item.CreateTasks();
            }
            int j = 0;
            if (firstNTemplate <= 0)
                firstNTemplate = templates.Count;
            for (int i = 0; i < firstNTemplate; i++)
            {
                SeqLogger.Info((j++) + 1 + "/" + templates.Count + " Template running: " + templates[i].FileName);
                SeqLogger.Indent++;
                try
                {
                    templates[i].RunTasks();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                SeqLogger.Indent--;
            }

            using (StreamWriter file = File.CreateText("Resources/Benchmark/" + generationDir + ".csv"))
            {
                file.WriteLine(Template.ToCSVHeader());
                for (int i = 0; i < firstNTemplate; i++)
                {
                    file.WriteLine(templates[i].ToCSV());
                }
                SeqLogger.Info("Resources/Benchmark/" + generationDir + ".csv created!", "BenchmarkRunner");
            }
        }

        [TestMethod]
        public void RunCubePickAndPlace()
        {
            //KOCKAPAKOLAS
            benchmarkTasks = new List<BenchmarkTask>();
            benchmarkTasks.Add(new BenchmarkTask()
            {
                TemplateDir = "Resources/Benchmark/Kockapakolas/Templates",
                Dir = "Resources/Benchmark/Kockapakolas",
                Parameters = new List<Dictionary<string, string>>() {
                    new Dictionary<string, string>() { ["T"] = "1000", ["MIP"] = "True", ["LSS"] = "Automatic", ["USIA"] = "False", },
                    //new Dictionary<string, string>() { ["T"] = "0", ["MIP"] = "False", ["LSS"] = "Automatic", ["USIA"] = "False", },
                    new Dictionary<string, string>() { ["T"] = "5000", ["MIP"] = "True", ["LSS"] = "Automatic", ["USIA"] = "False", },
                    //new Dictionary<string, string>() { ["T"] = "1000", ["MIP"] = "False", ["LSS"] = "Automatic", ["USIA"] = "True", },
                }
            });
            Run();
        }

        [TestMethod]
        public void RunPickAndPlace()
        {
            //PickAndPlace
            benchmarkTasks = new List<BenchmarkTask>();
            benchmarkTasks.Add(new BenchmarkTask()
            {
                TemplateDir = "Resources/Benchmark/PickAndPlace/Templates",
                Dir = "Resources/Benchmark/PickAndPlace",
                Parameters = new List<Dictionary<string, string>>() {
                    new Dictionary<string, string>() { ["T"] = "1000",["MIP"] = "False", ["LSS"] = "GreedyDescent", ["USIA"] = "False", },
                    new Dictionary<string, string>() { ["T"] = "1000",["MIP"] = "True",  ["LSS"] = "GreedyDescent", ["USIA"] = "False", },
                    new Dictionary<string, string>() { ["T"] = "1000",["MIP"] = "True",  ["LSS"] = "GreedyDescent", ["USIA"] = "True", },
                    new Dictionary<string, string>() { ["T"] = "1000",["MIP"] = "False", ["LSS"] = "GreedyDescent", ["USIA"] = "True", }
                }
            });

            Run();
        }

        [TestMethod]
        public void RunCSOPA()
        {
            //CSOPA
            benchmarkTasks = new List<BenchmarkTask>();
            benchmarkTasks.Add(new BenchmarkTask()
            {
                TemplateDir = "Resources/Benchmark/CSOPA/Templates",
                Dir = "Resources/Benchmark/CSOPA",
                Parameters = new List<Dictionary<string, string>>()
                {
                }
            });

            Run();
        }

        [TestMethod]
        public void RunCelta()
        {
            //CELTA
            benchmarkTasks = new List<BenchmarkTask>();
            benchmarkTasks.Add(new BenchmarkTask()
            {
                TemplateDir = "Resources/Benchmark/Celta/Templates",
                Dir = "Resources/Benchmark/Celta",
                Parameters = new List<Dictionary<string, string>>()
                {
                }
            });

            Run();
        }

        [TestMethod]
        public void RunKubik()
        {
            //KUBIK
            benchmarkTasks = new List<BenchmarkTask>();
            benchmarkTasks.Add(new BenchmarkTask()
            {
                TemplateDir = "Resources/Benchmark/Kubik/Templates",
                Dir = "Resources/Benchmark/Kubik",
                Parameters = new List<Dictionary<string, string>>()
                {
                }
            });

            Run();
        }

        [TestMethod]
        public void RunSeqTest()
        {
            //SEQTEST
            benchmarkTasks = new List<BenchmarkTask>();
            benchmarkTasks.Add(new BenchmarkTask()
            {
                TemplateDir = "Resources/Benchmark/SeqTest/Templates",
                Dir = "Resources/Benchmark/SeqTest",
                Parameters = new List<Dictionary<string, string>>()
                {
                    //new Dictionary<string, string>() { ["T"] = "1200000", ["MIP"] = "False", ["Strategy"] = "Automatic" },
                    //new Dictionary<string, string>() { ["T"] = "0",       ["MIP"] = "True",  ["Strategy"] = "Automatic" },
                    new Dictionary<string, string>() { ["T"] = "1000",    ["MIP"] = "True",  ["Strategy"] = "Automatic" },
                    new Dictionary<string, string>() { ["T"] = "5000",    ["MIP"] = "True",  ["Strategy"] = "Automatic" },
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

            Run();
        }
    }
}