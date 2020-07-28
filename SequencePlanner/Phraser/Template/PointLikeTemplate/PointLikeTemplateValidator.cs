using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public class PointLikeTemplateValidator
    {
        public new bool Validate(PointLikeTemplate template)
        {
            if (template.TaskType == TaskTypeEnum.Point_Like)
            {
                if (FindCircleInPrecedences(template.ProcessPrecedence) == false)
                    return false;
                if (CheckPointHierarchy(template) == false)
                    return false;
            }
            return true;
        }

        private bool FindCircleInPrecedences(List<PrecedenceOptionValue> precedence)
        {
            if (precedence == null)
                return true;
            foreach (var item in precedence)
            {

            }
            //TODO: Find circles
            return true;
        }

        private bool CheckPointHierarchy(PointLikeTemplate template)
        {
            string error = "Template validation error: ";
            if (template.PositionList != null)
            {
                foreach (var item in template.PositionList)
                {
                    bool findOnce = false;
                    foreach (var hierarchy in template.ProcessHierarchy)
                    {
                        if (item.ID == hierarchy.PositionID)
                            if (findOnce == false)
                                findOnce = true;
                            else
                            {
                                Console.WriteLine(error + "Process hierarchy contains position more then once, ID: " + item.ID + "! ");
                                return false;
                            }
                    }
                    if (findOnce == false)
                    {
                        Console.WriteLine(error + "Process hierarchy not contains position, ID: " + item.ID + "! ");
                        return false;
                    }
                    findOnce = false;
                }

                if (template.PositionList.Count != template.ProcessHierarchy.Count)
                {
                    Console.WriteLine(error + "Process hierarchy and position list should contain the same number of elements! ");
                    return false;
                }
            }
            return true;
        }
    }
}