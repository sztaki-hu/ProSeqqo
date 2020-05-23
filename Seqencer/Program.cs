using SequencePlanner;
using System;

namespace Seqencer
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Test test = new Test();
            test.ReadFileTest();
            test.RepresentationTest();
            test.SequencerTaskTest();
            
            Console.ReadKey();
        }
    }
}