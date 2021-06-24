using System.Collections.Generic;

namespace SequencePlanner.Model
{
    public class DisjointSet<Base>
    {
        public List<Base> Elements { get; set; }


        public DisjointSet(){
            Elements = new List<Base>();
        }
    }
}