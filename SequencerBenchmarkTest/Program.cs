using SequencerTest.Benchmark;
using System;
using System.Collections.Generic;

namespace SequencerBenchmarkTest
{
    class Program
    {
        static void Main()
        {
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
            System.Diagnostics.Process.GetCurrentProcess().PriorityBoostEnabled = true;
            Console.Title = "ProSeqqo Benchmark Console";
            System.Console.Write("Please write a name for the benchmark: ");
            var name = System.Console.ReadLine();
            Runner runner = new Runner();
            runner.InitWithTasks(name, GetBenchmarkTasks());
            //runner.Init();
            runner.RunBenchmark();
        }

        private static List<BenchmarkTask> GetBenchmarkTasks()
        {
            var benchmarkTasks = new List<BenchmarkTask>
            {
                //new BenchmarkTask()
                //{
                //    TemplateDir = "Resources/Benchmark/Kockapakolas/Templates",
                //    Dir = "Resources/Benchmark/Kockapakolas",
                //    Parameters = new List<Dictionary<string, string>>() {
                //    //new Dictionary<string, string>() { ["T"] = "10000", ["MIP"] = "True", ["LSS"] = "Automatic", ["USIA"] = "True", },
                //    //new Dictionary<string, string>() { ["T"] = "0", ["MIP"] = "False", ["LSS"] = "Automatic", ["USIA"] = "False", },
                //    new Dictionary<string, string>() { ["T"] = "0", ["MIP"] = "True", ["LSS"] = "Automatic", ["USIA"] = "False", },
                //    //new Dictionary<string, string>() { ["T"] = "10000", ["MIP"] = "False", ["LSS"] = "Automatic", ["USIA"] = "True", },
                //}
                //},

                new BenchmarkTask()
                {
                    TemplateDir = "Resources/Benchmark/PickAndPlace/Templates",
                    Dir = "Resources/Benchmark/PickAndPlace",
                    Parameters = new List<Dictionary<string, string>>() {
                    ////new Dictionary<string, string>() { ["T"] = "1000",["MIP"] = "False", ["LSS"] = "GreedyDescent", ["USIA"] = "False", },
                    ////new Dictionary<string, string>() { ["T"] = "1000",["MIP"] = "True",  ["LSS"] = "GreedyDescent", ["USIA"] = "False", },
                    ////new Dictionary<string, string>() { ["T"] = "1000",["MIP"] = "True",  ["LSS"] = "GreedyDescent", ["USIA"] = "True", },
                    ////new Dictionary<string, string>() { ["T"] = "1000",["MIP"] = "False", ["LSS"] = "GreedyDescent", ["USIA"] = "True", }

                    //new Dictionary<string, string>() { ["Time"] = "0", ["Strategy"] = "Automatic",           ["MIP"] = "True" },
                    //new Dictionary<string, string>() { ["Time"] = "0", ["Strategy"] = "GreedyDescent",       ["MIP"] = "True" },

                    //new Dictionary<string, string>() { ["Time"] = "100",  ["Strategy"] = "Automatic",           ["MIP"] = "False" },
                    //new Dictionary<string, string>() { ["Time"] = "100",  ["Strategy"] = "GreedyDescent",       ["MIP"] = "False" },
                    //new Dictionary<string, string>() { ["Time"] = "100",  ["Strategy"] = "GuidedLocalSearch",   ["MIP"] = "False" },
                    //new Dictionary<string, string>() { ["Time"] = "100",  ["Strategy"] = "TabuSearch",          ["MIP"] = "False" },
                    //new Dictionary<string, string>() { ["Time"] = "100",  ["Strategy"] = "ObjectiveTabuSearch", ["MIP"] = "False" },
                    //new Dictionary<string, string>() { ["Time"] = "100",  ["Strategy"] = "SimulatedAnnealing",  ["MIP"] = "False" },

                    //new Dictionary<string, string>() { ["Time"] = "1000",  ["Strategy"] = "Automatic",           ["MIP"] = "False" },
                    //new Dictionary<string, string>() { ["Time"] = "1000",  ["Strategy"] = "GreedyDescent",       ["MIP"] = "False" },
                    //new Dictionary<string, string>() { ["Time"] = "1000",  ["Strategy"] = "GuidedLocalSearch",   ["MIP"] = "False" },
                    //new Dictionary<string, string>() { ["Time"] = "1000",  ["Strategy"] = "TabuSearch",          ["MIP"] = "False" },
                    //new Dictionary<string, string>() { ["Time"] = "1000",  ["Strategy"] = "ObjectiveTabuSearch", ["MIP"] = "False" },
                    //new Dictionary<string, string>() { ["Time"] = "1000",  ["Strategy"] = "SimulatedAnnealing",  ["MIP"] = "False" },
                                                                                                                            
                    ////new Dictionary<string, string>() { ["Time"] = "10000", ["Strategy"] = "Automatic",           ["MIP"] = "False" },
                    ////new Dictionary<string, string>() { ["Time"] = "10000", ["Strategy"] = "GreedyDescent",       ["MIP"] = "False" },
                    //new Dictionary<string, string>() { ["Time"] = "10000", ["Strategy"] = "GuidedLocalSearch",   ["MIP"] = "False" },
                    //new Dictionary<string, string>() { ["Time"] = "10000", ["Strategy"] = "TabuSearch",          ["MIP"] = "False" },
                    //new Dictionary<string, string>() { ["Time"] = "10000", ["Strategy"] = "ObjectiveTabuSearch", ["MIP"] = "False" },
                    //new Dictionary<string, string>() { ["Time"] = "10000", ["Strategy"] = "SimulatedAnnealing",  ["MIP"] = "False" },
                                
                //    new Dictionary<string, string>() { ["Time"] = "60000", ["Strategy"] = "Automatic",           ["MIP"] = "False" },
                //    new Dictionary<string, string>() { ["Time"] = "60000", ["Strategy"] = "GreedyDescent",       ["MIP"] = "False" },
                    new Dictionary<string, string>() { ["Time"] = "60000", ["Strategy"] = "GuidedLocalSearch",   ["MIP"] = "False" },
                //    new Dictionary<string, string>() { ["Time"] = "60000", ["Strategy"] = "TabuSearch",          ["MIP"] = "False" },
                //    new Dictionary<string, string>() { ["Time"] = "60000", ["Strategy"] = "ObjectiveTabuSearch", ["MIP"] = "False" },
                //    new Dictionary<string, string>() { ["Time"] = "60000", ["Strategy"] = "SimulatedAnnealing",  ["MIP"] = "False" },
                }
                },

                //new BenchmarkTask()
                //{
                //    TemplateDir = "Resources/Benchmark/CSOPA/Templates",
                //    Dir = "Resources/Benchmark/CSOPA",
                //    Parameters = new List<Dictionary<string, string>>()
                //    {
                //        //new Dictionary<string, string>() { ["Time"] = "0", ["Strategy"] = "Automatic",           ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "0", ["Strategy"] = "GreedyDescent",       ["BID"] = "True" },

                //        //new Dictionary<string, string>() { ["Time"] = "1000", ["Strategy"] = "Automatic",           ["BID"] = "True" },
                //        //new Dictionary<string, string>() { ["Time"] = "1000", ["Strategy"] = "GreedyDescent",       ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "1000", ["Strategy"] = "GuidedLocalSearch",   ["BID"] = "True" },
                //        //new Dictionary<string, string>() { ["Time"] = "1000", ["Strategy"] = "TabuSearch",          ["BID"] = "True" },
                //        //new Dictionary<string, string>() { ["Time"] = "1000", ["Strategy"] = "ObjectiveTabuSearch", ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "1000", ["Strategy"] = "SimulatedAnnealing",  ["BID"] = "True" },

                //        //new Dictionary<string, string>() { ["Time"] = "10000", ["Strategy"] = "Automatic",           ["BID"] = "True" },
                //        //new Dictionary<string, string>() { ["Time"] = "10000", ["Strategy"] = "GreedyDescent",       ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "10000", ["Strategy"] = "GuidedLocalSearch",   ["BID"] = "True" },
                //        //new Dictionary<string, string>() { ["Time"] = "10000", ["Strategy"] = "TabuSearch",          ["BID"] = "True" },
                //        //new Dictionary<string, string>() { ["Time"] = "10000", ["Strategy"] = "ObjectiveTabuSearch", ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "10000", ["Strategy"] = "SimulatedAnnealing",  ["BID"] = "True" },

                //        new Dictionary<string, string>() { ["Time"] = "60000", ["Strategy"] = "GuidedLocalSearch",   ["BID"] = "True" },
                //       // new Dictionary<string, string>() { ["Time"] = "60000", ["Strategy"] = "TabuSearch",          ["BID"] = "True" },
                //       // new Dictionary<string, string>() { ["Time"] = "60000", ["Strategy"] = "ObjectiveTabuSearch", ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "60000", ["Strategy"] = "SimulatedAnnealing",  ["BID"] = "True" },

                //        new Dictionary<string, string>() { ["Time"] = "600000", ["Strategy"] = "GuidedLocalSearch",   ["BID"] = "True" },
                ////      new Dictionary<string, string>() { ["Time"] = "600000", ["Strategy"] = "TabuSearch",          ["BID"] = "True" },
                ////      new Dictionary<string, string>() { ["Time"] = "600000", ["Strategy"] = "ObjectiveTabuSearch", ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "600000", ["Strategy"] = "SimulatedAnnealing",  ["BID"] = "True" },
                //    }
                //},

                new BenchmarkTask()
                {
                    TemplateDir = "Resources/Benchmark/Celta/Templates",
                    Dir = "Resources/Benchmark/Celta",
                    Parameters = new List<Dictionary<string, string>>()
                    {
                //        new Dictionary<string, string>() { ["Time"] = "0", ["Strategy"] = "Automatic",           ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "0", ["Strategy"] = "GreedyDescent",       ["BID"] = "True" },

                //        new Dictionary<string, string>() { ["Time"] = "1000", ["Strategy"] = "Automatic",           ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "1000", ["Strategy"] = "GreedyDescent",       ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "1000", ["Strategy"] = "GuidedLocalSearch",   ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "1000", ["Strategy"] = "TabuSearch",          ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "1000", ["Strategy"] = "ObjectiveTabuSearch", ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "1000", ["Strategy"] = "SimulatedAnnealing",  ["BID"] = "True" },

                //        new Dictionary<string, string>() { ["Time"] = "10000", ["Strategy"] = "Automatic",           ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "10000", ["Strategy"] = "GreedyDescent",       ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "10000", ["Strategy"] = "GuidedLocalSearch",   ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "10000", ["Strategy"] = "TabuSearch",          ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "10000", ["Strategy"] = "ObjectiveTabuSearch", ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "10000", ["Strategy"] = "SimulatedAnnealing",  ["BID"] = "True" },

                //        new Dictionary<string, string>() { ["Time"] = "60000", ["Strategy"] = "GuidedLocalSearch",   ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "60000", ["Strategy"] = "TabuSearch",          ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "60000", ["Strategy"] = "ObjectiveTabuSearch", ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "60000", ["Strategy"] = "SimulatedAnnealing",  ["BID"] = "True" },

                //        new Dictionary<string, string>() { ["Time"] = "600000", ["Strategy"] = "GuidedLocalSearch",   ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "600000", ["Strategy"] = "TabuSearch",          ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "600000", ["Strategy"] = "ObjectiveTabuSearch", ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "600000", ["Strategy"] = "SimulatedAnnealing",  ["BID"] = "True" },

                //           new Dictionary<string, string>() { ["Time"] = "1800000", ["Strategy"] = "GuidedLocalSearch",   ["BID"] = "True" },
                //        new Dictionary<string, string>() { ["Time"] = "600000", ["Strategy"] = "TabuSearch",          ["BID"] = "True" },
                    }
                },

                //new BenchmarkTask()
                //{
                //    TemplateDir = "Resources/Benchmark/Kubik/Templates",
                //    Dir = "Resources/Benchmark/Kubik",
                //    Parameters = new List<Dictionary<string, string>>()
                //    {
                //    }
                //},

                //MESTERECSET CIKK
               // new BenchmarkTask()
                //{
                    //TemplateDir = "Resources/Benchmark/MesterEcset/Templates",
                    //Dir = "Resources/Benchmark/MesterEcset",
                    //Parameters = new List<Dictionary<string, string>>()
                    //{
                        //new Dictionary<string, string>() { ["Time"] = "10000", ["MIP"] = "False", ["Strategy"] = "Automatic", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "0", ["MIP"] = "True", ["Strategy"] = "Automatic", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "0", ["MIP"] = "True", ["Strategy"] = "GreedyDescent", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "2500", ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "5000", ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "10000", ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "20000", ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "60000", ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "120000", ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "600000", ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch", ["BID"] = "True" },

                        //new Dictionary<string, string>() { ["Time"] = "1800000", ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "1000", ["MIP"] = "True",  ["Strategy"] = "TabuSearch", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "2500", ["MIP"] = "True",  ["Strategy"] = "TabuSearch", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "5000", ["MIP"] = "True",  ["Strategy"] = "TabuSearch", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "10000", ["MIP"] = "True",  ["Strategy"] = "TabuSearch", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "60000", ["MIP"] = "True",  ["Strategy"] = "TabuSearch", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "600000", ["MIP"] = "True",  ["Strategy"] = "TabuSearch", ["BID"] = "True" },
 
                        //new Dictionary<string, string>() { ["Time"] = "1800000", ["MIP"] = "True",  ["Strategy"] = "TabuSearch", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "10000", ["MIP"] = "True",  ["Strategy"] = "ObjectiveTabuSearch", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "10000", ["MIP"] = "True",  ["Strategy"] = "SimulatedAnnealing", ["BID"] = "True" },
                //    }
                //},

                //new BenchmarkTask()
                //{
                //    TemplateDir = "Resources/Benchmark/SeqTest/Templates",
                //    Dir = "Resources/Benchmark/SeqTest",
                //    Parameters = new List<Dictionary<string, string>>()
                //{
                //    //new Dictionary<string, string>() { ["T"] = "1200000", ["MIP"] = "False", ["Strategy"] = "Automatic" },
                //    //new Dictionary<string, string>() { ["T"] = "0",       ["MIP"] = "True",  ["Strategy"] = "Automatic" },
                //    //new Dictionary<string, string>() { ["T"] = "1000",    ["MIP"] = "True",  ["Strategy"] = "Automatic" },
                //    //new Dictionary<string, string>() { ["T"] = "5000",    ["MIP"] = "True",  ["Strategy"] = "Automatic" },
                //    //new Dictionary<string, string>() { ["T"] = "1200000", ["MIP"] = "False", ["Strategy"] = "GuidedLocalSearch" },
                //    new Dictionary<string, string>() { ["T"] = "10",    ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch" },
                //    new Dictionary<string, string>() { ["T"] = "20",    ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch" },
                //    new Dictionary<string, string>() { ["T"] = "50",   ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch" },
                //    new Dictionary<string, string>() { ["T"] = "100",  ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch" },
                //    new Dictionary<string, string>() { ["T"] = "150",  ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch" },
                //    new Dictionary<string, string>() { ["T"] = "200",  ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch" },
                //    new Dictionary<string, string>() { ["T"] = "300",  ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch" },
                //    new Dictionary<string, string>() { ["T"] = "400",  ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch" },
                //    new Dictionary<string, string>() { ["T"] = "500",  ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch" },
                //    new Dictionary<string, string>() { ["T"] = "750",  ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch" },
                //}
                //}
            };

            return benchmarkTasks;
        }
    }
}
