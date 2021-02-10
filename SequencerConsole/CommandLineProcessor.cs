using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Serialization.Result;
using SequencePlanner.GTSPTask.Serialization.Task;
using SequencePlanner.GTSPTask.Task.LineLike;
using SequencePlanner.GTSPTask.Task.PointLike;
using SequencePlanner.Helper;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace SequencerConsole
{
    public class CommandLineProcessor
    {
        private static readonly bool RUN_IN_VISUALSTUDIO = false;
        private static readonly LogLevel LOG_LEVEL = LogLevel.Trace;

        private static string input;
        private static FormatType inputType = FormatType.Unknown;
        private static string output;
        private static FormatType outputType = FormatType.Unknown;
        private static TaskType taskType = TaskType.Unknown;
        private static bool debug;
        private static bool convert;
        private static bool validate;
        private static LogLevel log = LogLevel.Info;

        public static void CLI(string[] args)
        {
            if (args.Length == 0)
            {
                #if !DEBUG
                    Help(new string[] { "-h" });
                #else
                {
                    SeqLogger.LogLevel = LOG_LEVEL;
                    //Debug in VS
                    string example = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\Example";
                    string outdir = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\Example\\out";
                    string graph = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\Example\\graph";

                    //args = new string[] { "-i", example + "\\PickAndPlace_Original.txt",        "-o", outdir + "\\PickAndPlace_Original_out.json"         };
                    //args = new string[] { "-i", example + "\\PickAndPlace_Matrix.txt",          "-o", outdir + "\\PickAndPlace_Matrix_out.json",          };
                    //args = new string[] { "-i", example + "\\PickAndPlace_Matrix.txt",          "-o", outdir + "\\PickAndPlace_Matrix_out.txt",           };
                    //args = new string[] { "-i", example + "\\LineLike_Original.txt",            "-o", outdir + "\\LineLike_Original_out.json",            };
                    //args = new string[] { "-i", example + "\\LineLike_Matrix.txt",              "-o", outdir + "\\LineLike_Matrix_out.json",              };
                    //args = new string[] { "-i", example + "\\LineLike_Matrix.txt",              "-o", outdir + "\\LineLike_Matrix_out.seq",               };
                    //args = new string[] { "-i", example + "\\Kocka.txt",                        "-o", outdir + "\\Kocka_out.json",                        };
                    //args = new string[] { "-i", example + "\\CSOPA.txt",                        "-o", outdir + "\\CSOPA_out.json",                        };
                    //args = new string[] { "-i", example + "\\CelticLaser_Contour.txt",          "-o", outdir + "\\CelticLaser_Contour_out.txt",           };
                    //args = new string[] { "-i", example + "\\CelticLaser_Fill.txt",             "-o", outdir + "\\CelticLaser_Fill_out.txt",              };
                    //args = new string[] { "-i", example + "\\CelticLaser.txt",                  "-o", outdir + "\\CelticLaser_out.txt",                   };
                    //args = new string[] { "-i", example + "\\PointLike_PosProcPrecedences.txt", "-o", outdir + "\\PointLike_PosProcPrecedences_out.txt"   };
                    //args = new string[] { "-i", example + "\\PointLike_PosPrecedences.txt",     "-o", outdir + "\\PointLike_PosPrecedences_out.txt"       };
                    //args = new string[] { "-i", example + "\\LocalTests/DEV_LL.txt",            "-o", outdir + "\\LocalTests/DEV_LL_out.txt",             };
                    args = new string[] { "-i", example + "\\seqtest.txt",                        "-o", outdir + "\\seqtest_o.json",                        };
                    //args = new string[] { "-i", example + "\\seqtest2.txt",                     "-o", outdir + "\\seqtest2_o.json",                       };
                    //args = new string[] { "-i", example + "\\etalon.txt",                       "-o", outdir + "\\etalon_o.json",                         };
                    //args = new string[] { "-i", example + "\\etalonMatrix.txt",                 "-o", outdir + "\\etalonMatrix_o.seq",                    };
                    //args = new string[] { "-i", example + "\\seqtest3.txt",                     "-o", outdir + "\\seqtest3_o.json",                       };
                    //args = new string[] { "-i", example + "\\seq_fill_half.txt",                "-o", outdir + "\\seq_fill_half_o.json",                  };
                    //args = new string[] { "-i", example + "\\seq_contours_half.txt",            "-o", outdir + "\\seq_contours_half.json"                 };

                    Help(args);
                    Version(args);
                    input = Input(args);
                    output = Output(args);
                    debug = Debug(args);
                    validate = Validate(args);
                    log = LOG_LEVEL;
                    Run();
                }
                #endif
            }
            else
            {
                Help(args);
                Version(args);
                input = Input(args);
                output = Output(args);
                validate = Validate(args);
                convert = Convert(args);
                log = Log(args);
                if (convert)
                    Convert();
                else
                    Run();
            }
        }


        private static void Convert()
        {
            try
            {
                SeqLogger.LogLevel = log;
                if (taskType != TaskType.Unknown)
                {
                    if (taskType == TaskType.LineLike)
                        ConvertLineLike();
                    
                    if (taskType == TaskType.PoitnLike)
                        ConvertPointLike();
                    
                }
            }
            catch (Exception e)
            {
                SeqLogger.Critical(e.ToString());
            }
        }

        private static void ConvertPointLike()
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
                switch (outputType)
                {
                    case FormatType.SEQ:
                    case FormatType.TXT:
                        ser.ExportSEQ(task, output);
                        break;
                    case FormatType.JSON:
                        ser.ExportJSON(task, output);
                        break;
                    case FormatType.XML:
                        ser.ExportXML(task, output);
                        break;
                    default:
                        throw new TypeLoadException("Output file should be .txt/.seq/.json/.xml!");
                }
            }
        }

        private static void ConvertLineLike()
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
                switch (outputType)
                {
                    case FormatType.SEQ:
                    case FormatType.TXT:
                        ser.ExportSEQ(task, output);
                        break;
                    case FormatType.JSON:
                        ser.ExportJSON(task, output);
                        break;
                    case FormatType.XML:
                        ser.ExportXML(task, output);
                        break;
                    default:
                        throw new TypeLoadException("Output file should be .txt/.seq/.json/.xml!");
                }
            }
        }

        private static void Run()
        {
            try
            {
                SeqLogger.LogLevel = log;
                if (taskType != TaskType.Unknown)
                {
                    if (taskType == TaskType.LineLike)
                    {
                        var result = RunLineLike();
                        OutLineLike(result);
                    }
                    if (taskType == TaskType.PoitnLike)
                    {
                        var result = RunPointLike();
                        //result.ToLog(LogLevel.Info);
                        OutPointLine(result);
                    }
                }
                else
                    throw new SequencerException("Unknown task type!", "It should be TaskType: LineLike or PointLike");
            }
            catch (Exception e)
            {
                SeqLogger.Critical(e.ToString());
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
                    Console.WriteLine("\nSequencePlanner \u00a9 SZTAKI \nVersion: " + AssemblyName.GetAssemblyName("SequencePlanner.dll").Version);
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
                                      //"+-------------+----------+--------------------------------+------------------------------------+\n" +
                                      //"| -debug      |    -d    | -                              |  Write details of execution.       |\n" +
                                      "+-------------+----------+--------------------------------+------------------------------------+\n" +
                                      "| -convert    |    -c    | -in -out                       |  Change task format of -in to -out |\n" +
                                      "|             |          |                                |  t.seq/.txt/.json/.xml             |\n" +
                                      "+-------------+----------+--------------------------------+------------------------------------+\n" +
                                      "| -log        |    -l    | Trace                          |  Write details of execution.       |\n" +
                                      "|             |          | Debug                          |                                    |\n" +
                                      "|             |          | Info                           |  Default                           |\n" +
                                      "|             |          | Warning                        |                                    |\n" +
                                      "|             |          | Error                          |                                    |\n" +
                                      "|             |          | Critical                       |                                    |\n" +
                                      "+-------------+----------+--------------------------------+------------------------------------+\n" +
                                      "| -version    |    -v    |                                |  Version info                      |\n" +
                                      "+-------------+----------+--------------------------------+------------------------------------+\n");


                    Console.WriteLine("Example: Sequencer.exe -i test.txt -o outputFile.txt -l Info");
                    var url = "https://git.sztaki.hu/zahoranl/sequenceplanner/";
                    Console.WriteLine("Input file details: " + url);
                    Console.WriteLine("Press [w] to open SZTAKI GitLab!");
                    Console.WriteLine("Press any other key to exit!");
                    if (Console.ReadKey().Key == ConsoleKey.W)
                        System.Diagnostics.Process.Start("explorer", url);
                    System.Environment.Exit(0);
                }
            }
        }

        private static void Version(string[] args)
        {
            foreach (var item in args)
            {
                if (item.Equals("-version") || item.Equals("-v"))
                {
                    Console.WriteLine("SequencePlanner \u00a9 SZTAKI \nVersion: " + AssemblyName.GetAssemblyName("SequencePlanner.dll").Version+"\n");
                    System.Environment.Exit(0);
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
                    return (args[i + 1].ToUpper()) switch
                    {
                        "TRACE" => LogLevel.Trace,
                        "DEBUG" => LogLevel.Debug,
                        "INFO" => LogLevel.Info,
                        "WARNING" => LogLevel.Warning,
                        "ERROR" => LogLevel.Error,
                        "CRITICAL" => LogLevel.Critical,
                        _ => throw new SequencerException("Unkonwn loglevel! Use -help/-h for more details."),
                    };
                }
            }
            return LogLevel.Info;
        }

        private static bool Convert(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-convert") || args[i].Equals("-c"))
                {
                    return true;
                }
            }
            return false;
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
                if (line.Contains("LineLike"))
                    return TaskType.LineLike;
                if (line.Contains("PointLike"))
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