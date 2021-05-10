using SequencePlanner.GTSPTask.Serialization.Result;
using SequencePlanner.GTSPTask.Serialization.Task;
using SequencePlanner.Helper;
using SequencePlanner.Task;
using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace SequencerConsole
{
    public class CommandLineProcessor
    {
        private static string input;
        private static FormatType inputType = FormatType.Unknown;
        private static string output;
        private static FormatType outputType = FormatType.Unknown;
        private static TaskType taskType = TaskType.Unknown;
        private static bool convert;
        private static LogLevel log = LogLevel.Info;

        public static void CLI(string[] args)
        {
            Console.Title = "RobotSEQ Console";
            try
            {
                if (args.Length == 0)
                {
    #if !DEBUG
                        Help(new string[] { "-h" });
    #else
                    {
                        //Debug in VS
                        SeqLogger.LogLevel = LogLevel.Trace;
                        string example = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\Example";
                        string castle = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\Example\\CubeCastle";
                        string outdir = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\Example\\Out";

                        //args = new string[] { "-i", example + "\\CelticLaser.seq",                  "-o", outdir + "\\CelticLaser_out.json",                   };
                        //args = new string[] { "-i", example + "\\CelticLaser_Contour.seq",          "-o", outdir + "\\CelticLaser_Contour_out.json",           };
                        //args = new string[] { "-i", example + "\\CelticLaser_Fill.seq",             "-o", outdir + "\\CelticLaser_Fill_out.json",              };
                        //args = new string[] { "-i", example + "\\CSOPA.seq",                        "-o", outdir + "\\CSOPA_out.json",                        };
                        //args = new string[] { "-i", example + "\\Etalon.seq",                       "-o", outdir + "\\etalon_o.json",                         };
                        args = new string[] { "-i", example + "\\EtalonMatrix.seq",                 "-o", outdir + "\\etalonMatrix_o.json",                    };
                        //args = new string[] { "-i", example + "\\Frochliche.seq",                   "-o", outdir + "\\Frochliche.json"                        };
                        //args = new string[] { "-i", example + "\\Hybrid_Matrix.seq",                "-o", outdir + "\\Hybrid_Matrix_out.json",                 };
                        //args = new string[] { "-i", example + "\\Hybrid_Original.seq",              "-o", outdir + "\\Hybrid_Original_out.json",               };
                        //args = new string[] { "-i", example + "\\Kocka.seq",                        "-o", outdir + "\\Kocka_out.json",                        };
                        //args = new string[] { "-i", example + "\\Kocka2.seq",                        "-o", outdir + "\\Kocka_out.json",                        };
                        //args = new string[] { "-i", example + "\\Kubik.seq",                        "-o", outdir + "\\Kubik_out.json",                        };
                        //args = new string[] { "-i", example + "\\MesterEcset.seq",                  "-o", outdir + "\\MesterEcset.json",                       };
                        //args = new string[] { "-i", example + "\\MesterEcsetOrigin.seq",                  "-o", outdir + "\\MesterEcsetOrigin.json",                       };
                        //args = new string[] { "-i", example + "\\PickAndPlace_Matrix.seq",          "-o", outdir + "\\PickAndPlace_Matrix_out.json",          };
                        //args = new string[] { "-i", example + "\\PickAndPlace_Original.seq",        "-o", outdir + "\\PickAndPlace_Original_out.json"         };
                        //args = new string[] { "-i", example + "\\Seqtest.seq",                      "-o", outdir + "\\seqtest_o.json",                        };
                        //args = new string[] { "-i", castle + "\\CubeCastle[3_3_1].seq",             "-o", outdir + "\\seqtest_o.json",                        };

                        Help(args);
                        Version(args);
                        input = Input(args);
                        output = Output(args);
                        log = LogLevel.Trace;
                        SeqLogger.UseIndent = true;
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
                    convert = Convert(args);
                    log = Log(args);
                    if (convert)
                        Convert();
                    else
                        Run();
                }
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            } catch(Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }
        }


        private static void Convert()
        {
            try
            {
                SeqLogger.LogLevel = log;
                if (taskType != TaskType.Unknown)
                {
                    if (taskType == TaskType.General)
                        ConvertGeneral();
                }
            }
            catch (Exception e)
            {
                SeqLogger.Critical(e.ToString());
            }
        }

        private static void ConvertGeneral()
        {
            if (inputType != FormatType.Unknown && input != null)
            {
                GeneralTaskSerializer ser = new GeneralTaskSerializer();
                GeneralTask task = inputType switch
                {
                    FormatType.SEQ or FormatType.TXT => ser.ImportSEQ(input),
                    FormatType.JSON => ser.ImportJSON(input),
                    FormatType.XML => ser.ImportXML(input),
                    _ => throw new TypeLoadException("Input file should be .txt/.seq/.json/.xml!"),
                };
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
                    if (taskType == TaskType.General)
                    {
                        var result = RunTask();
                        result.ToLog(LogLevel.Info);
                        OutGeneral(result);
                    }
                }
                else
                    throw new SeqException("Unknown task type!", "It should be Task: General");
            }
            catch (Exception e)
            {
                SeqLogger.Critical(e.ToString());
                Console.ReadKey();
            }
        }

        private static GeneralTaskResult RunTask()
        {
            if (inputType != FormatType.Unknown && input != null)
            {
                GeneralTaskSerializer ser = new GeneralTaskSerializer();
                GeneralTask task = inputType switch
                {
                    FormatType.SEQ or FormatType.TXT => ser.ImportSEQ(input),
                    FormatType.JSON => ser.ImportJSON(input),
                    FormatType.XML => ser.ImportXML(input),
                    _ => throw new TypeLoadException("Input file should be .txt/.seq/.json/.xml!"),
                };
                return task.Run();
                //var ct = new CancellationToken();
                //WaitForSolution("Solver running!", ct);
            }
            return null;
        }
        private static void OutGeneral(GeneralTaskResult result)
        {
            if (output != null && outputType != FormatType.Unknown)
            {
                GeneralResultSerializer ser = new GeneralResultSerializer();
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
                    Console.WriteLine("SequencePlanner \u00a9 SZTAKI \nVersion: " + AssemblyName.GetAssemblyName("SequencePlanner.dll").Version + "\n");
                    System.Environment.Exit(0);
                }
            }
        }

        private static string Input(string[] args)
        {
            foreach (var item in args)
            {
                Console.WriteLine(item);
            }
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-input") || args[i].Equals("-i") || args.Length==1)
                {
                    string[] filename = null;
                    if (args.Length==1)
                        filename = args[0].Split(".");
                    else
                        filename = args[i + 1].Split(".");
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
                            throw new TypeLoadException("Input file should be .txt/.seq/.json/.xml");
                    }
                    else
                    {
                        throw new TypeLoadException("Input file should be .txt/.seq/.json/.xml.");
                    }
                    if(args.Length == 1)
                    {
                        taskType = CheckTaskType(args[0]);
                        return args[0];
                    }
                    else
                    {
                        taskType = CheckTaskType(args[i + 1]);
                        return args[i + 1];
                    }
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
                    if (filename != null && filename.Length > 1)
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
                            throw new TypeLoadException("Output file should be .txt/.seq/.json/.xml:" + args[i + 1]);
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
                        _ => throw new SeqException("Unkonwn loglevel! Use -help/-h for more details."),
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

        public static TaskType CheckTaskType(string input)
        {
            if (!File.Exists(input))
                SeqLogger.Critical("File not exists: " + input);
            foreach (var line in File.ReadAllLines(input))
            {
                if (line.ToUpper().Contains("General".ToUpper()))
                    return TaskType.General;
            }
            return TaskType.Unknown;
        }

        public static void WaitForSolution(string text, CancellationToken token)
        {
            var counter = 0;
            while (token.IsCancellationRequested)
            {
                switch (counter % 4)
                {
                    case 0: Console.Write("/" + text); break;
                    case 1: Console.Write("-" + text); break;
                    case 2: Console.Write("\\" + text); break;
                    case 3: Console.Write("|" + text); break;
                }
                Console.SetCursorPosition(Console.CursorLeft - (1 + text.Length), Console.CursorTop);
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
        Line,
        General,
        Unknown
    }
}