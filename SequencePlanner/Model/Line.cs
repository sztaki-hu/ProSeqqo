using SequencePlanner.Helper;

namespace SequencePlanner.Model
{
    public class Line: BaseNode
    {
        public static bool BIDIRECTIONAL_DEFAULT = false;
        public Position NodeA { get; set; }
        public Position NodeB { get; set; }
        //Normal line NodeA->NodeB, reverse NodeB->NodeA
        public bool Bidirectional { get; set; }
        public double Length { get; set; }

        public Line(): base()
        {
            Name = UserID + "_Line_" +GlobalID;
            NodeA = null;
            NodeB = null;
            Bidirectional = BIDIRECTIONAL_DEFAULT;
            Length = 0;
        }

        //The new line has new GlobalID
        public Line Copy()
        {
            return new Line
            {
                Name = Name,
                NodeA = NodeA,
                NodeB = NodeB,
                UserID = UserID,
                Bidirectional = Bidirectional,
                ResourceID = ResourceID,
                Virtual = Virtual,
                Length = Length
            };
        }

        public override BaseNode GetReverse()
        {
            return new Line()
            {
                UserID = this.UserID,
                SequencingID = this.SequencingID,
                ResourceID = this.ResourceID,
                Virtual = this.Virtual,
                Name = Name + "_Reverse",
                NodeA = NodeB,
                NodeB = NodeA,
                Bidirectional = this.Bidirectional,
                Length = Length
            };
        }

        public void Validate()
        {
            if (NodeA == null)
                throw new SeqException("Line with UserID: "+UserID+" NodeA should not be null.");
            if (NodeB == null)
                throw new SeqException("Line with UserID: " + UserID + " NodeB should not be null.");
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Line node = (Line)obj;
                base.Equals((BaseNode)node);
                if (!node.NodeA.Equals(NodeA))
                    return false;
                if (node.NodeB.Equals(NodeB))
                    return false;
                if (node.Bidirectional != Bidirectional)
                    return false;
                if (node.Length != Length)
                    return false;
                return true;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() & NodeA.GetHashCode() & NodeB.GetHashCode() & Bidirectional.GetHashCode() & Length.GetHashCode();
        }
    }
}