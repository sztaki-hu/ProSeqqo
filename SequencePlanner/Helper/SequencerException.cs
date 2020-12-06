using System;


namespace SequencePlanner.Helper
{
    public class SequencerException : Exception
    {
        public SequencerException() { }
        public SequencerException(string message) : base("\nError:" + message)
        {
            SeqLogger.Error(message); 
        }
        public SequencerException(string message, string instruction) : base("\nError: " + message + "\n" + "Instruction: " + instruction)
        {
            SeqLogger.Error(message);
        }
        public SequencerException(string message, Exception inner) : base(message, inner) {
            SeqLogger.Error(message);
        }
    }
}
