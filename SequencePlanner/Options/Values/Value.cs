using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Options.Values
{
    public class Value<T>
    {
        public T Valxue { get; set; }

        public Value(T asdf){
            Valxue = asdf;
        }

    }
}
