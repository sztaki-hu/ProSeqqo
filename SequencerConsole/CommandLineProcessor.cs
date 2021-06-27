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
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
            System.Diagnostics.Process.GetCurrentProcess().PriorityBoostEnabled = true;
            Console.Title = "ProSeqqo Console";
            try
            {
                if (args.Length == 0)
                {
    #if !DEBUG
                        Help(new string[] { "-h" });
    #else
                    {
                        //Debug in VS
                        SeqLogger.LogLevel = LogLevel.Info;
                        string example = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\Example";
                        string castle = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\Example\\CubeCastle";
                        string outdir = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\Example\\Out";

                        //args = new string[] { "-i", example + "\\CelticLaser.seq",                  "-o", outdir + "\\CelticLaser_out.json",                   };
                        //args = new string[] { "-i", example + "\\CelticLaser_Contour.seq",            "-o", outdir + "\\CelticLaser_Contour_out.json",           };
                        //args = new string[] { "-i", example + "\\CelticLaser_Contour_025K.seq",            "-o", outdir + "\\CelticLaser_Contour_025K.json",           };
                        //args = new string[] { "-i", example + "\\CelticLaser_Fill.seq",             "-o", outdir + "\\CelticLaser_Fill_out.json",              };
                        //args = new string[] { "-i", example + "\\CubeCastle.seq",                   "-o", outdir + "\\CubeCastle_out.json",              };
                        //args = new string[] { "-i", example + "\\CubeCastle[3_3_2].seq",                   "-o", outdir + "\\CubeCastle[3_3_2]_out.json",              };
                        //args = new string[] { "-i", example + "\\CSOPA.seq",                        "-o", outdir + "\\CSOPA_DEBUG_out.json",                        };
                        //args = new string[] { "-i", example + "\\CSOPA1.seq",                        "-o", outdir + "\\CSOPA1_DEBUG_out.json",                        };
                        //args = new string[] { "-i", example + "\\CSOPA2.seq",                        "-o", outdir + "\\CSOPA2_DEBUG_out.json",                        };
                        //args = new string[] { "-i", example + "\\CSOPA3.seq",                        "-o", outdir + "\\CSOPA3_DEBUG_out.json",                        };
                        //args = new string[] { "-i", example + "\\Etalon.seq",                       "-o", outdir + "\\etalon_o.json",                         };
                        args = new string[] { "-i", example + "\\EtalonMatrix.seq",                 "-o", outdir + "\\etalonMatrix_o.json",                    };
                        //args = new string[] { "-i", example + "\\Frochliche.seq",                   "-o", outdir + "\\Frochliche.json"                        };
                        //args = new string[] { "-i", example + "\\Hybrid_Matrix.seq",                "-o", outdir + "\\Hybrid_Matrix_out.json",                 };
                        //args = new string[] { "-i", example + "\\Hybrid_Original.seq",              "-o", outdir + "\\Hybrid_Original_out.json",               };
                        //args = new string[] { "-i", example + "\\Kocka.seq",                        "-o", outdir + "\\Kocka_out.json",                        };
                        //args = new string[] { "-i", example + "\\Kocka2.seq",                        "-o", outdir + "\\Kocka_out.json",                        };
                        //args = new string[] { "-i", example + "\\Kubik.seq",                        "-o", outdir + "\\Kubik_out.json",                        };
                        //args = new string[] { "-i", example + "\\MesterEcset.seq",                  "-o", outdir + "\\MesterEcset.json",                       };
                        //args = new string[] { "-i", example + "\\PickAndPlace_Matrix.seq",          "-o", outdir + "\\PickAndPlace_Matrix_out.json",          };
                        //args = new string[] { "-i", example + "\\PickAndPlace_Original.seq",        "-o", outdir + "\\PickAndPlace_Original_out.json"         };
                        //args = new string[] { "-i", example + "\\test1_debug.seq",                    "-o", outdir + "\\test1_debug_out.json" };
                        args = new string[] { "-i", example + "\\Seqtest.seq",                      "-o", outdir + "\\seqtest_o.json",                        };
                        args = new string[] { "-i", example + "\\Hello.seq",                      "-o", outdir + "\\hello_o.txt",                        };
                        args = new string[] { "-i", example + "\\Hello.xml",                      "-o", outdir + "\\hello_o.txt",                        };
                        args = new string[] { "-i", example + "\\Hello.json",                      "-o", outdir + "\\hello_o.txt",                        };
                        //args = new string[] { "-i", example + "\\CubeCastle[3_4_2]_C50_O720.seq",                      "-o", outdir + "\\CubeCastle[3_4_2]_C50_O720.json",                        };
                        //args = new string[] { "-i", castle +  "\\CubeCastle[3_3_1].seq",             "-o", outdir + "\\seqtest_o.json",                        };
                        //args = new string[] { "-i", castle +  "\\CubeCastle[3_3_1].seq",             "-o", outdir + "\\seqtest_o.json",                        };


                        Help(args);
                        Version(args);
                        input = Input(args);
                        output = Output(args);
                        log = LogLevel.Info;
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
                //GeneralTaskSerializer s = new GeneralTaskSerializer();
                //s.ExportJSON(task, output.Replace(".txt", "_export.json"));
                //s.ExportXML(task, output.Replace(".txt", "_export.xml"));
                //s.ExportSEQ(task, output.Replace(".txt", "_export.seq"));
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
            if (args.Length == 1)
            {
                outputType = FormatType.JSON;
                output = input;
                output = output.Replace(".json", "_out.json");
                output = output.Replace(".seq", "_out.json");
                output = output.Replace(".txt", "_out.json");
                output = output.Replace(".SEQ", "_out.json");
                output = output.Replace(".xml", "_out.json");
                return output;
            }
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