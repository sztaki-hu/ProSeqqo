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
                    ID = line.LineID,
                    Name = line.Name,
                    Start = FindPosition(line.PositionA),
                    End = FindPosition(line.PositionB),
                    Contour = contour
                };
                Task.GTSP.Lines.Add(newLine);
                contour.AddLine(newLine);
            }
            Task.GTSP.Build();

        }


        private void CreateContours() {
            foreach (var line in Template.LineList)
            {
                if (!InContourSet(line.ContourID))
                    Task.GTSP.Contours.Add(new Contour() { ID = line.ContourID });
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

        private Contour FindCountour(int ID)
        {
            foreach (var cont in Task.GTSP.Contours)
            {
                if (cont.ID == ID)
                    return cont;
            }
            return null;
        }

        private Position FindPosition(int ID)
        {
            foreach (var position in Task.PositionList)
            {
                if (position.ID == ID)
                    return position;
            }
            return null;
        }
    }
}
