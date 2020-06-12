using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options;
using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public static class TemplateValidator
    {

        public static bool Validate(Template template)
        {
            if (CheckDimensions(template)==false)
                return false;

            if(template.TaskType == TaskTypeEnum.Point_Like)
            {
                if (FindCircleInPrecedences(template.ProcessPrecedence) == false)
                    return false;
                if (FindCircleInPrecedences(template.PositionPrecedence) == false)
                    return false;
                if (CheckPointHierarchy(template) == false)
                    return false;
            }

            if (template.TaskType == TaskTypeEnum.Line_Like)
            {

                if (FindCircleInPrecedences(template.LinePrecedence) == false)
                    return false;
                if (FindCircleInPrecedences(template.ContourPrecedence) == false)
                    return false;
                if (CheckLineHierarchy(template) == false)
                    return false;
            }

            if (CheckCyclic(template) == false)
                return false;

            return true;
        }

        public static bool CheckDimensions(Template template)
        {
            int dim = template.Dimension;
            string error = "Template validation error: ";
            if (dim < 0)
            {
                Console.WriteLine(error + "Dimension < 0!");
                return false;
            }

            //Check trapezoid dimension
            if (template.DistanceFunction == DistanceFunctionEnum.Trapezoid_Time)
            {
                if (template.TrapezoidParamsAcceleration.Count != dim)
                {
                    Console.WriteLine(error + "Trapezoid acceleration dimension mismatch!");
                    return false;
                }

                if (template.TrapezoidParamsSpeed.Count != dim)
                {
                    Console.WriteLine(error + "Trapezoid speed dimension mismatch!");
                    return false;
                }
            }
              
            //Check Position dimensions
            if (template.PositionList != null)
            {
                foreach (var position in template.PositionList)
                {
                    if(position.Dim!=position.Position.Count || position.Position.Count != dim)
                    {
                        Console.WriteLine(error + "Position dimension mismatch, ID: " + position.ID + "!");
                        return false;

                    }
                }
            }
            return true;
        }

        public static bool FindCircleInPrecedences(List<PrecedenceOptionValue> precedence)
        {
            foreach (var item in precedence)
            {

            }
            //TODO: Find circles
            return true;
        }

        public static bool CheckCyclic(Template template)
        {
            string error = "Template validation error: ";

            if (template.CyclicSequence)
            {
                if(template.FinishDepotID!=-1)
                {
                    Console.WriteLine(error + "In case of cyclic sequence finish depo not needed!");
                    return false;
                }
            }
            else
            {
                if (template.FinishDepotID == -1 || template.StartDepotID == -1)
                {
                    Console.WriteLine(error + "In case of not cyclic sequence start and finish depo needed!");
                    return false;
                }
            }

            bool findPositionID = false;
            if (template.FinishDepotID != -1)
            {
                foreach (var hierarchyItem in template.ProcessHierarchy)
                {
                    if (hierarchyItem.PositionID == template.FinishDepotID)
                        findPositionID = true;
                }
                if (!findPositionID)
                {
                    Console.WriteLine(error + "Given finish depo position ID not find in the hierarchy!");
                    return false;
                }
            }

            findPositionID = false;
            if (template.StartDepotID != -1)
            {
                foreach (var hierarchyItem in template.ProcessHierarchy)
                {
                    if (hierarchyItem.PositionID == template.StartDepotID)
                        findPositionID = true;
                }
                if (!findPositionID)
                {
                    Console.WriteLine(error + "Given start depo position ID not find in the hierarchy!");
                    return false;
                }
            }

            return true;
        }

        public static bool CheckPointHierarchy(Template template)
        {
            string error = "Template validation error: ";
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
                            Console.WriteLine(error + "Process hierarchy contains position more then once, ID: "+item.ID+"! ");
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

            return true;
        }

        public static bool CheckLineHierarchy(Template template)
        {
            //TODO: Check line hierarchy
            return true;
        }

        public static bool CheckLineList()
        {
            //TODO: CheckLineList()
            return true;
        }

        public static bool CheckPositionList()
        {
            //TODO: CheckLineList()
            return true;
        }

        public static bool PositionMatrix()
        {
            //TODO: CheckLineList()
            return true;
        }
    }
}