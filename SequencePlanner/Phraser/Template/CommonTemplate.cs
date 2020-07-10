using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options;
using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public class CommonTemplate : Template
    {
        public TaskTypeEnum TaskType { get; set; }
        public EdgeWeightSourceEnum EdgeWeightSource { get; set; }
        public DistanceFunctionEnum DistanceFunction { get; set; }
        public int Dimension { get; set; }
        public int TimeLimit { get; set; }
        public bool CyclicSequence { get; set; }
        public int StartDepotID { get; set; }
        public int FinishDepotID { get; set; }
        public bool WeightMultiplierAuto { get; set; }
        public int WeightMultiplier { get; set; }
        public List<double> TrapezoidParamsAcceleration { get; set; }
        public List<double> TrapezoidParamsSpeed { get; set; }
        public List<PositionOptionValue> PositionList { get; set; }
        public PositionMatrixOptionValue PositionMatrix { get; set; }


        public GTSPTask Parse(OptionSet optionSet, bool validate = true)
        {
            Fill(optionSet);
            if (validate)
                Validate();
            else
                Console.WriteLine("Warning: Template not validated!");
            return Compile();
        }

        public GTSPTask Compile()
        {
            CommonTemplateCompiler compiler = new CommonTemplateCompiler();
            return compiler.Compile(this);
        }

        public void Validate()
        {
            CommonTemplateValidator validator = new CommonTemplateValidator();
            if (!validator.Validate(this))
                Console.WriteLine("Common Template Validation Error!");
        }

        public override string ToString()
        {
            string tmp = "\nCommonTemplate details:";
            tmp += "\n\tTaskType: " + TaskType.ToString();
            tmp += "\n\tEdgeWeightSource: " + EdgeWeightSource.ToString();
            tmp += "\n\tDistanceFunction: " + DistanceFunction.ToString();
            tmp += "\n\tDimension: " + Dimension.ToString();
            tmp += "\n\tTimeLimit: " + TimeLimit.ToString() + " sec";
            tmp += "\n\tCyclicSequence: " + CyclicSequence.ToString();
            tmp += "\n\tStartDepotID: " + StartDepotID.ToString();
            tmp += "\n\tFinishDepotID: " + FinishDepotID.ToString();
            tmp += "\n\tWeightMultiplierAuto: " + WeightMultiplierAuto.ToString();
            tmp += "\n\tWeightMultiplier: " + WeightMultiplier.ToString();
            tmp += "\n\tTrapezoidParamsAcceleration: " + TrapezoidParamsAcceleration;
            tmp += "\n\tTrapezoidParamsSpeed: " + TrapezoidParamsSpeed;
            tmp += "\n\tPositionList: " + PositionList?.ToString();
            tmp += "\n\tPositionMatrix: " + PositionMatrix?.ToString();
            tmp += "\n\n: ";
            return tmp;
        }
        private void Fill(OptionSet optionSet)
        {
            try
            {
                if (optionSet != null)
                {
                    TaskType = ((TaskType)optionSet.FindOption("TaskType")).Value;
                    EdgeWeightSource = ((EdgeWeightSource)optionSet.FindOption("EdgeWeightSource")).Value;
                    DistanceFunction = ((DistanceFunction)optionSet.FindOption("DistanceFunction")).Value;
                    Dimension = ((Dimension)optionSet.FindOption("Dimension")).Value;
                    TimeLimit = ((TimeLimit)optionSet.FindOption("TimeLimit")).Value;
                    CyclicSequence = ((CyclicSequence)optionSet.FindOption("CyclicSequence")).Value;
                    StartDepotID = ((StartDepot)optionSet.FindOption("StartDepot")).Value;
                    FinishDepotID = ((FinishDepot)optionSet.FindOption("FinishDepot")).Value;
                    WeightMultiplier = ((WeightMultiplier)optionSet.FindOption("WeightMultiplier")).Value;
                    TrapezoidParamsAcceleration = ((TrapezoidParamsAcceleration)optionSet.FindOption("TrapezoidParams/Acceleration")).Value;
                    TrapezoidParamsSpeed = ((TrapezoidParamsSpeed)optionSet.FindOption("TrapezoidParams/Speed")).Value;
                    PositionList = ((PositionList)optionSet.FindOption("PositionList")).Value;
                    PositionMatrix = ((PositionMatrix)optionSet.FindOption("PositionMatrix")).Value;
                    Afterwork();
                }
                else
                {
                    Console.WriteLine("Template:Validate failed, no optionSet");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Template:Validate failed " + e.Message);
            }
        }

        private void Afterwork()
        {
            if (DistanceFunction == Options.Values.DistanceFunctionEnum.Trapezoid_Time || DistanceFunction == Options.Values.DistanceFunctionEnum.Trapezoid_Time_WithTieBreaker)
            {
                EdgeWeightFunctions.setTrapezoidParam(TrapezoidParamsAcceleration.ToArray(), TrapezoidParamsSpeed.ToArray());
            }
        }
    }
}
