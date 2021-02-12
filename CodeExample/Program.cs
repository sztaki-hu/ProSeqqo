using CodeExample.LineLike;
using CodeExample.PointLike;
using System;

namespace CodeExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //var example1 = new PLExample1();
            //example1.Run();
            //example1.PrintSolution();

            var example2 = new LLExample1();
            example2.Run();
            example2.PrintSolution();
        }
    }
}
