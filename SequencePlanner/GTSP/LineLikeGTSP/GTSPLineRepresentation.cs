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
        public int StartID { get; set; }
        public int FinishID { get; set; }
        public Line StartLine { get; set; }
        public Line FinishLine { get; set; }

        public GTSPLineRepresentation()
        {
            StartID = 0;
            FinishID = 0;
            Positions = new List<Position>();
            Lines = new List<Line>();
            Contours = new List<Contour>();
            ConstraintsDisjoints = new List<ConstraintDisjoint>();
            ConstraintsOrder = new List<ConstraintOrder>();
            Graph = new GraphRepresentation() {
                WeightMultiplier = this.WeightMultiplier
            };
        }

        public void Build(List<Position> positions, List<LineListOptionValue> lines, List<PrecedenceOptionValue> linePrec, List<PrecedenceOptionValue> contPrec,int contourPenalty, bool cyclicSeq, Position startDepot, Position finishDepot)
        {

            Positions = positions;
            ContourPenalty = contourPenalty;
            CreateContours(lines);
            BuildGTSP(lines);
            CreateLinePrecedences(linePrec);
            CreateContourPrecedences(contPrec);
            Graph = new GraphRepresentation()
            {
                WeightMultiplier = this.WeightMultiplier
            };
            CreateEdges();
            ExtendStartFinish(cyclicSeq, startDepot, finishDepot);
            Graph.Build();
        }

        private void BuildGTSP(List<LineListOptionValue> lines)
        {
            foreach (var line in lines)
            {
                var contour = FindCountour(line.ContourID);
                ConstraintDisjoint constraint = new ConstraintDisjoint();
                var newLine = new Line()
                {
                    UID = line.LineID,
                    Name = line.Name,
                    Start = FindPositionByUID(line.PositionA),
                    End = FindPositionByUID(line.PositionB),
                    Contour = contour
                };
                Lines.Add(newLine);
                contour.AddLine(newLine);
                constraint.Add(newLine);

                if (line.Bidirectional)
                {
                    var newLineRev = new Line()
                    {
                        UID = line.LineID,
                        Name = line.Name + "_Reverse",
                        Start = FindPositionByUID(line.PositionB),
                        End = FindPositionByUID(line.PositionA),
                        Contour = contour
                    };
                    Lines.Add(newLineRev);
                    contour.AddLine(newLineRev);
                    constraint.Add(newLineRev);
                }
                ConstraintsDisjoints.Add(constraint);
            }
        }
        private void CreateEdges()
        {
            foreach (var lineFrom in Lines)
            {
                foreach (var lineTo in Lines)
                {
                    double weight;
                    if (lineFrom.ID != lineTo.ID)
                    {
                        weight = EdgeWeightCalculator.Calculate(lineFrom.End, lineTo.Start);
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
                        Tag = "[" + lineFrom.ID + "][C:" + lineFrom.Contour.UID + "]" + lineFrom.Name + "--" + weight + "--> [" + lineTo.ID + "][C:" + lineFrom.Contour.UID + "]" + lineTo.Name
                    });
                }
            }
        }

        private void CreateLinePrecedences(List<PrecedenceOptionValue> linePrec)
        {
            foreach (var precedence in linePrec)
            {
                //Find original line and reverse too!
                var beforeList = FindLine(precedence.BeforeID);
                var afterList = FindLine(precedence.AfterID);
                if (beforeList != null && afterList != null)
                {
                    foreach (var before in beforeList)
                    {
                        foreach (var after in afterList)
                        {
                            ConstraintsOrder.Add(new ConstraintOrder(before, after));
                        }
                    }
                }
            }
        }
        private void CreateContourPrecedences(List<PrecedenceOptionValue> contPrec)
        {
            foreach (var precedence in contPrec)
            {
                var contBefore = FindCountour(precedence.BeforeID);
                var contAfter = FindCountour(precedence.AfterID);
                if (contBefore != null && contAfter != null)
                {
                    foreach (var before in contBefore?.Lines)
                    {
                        foreach (var after in contAfter?.Lines)
                        {
                            ConstraintsOrder.Add(new ConstraintOrder(before, after));
                        }
                    }
                }

            }
        }
        private void CreateContours(List<LineListOptionValue> lines)
        {
            foreach (var line in lines)
            {
                if (!IsInContourSet(line.ContourID))
                    Contours.Add(new Contour(line.ContourID));
            }
        }
        private void ExtendStartFinish(bool cyclicSeq, Position startDepot, Position finishDepot)
        {
            if(!cyclicSeq && startDepot == null && finishDepot == null)
            {
                AddVirualStart(); 
                AddVirualFinish();
            }
        }

        private void AddVirualStart()
        {
            var a = new Position() { Name = "VirtualStartLineA" };
            var b = new Position() { Name = "VirtualStartLineB" };
            Positions.Add(a);
            Positions.Add(b);
            Line startLine = new Line()
            {
                Name = "StartLine",
                Virtual = true,
                Start = a,
                End = b,
                Contour = new Contour()
            };
            Lines.Add(startLine);

            foreach (var line in Lines)
            {
                Graph.Edges.Add(new Edge()
                {
                    NodeA = line,
                    NodeB = startLine,
                    Weight = 0,
                    Directed = true,
                    Tag = "VirtualStartLine"
                });
                Graph.Edges.Add(new Edge()
                {
                    NodeA = startLine,
                    NodeB = line,
                    Weight = 0,
                    Directed = true,
                    Tag = "VirtualStartLine"
                });
            }
            StartID = startLine.ID;
            StartLine = startLine;
        }

        private void AddVirualFinish()
        {
            var a = new Position() { Name = "VirtualFinishLineA" };
            var b = new Position() { Name = "VirtualFinishLineB" };
            Positions.Add(a);
            Positions.Add(b);
            Line finishLine = new Line()
            {
                Name = "FinishLine",
                Virtual = true,
                Start = a,
                End = b,
                Contour = new Contour()
            };
            Lines.Add(finishLine);

            foreach (var line in Lines)
            {
                Graph.Edges.Add(new Edge()
                {
                    NodeA = line,
                    NodeB = finishLine,
                    Weight = 0,
                    Directed = true,
                    Tag = "VirtualFinishLine"
                });
                Graph.Edges.Add(new Edge()
                {
                    NodeA = finishLine,
                    NodeB = line,
                    Weight = 0,
                    Directed = true,
                    Tag = "VirtualFinishLine"
                });
            }
            FinishID = finishLine.ID;
            FinishLine = finishLine;
        }

        private bool IsInContourSet(int ID)
        {
            foreach (var contour in Contours)
            {
                if (contour.ID == ID)
                    return true;
            }
            return false;
        }

        private Contour FindCountour(int UID)
        {
            foreach (var cont in Contours)
            {
                if (cont.UID == UID)
                    return cont;
            }
            return null;
        }
        private List<Line> FindLine(int UID)
        {
            List<Line> tmp = new List<Line>(); ;
            foreach (var line in Lines)
            {
                if (line.UID == UID)
                    tmp.Add(line);
            }
            return tmp;
        }
    }
}