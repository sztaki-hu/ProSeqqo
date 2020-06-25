using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options;
using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public class Template : ITemplate
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

        public OptionSet OptionSet { get; set; }

        public Template()
        {
            OptionSet = new OptionSet();
            GTSP = new GTSPRepresentation();
        }

        public SequencerTask Read(string file)
        {
            string[] lines = File.ReadAllLines(@file);
            SequencerOptionPhraser phraser = new SequencerOptionPhraser();
            List<string> linesList = phraser.ReadFile(lines);
            OptionSet.FillValues(linesList);
            OptionSet.Validate();
            SetOptionSet();
            Validate();
            return Compile();
        }

        public void Write(string file)
        {
            throw new NotImplementedException();
        }
        
        private void SetOptionSet()
        {
            try
            {
                if (OptionSet != null)
                {
                    TaskType = ((TaskType)OptionSet.FindOption("TaskType")).Value;
                    EdgeWeightSource = ((EdgeWeightSource)OptionSet.FindOption("EdgeWeightSource")).Value;
                    DistanceFunction = ((DistanceFunction)OptionSet.FindOption("DistanceFunction")).Value;
                    Dimension = ((Dimension)OptionSet.FindOption("Dimension")).Value;
                    TimeLimit = ((TimeLimit)OptionSet.FindOption("TimeLimit")).Value;
                    CyclicSequence = ((CyclicSequence)OptionSet.FindOption("CyclicSequence")).Value;
                    StartDepotID = ((StartDepot)OptionSet.FindOption("StartDepot")).Value;
                    FinishDepotID = ((FinishDepot)OptionSet.FindOption("FinishDepot")).Value;
                    WeightMultiplier = ((WeightMultiplier)OptionSet.FindOption("WeightMultiplier")).Value;
                    if (WeightMultiplier == -1)
                        WeightMultiplierAuto = true;

                    TrapezoidParamsAcceleration = ((TrapezoidParamsAcceleration)OptionSet.FindOption("TrapezoidParams/Acceleration")).Value;
                    TrapezoidParamsSpeed = ((TrapezoidParamsSpeed)OptionSet.FindOption("TrapezoidParams/Speed")).Value;
                    GTSP.EdgeWeightCalculator = EdgeWeightFunctions.toFunction(DistanceFunction);
                    ProcessHierarchy = ((ProcessHierarchy)OptionSet.FindOption("ProcessHierarchy")).Value;
                    ProcessPrecedence = ((ProcessPrecedence)OptionSet.FindOption("ProcessPrecedence")).Value;
                    PositionPrecedence = ((PositionPrecedence)OptionSet.FindOption("PositionPrecedence")).Value;
                    LineList = ((LineList)OptionSet.FindOption("LineList")).Value;
                    LinePrecedence = ((LinePrecedence)OptionSet.FindOption("LinePrecedence")).Value;
                    ContourPrecedence = ((ContourPrecedence)OptionSet.FindOption("ContourPrecedence")).Value;
                    ContourPenalty = ((ContourPenalty)OptionSet.FindOption("ContourPenalty")).Value;
                    PositionList = ((PositionList)OptionSet.FindOption("PositionList")).Value;
                    PositionNumber = ((PositionNumber)OptionSet.FindOption("PositionNumber")).Value;
                    PositionMatrix = ((PositionMatrix)OptionSet.FindOption("PositionMatrix")).Value;
                }
                else
                {
                    Console.WriteLine("Template:Validate failed, no OptionSet");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Template:Validate failed " + e.Message);
            }
        }

        public SequencerTask Compile()
        {
            return TemplateCompiler.Compile(this);
        }

        private void Validate()
        {
            if(!TemplateValidator.Validate(this))
                Console.WriteLine("Template validation error!");
        }
    }
}