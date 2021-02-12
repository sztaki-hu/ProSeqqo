using SequencePlanner.Helper;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Serialization.SerializationObject.Token
{
    public static class TokenConverter
    {
        public static bool GetBoolByHeader(string header, SEQTokenizer tokenizer)
        {
            try
            {
                bool result = false;
                bool find = false;
                foreach (var token in tokenizer.Tokens)
                {
                    if (token.Header.ToUpper().Equals(header.ToUpper()))
                    {
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
                        return result;
                    }
                }
                if (find == false)
                    SeqLogger.Warning("Can not find header with: " + header, nameof(TokenConverter));
                return false;
            }
            catch (Exception e)
            {
                throw new SeqException("Can not phrase header " + header, e);
            }
        }

        public static double GetDoubleByHeader(string header, SEQTokenizer tokenizer)
        {
            try
            {
                foreach (var token in tokenizer.Tokens)
                {
                    if (token.Header.ToUpper().Equals(header.ToUpper()))
                    {
                        double result = Double.Parse(token.Lines[0].Line);
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

        public static double[] GetDoubleVectorByHeader(string header, SEQTokenizer tokenizer)
        {
            try
            {
                foreach (var token in tokenizer.Tokens)
                {
                    if (token.Header.ToUpper().Equals(header.ToUpper()))
                    {
                        string[] parts = token.Lines[0].Line.Split('[', ';', ']');
                        double[] vector = new double[parts.Length - 2];
                        for (int i = 1; i < parts.Length-1; i++)
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

        public static int GetIntByHeader(string header, SEQTokenizer tokenizer)
        {
            try
            {
                foreach (var token in tokenizer.Tokens)
                {
                    if (token.Header.ToUpper().Equals(header.ToUpper()))
                    {
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

        public static List<ProcessHierarchySerializationObject> GetProcessHierarchyByHeader(string header, SEQTokenizer tokenizer)
        {
            try
            {
                var hierarchy = new List<ProcessHierarchySerializationObject>();
                foreach (var token in tokenizer.Tokens)
                {
                    if (token.Header.ToUpper().Equals(header.ToUpper()))
                    {
                        for (int i = 0; i < token.Lines.Count; i++)
                        {
                            string[] line = token.Lines[i].Line.Split(";");
                            ProcessHierarchySerializationObject hierarchyObj = new ProcessHierarchySerializationObject
                            {
                                PositionID = Int32.Parse(line[0]),
                                TaskID = Int32.Parse(line[1]),
                                AlternativeID = Int32.Parse(line[2]),
                                ProcessID = Int32.Parse(line[3])
                            };
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

        public static StrictEdgeWeightSetSerializationObject GetStrictEdgeWeightSet(string header, SEQTokenizer tokenizer)
        {
            try
            {
                var edgeSet = new StrictEdgeWeightSetSerializationObject();
                foreach (var token in tokenizer.Tokens)
                {
                    if (token.Header.ToUpper().Equals(header.ToUpper()))
                    {
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

        public static List<LineSerializationObject> GetLineListByHeader(string header, SEQTokenizer tokenizer)
        {
            try
            {
                var lines = new List<LineSerializationObject>();
                foreach (var token in tokenizer.Tokens)
                {
                    if (token.Header.ToUpper().Equals(header.ToUpper()))
                    {
                        for (int i = 0; i < token.Lines.Count; i++)
                        {
                            string[] line = token.Lines[i].Line.Split(";");
                            LineSerializationObject lineObj = new LineSerializationObject
                            {
                                LineID = Int32.Parse(line[0]),
                                ContourID = Int32.Parse(line[1]),
                                PositionIDA = Int32.Parse(line[2]),
                                PositionIDB = Int32.Parse(line[3])
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

        public static List<PositionSerializationObject> GetPositionListByHeader(string header, SEQTokenizer tokenizer)
        {
            try
            {
                var positions = new List<PositionSerializationObject>();
                foreach (var token in tokenizer.Tokens)
                {
                    if (token.Header.ToUpper().Equals(header.ToUpper()))
                    {
                        for (int j = 0; j < token.Lines.Count; j++)
                        {

                            PositionSerializationObject posObj = new PositionSerializationObject();
                            List<double> config = new List<double>();
                            string[] line = token.Lines[j].Line.Split(';', '[', ']');     //1;[x;y;z;];Name--->[1][][x][y][z][][Name]
                            posObj.ID = int.Parse(line[0]);
                            int i = 2;
                            while (!line[i].Equals(""))
                            {
                                config.Add(Convert.ToDouble(line[i]));
                                i++;
                            }
                            posObj.Position = config.ToArray();
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
                            positions.Add(posObj);
                        }
                    }
                }
                return positions;
            }
            catch (Exception e)
            {
                throw new SeqException("Can not phrase header " + header, e);
            }
        }

        public static List<OrderConstraintSerializationObject> GetPrecedenceListByHeader(string header, SEQTokenizer tokenizer)
        {
            try
            {
                var precedences = new List<OrderConstraintSerializationObject>();
                foreach (var token in tokenizer.Tokens)
                {
                    if (token.Header.ToUpper().Equals(header.ToUpper()))
                    {
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

        public static string GetStringByHeader(string header, SEQTokenizer tokenizer)
        {
            try
            {
                string result = null;
                bool find = false;
                foreach (var token in tokenizer.Tokens)
                {
                    if (token.Header.ToUpper().Equals(header.ToUpper()))
                    {
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
    }
}
