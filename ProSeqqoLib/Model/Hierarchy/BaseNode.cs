namespace ProSeqqoLib.Model.Hierarchy
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
                Name = Name + "_Reverse"
            };
        }

        //REVERSE
        public BaseNode GetCopy(BaseNode node = null)
        {
            if (node is null)
                return new BaseNode()
                {
                    UserID = UserID,
                    ResourceID = ResourceID,
                    SequencingID = SequencingID,
                    Virtual = Virtual,
                    Name = Name
                };
            else
            {
                node.UserID = UserID;
                node.ResourceID = ResourceID;
                node.SequencingID = SequencingID;
                node.Virtual = Virtual;
                node.Name = Name;
                return node;
            }
        }
        public override string ToString()
        {
            var str = Name;
            str += "[GID:" + GlobalID;
            str += "|UID:" + UserID;
            str += "|SID:" + SequencingID;
            str += "|RID:" + ResourceID + "]";
            return str;
        }
        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                BaseNode node = (BaseNode)obj;
                if (node.UserID != UserID)
                    return false;
                if (node.ResourceID != ResourceID)
                    return false;
                if (node.GlobalID != GlobalID)
                    return false;
                if (node.SequencingID != SequencingID)
                    return false;
                if (node.Virtual != Virtual)
                    return false;
                if (!node.Name.Equals(Name))
                    return false;
                return true;
            }
        }
        public override int GetHashCode()
        {
            return UserID.GetHashCode() & ResourceID.GetHashCode() & GlobalID.GetHashCode() & SequencingID.GetHashCode() & Virtual.GetHashCode() & Name.GetHashCode();
        }
    }
}
