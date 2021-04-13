using System;
using System.Collections.Generic;

namespace SequencePlanner.Helper
{
    public static class SeqLogger
    {
        private static LogLevel logLevel = LogLevel.Trace;
        private static int indent = 0;
        private static List<string> backlog = new List<string>();

        public static LogLevel LogLevel { get => logLevel; set => logLevel = value; }
        public static bool UseIndent { get; set; }
        public static int Indent { get => indent; set => indent = value; }
        public static List<string> Backlog { get { return backlog; } set { backlog = value; } }


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
                if(UseIndent)
                    for (int i = 0; i < Indent; i++)
                    {
                        indent += "   ";
                    }
                var log = level.ToString()+":  " + indent;
                Console.Write(level.ToString()+ ":  " + indent);
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
    }
}