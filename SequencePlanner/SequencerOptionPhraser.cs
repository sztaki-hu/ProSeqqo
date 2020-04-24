using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SequencePlanner.Options
{
    public class SequencerOptionPhraser
    {
        public SequencerOptionSet OptionSet { get; set; }
        public SequencerOptionPhraser()
        {
            

        }

        public void ReadFile(string file)
        {
            List<string> lines = findLines(file);
            lines = deleteComments(lines);
            lines = deleteWhiteSpace(lines);
            lines = seperateByDoubleDot(lines);

            foreach (var line in lines)
            {
                Console.WriteLine("<"+line+">");
            }
            
            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }

        public List<string> findLines(string file)
        {
            string[] lines = File.ReadAllLines(@file);
            List<string> lineList = new List<string>();
            foreach (string line in lines)
            {
                if (!line.Contains("#"))
                {
                    lineList.Add(line);
                }
            }
            return lineList;
        }

        public List<string> deleteWhiteSpace(List<string> lines)
        {
            List<string> cleanLines = new List<string>();

                for (int i = 0; i < lines.Count; i++)
                {
                    lines[i] = lines[i].Replace(" ", "");
                    lines[i] = lines[i].Replace("\n", "");
                    lines[i] = lines[i].Replace("\t", "");
                   
                    if (!lines[i].Equals(""))
                        cleanLines.Add(lines[i]);
                }
            return cleanLines;
        }

        public List<string> deleteComments(List<string> lines)
        {
            List<string> noComments = new List<string>();
            string[] words;
            foreach (var line in lines)
            {
                words = line.Split('#');
                noComments.Add(words[0]);
            }
            return noComments;
        }

        public List<string> seperateByDoubleDot(List<string> lines)
        {
            List<string> sepByComma = new List<string>();
            string[] words;
            foreach (var line in lines)
            {
                words = line.Split(':');
                foreach (var word in words)
                {
                    if (!word.Equals(""))
                        sepByComma.Add(word);
                }
                
            }
            return sepByComma;
        }
    }
}