using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public class CommonTemplateValidator
    {
        public bool Validate(CommonTemplate template)
        {
            if (CheckDimensions(template) == false)
                return false;
            if (PositionMatrix(template) == false)
                return false;
            if (CheckCyclic(template) == false)
                return false;
            if (CheckNotCyclic(template) == false)
                return false;
            if (CheckStartAndFinish(template) == false)
                return false;
            return true;
        }

        private static bool CheckDimensions(CommonTemplate template)
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
                    if (position.Dim != position.Position.Count || position.Position.Count != dim)
                    {
                        Console.WriteLine(error + "Position dimension mismatch, ID: " + position.ID + "!");
                        return false;

                    }
                }
            }
            return true;
        }

        private static bool CheckCyclic(CommonTemplate template)
        {
            string error = "Template validation error: ";
            if (template.CyclicSequence)
            {
                if (template.FinishDepotID != -1)
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

        private static bool CheckNotCyclic(CommonTemplate template)
        {
            return true;
        }

        private static bool CheckStartAndFinish(CommonTemplate template)
        {
            return true;
        }

        private static bool PositionMatrix(CommonTemplate template)
        {
            string error = "Template validation error: ";
            if (template.PositionMatrix != null)
            {
                if (template.PositionMatrix.ID.Count != template.PositionMatrix.Matrix.GetLength(0) || template.PositionMatrix.ID.Count != template.PositionMatrix.Matrix.GetLength(1))
                {
                    Console.WriteLine(error + " Position Matrix ID header and matrix should contain the same number of elements n+1 row and n column! ");
                    return false;
                }
            }
            return true;
        }
    }
}

//TODO: Validate
//TaskType
//Dimension
//TimeLimit
//Cyclic
//Start
//Finish
//WeightMultiplyer
//EdgeWeightSource
//DistaceFunc
//TrapezoidAdd/Speed
//PositionList
//PositionMatrix