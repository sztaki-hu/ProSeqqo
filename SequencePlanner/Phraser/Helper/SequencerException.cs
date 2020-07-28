using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Helper
{
    public class SequencerException: Exception
    {
        public SequencerException(){}
        public SequencerException(string message) : base("\nError:" + message) { }
        public SequencerException(string message, string instruction): base("\nError: "+message + "\n" + "Instruction: " + instruction) { }
        public SequencerException(string message, Exception inner) : base(message, inner) { }
    }
}