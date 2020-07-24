﻿using SequencePlanner.GTSP;
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
        private static readonly bool RUN_IN_VISUALSTUDIO = true;

        private static string input;
        private static string output;
        private static string graphviz;
        private static bool debug;
        private static bool validate;

        public static ORToolsResult CLI(string[] args)
        {
            if (args.Length == 0)
            {
                if (!RUN_IN_VISUALSTUDIO) {
                    //Release
                    Help(new string[] { "-h" });
                    return null;
                }
                else
                {
                    //Debug in VS
                    args = new string[] { "-i", "example/test10.txt", "-o", "example/test10_out.txt", "-g", "example/test10_graph.dot" };
                    //args = new string[] { "-i", "example/test10mx.txt", "-o", "example/test10mx_out.txt", "-g", "example/test10mx_graph.dot", "-d" };
                    //args = new string[] {"-i", "example/test_cam_pnp.txt", "-o", "example/test_cam_pnp_out.txt", "-g", "example/test_cam_pnp_graph.dot" };
                    args = new string[] {"-i", "example/LineLike.txt", "-o", "example/LineLike_out.txt", "-g", "example/LineLike_g.dot", "-d" };
                    //args = new string[] {"-i", "example/Kocka.txt", "-o", "example/Kocka_out.txt", "-g", "example/Kocka_graph.dot", "-d" };
                    //args = new string[] {"-i", "example/CSOPA.txt", "-o", "example/CSOPA_out.txt", "-g", "example/CSOPA_g.dot", "-d" };
                    Help(args);
                    input = Input(args);
                    output = Output(args);
                    graphviz = GraphVizOutput(args);
                    debug = Debug(args);
                    validate = Validate(args);
                    return Run();
                }
            }
            else
            {
                Help(args);
                input = Input(args);
                output = Output(args);
                graphviz = GraphVizOutput(args);
                debug = Debug(args);
                validate = Validate(args);
                return Run();
            }
        }

        private static ORToolsResult Run()
        {
            try
            {
                if (input != null)
                {
                    TemplateManager.DEBUG = debug;
                    TemplateManager manager = new TemplateManager();
                    var solution = manager.Solve(input, validate);
                    if (solution != null)
                    {
                        solution.WriteFull();
                        if (output != null)
                        {
                            solution.WriteOutputFile(output);
                        }
                    }
                    if (graphviz != null)
                    {
                        solution.CreateGraphViz(graphviz);
                    }
                    return solution;
                }
                return null;
            }
            catch(Exception e)
            {
                if(TemplateManager.DEBUG)
                    Console.WriteLine(e);
                else
                    Console.WriteLine(e.Message);
                return null;
            }
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
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-output") || args[i].Equals("-o"))
                {
                    return args[i + 1];
                }
            }
            //Console.WriteLine("Output file needed!");
            return null;
        }
        private static string GraphVizOutput(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-graphViz") || args[i].Equals("-g"))
                {
                    return args[i + 1];
                }
            }
            //Console.WriteLine("GraphViz output file path needed! Use -h/-help command for details!");
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
        private static bool Validate(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-notvalidate") || args[i].Equals("-nv"))
                {
                    return false;
                }
            }
            return true;
        }
    }
}