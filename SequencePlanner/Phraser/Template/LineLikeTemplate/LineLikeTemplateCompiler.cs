using SequencePlanner.GTSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public class LineLikeTemplateCompiler: TemplateCompiler
    {
        private LineLikeTask Task { get; set; }
        private LineLikeTemplate Template { get; set; }

        public LineLikeTask Compile(LineLikeTemplate template, CommonTask common)
        {
            Template = template;
            Task = new LineLikeTask(common);
            BuildGTSP();
            FindStartDepo();
            return Task;
        }

        private void BuildGTSP()
        {
            CreateContours();
            foreach (var line in Template.LineList)
            {
                var contour = FindCountour(line.ContourID);
                var newLine = new Line()
                {
                    UID = line.LineID,
                    Name = line.Name,
                    Start = FindPosition(line.PositionA),
                    End = FindPosition(line.PositionB),
                    Contour = contour
                };
                var newLineRev = new Line()
                {
                    UID = line.LineID,
                    Name = line.Name+"_Reverse",
                    Start = FindPosition(line.PositionB),
                    End = FindPosition(line.PositionA),
                    Contour = contour
                };
                Task.GTSP.Lines.Add(newLine);
                Task.GTSP.Lines.Add(newLineRev);
                contour.AddLine(newLine);
                contour.AddLine(newLineRev);

                ConstraintDisjoint constraint = new ConstraintDisjoint();
                constraint.Add(newLine);
                constraint.Add(newLineRev);
                Task.GTSP.ConstraintsDisjoints.Add(constraint);
            }
            Task.GTSP.ContourPenalty = Template.ContourPenalty;
            CreateLinePrecedences();
            CreateContourPrecedences();
            Task.GTSP.Build();
        }

        private void CreateLinePrecedences()
        {
            foreach (var precedence in Template.LinePrecedence)
            {
                //Find original line and reverse too!
                var beforeList = FindLine(precedence.BeforeID);
                var afterList = FindLine(precedence.AfterID);
                if(beforeList!=null && afterList != null) {
                    foreach (var before in beforeList)
                    {
                        foreach (var after in afterList)
                        {
                            Task.GTSP.ConstraintsOrder.Add(new ConstraintOrder(before, after));
                        }
                    }
                }
            }
        }

        private void CreateContourPrecedences()
        {
            foreach (var precedence in Template.ContourPrecedence)
            {
                var contBefore = FindCountour(precedence.BeforeID);
                var contAfter = FindCountour(precedence.AfterID);
                if(contBefore != null && contAfter != null)
                {
                    foreach (var before in contBefore?.Lines)
                    {
                        foreach (var after in contAfter?.Lines)
                        {
                            Task.GTSP.ConstraintsOrder.Add(new ConstraintOrder(before, after));
                        }
                    }
                }
                
            }
        }

        private void FindStartDepo()
        {
            foreach (var line in Task.GTSP.Lines)
            {
                if (line.UID == Template.StartDepotID)
                    Task.StartDepot = line;
            }
        }

        private void CreateContours() {
            foreach (var line in Template.LineList)
            {
                if (!InContourSet(line.ContourID))
                    Task.GTSP.Contours.Add(new Contour() { UID = line.ContourID });
            }
        }

        private bool InContourSet(int ID)
        {
            foreach (var contour in Task.GTSP.Contours)
            {
                if (contour.ID == ID)
                    return true;
            }
            return false;
        }

        private Contour FindCountour(int UID)
        {
            foreach (var cont in Task.GTSP.Contours)
            {
                if (cont.UID == UID)
                    return cont;
            }
            return null;
        }

        private List<Line> FindLine(int UID)
        {
            List<Line> tmp = new List<Line>(); ;
            foreach (var line in Task.GTSP.Lines)
            {
                if (line.UID == UID)
                    tmp.Add(line);
            }
            return tmp;
        }

        private Position FindPosition(int ID)
        {
            foreach (var position in Task.PositionList)
            {
                if (position.GID == ID)
                    return position;
            }
            return null;
        }
    }
}
