using System.Collections.Generic;

namespace SequencePlanner.Model
{
    public class Process : BaseNode
    {
        public List<Alternative> Alternatives { get; set; }


        public Process(): base()
        {
            Name = UserID + "_Process_" + GlobalID;
            Alternatives = new List<Alternative>();
        }


        //DEFAULT
        public Process GetCopy()
        {
            var copy = (Process) base.GetCopy(new Process());
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
                Process node = (Process)obj;
                base.Equals((BaseNode)node);
                if (!node.Alternatives.Equals(Alternatives))
                    return false;
                return true;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() & Alternatives.GetHashCode();
        }
    }
}
