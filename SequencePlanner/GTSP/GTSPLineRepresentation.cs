using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class GTSPLineRepresentation : GTSPRepresentation
    {
        public List<Contour> Contours { get; set; }
        public List<Line> Lines { get; set; }

        public GTSPLineRepresentation()
        {
            Positions = new List<Position>();
            Lines = new List<Line>();
            Contours = new List<Contour>();
            ConstraintsDisjoints = new List<ConstraintDisjoint>();
            ConstraintsOrder = new List<ConstraintOrder>();
            Graph = new GraphRepresentation() {
                EdgeWeightCalculator = this.EdgeWeightCalculator,
                WeightMultiplier = this.WeightMultiplier
            };
        }
        public void Build()
        {

            CreateEdges();
            Graph.Build();
        }

        private void CreateEdges()
        {
            var weight = 0.0;
            var weightReverse = 0.0;
            foreach (var lineFrom in Lines)
            {
                foreach (var lineTo in Lines)
                {

                    if(lineFrom.LID != lineTo.LID)
                    {
                        weight = EdgeWeightCalculator.Calculate(lineFrom.End.Configuration, lineTo.Start.Configuration);
                        weightReverse = EdgeWeightCalculator.Calculate(lineTo.End.Configuration, lineFrom.Start.Configuration);
                    }
                    else
                    {
                        weight = 0.0;
                        weightReverse = 0.0;
                    }
                    Graph.Edges.Add(new Edge()
                    {
                        NodeA = lineFrom.End,
                        NodeB = lineTo.Start,
                        Weight = weight,
                        Directed = false,
                        Tag = "[" + lineFrom.ID + "]" + lineFrom.Name + "-> [" + lineTo.ID + "]" + lineTo.Name
                    });
                    Graph.Edges.Add(new Edge()
                    {
                        NodeA = lineTo.End,
                        NodeB = lineFrom.Start,
                        Weight = weightReverse,
                        Directed = false,
                        Tag = "[" + lineTo.ID + "]" + lineTo.Name + " -> [" + lineFrom.ID + "]" + lineFrom.Name
                    });
                }
            }
        }
    }
}
