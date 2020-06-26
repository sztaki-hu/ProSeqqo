﻿using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options;
using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public abstract class Template
    {
        public bool Validated { get; set; }
        public OptionSet OptionSet { get; set;  }

        public abstract SequencerTask Compile();
        public abstract void Validate();
    }
}