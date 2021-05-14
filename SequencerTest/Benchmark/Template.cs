using SequencePlanner.GTSPTask.Serialization.Result;
using SequencePlanner.GTSPTask.Serialization.Task;
using SequencePlanner.Helper;
using SequencePlanner.Task;
using System;
using System.Collections.Generic;
using System.IO;

namespace SequencerTest.Benchmark
{
    public class Template
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string GeneratePath { get; set; }
        public string OutPath { get; set; }
        public TaskType TaskType { get; set; }
        public FormatType FormatType { get; set; }
        public List<Dictionary<string, string>> ParameterCombinatrions { get; set; }
        public List<string> GeneretedTasks { get; set; }
        public List<GeneralTaskResult> PointResults { get; set; }

        public Template(string path, string generatePath, string outPath, List<Dictionary<string, string>> parameterCombinatrions)
        {
            ParameterCombinatrions = parameterCombinatrions;
            if (parameterCombinatrions is null)
                ParameterCombinatrions = new List<Dictionary<string, string>>();
            FilePath = path;
            FileName = System.IO.Path.GetFileName(path);
            OutPath = outPath;
            GeneratePath = generatePath;
            GeneretedTasks = new List<string>();
            PointResults = new List<GeneralTaskResult>();
            TaskType = TaskType.Unknown;
            FormatType = FormatType.Unknown;
            TaskType = CheckTaskType(FilePath);
            FormatType = CheckFormat(FilePath);
        }

        public void CreateTasks()
        {
            string newGeneration;
            if (ParameterCombinatrions.Count > 0)
            {
                var i = 0;
                foreach (var param in ParameterCombinatrions)
                {
                    newGeneration = Path.Combine(GeneratePath, FileName.Split(".")[0] + "_" + (i++) + "." + FileName.Split(".")[1]);
                    File.Copy(FilePath, newGeneration);
                    foreach (var item in param)
                    {
                        string text = File.ReadAllText(newGeneration);
                        text = text.Replace("<*" + item.Key + "*>", item.Value);
                        File.WriteAllText(newGeneration, text);
                    }
                    SeqLogger.Info("Task created: " + newGeneration);
                    GeneretedTasks.Add(newGeneration);
                }
            }
            else
            {
                newGeneration = Path.Combine(GeneratePath, FileName);
                File.Copy(FilePath, newGeneration);
                GeneretedTasks.Add(newGeneration);
            }
        }

        public void RunTasks()
        {
            int i = 1;
            foreach (var task in GeneretedTasks)
            {
                SeqLogger.Info(i++ + "/" + GeneretedTasks.Count + " Task running: " + Path.GetFileName(task));
                RunTask(task);
                SeqLogger.InitBacklog();
            }
        }

        private void RunTask(string path)
        {
            TaskType = CheckTaskType(FilePath);
            FormatType = CheckFormat(FilePath);
            GeneralTaskSerializer serPL;
            GeneralTask pointLikeTask;
            GeneralTaskResult pointTaskResult;

            if (TaskType != TaskType.Unknown && FormatType != FormatType.Unknown)
            {
                if (TaskType == TaskType.General)
                {
                    try
                    {
                        SeqLogger.LogLevel = LogLevel.Info;
                        serPL = new GeneralTaskSerializer();
                        pointLikeTask = FormatType switch
                        {
                            FormatType.SEQ or FormatType.TXT => serPL.ImportSEQ(path),
                            FormatType.JSON => serPL.ImportJSON(path),
                            FormatType.XML => serPL.ImportXML(path),
                            _ => throw new TypeLoadException("Input file should be .txt/.seq/.json/.xml!"),
                        };
                        pointTaskResult = pointLikeTask.Run();
                        GeneralResultSerializer serRes = new GeneralResultSerializer();
                        serRes.ExportJSON(pointTaskResult, Path.Combine(OutPath, Path.GetFileName(path).Split(".")[0] + ".json"));
                        SeqLogger.LogLevel = LogLevel.Info;
                        SeqLogger.Info(pointTaskResult.StatusMessage + " Solver Time:" + pointTaskResult.SolverTime + " Cost:" + pointTaskResult.MotionCosts);
                        //pointTaskResult.Log.Clear();
                        PointResults.Add(pointTaskResult);
                    }
                    catch (Exception e)
                    {
                        SeqLogger.Indent = 0;
                        SeqLogger.Critical(e.ToString());
                        //PointResults.Add(new GeneralTaskResult()
                        //{
                        //    StatusMessage = "Stop with error",
                        //    ErrorMessage = new List<string>() { e.ToString() }
                        //});
                    }
                }
            }
        }

        public string ToCSVHeader()
        {
            var s = ";";
            var tmp="";
            if(ParameterCombinatrions is not null && ParameterCombinatrions.Count>0)
            foreach (var item in ParameterCombinatrions[0])
            {
                tmp += item.Key+s;
            }
            return "TemplateName" + s + nameof(GeneretedTasks) + s + nameof(TaskType) + s + GeneralTaskResult.CSVHeader() + s + tmp;
        }

        public string ToCSV()
        {
            var tmp = "";
            var s = ";";
            for (int i = 0; i < GeneretedTasks.Count; i++)
            {
                tmp += FileName + s;
                tmp += Path.GetFileName(GeneretedTasks[i]) + s;
                tmp += TaskType.ToString() + s;

                if (TaskType == TaskType.General && PointResults is not null && PointResults.Count > i)
                    tmp += PointResults[i].ToCSV();

                if (ParameterCombinatrions.Count > i)
                {
                    tmp += s;
                    foreach (var item in ParameterCombinatrions[i])
                    {
                        tmp += item.Value + s;
                    }
                }
                if (i != GeneretedTasks.Count - 1)
                    tmp += "\n";
            }
            return tmp;
        }

        private static TaskType CheckTaskType(string input)
        {
            foreach (var line in File.ReadAllLines(input))
            {
                if (line.Contains("General"))
                    return TaskType.General;
            }
            return TaskType.Unknown;
        }

        private static FormatType CheckFormat(string path)
        {
            if (path != null && path.Length > 1)
            {
                if (path.ToUpper().Contains("SEQ"))
                    return FormatType.SEQ;
                if (path.ToUpper().Contains("TXT"))
                    return FormatType.TXT;
                if (path.ToUpper().Contains("JSON"))
                    return FormatType.JSON;
                if (path.ToUpper().Contains("XML"))
                    return FormatType.XML;
                throw new TypeLoadException("Input file should be .txt/.seq/.json/.xml: " + path);
            }
            else
            {
                throw new TypeLoadException("Input file should be .txt/.seq/.json/.xml:" + path);
            }
        }
    }
}