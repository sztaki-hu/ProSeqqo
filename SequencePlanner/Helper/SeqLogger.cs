using System;
using System.Collections.Generic;

namespace SequencePlanner.Helper
{
    public static class SeqLogger
    {
        public static LogLevel LogLevel = LogLevel.Trace;
        public static int Indent=0;
        public static List<string> Backlog { get; set; }

        public static void Trace(string message, string nameOfClass = null)
        {
            WriteLog(LogLevel.Trace, message, nameOfClass);
        }

        public static void Debug(string message, string nameOfClass = null)
        {
            WriteLog(LogLevel.Debug, message, nameOfClass);
        }

        public static void Info(string message, string nameOfClass = null)
        {
            WriteLog(LogLevel.Info, message, nameOfClass);
        }

        public static void Warning(string message, string nameOfClass = null)
        {
            WriteLog(LogLevel.Warning, message, nameOfClass);
        }
        public static void Error(string message, string nameOfClass = null)
        {
            WriteLog(LogLevel.Error, message, nameOfClass);
        }

        public static void Critical(string message, string nameOfClass = null)
        {
            WriteLog(LogLevel.Critical, message, nameOfClass);
        }

        public static void InitBacklog()
        {
            Backlog.Clear();
        }

        public static void WriteLog(LogLevel level, string message, string nameOfClass = null)
        {
            Console.ForegroundColor = ConsoleColor.White;
            if (level >= LogLevel)
            {
                switch (level)
                {
                    case LogLevel.Trace: Console.ForegroundColor = ConsoleColor.Magenta; break;
                    case LogLevel.Debug: Console.ForegroundColor = ConsoleColor.Green; break;
                    case LogLevel.Info: Console.ForegroundColor = ConsoleColor.DarkGreen; break;
                    case LogLevel.Warning: Console.ForegroundColor = ConsoleColor.Yellow; break;
                    case LogLevel.Error: Console.ForegroundColor = ConsoleColor.Red; break;
                    case LogLevel.Critical: Console.ForegroundColor = ConsoleColor.DarkRed; break;
                }
                var indent = "";
                for (int i = 0; i < Indent; i++)
                {
                    indent += "  ";
                }
                var log = indent + level.ToString() + "   ";
                Console.Write(indent + level.ToString() + ":\t");
                Console.ForegroundColor = ConsoleColor.White;
                if (nameOfClass != null)
                {
                    Console.Write(nameOfClass + ":\t");
                    log += nameOfClass + ":   ";
                }
                Console.Write(message+"\n");
                log += message;
                if (Backlog == null)
                    Backlog = new List<string>();
                else
                    Backlog.Add(log);
            }
        }

        internal static string ToList(List<double> list)
        {
            string tmp = "";
            for (int i = 0; i < list.Count - 1; i++)
            {
                tmp += list[i].ToString("0.##") + "; ";
            }
            tmp += list[list.Count - 1];
            return tmp;
        }

        public  static string ToList(List<long> list)
        {
            string tmp = "";
            for (int i = 0; i < list.Count - 1; i++)
            {
                tmp += list[i] + ", ";
            }
            tmp += list[list.Count - 1];
            return tmp;
        }

        public static string ToList(List<int> list)
        {
            string tmp = "";
            for (int i = 0; i < list.Count-1; i++)
            {
                tmp += list[i] + ", ";
            }
            tmp += list[list.Count-1];
            return tmp;
        }
    }

    public enum LogLevel
    {
        Trace =    0,
        Debug =    1,
        Info =     2,
        Warning =  3,
        Error =    4,
        Critical = 5,
        Off = 6
    }
}
