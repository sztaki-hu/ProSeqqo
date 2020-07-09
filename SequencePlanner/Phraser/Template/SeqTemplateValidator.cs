using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SequencePlanner.Phraser.Template
{
    public static class SeqTemplateValidator
    {

        public static bool Validate(SeqTemplate template)
        {
            if (CheckDimensions(template)==false)
                return false;

            if (PositionMatrix(template) == false)
                return false;

            if (template.TaskType == TaskTypeEnum.Point_Like)
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
            if (CheckNotCyclic(template) == false)
                return false;
            if (CheckStartAndFinish(template) == false)
                return false;

            return true;
        }

        private static bool CheckDimensions(SeqTemplate template)
        {
            int dim = template.Dimension;
            string error = "Template validation error: ";
            if (dim < 0)
            {
                Console.WriteLine(error + "Dimension < 0!");
                return false;
            }

            //Check trapezoid dimension
            if (template.DistanceFunction == DistanceFunctionEnum.Trapezoid_Time || template.DistanceFunction == DistanceFunctionEnum.Trapezoid_Time_WithTieBreaker)
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

        private static bool FindCircleInPrecedences(List<PrecedenceOptionValue> precedence)
        {
            if (precedence == null)
                return true;
            foreach (var item in precedence)
            {

            }
            //TODO: Find circles
            return true;
        }

        private static bool CheckCyclic(SeqTemplate template)
        {
            string error = "Template validation error: ";
            if (template.CyclicSequence)
            {
                if(template.FinishDepotID!=-1)
                {
                    Console.WriteLine(error + "In case of cyclic sequence finish depo not needed!");
                    return false;
                }
                if (template.StartDepotID == -1)
                {
                    Console.WriteLine(error + "In case of cyclic sequence start depo needed!");
                    return false;
                }
            }
            return true;
        }

        private static bool CheckNotCyclic(SeqTemplate template)
        {
            //string error = "Template validation error: ";

            //if (!template.CyclicSequence)
            //{
            //    if (template.FinishDepotID == -1 && template.StartDepotID == -1)
            //    {
            //        Console.WriteLine(error + "In case of not cyclic sequence start  finish depo needed!");
            //        return false;
            //    }
            //}
            return true;
        }

        private static bool CheckStartAndFinish(SeqTemplate template)
        {
            string error = "Template validation error: ";
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

        private static bool CheckStartOrFinishSingleInProcess(Position pos)
        {
            //TODO: Start and Finish check no neighbour WARNING
            return true;
        }

        private static bool CheckPointHierarchy(SeqTemplate template)
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

        private static bool CheckLineHierarchy(SeqTemplate template)
        {
            //TODO: Check line hierarchy
            return true;
        }

        private static bool CheckLineList()
        {
            //TODO: CheckLineList()
            return true;
        }

        private static bool CheckPositionList()
        {
            //TODO: CheckLineList()
            return true;
        }

        private static bool PositionMatrix(SeqTemplate template)
        {
            string error = "Template validation error: ";
            if (template.PositionMatrix != null)
            {
                if(template.PositionMatrix.ID.Count!=template.PositionMatrix.Matrix.GetLength(0) || template.PositionMatrix.ID.Count != template.PositionMatrix.Matrix.GetLength(1))
                {
                    Console.WriteLine(error + " Position Matrix ID header and matrix should contain the same number of elements n+1 row and n column! ");
                    return false;
                }
                    
            }

            return true;
        }
    }
}