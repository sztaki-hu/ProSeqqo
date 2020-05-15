using SequencePlanner.Options.Values;
using SequencePlanner.Template;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner
{
    public class SequencerTemplate
    {
        public ValueEnum.TaskType TaskType { get; set; }
        public int Dimension { get; set; }
        public double TimeLimit { get; set; }
        public bool CyclicSequence { get; set; }
        public int StartDepot { get; set; }
        public int FinishDepot { get; set; }
        public int WeightMultiplier { get; set; }
        public ValueEnum.EdgeWeightSource EdgeWeightSource { get; set; }
        public ValueEnum.DistanceFunction DistanceFunction { get; set; }

        public double[] TrapezoidAcceleration { get; set; }
        public double[] TrapezoidMaxSpeed { get; set; }
        public int PositionNumber { get; set; }

        public List<Position> Positions { get; set; }
        public int[][] PositionMatrix { get; set; }
        public List<List<Position>> PositionListMatrix { get; set; }

        public List<Line> Lines { get; set; }
        public Precedence<Line> LinePrecedences{get;set;}
        public Precedence<Line> ContourPrecedences{get;set;}

        public List<ProcessHierarchy> ProcessHierarchies { get; set; }
        public Precedence<Position> PositionPrecedence { get; set; }
        public Precedence<int> ProcessPrecedences { get; set; }

        public SequencerTemplate()
        {

        }

        public void Export(string file)
        {
            string dd = ":"; 
            string content = "";
            content += "TaskType: " + TaskType.ToString()+"\n";
            content += "EdgeWeightSource: " +EdgeWeightSource.ToString() + "\n";
            content += "DistanceFunction: " +DistanceFunction.ToString() + "\n";
            content += "Dimensions: " +Dimension.ToString() + "\n";
            content += "TimeLimit: " +TimeLimit.ToString() + "\n";
            content += "TrapezoidParams/Acceleration: " + TrapezoidAcceleration.ToString() + "\n";
            content += "TrapezoidParams/Speed: " + TrapezoidMaxSpeed.ToString() + "\n";
            content += "WeightMultiplier: " +WeightMultiplier.ToString() + "\n";
            content += "CyclicSequence: " +CyclicSequence.ToString() + "\n";
            content += "StartDepot: " +StartDepot.ToString() + "\n";
            content += "FinishDepot: " +FinishDepot.ToString() + "\n";
            //content += "Positions: " +.ToString() + "\n";
            //content += ": " +.ToString() + "\n";
            //content += ": " +.ToString() + "\n";
            //content += ": " +.ToString() + "\n";
            //content += ": " +.ToString() + "\n";
            //content += ": " +.ToString() + "\n";
            //content += ": " +.ToString() + "\n";
            //content += ": " +.ToString() + "\n";
            //content += ": " +.ToString() + "\n";
            //content += ": " +.ToString() + "\n";
            //content += ": " +.ToString() + "\n";

            //System.IO.File.WriteAllLines("export.txt", content);

        }
    }
}