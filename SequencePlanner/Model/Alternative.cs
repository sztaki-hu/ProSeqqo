using SequencePlanner.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Model
{
    public class Alternative: BaseNode
    {
        public List<Task> Tasks { get; set; }

        public Alternative():base()
        {
            Name = UserID + "_Alternative_" + GlobalID;
            Tasks = new List<Task>();
        }


        //DEFAULT
        public Alternative GetCopy()
        {
            var copy = (Alternative)base.GetCopy(new Alternative());
            copy.Tasks = Tasks;
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
                Alternative alternative = (Alternative)obj;
                base.Equals((BaseNode)alternative);
                if (!alternative.Tasks.Equals(Tasks))
                    return false;
                return true;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() & Tasks.GetHashCode();
        }
    }
}