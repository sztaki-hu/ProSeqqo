using ProSeqqoLib.Function.DistanceFunction;
using ProSeqqoLib.Helper;
using ProSeqqoLib.Task.Serialization.Token;

namespace ProSeqqoLib.Task.Serialization.Model
{
    public class DistanceFunctionSerializationObject
    {
        public string Function { get; set; }
        public DistanceMatrixSerializationObject DistanceMatrix { get; set; }
        public double[] TrapezoidAcceleration { get; set; }
        public double[] TrapezoidSpeed { get; set; }


        public DistanceFunctionSerializationObject() { }
        public DistanceFunctionSerializationObject(GeneralTask task)
        {
            var distFunc = task.CostManager.DistanceFunction;
            Function = distFunc.FunctionName;
            if (Function == "Matrix")
            {
                DistanceMatrix = new DistanceMatrixSerializationObject
                {
                    DistanceMatrix = ((MatrixDistanceFunction)distFunc).CostMatrix
                };
                foreach (var motion in task.Hierarchy.Configs)
                {
                    if (!motion.Virtual)
                    {
                        DistanceMatrix.IDHeader.Add(motion.ID);
                        DistanceMatrix.NameFooter.Add(motion.Name);
                        DistanceMatrix.ResourceFooter.Add(motion.Resource.ID);
                    }
                }
            }

            if (Function == "TrapezoidTime")
            {
                TrapezoidAcceleration = ((TrapezoidTimeDistanceFunction)distFunc).MaxAcceleration;
                TrapezoidSpeed = ((TrapezoidTimeDistanceFunction)distFunc).MaxSpeed;
            }

            if (Function == "TrapezoidTimeWithTimeBreaker")
            {
                TrapezoidAcceleration = ((TrapezoidTimeDistanceFunction)distFunc).MaxAcceleration;
                TrapezoidSpeed = ((TrapezoidTimeDistanceFunction)distFunc).MaxSpeed;
            }
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
                seq += "ConfigMatrix: " + newline;
                seq += DistanceMatrix.ToSEQ();
            }
            return seq;
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