using SequencePlanner.Options;
using System;

namespace Seqencer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            SequencerOptions sequencerOptions = new SequencerOptions();
            sequencerOptions.ReadFile("test.txt");
        }
    }
}
