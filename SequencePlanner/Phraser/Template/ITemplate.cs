using SequencePlanner.Phraser.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public interface ITemplate
    {
        OptionSet OptionSet { get; set; }

        void Read(string file);
        void Write(string file);   
    }
}