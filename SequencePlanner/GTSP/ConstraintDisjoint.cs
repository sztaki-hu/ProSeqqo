using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSP
{
    public class ConstraintDisjoint
    {
        public long[] DisjointSet { get { return NodeListID.ToArray(); } private set { } }
        private List<long> NodeListID;
        private List<Position> PositionList;
        private List<Line> LineList;


        public ConstraintDisjoint()
        {
            NodeListID = new List<long>();
            PositionList = new List<Position>();
            LineList = new List<Line>();
        }

        public void AddLine(Line line)
        {
            LineList.Add(line);
            NodeListID.Add(line.LID);
        }

        public void AddPosition(Position position)
        {
            PositionList.Add(position);
            NodeListID.Add(position.PID);
        }

        public void AddLine(List<Line> lines)
        {
            foreach (var line in lines)
            {
                AddLine(line);
            }
        }

        public void AddPosition(List<Position> positions)
        {
            foreach (var position in positions)
            {
                AddPosition(position);
            }
        }
    }
}