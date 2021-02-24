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
        List<string> directories = new List<string>();
        List<string> templateDirectories = new List<string>();
        List<List<Dictionary<string, string>>> parameters = new List<List<Dictionary<string, string>>>();
        List<Template> templates = new List<Template>();
        string generationDir;


        [TestInitialize]
        public void Init()
        {
            templateDirectories.Add("Resources/Benchmark/PickAndPlace/Templates");
            templateDirectories.Add("Resources/Benchmark/CSOPA/Templates");
            templateDirectories.Add("Resources/Benchmark/Celta/Templates");
            directories.Add("Resources/Benchmark/PickAndPlace");
            directories.Add("Resources/Benchmark/CSOPA");
            directories.Add("Resources/Benchmark/Celta");
            parameters.Add(new List<Dictionary<string, string>>());
            parameters[0].Add(new Dictionary<string, string>() { ["T"] = "1000", ["LSS"] = "GreedyDescent",     ["USIA"] = "False", });
            parameters[0].Add(new Dictionary<string, string>() { ["T"] = "1000", ["LSS"] = "GuidedLocalSearch", ["USIA"] = "False", });
            parameters[0].Add(new Dictionary<string, string>() { ["T"] = "1000", ["LSS"] = "GreedyDescent",     ["USIA"] = "True", });
            parameters[0].Add(new Dictionary<string, string>() { ["T"] = "1000", ["LSS"] = "GuidedLocalSearch", ["USIA"] = "True", });
            //parameters[0].Add(new Dictionary<string, string>() { ["T"] = "5000", ["LSS"] = "GreedyDescent",     ["USIA"] = "False", });
            //parameters[0].Add(new Dictionary<string, string>() { ["T"] = "5000", ["LSS"] = "GuidedLocalSearch", ["USIA"] = "False", });
            parameters.Add(new List<Dictionary<string, string>>());
            parameters.Add(new List<Dictionary<string, string>>());
            var t = DateTime.Now;
            var d = "_";
            generationDir = "Benchmark_" + t.Year+d+t.Month+d+t.Day+d+t.Hour+d+t.Minute+d+t.Second;
            GenerateInstances();
        }

        private void GenerateInstances()
        {
            for (int i = 0; i < templateDirectories.Count; i++)
            {
                var genDir = Path.Combine(directories[i], generationDir);
                Directory.CreateDirectory(genDir);
                var outDir = Path.Combine(genDir, "Out");
                Directory.CreateDirectory(outDir);
                var files = Directory.GetFiles(templateDirectories[i]);
                foreach (var item in files)
                {
                    var tmp = new Template(item, genDir, outDir, parameters[i]);
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
            int i = 0;
            foreach (var item in templates)
            {
                SeqLogger.Info(i++ + "/" + templates.Count + "Template running: "+item.FileName);
                SeqLogger.Indent++;
                item.RunTasks();
                SeqLogger.Indent--;
            }

            using (StreamWriter file = File.CreateText("Resources/Benchmark/" + generationDir + ".csv"))
            {
                foreach (var item in templates)
                {
                    file.WriteLine(item.ToCSV());
                }
            }
        }  
    }
}