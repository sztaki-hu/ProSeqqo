using System;


namespace SequencePlanner.Helper
{
    public class SeqException : Exception
    {
        public SeqException() { }
        public SeqException(string message) : base("\nError:" + message)
        {
            SeqLogger.Critical(message); 
        }
        public SeqException(string message, string instruction) : base("\nError: " + message + "\n" + "Instruction: " + instruction)
        {
            SeqLogger.Critical(message);
        }
        public SeqException(string message, Exception inner) : base(message, inner) {
            SeqLogger.Critical(message);
        }
    }
}
