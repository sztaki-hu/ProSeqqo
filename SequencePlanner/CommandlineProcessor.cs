﻿using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Mapper;
using SequencePlanner.Phraser.Options;
using SequencePlanner.Phraser.Template;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SequencePlanner
{
    public static class CommandlineProcessor
    {
        private static string input;
        private static string output;
        private static string graphviz;
        private static bool debug;


        public static void CLI(string[] args)
        {
            if (args.Length == 0)
            {
                //Release
                //Help(new string[] { "-h" });

                //Debug
                //DefaultRun();
                args = new string[] {"-i", "test/test10.txt", "-o", "test/test10_out.txt", "-g", "test/test10_graph.dot"};
                Help(args);
                input = Input(args);
                output = Output(args);
                graphviz = GraphVizOutput(args);
                debug = Debug(args);
                Run();
            }
            else
            {
                Help(args);
                input = Input(args);
                output = Output(args);
                graphviz = GraphVizOutput(args);
                debug = Debug(args);
                Run();
            }
        }

        private static void Run()
        {
            if (input != null)
            {
                SeqOptionSet optionSet = new SeqOptionSet();
                SeqGTSPTask.DEBUG = debug;
                optionSet.ReadFile(input);
                optionSet.Validate();
                Template template = SeqOptionsToTemplate.Map(optionSet);
                template.Validate();
                SeqGTSPTask task = template.Compile();
                task.Build();
                var solution = task.Run();
                solution.WriteFull();
                if (output != null)
                {
                    solution.WriteOutputFile(output);
                    Console.WriteLine("Output file created at "+output+"!");
                }

                if (graphviz != null)
                {
                    GraphViz.CreateGraphViz(task.GTSP, graphviz);
                    Console.WriteLine("GraphViz file created at " + graphviz + "!");
                }
                //task.GTSP.Graph.WriteGraph();
            }
        }

        private static void DefaultRun()
        {
            //Help(new string[] { "-h" });
            SeqOptionSet optionSet = new SeqOptionSet();
            optionSet.ReadFile("test/test10.txt");
            optionSet.Validate();
            Template template = SeqOptionsToTemplate.Map(optionSet);
            template.Validate();
            SeqGTSPTask task = template.Compile();
            task.Build();
            task.Run();
            GraphViz.CreateGraphViz(task.GTSP, "test/test10.dot");
            task.GTSP.Graph.WriteGraph();
        }

        private static void Help(string[] args)
        {
            foreach (var item in args)
            {
                if(item.Equals("-help") || item.Equals("-h"))
                {
                    Console.WriteLine("\nCommands:");
                    Console.WriteLine("+-------------+----------+--------------------------------+------------------------------------+\n" +
                                      "|   Command   | Shortcut |           Parameter            |               Comment              |\n" +
                                      "+-------------+----------+--------------------------------+------------------------------------+\n" +
                                      "| -help       |    -h    | -                              |                                    |\n" +
                                      "+-------------+----------+--------------------------------+------------------------------------+\n" +
                                      "| -in         |    -i    | < Input path >                 |                                    |\n" +
                                      "+-------------+----------+--------------------------------+------------------------------------+\n" +
                                      "| -out        |    -o    | < Output path >                |                                    |\n" +
                                      "+-------------+----------+--------------------------------+------------------------------------+\n" +
                                      "| -graphViz   |    -g    | < Graphviz output file paht>   |  GraphViz diagram output file(.dot)|\n" +
                                      "+-------------+----------+--------------------------------+------------------------------------+\n" +
                                      "| -debug      |    -d    | -                              |  Write details of execution.       |\n" +
                                      "+-------------+----------+--------------------------------+------------------------------------+");
                    Console.WriteLine("Example: Sequencer.exe -i test.txt -o outputFile.txt -g visualgraph.dot");
                    var url = "https://git.sztaki.hu/zahoranl/sequenceplanner/";
                    Console.WriteLine("Input file details: "+url);
                    Console.WriteLine("Press [w] to open SZTAKI GitLab!");
                    Console.WriteLine("Press any other key to exit!");
                    if (Console.ReadKey().Key == ConsoleKey.W)
                        System.Diagnostics.Process.Start("explorer", url);
                }
            }
        }
        private static string Input(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-input") || args[i].Equals("-i"))
                {
                    return args[i + 1];
                }
            }
            Console.WriteLine("Input file needed! Use -h/-help command for details!");
            return null; 
        }
        private static string Output(string[] args)
        {
            var findCommand = false;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-output") || args[i].Equals("-o"))
                {
                    findCommand = true;
                    return args[i + 1];
                }
            }
            if (findCommand)
            {
                Console.WriteLine("Output file needed!");
            }
            return null;
        }
        private static string GraphVizOutput(string[] args)
        {
            var findCommand = false;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-graphViz") || args[i].Equals("-g"))
                {
                    findCommand = true;
                    return args[i + 1];
                }
            }
            if (findCommand)
            {
                Console.WriteLine("GraphViz output file path needed! Use -h/-help command for details!");
            }
            return null;
        }
        private static bool Debug(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-debug") || args[i].Equals("-d"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
