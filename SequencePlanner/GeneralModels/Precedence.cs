using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePlanner.GeneralModels
{
    public class Precedence<Base>
    {
        public Base Before { get; set; }
        public Base After { get; set; }

        public Precedence(Base before, Base after){
            Before = before;
            After = after;
        }
    }
}