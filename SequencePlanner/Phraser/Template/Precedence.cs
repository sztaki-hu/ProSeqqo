using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public class Precedence<T>
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public T Before { get; set; }
        public T After { get; set; }

        public Precedence(int id, T before, T after, string name = null)
        {
            ID = id;
            Before = before;
            After = after;
            if (name == null)
                Name = "Line_" + ID;
            else
                Name = name;
        }
    }
}
