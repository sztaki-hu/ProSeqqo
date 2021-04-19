using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequencePlanner.GeneralModels
{
    public class ProcessPrecedence: Precedence<Process>
    {
        public ProcessPrecedence(Process before, Process after): base(before, after){

        }
    }
}
