using SequencePlanner.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Model
{
    public class StrictEdgeWeightSet
    {
        private List<StrictEdgeWeight> List { get; set; }


        public StrictEdgeWeightSet()
        {
            List = new List<StrictEdgeWeight>();
        }

        public StrictEdgeWeightSet(List<StrictEdgeWeight> strictEdges)
        {
            List = strictEdges;
        }

        public List<StrictEdgeWeight> GetAll()
        {
            return List;
        }
            
        public List<StrictEdgeWeight> Get(Position position)
        {
            var tmp = new List<StrictEdgeWeight>();
            foreach (var item in List)
            {
                if ((item.A.GlobalID == position.GlobalID) || (item.B.GlobalID == position.GlobalID))
                    tmp.Add(item);
            }
            if (tmp.Count == 0)
                return null;
            else
                return tmp;
        }

        public StrictEdgeWeight Get(BaseNode node1, BaseNode node2)
        {
            foreach (var item in List)
            {
                if (item.FitFor(node1, node2))
                    return item;
            }
            return null;
        }

        public void Add(Position a, Position b, double weight)
        {
            List.Add(new StrictEdgeWeight(a,b,weight));
        }

        public void Add(StrictEdgeWeight strictEdge)
        {
            List.Add(strictEdge);
        }

        public void Delete(Position A, Position B, bool anyDirection = true)
        {
            List.Remove(Get(A, B));
            if (anyDirection)
                List.Remove(Get(B, A));
        }


        public void DeleteAll()
        {
            List.Clear();
        }

        //Delete edges that contains the given position
        public void Delete(Position position){
            for (int i = 0; i < List.Count; i++)
            {
                if (List[i].A.GlobalID == position.GlobalID || List[i].B.GlobalID == position.GlobalID)
                {
                    List.RemoveAt(i);
                    i--;
                }
            }
        }
        public void ToLog(LogLevel level)
        {
            foreach (var edge in List)
            {
                SeqLogger.WriteLog(level, edge.ToString() , nameof(StrictEdgeWeightSet));

            }
        }
    }
}