using SequencePlanner.GTSPTask.Result;
using SequencePlanner.GTSPTask.Serialization.Result;
using SequencePlanner.GTSPTask.Serialization.Task;
using SequencePlanner.GTSPTask.Task.LineTask;
using SequencePlanner.GTSPTask.Task.General;
using SequencePlanner.Helper;
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
        public List<Dictionary<string, string>> ParameterCombinatrions  { get; set; }
        public List<string> GeneretedTasks { get; set; }
        public List<LineTaskResult> LineResults { get; set; }
        public List<GeneralTaskResult> PointResults { get; set; }

        public Template(string path, string generatePath,  string outPath, List<Dictionary<string, string>> parameterCombinatrions)
        {
            ParameterCombinatrions = parameterCombinatrions;
            if(parameterCombinatrions is null)
                ParameterCombinatrions = new List<Dictionary<string, string>>();
            FilePath = path;
            FileName = System.IO.Path.GetFileName(path);
            OutPath = outPath;
            GeneratePath = generatePath;
            GeneretedTasks = new List<string>();
            LineResults = new List<LineTaskResult>();
            PointResults = new List<GeneralTaskResult>();
            TaskType = TaskType.Unknown;
            FormatType = FormatType.Unknown;
            TaskType = CheckTaskType(FilePath);
            FormatType = CheckFormat(FilePath);
        }

        public void CreateTasks()
        {
            string newGeneration = "";
            if (ParameterCombinatrions.Count > 0)
            {
                var i = 0;
                foreach (var param in ParameterCombinatrions)
                {
                    newGeneration = Path.Combine(GeneratePath, FileName.Split(".")[0]+"_"+(i++)+"."+ FileName.Split(".")[1]);
                    File.Copy(FilePath, newGeneration);
                    foreach (var item in param)
                    {
                        string text = File.ReadAllText(newGeneration);
                        text = text.Replace("<*"+item.Key+"*>", item.Value);
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
                SeqLogger.Info(i++ + "/" + GeneretedTasks.Count+" Task running: "+Path.GetFileName(task) );
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
            LineTask lineTask;
            LineTaskSerializer serLL;
            LineTaskResult lineTaskResult;

            if (TaskType != TaskType.Unknown && FormatType != FormatType.Unknown)
            {
                if (TaskType == TaskType.General)
                {
                    try
                    {
                        SeqLogger.LogLevel = LogLevel.Info;
                        serPL = new GeneralTaskSerializer();
                        switch (FormatType)
                        {
                            case FormatType.SEQ:
                            case FormatType.TXT:
                                pointLikeTask = serPL.ImportSEQ(path);
                                break;
                            case FormatType.JSON:
                                pointLikeTask = serPL.ImportJSON(path);
                                break;
                            case FormatType.XML:
                                pointLikeTask = serPL.ImportXML(path);
                                break;
                            default:
                                throw new TypeLoadException("Input file should be .txt/.seq/.json/.xml!");
                        }
                        pointTaskResult = pointLikeTask.RunModel();
                        GeneralResultSerializer serRes = new GeneralResultSerializer();
                        serRes.ExportJSON(pointTaskResult, Path.Combine(OutPath, Path.GetFileName(path).Split(".")[0] + ".json"));
                        SeqLogger.LogLevel = LogLevel.Info;
                        SeqLogger.Info(pointTaskResult.StatusMessage+" Solver Time:"+ pointTaskResult.SolverTime+" Cost:"+ pointTaskResult.CostSum);
                        pointTaskResult.Log.Clear();
                        PointResults.Add(pointTaskResult);    
                    }catch(Exception e)
                    {
                        SeqLogger.Indent = 0;
                        PointResults.Add(new GeneralTaskResult() { 
                            StatusMessage = "Stop with error",    
                            ErrorMessage = new List<string>() { e.ToString() } 
                        });
                    }
                }

                if (TaskType == TaskType.Line)
                {
                    try { 
                    SeqLogger.LogLevel = LogLevel.Error;
                    serLL = new LineTaskSerializer();
                    switch (FormatType)
                    {
                        case FormatType.SEQ:
                        case FormatType.TXT:
                            lineTask = serLL.ImportSEQ(path);
                            break;
                        case FormatType.JSON:
                            lineTask = serLL.ImportJSON(path);
                            break;
                        case FormatType.XML:
                            lineTask = serLL.ImportXML(path);
                            break;
                        default:
                            throw new TypeLoadException("Input file should be .txt/.seq/.json/.xml!");
                    }

                    lineTaskResult = lineTask.RunModel();
                    LineResultSerializer serRes = new LineResultSerializer();
                    serRes.ExportJSON(lineTaskResult, Path.Combine(OutPath, Path.GetFileName(path).Split(".")[0] + ".json"));
                    SeqLogger.LogLevel = LogLevel.Info;
                    SeqLogger.Info(lineTaskResult.StatusMessage+" Solver Time:"+lineTaskResult.SolverTime+" Cost:"+lineTaskResult.CostSum);
                    lineTaskResult.Log.Clear();
                    LineResults.Add(lineTaskResult);
                }catch (Exception e)
                {
                        SeqLogger.Indent = 0;
                    PointResults.Add(new GeneralTaskResult()
                    {
                        StatusMessage = "Stop with error",
                        ErrorMessage = new List<string>() { e.ToString() }
                    });
                }
            }

            }
            else
                SeqLogger.Error("Task type or file format error: " + path);
        }

        public static string ToCSVHeader()
        {
            var s = ";";
            return "TemplateName" + s + nameof(GeneretedTasks) + s + nameof(TaskType) + s + TaskResult.ToCSVHeader() + s+ nameof(ParameterCombinatrions);
        }
        public string ToCSV()
        {
            var tmp = "";
            var s = ";";
            for (int i = 0; i < GeneretedTasks.Count; i++)
            {
                tmp += FileName+s;
                tmp += Path.GetFileName(GeneretedTasks[i])+s;
                tmp += TaskType.ToString()+s;
                
                if (TaskType == TaskType.Line && LineResults is not null && LineResults.Count>i)
                    tmp += LineResults[i].ToCSV();
                if (TaskType == TaskType.General && PointResults is not null && PointResults.Count > i)
                    tmp += PointResults[i].ToCSV();
                
                if(ParameterCombinatrions.Count>i)
                {
                    tmp += s;
                    foreach (var item in ParameterCombinatrions[i])
                    {
                        tmp += item.Key + "=" + item.Value+",";
                    }
                }
                if (i!=GeneretedTasks.Count-1)
                    tmp += "\n";
            }
            return tmp;
        }

        private static TaskType CheckTaskType(string input)
        {
            foreach (var line in File.ReadAllLines(input))
            {
                if (line.Contains("Line"))
                    return TaskType.Line;
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
