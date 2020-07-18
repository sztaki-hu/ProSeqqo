using SequencePlanner.Phraser.Helper;
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
            CheckDimensions(template);
            PositionMatrix(template);
            CheckCyclic(template);
            return true;
        }

        private static void CheckDimensions(CommonTemplate template)
        {
            int dim = template.Dimension;
            if (dim < 0)
            {
                throw new SequencerException("Dimension should be >= 0, but now: " + dim + ".", "Change for Dimension: 0 if not used.");
            }

            //Check trapezoid dimension
            if (template.DistanceFunction == DistanceFunctionEnum.Trapezoid_Time || template.DistanceFunction == DistanceFunctionEnum.Trapezoid_Time_WithTieBreaker)
            {
                if (template.TrapezoidParamsAcceleration.Count != dim)
                {
                    throw new SequencerException("TrapezoidParamsAcceleration dimension should be equal with Dimension, but now: " + dim + "!="+ template.TrapezoidParamsAcceleration.Count, "Format example with 2 dimension: TrapezoidParamsAcceleration: [1.0; 2.0]");
                }

                if (template.TrapezoidParamsSpeed.Count != dim)
                {
                    throw new SequencerException("TrapezoidParamsSpeed dimension should be equal with Dimension, but now: " + dim + "!=" + template.TrapezoidParamsSpeed.Count, "Format example with 2 dimension: TrapezoidParamsSpeed: [1.0; 2.0]");
                }
            }

            //Check Position dimensions
            if (template.PositionList != null)
            {
                foreach (var position in template.PositionList)
                {
                    if (position.Dim != position.Position.Count || position.Position.Count != dim)
                    {
                        throw new SequencerException("Position dimension in PositionList should be equal with Dimension, but now: " + dim + "!=" + position.Position.Count + "at PositionID: "+position.ID+".", "Format example with 2 dimension: [5,0;  5,0]");
                    }
                }
            }
        }

        private static void CheckCyclic(CommonTemplate template)
        {
            if (template.CyclicSequence)
            {
                if (template.FinishDepotID != -1)
                {
                    throw new SequencerException("In case of cyclic sequence finish depo not needed!", "Delete FinishDepot: "+template.FinishDepotID+" from input file.");
                }
                if (template.StartDepotID == -1)
                {
                    throw new SequencerException("In case of cyclic sequence start depo not needed!", "Add StartDepot: X to input file.");
                }
            }
        }

        private static void PositionMatrix(CommonTemplate template)
        {
            if (template.PositionMatrix != null)
            {
                if (template.PositionMatrix.ID.Count != template.PositionMatrix.Matrix.GetLength(0) || template.PositionMatrix.ID.Count != template.PositionMatrix.Matrix.GetLength(1))
                {
                    throw new SequencerException("PositionMatrix ID header length should be equal with matrix size!", "Check the length of ID header and matrix");
                }
                if(template.PositionMatrix.Matrix.GetLength(0) != template.PositionMatrix.Matrix.GetLength(1))
                {
                    throw new SequencerException("PositionMatrix should contain matrix with size n X n with an ID header", "Check number of rows and column in PositionMatrix");
                }
            }
        }
    }
}