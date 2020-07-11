using SequencePlanner.Phraser.Options;
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

        public ORToolsResult Solve(string file)
        {
            SeqOptionSet optionSet = new SeqOptionSet();
            optionSet.ReadFile(file);
            optionSet.Validate();
            CommonTemplate common = new CommonTemplate();
            CommonTask commonTask = (CommonTask) common.Parse(optionSet);

            if (common.TaskType == Options.Values.TaskTypeEnum.Line_Like)
            {
                LineLikeTemplate lineTemplate = new LineLikeTemplate(commonTask);
                LineLikeTask lineTask = lineTemplate.Parse(optionSet);
                return lineTask.Run();
            }
            if(common.TaskType == Options.Values.TaskTypeEnum.Point_Like)
            {
                PointLikeTemplate pointTemplate = new PointLikeTemplate(commonTask);
                PointLikeTask pointTask = pointTemplate.Parse(optionSet);
                return pointTask.Run();
            }
            return null;
        }
    }
}