using SequencePlanner.Phraser.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SequencePlanner.Phraser
{
    public class SequencerOptionPhraser
    {
        public List<string> Lines { get; set; }

        public SequencerOptionPhraser()
        {
        }

        public List<string> ReadFile(string[] rawLines)
        {
            List<string> lines = findLines(rawLines);
            lines = deleteComments(lines);
            lines = deleteWhiteSpace(lines);
            lines = changeDotToComma(lines);
            lines = seperateByDoubleDot(lines);
            Lines = lines;
            return Lines;
        }

        private List<string> findLines(string[] lines)
        {
            List<string> lineList = new List<string>();
            foreach (string line in lines)
            {
                    lineList.Add(line);
            }
            return lineList;
        }
        private List<string> deleteWhiteSpace(List<string> lines)
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
        private List<string> deleteComments(List<string> lines)
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
        private List<string> seperateByDoubleDot(List<string> lines)
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
        private List<string> changeDotToComma(List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                lines[i] = lines[i].Replace(".", ",");
            }
            return lines;
        }
    }
}