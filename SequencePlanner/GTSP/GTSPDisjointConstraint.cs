using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.GTSP
{
    public class GTSPDisjointConstraint
    {
        public long[] DisjointSetSeq { get { GetSeqIDs(); return NodeListID.ToArray(); } private set { } }
        public int[] DisjointSetSeqInt { get { GetSeqIDs(); return NodeListIDint.ToArray(); } private set { } }
        public List<int> DisjointSetUser { get => GetUserIDs(); private set { } }
        private List<long> NodeListID;
        private List<int> NodeListIDint;
        private readonly List<BaseNode> NodeList;

        public GTSPDisjointConstraint()
        {
            NodeListID = new List<long>();
            NodeListIDint = new List<int>();
            NodeList = new List<BaseNode>();
        }

        public void Add(BaseNode node)
        {
            NodeList.Add(node);
            NodeListID.Add(node.SequencingID);
        }

        public void Add(List<BaseNode> lines)
        {
            foreach (var line in lines)
            {
                Add(line);
            }
        }

        private void GetSeqIDs()
        {
            NodeListID = new List<long>();
            NodeListIDint = new List<int>();
            foreach (var item in NodeList)
            {
                NodeListID.Add(item.SequencingID);
                NodeListIDint.Add(item.SequencingID);
            }
        }

        private List<int> GetUserIDs()
        {
            List<int> userIDs = new List<int>();
            foreach (var item in NodeList)
            {
                userIDs.Add(item.UserID);
            }
            return userIDs;
        }

        public override string ToString()
        {
            var tmp = "";
            foreach (var item in NodeList)
            {
                tmp += item.ToString() + ", ";
            }
            return "Disjoint set: " + tmp;
        }
    }
}