using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options;
using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public class TemplateManager
    {
        public static bool DEBUG = false;
        public TemplateManager()
        {

        }

        public ORToolsResult Solve(string file, bool validate)
        {
            FullOptionSet optionSet = new FullOptionSet();
            optionSet.ReadFile(file, validate);
            var taskType = ((TaskType)optionSet.FindOption("TaskType")).Value;
            if (taskType == TaskTypeEnum.Line_Like)
            {
                LineLikeTemplate lineTemplate = new LineLikeTemplate(optionSet, validate);
                LineLikeTask lineTask = (LineLikeTask)lineTemplate.Compile();
                return lineTask.Run();
            }
            if (taskType == Options.Values.TaskTypeEnum.Point_Like)
            {
                PointLikeTemplate pointTemplate = new PointLikeTemplate(optionSet, validate);
                PointLikeTask pointTask = (PointLikeTask) pointTemplate.Compile();
                return pointTask.Run();
            }            
            return null;
        }
    }
}