using ProSeqqoLib.Helper;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ProSeqqoLib.Task.Serialization.Token
{
    public class SEQTokenizer
    {
        public List<Token> Tokens { get; set; }


        public void Tokenize(List<string> seqString)
        {
            SeqLogger.Debug("Tokenization started!", nameof(SEQTokenizer));
            SeqLogger.Indent++;
            Tokens = new List<Token>();
            var lines = new List<TokenLineDeserializationObject>();
            for (int i = 0; i < seqString.Count; i++)
            {
                lines.Add(new TokenLineDeserializationObject() { LineNumber = i + 1, Line = seqString[i] });
            }

            lines = DeleteComments(lines);
            lines = DeleteWhiteSpace(lines);
            lines = ChangeDotToComma(lines);
            lines = SeperateByDoubleDot(lines);
            lines = DeleteEmptyLines(lines);
            Tokenize(lines);
            SeqLogger.Indent--;
            SeqLogger.Debug("Tokenization finished!", nameof(SEQTokenizer));
        }

        private List<TokenLineDeserializationObject> DeleteEmptyLines(List<TokenLineDeserializationObject> lines)
        {
            List<TokenLineDeserializationObject> noEmptyLine = new List<TokenLineDeserializationObject>();
            foreach (var line in lines)
            {
                if (!line.Line.Equals(""))
                    noEmptyLine.Add(line);
            }
            SeqLogger.Trace("Tokenization: Empty lines deleted!", nameof(SEQTokenizer));
            return noEmptyLine;
        }

        private void Tokenize(List<TokenLineDeserializationObject> lines)
        {
            Token lastToken = new Token();
            foreach (var item in lines)
            {
                if (item.KeyWord)
                {
                    lastToken = new Token() { Header = item.Line };
                    Tokens.Add(lastToken);
                }
                else
                {
                    lastToken.Lines.Add(item);
                }
            }
            SeqLogger.Trace("Tokenization: Tokenized!", nameof(SEQTokenizer));
        }
        public Token FindTokenByHeader(string header)
        {
            foreach (var item in Tokens)
            {
                if (item.Header == header)
                {
                    item.Phrased = true;
                    return item;
                }
            }
            return null;
        }
        public void CheckNotPhrased()
        {
            foreach (var token in Tokens)
            {
                if (!token.Phrased)
                    if(token.Lines.Count>0)
                        throw new SeqException(token.Header + " is not used / valid option in line " + token.Lines[0].LineNumber + "!");
                    else
                        throw new SeqException(token.Header + " is not used / valid option!");
            }
        }

        private List<TokenLineDeserializationObject> DeleteWhiteSpace(List<TokenLineDeserializationObject> lines)
        {
            List<TokenLineDeserializationObject> cleanLines = new List<TokenLineDeserializationObject>();

            for (int i = 0; i < lines.Count; i++)
            {
                lines[i].Line = lines[i].Line.Replace(" ", "");
                lines[i].Line = lines[i].Line.Replace("\n", "");
                lines[i].Line = lines[i].Line.Replace("\t", "");

                if (!lines[i].Equals(""))
                    cleanLines.Add(lines[i]);
            }
            SeqLogger.Trace("Tokenization: White spaces deleted!", nameof(SEQTokenizer));
            return cleanLines;
        }
        private List<TokenLineDeserializationObject> DeleteComments(List<TokenLineDeserializationObject> lines)
        {
            List<TokenLineDeserializationObject> noComments = new List<TokenLineDeserializationObject>();
            string[] words;
            foreach (var line in lines)
            {
                words = line.Line.Split(new string[] { "#", "//" }, StringSplitOptions.None);
                noComments.Add(new TokenLineDeserializationObject()
                {
                    LineNumber = line.LineNumber,
                    Line = words[0]
                });
            }
            SeqLogger.Trace("Tokenization: Comments deleted!", nameof(SEQTokenizer));
            return noComments;
        }
        private List<TokenLineDeserializationObject> SeperateByDoubleDot(List<TokenLineDeserializationObject> lines)
        {
            List<TokenLineDeserializationObject> sepByComma = new List<TokenLineDeserializationObject>();
            string[] words;
            foreach (var line in lines)
            {
                words = line.Line.Split(':');
                for (int i = 0; i < words.Length; i++)
                {
                    if (!words[i].Equals(""))
                        sepByComma.Add(new TokenLineDeserializationObject() { LineNumber = line.LineNumber, Line = words[i], KeyWord = (i == 0 && words.Length > 1) });
                }
                foreach (var word in words)
                {

                }

            }
            SeqLogger.Trace("Tokenization: Seperated by double dots!", nameof(SEQTokenizer));
            return sepByComma;
        }
        private List<TokenLineDeserializationObject> ChangeDotToComma(List<TokenLineDeserializationObject> lines)
        {
            if (Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator == ",")
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    lines[i].Line = lines[i].Line.Replace(".", ",");
                }
                SeqLogger.Trace("Tokenization: Commas to dots!", nameof(SEQTokenizer));
            }
            else
            {
                if (Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator == ".")
                {
                    for (int i = 0; i < lines.Count; i++)
                    {
                        lines[i].Line = lines[i].Line.Replace(",", ".");
                    }
                }
                else
                {
                    for (int i = 0; i < lines.Count; i++)
                    {
                        lines[i].Line = lines[i].Line.Replace(",", ".");
                    }
                }
                SeqLogger.Trace("Tokenization: Dots to commas!", nameof(SEQTokenizer));
            }
            return lines;
        }
    }
}