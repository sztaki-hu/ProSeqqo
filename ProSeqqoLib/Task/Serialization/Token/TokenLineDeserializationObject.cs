using ProSeqqoLib.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProSeqqoLib.Task.Serialization.Token
{
    public class TokenLineDeserializationObject
    {
        public int LineNumber { get; set; }
        public string Line { get; set; }
        public bool KeyWord { get; set; }


        public int ToInt()
        {
            try
            {
                return Int32.Parse(Line);
            }
            catch (Exception)
            {
                throw new SeqException("Can't phrase int value at Line " + LineNumber + " - " + KeyWord);
            }
        }
        public bool ToBool()
        {
            try
            {
                return bool.Parse(Line);
            }
            catch (Exception)
            {
                throw new SeqException("Can't phrase bool value at Line " + LineNumber + " - " + KeyWord);
            }
        }

        public List<string> ToCSV()
        {
            try
            {
                return Line.Split(";").ToList();
            }
            catch (Exception)
            {
                throw new SeqException("Can't phrase csv value at Line " + LineNumber + " - " + KeyWord);
            }
        }
    }
}