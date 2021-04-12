using System;

namespace SequencePlanner.Model
{
    public class BaseNode
    {
        private static int MAX_GLOBALID = 0;
        public int UserID { get; set; }
        public int GlobalID { get; set; }
        public int ResourceID { get; set; }
        public int SequencingID { get; set; }
        public bool Virtual { get; set; }
        public string Name { get; set; }

       
        public BaseNode()
        {
            GlobalID = ++MAX_GLOBALID;
            UserID = GlobalID;
            SequencingID = -1;
            ResourceID = -1;
            Virtual = false;
            Name = UserID + "_BaseNode_" + GlobalID;
        }

        public virtual BaseNode GetReverse()
        {
            return new BaseNode()
            {
                UserID = this.UserID,
                SequencingID = this.SequencingID,
                ResourceID = this.ResourceID,
                Virtual = this.Virtual,
                Name = Name+"_Reverse"
            };
        }

        public override string ToString()
        {
            var str = Name;
            str += " [Global:" + GlobalID;
            str += "|UserID:" + UserID;
            str += "|SeqID:" + SequencingID+"]";
            return str;
        }
    }
}
