﻿using SequencePlanner.Options;
using System;

namespace Seqencer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            SequencerOptionPhraser sequencerOptions = new SequencerOptionPhraser();
            sequencerOptions.ReadFile("test.txt");
        }
    }
}