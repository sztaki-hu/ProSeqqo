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

        [TestInitialize]
        public void Init()
        {
            benchmarkTasks = new List<BenchmarkTask>();
            benchmarkTasks.Add(new BenchmarkTask()
            {
                TemplateDir = "Resources/Benchmark/Kockapakolas/Templates",
                Dir = "Resources/Benchmark/Kockapakolas",
                Parameters = new List<Dictionary<string, string>>() {
                    new Dictionary<string, string>() { ["T"] = "10000", ["MIP"] = "True", ["LSS"] = "GreedyDescent", ["USIA"] = "False", },
                    new Dictionary<string, string>() { ["T"] = "10000", ["MIP"] = "False", ["LSS"] = "GreedyDescent", ["USIA"] = "False", },
                }
            });

            //benchmarkTasks.Add(new BenchmarkTask()
            //{
            //    TemplateDir = "Resources/Benchmark/PickAndPlace/Templates",
            //    Dir = "Resources/Benchmark/PickAndPlace",
            //    Parameters = new List<Dictionary<string, string>>() {
            //        new Dictionary<string, string>() { ["T"] = "1000", ["LSS"] = "GreedyDescent", ["USIA"] = "False", },
            //        new Dictionary<string, string>() { ["T"] = "1000", ["LSS"] = "GreedyDescent",     ["USIA"] = "True", }
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

            //templateDirectories.Add("Resources/Benchmark/PickAndPlace/Templates");
            //templateDirectories.Add("Resources/Benchmark/CSOPA/Templates");
            //templateDirectories.Add("Resources/Benchmark/Celta/Templates");
            //templateDirectories.Add("Resources/Benchmark/Kockapakolas/Templates");
            //directories.Add("Resources/Benchmark/PickAndPlace");
            //directories.Add("Resources/Benchmark/CSOPA");
            //directories.Add("Resources/Benchmark/Celta");
            //directories.Add("Resources/Benchmark/Kockapakolas");
            //parameters.Add(new List<Dictionary<string, string>>());
            ////parameters[0].Add(new Dictionary<string, string>() { ["T"] = "1000", ["LSS"] = "GreedyDescent",     ["USIA"] = "False", });
            //parameters[0].Add(new Dictionary<string, string>() { ["T"] = "1000", ["LSS"] = "GuidedLocalSearch", ["USIA"] = "False", });
            ////parameters[0].Add(new Dictionary<string, string>() { ["T"] = "1000", ["LSS"] = "GreedyDescent",     ["USIA"] = "True", });
            //parameters[0].Add(new Dictionary<string, string>() { ["T"] = "1000", ["LSS"] = "GuidedLocalSearch", ["USIA"] = "True", });
            //parameters[0].Add(new Dictionary<string, string>() { ["T"] = "5000", ["LSS"] = "GreedyDescent",     ["USIA"] = "False", });
            //parameters[0].Add(new Dictionary<string, string>() { ["T"] = "5000", ["LSS"] = "GuidedLocalSearch", ["USIA"] = "False", });
            //parameters.Add(new List<Dictionary<string, string>>());
            //parameters.Add(new List<Dictionary<string, string>>());
            var t = DateTime.Now;
            var d = "_";
            generationDir = "Benchmark_" + t.Year+d+t.Month+d+t.Day+d+t.Hour+d+t.Minute+d+t.Second;
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

        [TestMethod]
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
                SeqLogger.Info(j++ + "/" + templates.Count + " Template running: "+templates[i].FileName);
                SeqLogger.Indent++;
                try
                {
                    templates[i].RunTasks();
                }
                catch(Exception e)
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
    }
}