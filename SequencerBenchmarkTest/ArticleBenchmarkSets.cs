using SequencerTest.Benchmark;
using System.Collections.Generic;

namespace SequencerBenchmarkTest
{
    public static class ArticleBenchmarkSets
    {
        public static List<BenchmarkTask> GetArticleTest()
        {
            return GetPickAndPlaceTest();
            //return GetCSOPATest();
            //return GetLaserTest();
            //return GetCubeTest();
            //return GetMesterEcsetTest();
        }


        public static List<BenchmarkTask> GetPickAndPlaceTest()
        {
            var benchmarkTasks = new List<BenchmarkTask>
            {
                 new BenchmarkTask()
                 {
                    TemplateDir = "Resources/Benchmark/PickAndPlace/Templates",
                    Dir = "Resources/Benchmark/PickAndPlace",
                    Parameters = new List<Dictionary<string, string>>() {
                        //new Dictionary<string, string>() { ["Time"] = "0",       ["Strategy"] = "GreedyDescent",       ["MIP"] = "False" },
                        new Dictionary<string, string>() { ["Time"] = "0",       ["Strategy"] = "GreedyDescent",       ["MIP"] = "False",  ["USA"]="False" },

                        //new Dictionary<string, string>() { ["Time"] = "100",     ["Strategy"] = "GuidedLocalSearch",   ["MIP"] = "False",  ["USA"]="True"},
                        //new Dictionary<string, string>() { ["Time"] = "100",     ["Strategy"] = "GuidedLocalSearch",   ["MIP"] = "False",  ["USA"]="False"},
                        //new Dictionary<string, string>() { ["Time"] = "1 000",   ["Strategy"] = "GuidedLocalSearch",   ["MIP"] = "False",  ["USA"]="True"},
                        //new Dictionary<string, string>() { ["Time"] = "1 000",   ["Strategy"] = "GuidedLocalSearch",   ["MIP"] = "False",  ["USA"]="False"},
                        //new Dictionary<string, string>() { ["Time"] = "10 000",  ["Strategy"] = "GuidedLocalSearch",   ["MIP"] = "False",   ["USA"]="True"},
                        //new Dictionary<string, string>() { ["Time"] = "10 000",  ["Strategy"] = "GuidedLocalSearch",   ["MIP"] = "False"   ["USA"]="False"},
                        //new Dictionary<string, string>() { ["Time"] = "60 000",  ["Strategy"] = "GuidedLocalSearch",   ["MIP"] = "False",   ["USA"]="True" },
                        //new Dictionary<string, string>() { ["Time"] = "60 000",  ["Strategy"] = "GuidedLocalSearch",   ["MIP"] = "False" },
                        //new Dictionary<string, string>() { ["Time"] = "600 000", ["Strategy"] = "GuidedLocalSearch",   ["MIP"] = "False" },

                        //new Dictionary<string, string>() { ["Time"] = "100",     ["Strategy"] = "TaboSearch",   ["MIP"] = "False"   },
                        //new Dictionary<string, string>() { ["Time"] = "1 000",   ["Strategy"] = "TabuSearch",   ["MIP"] = "False"  },
                        //new Dictionary<string, string>() { ["Time"] = "10 000",  ["Strategy"] = "TabuSearch",   ["MIP"] = "False"  },
                      //  new Dictionary<string, string>() { ["Time"] = "60 000",  ["Strategy"] = "TabuSearch",   ["MIP"] = "False"  },
                        //new Dictionary<string, string>() { ["Time"] = "600 000", ["Strategy"] = "TabuSearch",   ["MIP"] = "False" },

                        //new Dictionary<string, string>() { ["Time"] = "100",     ["Strategy"] = "SimulatedAnnealing",   ["MIP"] = "False" },
                        //new Dictionary<string, string>() { ["Time"] = "1 000",   ["Strategy"] = "SimulatedAnnealing",   ["MIP"] = "False" },
                        //new Dictionary<string, string>() { ["Time"] = "10 000",  ["Strategy"] = "SimulatedAnnealing",   ["MIP"] = "False" },
                       // new Dictionary<string, string>() { ["Time"] = "60 000",  ["Strategy"] = "SimulatedAnnealing",   ["MIP"] = "False" },
                        //new Dictionary<string, string>() { ["Time"] = "600 000", ["Strategy"] = "SimulatedAnnealing",   ["MIP"] = "False" },
                    }
                 }
            };
            return benchmarkTasks;
        }

        public static List<BenchmarkTask> GetCSOPATest()
        {
            var benchmarkTasks = new List<BenchmarkTask>
            {
                new BenchmarkTask()
                {
                    TemplateDir = "Resources/Benchmark/CSOPA/Templates",
                    Dir = "Resources/Benchmark/CSOPA",
                    Parameters = new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() { ["Time"] = "0",      ["Strategy"] = "GreedyDescent",     ["BID"] = "True" },
                        new Dictionary<string, string>() { ["Time"] = "1 000",   ["Strategy"] = "GuidedLocalSearch", ["BID"] = "True" },
                        new Dictionary<string, string>() { ["Time"] = "10 000",  ["Strategy"] = "GuidedLocalSearch", ["BID"] = "True" },
                        new Dictionary<string, string>() { ["Time"] = "60 000",  ["Strategy"] = "GuidedLocalSearch", ["BID"] = "True" },
                        new Dictionary<string, string>() { ["Time"] = "600 000", ["Strategy"] = "GuidedLocalSearch", ["BID"] = "True" }
                    }
                }
            };
            return benchmarkTasks;
        }

        public static List<BenchmarkTask> GetLaserTest()
        {
            var benchmarkTasks = new List<BenchmarkTask>
            {
                new BenchmarkTask()
                {
                    TemplateDir = "Resources/Benchmark/Celta/Templates",
                    Dir = "Resources/Benchmark/Celta",
                    Parameters = new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() { ["Time"] = "0",         ["Strategy"] = "GreedyDescent",       ["BID"] = "True" },
                        new Dictionary<string, string>() { ["Time"] = "1 000",     ["Strategy"] = "GuidedLocalSearch",   ["BID"] = "True" },
                        new Dictionary<string, string>() { ["Time"] = "10 000",    ["Strategy"] = "GuidedLocalSearch",   ["BID"] = "True" },
                        new Dictionary<string, string>() { ["Time"] = "60 000",    ["Strategy"] = "GuidedLocalSearch",   ["BID"] = "True" },
                        new Dictionary<string, string>() { ["Time"] = "600 000",   ["Strategy"] = "GuidedLocalSearch",   ["BID"] = "True" },
                        new Dictionary<string, string>() { ["Time"] = "1 800 000", ["Strategy"] = "GuidedLocalSearch",   ["BID"] = "True" },
                    }
                }
            };
            return benchmarkTasks;
        }

        public static List<BenchmarkTask> GetCubeTest()
        {
            var benchmarkTasks = new List<BenchmarkTask>
            {
                new BenchmarkTask()
                {
                    TemplateDir = "Resources/Benchmark/Kubik2/Templates",
                    Dir = "Resources/Benchmark/Kubik2",
                    Parameters = new List<Dictionary<string, string>>() {
                          new Dictionary<string, string>() { ["T"] = "0",         ["LSS"] = "GreedyDescent" },
                          new Dictionary<string, string>() { ["T"] = "30 000",    ["LSS"] = "GuidedLocalSearch" },
                          new Dictionary<string, string>() { ["T"] = "60 000",    ["LSS"] = "GuidedLocalSearch" },
                          new Dictionary<string, string>() { ["T"] = "300 000",   ["LSS"] = "GuidedLocalSearch" },
                          new Dictionary<string, string>() { ["T"] = "600 000",   ["LSS"] = "GuidedLocalSearch" },
                    }
                }
            };
            return benchmarkTasks;
        }

        public static List<BenchmarkTask> GetMesterEcsetTest()
        {
            var benchmarkTasks = new List<BenchmarkTask>
            {
                new BenchmarkTask()
                {
                    TemplateDir = "Resources/Benchmark/MesterEcset/Templates",
                    Dir = "Resources/Benchmark/MesterEcset",
                    Parameters = new List<Dictionary<string, string>>()
                    {
                        new Dictionary<string, string>() { ["Time"] = "0",         ["MIP"] = "True", ["Strategy"] = "GreedyDescent", ["BID"] = "True" },

                        //new Dictionary<string, string>() { ["Time"] = "5 000",     ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "10 000",    ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "60 000",    ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "600 000",   ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "1 800 000", ["MIP"] = "True",  ["Strategy"] = "GuidedLocalSearch", ["BID"] = "True" },

                        //new Dictionary<string, string>() { ["Time"] = "5 000",     ["MIP"] = "True",  ["Strategy"] = "TabuSearch", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "10 000",    ["MIP"] = "True",  ["Strategy"] = "TabuSearch", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "60 000",    ["MIP"] = "True",  ["Strategy"] = "TabuSearch", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "600 000",   ["MIP"] = "True",  ["Strategy"] = "TabuSearch", ["BID"] = "True" },
                        //new Dictionary<string, string>() { ["Time"] = "1 800 000", ["MIP"] = "True",  ["Strategy"] = "TabuSearch", ["BID"] = "True" }
                    }
                },
            };
            return benchmarkTasks;
        }
    }
}