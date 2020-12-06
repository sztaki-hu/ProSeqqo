using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Serialization.Result;
using SequencePlanner.GTSPTask.Serialization.Task;
using SequencePlanner.GTSPTask.Task.LineLike;
using SequencePlanner.GTSPTask.Task.PointLike;
using SequencePlanner.Helper;
using System;
using System.IO;
using System.Threading;

namespace SequencerConsole
{
    public class CommandLineProcessor
    {
        private static readonly bool RUN_IN_VISUALSTUDIO = true;
        private static readonly LogLevel LOG_LEVEL = LogLevel.Trace;

        private static string input;
        private static FormatType inputType = FormatType.Unknown;
        private static string output;
        private static FormatType outputType = FormatType.Unknown;
        private static TaskType taskType = TaskType.Unknown;
        private static bool debug;
        private static bool validate;
        private static LogLevel log = LogLevel.Info;

        public static void CLI(string[] args)
        {
            SeqLogger.LogLevel = LOG_LEVEL;
            if (args.Length == 0)
            {
                if (!RUN_IN_VISUALSTUDIO)
                {
                    //Release
                    Help(new string[] { "-h" });
                }
                else
                {
                    //Debug in VS
                    string example = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\Example";
                    string outdir = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\Example\\out";
                    string graph = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\Example\\graph";

                    //args = new string[] { "-i", example + "\\PickAndPlace_Original.txt",        "-o", outdir + "\\PickAndPlace_Original_out.json",       "-g", graph + "\\PickAndPlace_Original_graph.dot",        "-d" };
                    //args = new string[] { "-i", example + "\\PickAndPlace_Matrix.txt",          "-o", outdir + "\\PickAndPlace_Matrix_out.json",          "-g", graph + "\\PickAndPlace_Matrix_graph.dot",          "-d" };
                    //args = new string[] { "-i", example + "\\PickAndPlace_Matrix.txt",          "-o", outdir + "\\PickAndPlace_Matrix_out.txt",          "-g", graph + "\\PickAndPlace_Matrix_graph.dot",          "-d" };
                    args = new string[] { "-i", example + "\\LineLike_Original.txt",            "-o", outdir + "\\LineLike_Original_out.json",           "-g", graph + "\\LineLike_Original_graph.dot",            "-d" };
                    //args = new string[] { "-i", example + "\\LineLike_Matrix.txt",              "-o", outdir + "\\LineLike_Matrix_out.json",             "-g", graph + "\\LineLike_Matrix_graph.dot",              "-d" };
                    //args = new string[] { "-i", example + "\\Kocka.txt",                        "-o", outdir + "\\Kocka_out.txt",                        "-g", graph + "\\Kocka_graph.dot",                        "-d" };
                    //args = new string[] { "-i", example + "\\CSOPA.txt",                        "-o", outdir + "\\CSOPA_out.json",                        "-g", graph + "\\CSOPA_graph.dot",                        "-d" };
                    //args = new string[] { "-i", example + "\\CelticLaser_Contour.txt",          "-o", outdir + "\\CelticLaser_Contour_out.txt",          "-g", graph + "\\CelticLaser_Contour_graph.dot",          "-d" };
                    //args = new string[] { "-i", example + "\\CelticLaser_Fill.txt",             "-o", outdir + "\\CelticLaser_Fill_out.txt",             "-g", graph + "\\CelticLaser_Fill_graph.dot",             "-d" };
                    //args = new string[] { "-i", example + "\\CelticLaser.txt",                  "-o", outdir + "\\CelticLaser_out.txt",                  "-g", graph + "\\CelticLaser_graph.dot",                  "-d" };
                    //args = new string[] { "-i", example + "\\PointLike_PosProcPrecedences.txt", "-o", outdir + "\\PointLike_PosProcPrecedences_out.txt", "-g", graph + "\\PointLike_PosProcPrecedences_graph.dot", "-d" };
                    //args = new string[] { "-i", example + "\\PointLike_PosPrecedences.txt",     "-o", outdir + "\\PointLike_PosPrecedences_out.txt",     "-g", graph + "\\PointLike_PosPrecedences_graph.dot",     "-d" };
                    //args = new string[] { "-i", example + "\\LocalTests/DEV_LL.txt",            "-o", outdir + "\\LocalTests/DEV_LL_out.txt",            "-g", graph + "\\LocalTests/DEV_LL_graph.dot",            "-d" };
                    //args = new string[] { "-i", example + "\\seqtest.txt",                      "-o", outdir + "\\seqtest_o.json",                        "-g", graph + "\\seqtest_graph.dot",                      "-d" };
                    

                    Help(args);
                    input = Input(args);
                    output = Output(args);
                    debug = Debug(args);
                    validate = Validate(args);
                    log = LOG_LEVEL;
                    Run();
                }
            }
            else
            {
                Help(args);
                input = Input(args);
                output = Output(args);
                debug = Debug(args);
                validate = Validate(args);
                log = Log(args);
                Run();
            }
        }

        private static void Run()
        {
            try
            {
                SeqLogger.LogLevel = log;
                if(taskType!= TaskType.Unknown)
                {
                    if (taskType == TaskType.LineLike)
                    {
                        var result = RunLineLike();
                        OutLineLike(result);
                    }
                    if (taskType == TaskType.PoitnLike)
                    {
                        var result = RunPointLike();
                        OutPointLine(result);
                    }
                }
            }
            catch (Exception e)
            {
                if (debug)
                    Console.WriteLine(e);
                else
                    Console.WriteLine(e.Message);
            }
        }

        private static LineTaskResult RunLineLike()
        {
            if (inputType != FormatType.Unknown && input != null)
            {
                LineLikeTaskSerializer ser = new LineLikeTaskSerializer();
                LineLikeTask task;
                switch (inputType)
                {
                    case FormatType.SEQ:
                    case FormatType.TXT:
                        task = ser.ImportSEQ(input);
                        break;
                    case FormatType.JSON:
                        task = ser.ImportJSON(input);
                        break;
                    case FormatType.XML:
                        task = ser.ImportXML(input);
                        break;
                    default:
                        throw new TypeLoadException("Input file should be .txt/.seq/.json/.xml!");
                }
                if (validate)
                    task.ValidateModel();
                return task.RunModel();
                //var ct = new CancellationToken();
                //WaitForSolution("Solver running!", ct);
            }
            return null;
        }

        private static PointTaskResult RunPointLike()
        {
            if (inputType != FormatType.Unknown && input != null)
            {
                PointLikeTaskSerializer ser = new PointLikeTaskSerializer();
                PointLikeTask task;
                switch (inputType)
                {
                    case FormatType.SEQ:
                    case FormatType.TXT:
                        task = ser.ImportSEQ(input);
                        break;
                    case FormatType.JSON:
                        task = ser.ImportJSON(input);
                        break;
                    case FormatType.XML:
                        task = ser.ImportXML(input);
                        break;
                    default:
                        throw new TypeLoadException("Input file should be .txt/.seq/.json/.xml!");
                }
                if (validate)
                    task.ValidateModel();
                return task.RunModel();
                //var ct = new CancellationToken();
                //WaitForSolution("Solver running!", ct);
            }
            return null;
        }
        private static void OutPointLine(PointTaskResult result)
        {
            if (output != null && outputType != FormatType.Unknown)
            {
                PointLikeResultSerializer ser = new PointLikeResultSerializer();
                switch (outputType)
                {
                    case FormatType.SEQ:
                    case FormatType.TXT:
                        ser.ExportSEQ(result,output);
                        break;
                    case FormatType.JSON:
                        ser.ExportJSON(result, output);
                        break;
                    case FormatType.XML:
                        ser.ExportXML(result, output);
                        break;
                    default:
                        throw new TypeLoadException("Output file should be .txt/.seq/.json/.xml!");
                }
                //System.Diagnostics.Process.Start(@output);
            }
        }
        private static void OutLineLike(LineTaskResult result)
        {
            if (output != null && outputType != FormatType.Unknown)
            {
                LineLikeResultSerializer ser = new LineLikeResultSerializer();
                switch (outputType)
                {
                    case FormatType.SEQ:
                    case FormatType.TXT:
                        ser.ExportSEQ(result, output);
                        break;
                    case FormatType.JSON:
                        ser.ExportJSON(result, output);
                        break;
                    case FormatType.XML:
                        ser.ExportXML(result, output);
                        break;
                    default:
                        throw new TypeLoadException("Output file should be .txt/.seq/.json/.xml!");
                }
                //System.Diagnostics.Process.Start(@output);
            }
        }

        private static void Help(string[] args)
        {
            foreach (var item in args)
            {
                if (item.Equals("-help") || item.Equals("-h"))
                {
                    Console.WriteLine("\nCommands:");
                    Console.WriteLine("+-------------+----------+--------------------------------+------------------------------------+\n" +
                                      "|   Command   | Shortcut |           Parameter            |               Comment              |\n" +
                                      "+-------------+----------+--------------------------------+------------------------------------+\n" +
                                      "| -help       |    -h    | -                              |                                    |\n" +
                                      "+-------------+----------+--------------------------------+------------------------------------+\n" +
                                      "| -in         |    -i    | < Input path >                 |  .seq/.txt/.json/.xml              |\n" +
                                      "+-------------+----------+--------------------------------+------------------------------------+\n" +
                                      "| -out        |    -o    | < Output path >                |  .seq/.txt/.json/.xml              |\n" +
                                      //"+-------------+----------+--------------------------------+------------------------------------+\n" +
                                      //"| -graphViz   |    -g    | < Graphviz output file paht>   |  GraphViz diagram output file(.dot)|\n" +
                                      "+-------------+----------+--------------------------------+------------------------------------+\n" +
                                      "| -debug      |    -d    | -                              |  Write details of execution.       |\n" +
                                      "+-------------+----------+--------------------------------+------------------------------------+\n" +
                                      "| -log        |    -l    | Trace                          |  Write details of execution.       |\n" +
                                      "|             |          | Debug                          |                                    |\n" +                                      "|             |          | Info                           |                                    |\n" +
                                      "|             |          | Info                           |  Default                           |\n" +                                      "|             |          | Info                           |                                    |\n" +
                                      "|             |          | Warning                        |                                    |\n" +                                      "|             |          | Info                           |                                    |\n" +
                                      "|             |          | Error                          |                                    |\n" +                                      "|             |          | Info                           |                                    |\n" +
                                      "|             |          | Critical                       |                                    |\n" +                                      "|             |          | Info                           |                                    |\n" +
                                      "|             |          | Off                            |                                    |\n" +                                      "|             |          | Info                           |                                    |\n" +
                                      "+-------------+----------+--------------------------------+------------------------------------+\n");


                    Console.WriteLine("Example: Sequencer.exe -i test.txt -o outputFile.txt -g visualgraph.dot");
                    var url = "https://git.sztaki.hu/zahoranl/sequenceplanner/";
                    Console.WriteLine("Input file details: " + url);
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
                    var filename = args[i + 1].Split(".");
                    if (filename != null && filename.Length > 1)
                    {
                        if (filename[1].ToUpper() == "SEQ")
                            inputType = FormatType.SEQ;
                        if (filename[1].ToUpper() == "TXT")
                            inputType = FormatType.TXT;
                        if (filename[1].ToUpper() == "JSON")
                            inputType = FormatType.JSON;
                        if (filename[1].ToUpper() == "XML")
                            inputType = FormatType.XML;
                        if (inputType == FormatType.Unknown)
                            throw new TypeLoadException("Input file should be .txt/.seq/.json/.xml:" + args[i + 1]);
                    }
                    else
                    {
                        throw new TypeLoadException("Input file should be .txt/.seq/.json/.xml:" + args[i + 1]);
                    }
                    taskType = CheckTaskType(args[i + 1]);
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
                    var filename = args[i + 1].Split(".");
                    if (filename!=null && filename.Length>1)
                    {
                        if (filename[1].ToUpper() == "SEQ")
                            outputType = FormatType.SEQ;
                        if (filename[1].ToUpper() == "TXT")
                            outputType = FormatType.TXT;
                        if (filename[1].ToUpper() == "JSON")
                            outputType = FormatType.JSON;
                        if (filename[1].ToUpper() == "XML")
                            outputType = FormatType.XML;
                        if (outputType == FormatType.Unknown)
                            throw new TypeLoadException("Output file should be .txt/.seq/.json/.xml:"+args[i + 1]);
                    }
                    else
                    {
                        throw new TypeLoadException("Output file should be .txt/.seq/.json/.xml:" + args[i + 1]);
                    }
                    return args[i + 1];
                }
            }
            //Console.WriteLine("Output file needed!");
            return null;
        }
        private static LogLevel Log(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-log") || args[i].Equals("-l"))
                {
                    switch (args[i + 1])
                    {
                        case "Trace:":    return LogLevel.Trace;
                        case "Debug:":    return LogLevel.Debug;
                        case "Info:":     return LogLevel.Info;
                        case "Warning:":  return LogLevel.Warning;
                        case "Error:":    return LogLevel.Error;
                        case "Critical:": return LogLevel.Critical;
                        default: throw new SequencerException("Unkonwn loglevel! Use -help/-h for more details.");
                    }
                }
            }
            return LogLevel.Info;
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

        public static TaskType CheckTaskType(string input)
        {
            foreach (var line in File.ReadAllLines(input))
            {
                if (line.Contains("Line_Like"))
                    return TaskType.LineLike;
                if (line.Contains("Point_Like"))
                    return TaskType.PoitnLike;
            }
            return TaskType.Unknown;
        }
        public static void WaitForSolution(string text, CancellationToken token)
        {
            var counter = 0;
            while(token.IsCancellationRequested)
            {
                switch (counter % 4)
                {
                    case 0: Console.Write("/" + text); break;
                    case 1: Console.Write("-" + text); break;
                    case 2: Console.Write("\\"+ text); break;
                    case 3: Console.Write("|" + text); break;
                }
                Console.SetCursorPosition(Console.CursorLeft - (1+text.Length), Console.CursorTop);
                counter++;
                Thread.Sleep(100);
            }
        }
    }

    public enum FormatType
    {
        JSON,
        XML,
        TXT,
        SEQ,
        Unknown
    }
    public enum TaskType
    {
        LineLike,
        PoitnLike,
        Unknown
    }
}