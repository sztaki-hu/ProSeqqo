using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public class SeqTemplate: Template
    {
        public TaskTypeEnum TaskType { get; set; }
        public EdgeWeightSourceEnum EdgeWeightSource { get; set; }
        public DistanceFunctionEnum DistanceFunction { get; set; }
        public int Dimension { get; set; }
        public int TimeLimit { get; set; }
        public bool CyclicSequence { get; set; }

        public int StartDepotID { get; set; }
        public int FinishDepotID { get; set; }
        public Position StartDepot { get; set; }
        public Position FinishDepot { get; set; }
        public bool WeightMultiplierAuto { get; set; }
        public int WeightMultiplier { get; set; }
        public List<double> TrapezoidParamsAcceleration { get; set; }
        public List<double> TrapezoidParamsSpeed { get; set; }
        public List<ProcessHierarchyOptionValue> ProcessHierarchy { get; set; }
        public List<PrecedenceOptionValue> ProcessPrecedence { get; set; }
        public List<PrecedenceOptionValue> PositionPrecedence { get; set; }
        public List<LineListOptionValue> LineList { get; set; }
        public List<PrecedenceOptionValue> LinePrecedence { get; set; }
        public List<PrecedenceOptionValue> ContourPrecedence { get; set; }
        public List<PositionOptionValue> PositionList { get; set; }
        public int PositionNumber { get; set; }
        public PositionMatrixOptionValue PositionMatrix { get; set; }
        public int ContourPenalty { get; set; }

        public GTSPRepresentation GTSP { get; set; }

        public SeqTemplate()
        {
            GTSP = new GTSPRepresentation();
        }

        public override SeqGTSPTask Compile()
        {
            var task = SeqTemplateCompiler.Compile(this);
            Console.WriteLine(ToString());
            return task;

        }

        public override void Validate()
        {
            if (!SeqTemplateValidator.Validate(this))
                Console.WriteLine("Template validation error!");
        }

        public override string ToString()
        {
            string tmp = "\nTemplate details:";
            tmp += "\n\tTaskType: " + TaskType.ToString(); 
            tmp += "\n\tEdgeWeightSource: " + EdgeWeightSource.ToString();
            tmp += "\n\tDistanceFunction: " + DistanceFunction.ToString();
            tmp += "\n\tDimension: " + Dimension.ToString(); 
            tmp += "\n\tTimeLimit: " + TimeLimit.ToString() + " sec";
            tmp += "\n\tCyclicSequence: " + CyclicSequence.ToString();
            tmp += "\n\tStartDepotID: " + StartDepotID.ToString();
            tmp += "\n\tFinishDepotID: " + FinishDepotID.ToString();
            tmp += "\n\tStartDepot: " + StartDepot?.ToString();
            tmp += "\n\tFinishDepot: " + FinishDepot?.ToString();
            tmp += "\n\tWeightMultiplierAuto: " + WeightMultiplierAuto.ToString();
            tmp += "\n\tWeightMultiplier: " + WeightMultiplier.ToString();
            tmp += "\n\tTrapezoidParamsAcceleration: " + TrapezoidParamsAcceleration;
            tmp += "\n\tTrapezoidParamsSpeed: " + TrapezoidParamsSpeed;
            tmp += "\n\tProcessHierarchy: " + ProcessHierarchy?.ToString();
            tmp += "\n\tProcessPrecedence: " + ProcessPrecedence?.ToString();
            tmp += "\n\tPositionPrecedence: " + PositionPrecedence?.ToString();
            tmp += "\n\tLineList: " + LineList?.ToString();
            tmp += "\n\tLinePrecedence: " + LinePrecedence?.ToString();
            tmp += "\n\tContourPrecedence: " + ContourPrecedence?.ToString();
            tmp += "\n\tPositionList: " + PositionList?.ToString();
            tmp += "\n\tPositionNumber: " + PositionNumber.ToString();
            tmp += "\n\tPositionMatrix: " + PositionMatrix?.ToString();
            tmp += "\n\tContourPenalty: " + ContourPenalty.ToString();
            tmp += "\n\tGTSP: " + GTSP?.ToString();
            tmp += "\n\n: ";
            return tmp;
        }
    }
}
