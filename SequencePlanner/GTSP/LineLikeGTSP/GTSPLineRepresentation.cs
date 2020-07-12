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
        public int ContourPenalty { get; set; }

        public GTSPLineRepresentation()
        {
            Positions = new List<Position>();
            Lines = new List<Line>();
            Contours = new List<Contour>();
            ConstraintsDisjoints = new List<ConstraintDisjoint>();
            ConstraintsOrder = new List<ConstraintOrder>();
            Graph = new GraphRepresentation() {
                WeightMultiplier = this.WeightMultiplier
            };
        }
        public void Build()
        {
            Graph = new GraphRepresentation()
            {
                WeightMultiplier = this.WeightMultiplier
            };
            CreateEdges();
            Graph.Build();
        }

        private void CreateEdges()
        {
            var weight = 0.0;
            foreach (var lineFrom in Lines)
            {
                foreach (var lineTo in Lines)
                {
                    if(lineFrom.ID != lineTo.ID)
                    {
                        weight = EdgeWeightCalculator.Calculate(lineFrom.End.Configuration, lineTo.Start.Configuration);
                        if (lineFrom.Contour.ID != lineTo.Contour.ID)
                            weight += ContourPenalty;
                    }
                    else
                    {
                        weight = 0.0;
                    }
                    Graph.Edges.Add(new Edge()
                    {
                        NodeA = lineFrom,
                        NodeB = lineTo,
                        Weight = weight,
                        Directed = true,
                        Tag = "[" + lineFrom.ID + "][C:"+lineFrom.Contour.UID+"]" + lineFrom.Name + "--"+weight+"--> [" + lineTo.ID + "][C:" + lineFrom.Contour.UID + "]" + lineTo.Name
                    });
                }
            }
        }

    }
}