using System;


namespace SequencePlanner.Helper
{
    public class SeqException : Exception
    {
        public SeqException() { }
        public SeqException(string message) : base("\nError:" + message)
        {
            SeqLogger.Error(message); 
        }
        public SeqException(string message, string instruction) : base("\nError: " + message + "\n" + "Instruction: " + instruction)
        {
            SeqLogger.Error(message);
        }
        public SeqException(string message, Exception inner) : base(message, inner) {
            SeqLogger.Error(message);
        }
    }
}
