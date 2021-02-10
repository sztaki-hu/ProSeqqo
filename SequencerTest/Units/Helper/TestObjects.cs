using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencerTest.Units.Helper
{
    public static class TestObjects
    {
        public static Line GetLine()
        {
            Line line = new Line()
            {
                NodeA = GetLinePosA(),
                NodeB = GetLinePosB(),
                Bidirectional = true,
                Length = 5,
                Name = "Line",
                ResourceID = 200,
                SequencingID = 3,
                UserID = 100,
                Virtual = false
            };
            return line;
        }

        public static Position GetLinePosA()
        {
            Position position = new Position()
            {
                Name = "PositionA",
                UserID = 100,
                ResourceID = 200,
                SequencingID = 2,
                Virtual = false,
                Vector = new double[] { 1, 2 }
            };
            return position;
        }

        public static Position GetLinePosB()
        {
            Position position = new Position()
            {
                Name = "PositionB",
                UserID = 101,
                ResourceID = 201,
                SequencingID = 1,
                Virtual = false,
                Vector = new double[] { 3, 4 }
            };
            return position;
        }

        public static bool CheckLine(Line line)
        {
            if (!CheckLinePosA(line.NodeA))
                return false;
            if (!CheckLinePosB(line.NodeB))
                return false;
            if (!line.Bidirectional)
                return false;
            if (line.Length != 5)
                return false;
            if (line.Name != "Line")
                return false;
            if (line.SequencingID != 3)
                return false;
            if (line.ResourceID != 200)
                return false;
            if (line.UserID != 100)
                return false;
            if (line.Virtual)
                return false;
            return true;
        }

        public static bool CheckLinePosA(Position position)
        {
            if (position.Name != "PositionA")
                return false;
            if (position.UserID != 100)
                return false;
            if (position.ResourceID != 200)
                return false;
            //if (position.SequencingID != 2)
            //    return false;
            if (position.Virtual)
                return false;
            if (position.Vector[0] != 1)
                return false;
            if (position.Vector[1] != 2)
                return false;
            if (position.Vector.Length != 2)
                return false;
            return true;
        }

        public static bool CheckLinePosB(Position position)
        {
            if (position.Name != "PositionB")
                return false;
            if (position.UserID != 101)
                return false;
            if (position.ResourceID != 201)
                return false;
            //if (position.SequencingID != 1)
            //    return false;
            if (position.Virtual)
                return false;
            if (position.Vector[0] != 3)
                return false;
            if (position.Vector[1] != 4)
                return false;
            if (position.Vector.Length != 2)
                return false;
            return true;
        }
        public  static Position GetPosA()
        {
            return GetLinePosA();
        }

        public static Position GetPosB()
        {
            return GetLinePosB();
        }

        public static bool CheckPosA(Position position)
        {
            return CheckLinePosA(position);
        }

        public static bool CheckPosB(Position position)
        {
            return CheckLinePosB(position);
        }
    
        public static Alternative GetAlternative()
        {
            var Tasks = new List<Task>();
            Task TA = new Task();
            TA.Positions.Add(TestObjects.GetPosA());
            TA.Positions.Add(TestObjects.GetPosB());
            Task TB = new Task();
            TB.Positions.Add(TestObjects.GetPosA());
            TB.Positions.Add(TestObjects.GetPosB());
            Task TC = new Task();
            TC.Positions.Add(TestObjects.GetPosA());
            TC.Positions.Add(TestObjects.GetPosB());
            Tasks.Add(TA);
            Tasks.Add(TB);
            Tasks.Add(TC);
            
            return new Alternative() {
                UserID = 99,
                ResourceID = 100,
                SequencingID = 101,
                Name = "Alternative",
                Virtual = false, 
                Tasks = Tasks
            };
        }

        public static bool CheckAlternative(Alternative alternative)
        {
            if (!CheckLinePosA(alternative.Tasks[0].Positions[0]))
                return false;
            if (!CheckLinePosB(alternative.Tasks[0].Positions[1]))
                return false;
            if (!CheckLinePosA(alternative.Tasks[1].Positions[0]))
                return false;
            if (!CheckLinePosB(alternative.Tasks[1].Positions[1]))
                return false;
            if (!CheckLinePosA(alternative.Tasks[2].Positions[0]))
                return false;
            if (!CheckLinePosB(alternative.Tasks[2].Positions[1]))
                return false;
            if(alternative.Name != "Alternative")
                return false;
            if (alternative.ResourceID != 100)
                return false;
            if (alternative.UserID != 99)
                return false;
            if (alternative.Virtual)
                return false;
            return true;
        }
    }
}