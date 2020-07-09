using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options;
using SequencePlanner.Phraser.Template;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Mapper
{
    public static class SeqOptionsToTemplate
    {
        public static SeqTemplate Map(SeqOptionSet optionSet)
        {
            SeqTemplate template = new SeqTemplate();
            try
            {
                if (optionSet != null)
                {
                    template.TaskType = ((TaskType)optionSet.FindOption("TaskType")).Value;
                    template.EdgeWeightSource = ((EdgeWeightSource)optionSet.FindOption("EdgeWeightSource")).Value;
                    template.DistanceFunction = ((DistanceFunction)optionSet.FindOption("DistanceFunction")).Value;
                    template.Dimension = ((Dimension)optionSet.FindOption("Dimension")).Value;
                    template.TimeLimit = ((TimeLimit)optionSet.FindOption("TimeLimit")).Value;
                    template.CyclicSequence = ((CyclicSequence)optionSet.FindOption("CyclicSequence")).Value;
                    template.StartDepotID = ((StartDepot)optionSet.FindOption("StartDepot")).Value;
                    template.FinishDepotID = ((FinishDepot)optionSet.FindOption("FinishDepot")).Value;
                    template.WeightMultiplier = ((WeightMultiplier)optionSet.FindOption("WeightMultiplier")).Value;
                    if (template.WeightMultiplier == -1)
                        template.WeightMultiplierAuto = true;

                    template.TrapezoidParamsAcceleration = ((TrapezoidParamsAcceleration)optionSet.FindOption("TrapezoidParams/Acceleration")).Value;
                    template.TrapezoidParamsSpeed = ((TrapezoidParamsSpeed)optionSet.FindOption("TrapezoidParams/Speed")).Value;
                    if (template.DistanceFunction == Options.Values.DistanceFunctionEnum.Trapezoid_Time || template.DistanceFunction == Options.Values.DistanceFunctionEnum.Trapezoid_Time_WithTieBreaker)
                    {
                        EdgeWeightFunctions.setTrapezoidParam(template.TrapezoidParamsAcceleration.ToArray(), template.TrapezoidParamsSpeed.ToArray());
                    }
                    template.GTSP.EdgeWeightCalculator = EdgeWeightFunctions.toFunction(template.DistanceFunction);
                    template.GTSP.WeightMultiplier = ((WeightMultiplier)optionSet.FindOption("WeightMultiplier")).Value;
                    template.ProcessHierarchy = ((ProcessHierarchy)optionSet.FindOption("ProcessHierarchy")).Value;
                    template.ProcessPrecedence = ((ProcessPrecedence)optionSet.FindOption("ProcessPrecedence")).Value;
                    template.PositionPrecedence = ((PositionPrecedence)optionSet.FindOption("PositionPrecedence")).Value;
                    template.LineList = ((LineList)optionSet.FindOption("LineList")).Value;
                    template.LinePrecedence = ((LinePrecedence)optionSet.FindOption("LinePrecedence")).Value;
                    template.ContourPrecedence = ((ContourPrecedence)optionSet.FindOption("ContourPrecedence")).Value;
                    template.ContourPenalty = ((ContourPenalty)optionSet.FindOption("ContourPenalty")).Value;
                    template.PositionList = ((PositionList)optionSet.FindOption("PositionList")).Value;
                    template.PositionNumber = ((PositionNumber)optionSet.FindOption("PositionNumber")).Value;
                    template.PositionMatrix = ((PositionMatrix)optionSet.FindOption("PositionMatrix")).Value;
                    template.GTSP.PositionMatrix = template.PositionMatrix;
                    if (template.PositionList == null && template.PositionMatrix!=null)
                    {
                        template.PositionList = new List<Options.Values.PositionOptionValue>();
                        string name = "";
                        for (int i = 0; i < template.PositionMatrix.ID.Count; i++)
                        {
                            if (template.PositionMatrix.Name.Count > 0)
                            {
                                name = template.PositionMatrix.Name[i];
                            }
                            else
                            {
                                name = "Position_" + i;
                            }
                            template.PositionList.Add(new Options.Values.PositionOptionValue() { ID = template.PositionMatrix.ID[i], Name = name, Dim = 0, Position = new List<double>() });
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Template:Validate failed, no optionSet");
                }
                return template;
            }
            catch (Exception e)
            {
                Console.WriteLine("Template:Validate failed " + e.Message);
                return null;
            }
        }
    }
}
