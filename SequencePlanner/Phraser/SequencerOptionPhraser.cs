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

        public List<string> ReadFile(string[] rawLines)
        {
            List<string> lines = FindLines(rawLines);
            lines = DeleteComments(lines);
            lines = DeleteWhiteSpace(lines);
            lines = ChangeDotToComma(lines);
            lines = SeperateByDoubleDot(lines);
            Lines = lines;
            return Lines;
        }

        private List<string> FindLines(string[] lines)
        {
            List<string> lineList = new List<string>();
            foreach (string line in lines)
            {
                    lineList.Add(line);
            }
            return lineList;
        }
        private List<string> DeleteWhiteSpace(List<string> lines)
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
        private List<string> DeleteComments(List<string> lines)
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
        private List<string> SeperateByDoubleDot(List<string> lines)
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
        private List<string> ChangeDotToComma(List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                lines[i] = lines[i].Replace(".", ",");
            }
            return lines;
        }
    }
}