using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options;
using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public class Template
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
        public List<ProcessHierarchyOptionValue> ProcessHierarchy { get; set; }
        public List<PrecedenceOptionValue> ProcessPrecedence   {get;set;}
        public List<PrecedenceOptionValue> PositionPrecedence  {get;set;}
        public List<LineListOptionValue> LineList {get;set;}
        public List<PrecedenceOptionValue> LinePrecedence      {get;set;}
        public List<PrecedenceOptionValue> ContourPrecedence   {get;set;}
        public List<PositionOptionValue> PositionList        {get;set;}
        public int PositionNumber      {get;set;}
        public PositionMatrixOptionValue PositionMatrix      {get;set;}
        public int ContourPenalty      {get;set;}

        public GTSPRepresentation GTSP { get; set; }
        public List<Position> Solution { get; private set; }
        public List<Position> CleanSolution { get; private set; }

        public bool Validated { get; set; }

        public OptionSet OptionSet { get; set;  }

        public Template()
        {
            GTSP = new GTSPRepresentation();
            OptionSet = new SeqOptionSet();
        }
        
        public SequencerTask Compile()
        {
            return TemplateCompiler.Compile(this);
        }

        public void Validate()
        {
            if(!TemplateValidator.Validate(this))
                Console.WriteLine("Template validation error!");
        }
    }
}