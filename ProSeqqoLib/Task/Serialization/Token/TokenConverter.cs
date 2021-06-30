using ProSeqqoLib.Helper;
using ProSeqqoLib.Task.Serialization.Model;
using System;
using System.Collections.Generic;

namespace ProSeqqoLib.Task.Serialization.Token
{
    public static class TokenConverter
    {
        public static string GetStringByHeader(this SEQTokenizer tokenizer, string header)
        {
            try
            {
                string result = null;
                bool find = false;
                foreach (var token in tokenizer.Tokens)
                {
                    if (token.Header.ToUpper().Equals(header.ToUpper()))
                    {
                        token.Phrased = true;
                        find = true;
                        result = token.Lines[0].Line;
                    }
                }
                if (find == false)
                    SeqLogger.Warning("Can not find header with: " + header, nameof(TokenConverter));
                return result;
            }
            catch (Exception e)
            {
                throw new SeqException("Can not phrase header " + header, e);
            }
        }

        public static int GetIntByHeader(this SEQTokenizer tokenizer, string header)
        {
            try
            {
                foreach (var token in tokenizer.Tokens)
                {
                    if (token.Header.ToUpper().Equals(header.ToUpper()))
                    {
                        token.Phrased = true;
                        int result = Int32.Parse(token.Lines[0].Line);
                        SeqLogger.Debug(token.Header + ": " + result, nameof(TokenConverter));
                        return result;
                    }
                }
                return -1;
            }
            catch (Exception e)
            {
                throw new SeqException("Can not phrase header " + header, e);
            }
        }

        public static bool GetBoolByHeader(this SEQTokenizer tokenizer, string header)
        {
            try
            {
                bool result = false;
                bool find = false;
                foreach (var token in tokenizer.Tokens)
                {
                    if (token.Header.ToUpper().Equals(header.ToUpper()))
                    {
                        token.Phrased = true;
                        if (token.Lines[0].Line.ToUpper().Equals("TRUE"))
                        {
                            find = true;
                            result = true;
                        }

                        if (token.Lines[0].Line.ToUpper().Equals("FALSE"))
                        {
                            find = true;
                            result = false;
                        }
                        if (find)
                            SeqLogger.Debug(token.Header + ": " + result, nameof(TokenConverter));
                    }
                }
                if (find)
                    return result;
                else
                    SeqLogger.Warning("Can not find header with: " + header, nameof(TokenConverter));
                return false;
            }
            catch (Exception e)
            {
                throw new SeqException("Can not phrase header " + header, e);
            }
        }

        public static double GetDoubleByHeader(this SEQTokenizer tokenizer, string header)
        {
            try
            {
                foreach (var token in tokenizer.Tokens)
                {
                    if (token.Header.ToUpper().Equals(header.ToUpper()))
                    {
                        token.Phrased = true;
                        double result = Double.Parse(token.Lines[0].Line);
                        SeqLogger.Debug(token.Header + ": " + result, nameof(TokenConverter));
                        return result;
                    }
                }
                return 0;
            }
            catch (Exception e)
            {
                throw new SeqException("Can not phrase header " + header, e);
            }
        }

        public static double[] GetDoubleVectorByHeader(this SEQTokenizer tokenizer, string header)
        {
            try
            {
                foreach (var token in tokenizer.Tokens)
                {
                    if (token.Header.ToUpper().Equals(header.ToUpper()))
                    {
                        token.Phrased = true;
                        string[] parts = token.Lines[0].Line.Split('[', ';', ']');
                        double[] vector = new double[parts.Length - 2];
                        for (int i = 1; i < parts.Length - 1; i++)
                        {
                            vector[i - 1] = double.Parse(parts[i]);
                        }

                        return vector;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                throw new SeqException("Can not phrase header " + header, e);
            }
        }

        public static List<ProcessHierarchySerializationObject> GetProcessHierarchyByHeader(this SEQTokenizer tokenizer, string header)
        {
            try
            {
                var hierarchy = new List<ProcessHierarchySerializationObject>();
                foreach (var token in tokenizer.Tokens)
                {
                    if (token.Header.ToUpper().Equals(header.ToUpper()))
                    {
                        token.Phrased = true;
                        for (int i = 0; i < token.Lines.Count; i++)
                        {
                            var confs = GetIntVector(token.Lines[i].Line);
                            string[] line = RemoveVector(token.Lines[i].Line).Split(";");
                            ProcessHierarchySerializationObject hierarchyObj = new ProcessHierarchySerializationObject
                            {
                                ProcessID = Int32.Parse(line[0]),
                                AlternativeID = Int32.Parse(line[1]),
                                TaskID = Int32.Parse(line[2]),
                                MotionID = Int32.Parse(line[3]),
                                ConfigIDs = confs,
                            };
                            if (line.Length > 4)
                            {
                                if (line[4].ToUpper().Equals("TRUE") || line[4].ToUpper().Equals("FALSE"))
                                    hierarchyObj.Bidirectional = bool.Parse(line[4]);
                                else
                                    hierarchyObj.Bidirectional = null;
                            }
                            if (line.Length > 5)
                                hierarchyObj.Name = line[5];
                            hierarchy.Add(hierarchyObj);
                        }
                        SeqLogger.Debug(token.Header + ": " + hierarchy.Count + " lines found", nameof(TokenConverter));
                        return hierarchy;
                    }
                }
                return hierarchy;
            }
            catch (Exception e)
            {
                throw new SeqException("Can not phrase header " + header, e);
            }
        }

        public static List<ConfigSerializationObject> GetConfigListByHeader(this SEQTokenizer tokenizer, string header)
        {
            try
            {
                var configs = new List<ConfigSerializationObject>();
                foreach (var token in tokenizer.Tokens)
                {
                    if (token.Header.ToUpper().Equals(header.ToUpper()))
                    {
                        token.Phrased = true;
                        for (int j = 0; j < token.Lines.Count; j++)
                        {

                            ConfigSerializationObject posObj = new ConfigSerializationObject();
                            List<double> config = new List<double>();
                            string[] line = token.Lines[j].Line.Split(';', '[', ']');     //1;[x;y;z;];Name--->[1][][x][y][z][][Name]
                            posObj.ID = int.Parse(line[0]);
                            int i = 2;
                            while (!line[i].Equals(""))
                            {
                                config.Add(Convert.ToDouble(line[i]));
                                i++;
                            }
                            posObj.Config = config.ToArray();
                            i++;
                            if (line.Length != i)
                            {
                                posObj.Name = line[i];
                            }
                            i++;
                            if (line.Length > i)
                            {
                                posObj.ResourceID = Convert.ToInt32(line[i]);
                            }
                            configs.Add(posObj);
                        }
                    }
                }
                return configs;
            }
            catch (Exception e)
            {
                throw new SeqException("Can not phrase header " + header, e);
            }
        }

        public static List<LineSerializationObject> GetLineListByHeader(this SEQTokenizer tokenizer, string header)
        {
            try
            {
                var lines = new List<LineSerializationObject>();
                foreach (var token in tokenizer.Tokens)
                {
                    if (token.Header.ToUpper().Equals(header.ToUpper()))
                    {
                        token.Phrased = true;
                        for (int i = 0; i < token.Lines.Count; i++)
                        {
                            string[] line = token.Lines[i].Line.Split(";");
                            LineSerializationObject lineObj = new LineSerializationObject
                            {
                                LineID = Int32.Parse(line[0]),
                                ContourID = Int32.Parse(line[1]),
                                ConfigIDA = Int32.Parse(line[2]),
                                ConfigIDB = Int32.Parse(line[3])
                            };
                            if (line.Length >= 5)
                                lineObj.Name = line[4];

                            if (line.Length >= 6)
                                lineObj.ResourceID = Int32.Parse(line[5]);

                            if (line.Length >= 7)
                                lineObj.Bidirectional = Boolean.Parse(line[6]);
                            lines.Add(lineObj);
                        }
                        SeqLogger.Debug(token.Header + ": " + lines.Count + " lines found", nameof(TokenConverter));
                        return lines;
                    }
                }
                return lines;
            }
            catch (Exception e)
            {
                throw new SeqException("Can not phrase header " + header, e);
            }
        }

        public static List<HybridLineSerializationObject> GetHybridMotionListByHeader(this SEQTokenizer tokenizer, string header)
        {
            try
            {
                var lines = new List<HybridLineSerializationObject>();
                foreach (var token in tokenizer.Tokens)
                {
                    if (token.Header.ToUpper().Equals(header.ToUpper()))
                    {
                        token.Phrased = true;
                        for (int i = 0; i < token.Lines.Count; i++)
                        {
                            string[] line = token.Lines[i].Line.Split(";");
                            HybridLineSerializationObject lineObj = new HybridLineSerializationObject
                            {
                                LineID = Int32.Parse(line[0]),
                                ConfigIDA = Int32.Parse(line[1]),
                                ConfigIDB = Int32.Parse(line[2])
                            };
                            if (line.Length >= 4)
                                lineObj.Name = line[3];

                            if (line.Length >= 5)
                                lineObj.Bidirectional = Boolean.Parse(line[4]);
                            lines.Add(lineObj);
                        }
                        SeqLogger.Debug(token.Header + ": " + lines.Count + " lines found", nameof(TokenConverter));
                        return lines;
                    }
                }
                return lines;
            }
            catch (Exception e)
            {
                throw new SeqException("Can not phrase header " + header, e);
            }
        }

        public static List<OrderConstraintSerializationObject> GetPrecedenceListByHeader(this SEQTokenizer tokenizer, string header)
        {
            try
            {
                var precedences = new List<OrderConstraintSerializationObject>();
                foreach (var token in tokenizer.Tokens)
                {
                    if (token.Header.ToUpper().Equals(header.ToUpper()))
                    {
                        token.Phrased = true;
                        for (int i = 0; i < token.Lines.Count; i++)
                        {
                            string[] line = token.Lines[i].Line.Split(";");
                            precedences.Add(new OrderConstraintSerializationObject()
                            {
                                BeforeID = Int32.Parse(line[0]),
                                AfterID = Int32.Parse(line[1])
                            });
                        }
                        SeqLogger.Trace(token.Header + ": " + precedences.Count + " precedence found", nameof(TokenConverter));
                        return precedences;
                    }
                }
                return precedences;
            }
            catch (Exception e)
            {
                throw new SeqException("Can not phrase header " + header, e);
            }
        }

        public static StrictEdgeWeightSetSerializationObject GetStrictEdgeWeightSet(this SEQTokenizer tokenizer, string header)
        {
            try
            {
                var edgeSet = new StrictEdgeWeightSetSerializationObject();
                foreach (var token in tokenizer.Tokens)
                {
                    if (token.Header.ToUpper().Equals(header.ToUpper()))
                    {
                        token.Phrased = true;
                        for (int i = 0; i < token.Lines.Count; i++)
                        {
                            string[] line = token.Lines[i].Line.Split(";");
                            StrictEdgeWeightSerializationObject edge = new StrictEdgeWeightSerializationObject
                            {
                                A = Int32.Parse(line[0]),
                                B = Int32.Parse(line[1]),
                                Weight = Double.Parse(line[2])
                            };
                            if (line.Length > 3)
                                edge.Bidirectional = Boolean.Parse(line[3]);
                            edgeSet.Weights.Add(edge);
                        }
                        SeqLogger.Trace(token.Header + ": " + edgeSet.Weights.Count + "weights found", nameof(TokenConverter));
                    }
                }
                return edgeSet;
            }
            catch (Exception e)
            {
                throw new SeqException("Can not phrase header " + header, e);
            }
        }

        public static List<int> GetIntVector(string line)
        {
            string[] parts = line.Split('[', ']');
            string[] vec = parts[1].Split(';');
            int[] vector = new int[vec.Length];
            for (int i = 0; i < vec.Length; i++)
            {
                vector[i] = int.Parse(vec[i]);
            }
            return new List<int>(vector);
        }

        public static string RemoveVector(string line)
        {
            string[] parts = line.Split('[', ']');

            return string.Concat(parts[0].Remove(parts[0].Length - 1), parts[parts.Length - 1]);
        }
    }
}