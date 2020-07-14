using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options;
using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public interface IAbstractTask
    {
        void Build();
        ORToolsResult Run();
    }
}