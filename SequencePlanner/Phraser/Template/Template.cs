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

        public GraphRepresentation Graph { get; set; }
        public List<Position> Solution { get; private set; }
        public List<Position> CleanSolution { get; private set; }

        public bool Validated { get; set; }

        public OptionSet OptionSet { get; set; }

        public Template()
        {
            OptionSet = new OptionSet();
            Graph = new GraphRepresentation();
        }

        public void Read(string file)
        {
            string[] lines = File.ReadAllLines(@file);
            SequencerOptionPhraser phraser = new SequencerOptionPhraser();
            List<string> linesList = phraser.ReadFile(lines);
            OptionSet.FillValues(linesList);
            OptionSet.Validate();
            Validate();
        }

        public void Write(string file)
        {
            throw new NotImplementedException();
        }
        
        public SequencerTask Compile()
        {
            //return TemplateCompiler.Compile(this);
            try
            {
                if (OptionSet != null)
                {
                    TaskType = ((TaskType)OptionSet.findOption("TaskType")).Value;
                    EdgeWeightSource = ((EdgeWeightSource)OptionSet.findOption("EdgeWeightSource")).Value;
                    DistanceFunction = ((DistanceFunction)OptionSet.findOption("DistanceFunction")).Value;
                    
                    Dimension = ((Dimension)OptionSet.findOption("Dimension")).Value;
                    TimeLimit = ((TimeLimit)OptionSet.findOption("TimeLimit")).Value;
                    CyclicSequence = ((CyclicSequence)OptionSet.findOption("CyclicSequence")).Value;
                    StartDepotID = ((StartDepot)OptionSet.findOption("StartDepot")).Value;
                    FinishDepotID = ((FinishDepot)OptionSet.findOption("FinishDepot")).Value;

                    WeightMultiplier = ((WeightMultiplier)OptionSet.findOption("WeightMultiplier")).Value;
                    if (WeightMultiplier == -1)
                        WeightMultiplierAuto = true;

                    TrapezoidParamsAcceleration = ((TrapezoidParamsAcceleration)OptionSet.findOption("TrapezoidParams/Acceleration")).Value;
                    TrapezoidParamsSpeed = ((TrapezoidParamsSpeed)OptionSet.findOption("TrapezoidParams/Speed")).Value;
                    Graph.EdgeWeightCalculator = EdgeWeightFunctions.toFunction(DistanceFunction);

                    ProcessHierarchy = ((ProcessHierarchy)OptionSet.findOption("ProcessHierarchy")).Value;
                    ProcessPrecedence = ((ProcessPrecedence)OptionSet.findOption("ProcessPrecedence")).Value;
                    PositionPrecedence = ((PositionPrecedence)OptionSet.findOption("PositionPrecedence")).Value;
                    LineList = ((LineList)OptionSet.findOption("LineList")).Value;
                    LinePrecedence = ((LinePrecedence)OptionSet.findOption("LinePrecedence")).Value;
                    ContourPrecedence = ((ContourPrecedence)OptionSet.findOption("ContourPrecedence")).Value;
                    ContourPenalty = ((ContourPenalty)OptionSet.findOption("ContourPenalty")).Value;

                    PositionList = ((PositionList)OptionSet.findOption("PositionList")).Value;
                    PositionNumber = ((PositionNumber)OptionSet.findOption("PositionNumber")).Value;
                    PositionMatrix = ((PositionMatrix)OptionSet.findOption("PositionMatrix")).Value;

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
            return TemplateCompiler.Compile(this);
        }

        private void Validate()
        {
           
        }
    }
}