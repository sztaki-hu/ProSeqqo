using SequencePlanner.Function.DistanceFunction;
using SequencePlanner.GTSPTask.Serialization.SerializationObject.Token;
using SequencePlanner.Helper;
using SequencePlanner.Model;

namespace SequencePlanner.GTSPTask.Serialization.SerializationObject
{
    public class DistanceFunctionSerializationObject
    {
        public string Function { get; set; }
        public DistanceMatrixSerializationObject DistanceMatrix { get; set; }
        public double[] TrapezoidAcceleration { get; set; }
        public double[] TrapezoidSpeed { get; set; }


        public DistanceFunctionSerializationObject(){}
        public DistanceFunctionSerializationObject(PositionMatrix positionMatrix)
        {
            Function = positionMatrix.DistanceFunction.FunctionName;
            if (Function == "Matrix")
            {
                DistanceMatrix = new DistanceMatrixSerializationObject
                {
                    DistanceMatrix = ((MatrixDistanceFunction)positionMatrix.DistanceFunction).CostMatrix
                };
                foreach (var position in positionMatrix.Positions)
                {
                    if (!position.Node.Virtual)
                    {
                        DistanceMatrix.IDHeader.Add(position.Node.UserID);
                        DistanceMatrix.NameFooter.Add(position.Node.Name);
                        DistanceMatrix.ResourceFooter.Add(position.Node.ResourceID);
                    }
                }
            }

            if (Function == "TrapezoidTime")
            {
                TrapezoidAcceleration = ((TrapezoidTimeDistanceFunction)positionMatrix.DistanceFunction).MaxAcceleration;
                TrapezoidSpeed = ((TrapezoidTimeDistanceFunction)positionMatrix.DistanceFunction).MaxSpeed;
            }

            if (Function == "TrapezoidTimeWithTimeBreaker")
            {
                TrapezoidAcceleration = ((TrapezoidTimeDistanceFunction)positionMatrix.DistanceFunction).MaxAcceleration;
                TrapezoidSpeed = ((TrapezoidTimeDistanceFunction)positionMatrix.DistanceFunction).MaxSpeed;
            }
        }


        public string ToSEQShort()
        {
            string open = "[";
            string close = "]";
            string separator = ";";
            string newline = "\n";
            string seq = "";
            seq += "DistanceFunction: " + Function + newline;
            if (TrapezoidAcceleration != null)
            {
                seq += "TrapezoidAcceleration: ";
                seq += open;
                for (int i = 0; i < TrapezoidAcceleration.Length; i++)
                {
                    seq += TrapezoidAcceleration[i];
                    if (i < TrapezoidAcceleration.Length - 1)
                        seq += separator;
                }
                seq += close + newline;
            }

            if (TrapezoidSpeed != null)
            {
                seq += "TrapezoidSpeed: ";
                seq += open;
                for (int i = 0; i < TrapezoidSpeed.Length; i++)
                {
                    seq += TrapezoidSpeed[i];
                    if (i < TrapezoidSpeed.Length - 1)
                        seq += separator;
                }
                seq += close + newline;
            }
            return seq;
        }

        public string ToSEQLong()
        {
            string newline = "\n";
            string seq = "";
            if (DistanceMatrix != null)
            {
                seq += "PositionMatrix: " + newline;
                seq += DistanceMatrix.ToSEQ();
            }
            return seq;
        }

        public IDistanceFunction ToDistanceFunction()
        {
            IDistanceFunction newDistanceFunction = Function switch
            {
                "Euclidian" => new EuclidianDistanceFunction(),
                "Manhattan" => new ManhattanDistanceFunction(),
                "Max" => new MaxDistanceFunction(),
                "TrapezoidTime" => new TrapezoidTimeDistanceFunction(TrapezoidAcceleration, TrapezoidSpeed),
                "TrapezoidTimeDistanceWithTimeBreaker" => new TrapezoidTimeWithTimeBreakerDistanceFunction(TrapezoidAcceleration, TrapezoidSpeed),
                "Matrix" => new MatrixDistanceFunction(DistanceMatrix.DistanceMatrix, DistanceMatrix.IDHeader),
                _ => throw new SeqException("DistanceFunction is unknown!"),
            };
            return newDistanceFunction;
        }

        public void FillBySEQTokens(SEQTokenizer tokenizer)
        {
            Function = tokenizer.GetStringByHeader("DistanceFunction");
            if (Function != null)
            {
                if (Function == "Matrix")
                {
                    DistanceMatrix = new DistanceMatrixSerializationObject();
                    DistanceMatrix.FillBySEQTokens(tokenizer);
                }
                if (Function == "TrapezoidTime" || Function == "TrapezoidTimeDistanceWithTimeBreaker")
                {
                    TrapezoidAcceleration = tokenizer.GetDoubleVectorByHeader("TrapezoidAcceleration");
                    TrapezoidSpeed = tokenizer.GetDoubleVectorByHeader("TrapezoidSpeed");
                }
            }
        }   
    }
}