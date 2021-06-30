using System.Collections.Generic;

namespace ProSeqqoLib.Model
{
    public class DisjointSet<Base>
    {
        public List<Base> Elements { get; set; }


        public DisjointSet(){
            Elements = new List<Base>();
        }
    }
}