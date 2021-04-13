using System.Collections.Generic;

namespace SequencePlanner.Model
{
    public class Task: BaseNode
    {
        public List<GTSPNode> Positions { get; set; }


        public Task():base()
        {
            Name = UserID + "_Task_" + GlobalID;
            Positions = new List<GTSPNode>();
        }


        //DEFAULT
        public Task GetCopy()
        {
            var copy = (Task)base.GetCopy(new Task());
            copy.Positions = Positions;
            return copy;
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
                Task node = (Task)obj;
                base.Equals((BaseNode)node);
                node.Positions = Positions;
                return true;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() & Positions.GetHashCode();
        }
    }
}