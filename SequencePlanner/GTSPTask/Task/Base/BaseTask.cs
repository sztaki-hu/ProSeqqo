using System;
using System.Diagnostics;
using SequencePlanner.Model;
using SequencePlanner.Helper;
using SequencePlanner.OR_Tools;

namespace SequencePlanner.GTSPTask.Task.Base
{
    public abstract class BaseTask: IBaseTask
    {
        public int Dimension { get;  set; }
        public bool CyclicSequence  { get;  set; }
        public int WeightMultipier  { get;  set; }
        public Position StartDepot  { get;  set; }
        public Position FinishDepot  { get;  set; }
        public PositionMatrix PositionMatrix  { get;  set; }
        public int TimeLimit { get; set; }
        public bool UseMIPprecedenceSolver { get; set; }
        public bool Validate { get; set; }
        public LocalSearchStrategyEnum.Metaheuristics LocalSearchStrategy { get; set; }
        public event IBaseTask.TaskCompleted SequencingTaskCompleted;
        protected Stopwatch Timer { get; set; }
        protected TimeSpan MIPRunTime { get; set; }
        protected IDepotMapper DepotMapper { get; set; }


        public BaseTask()
        {
            Timer = new Stopwatch();
            PositionMatrix = new PositionMatrix();
            LocalSearchStrategy = LocalSearchStrategyEnum.Metaheuristics.Automatic;
            WeightMultipier = 1000;
            Validate = false;
        }


        protected int[,] ScaleUpWeights(double[,] matrix)
        {
            int[,] roundedMatrix = new int[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    roundedMatrix[i, j] = Convert.ToInt32(WeightMultipier * matrix[i, j]);
                }
            }
            return roundedMatrix;
        }

        public abstract void ValidateModel();

        public void ToLog(LogLevel level)
        {
            SeqLogger.WriteLog(level, "Dimension: " + Dimension, nameof(BaseTask));
            SeqLogger.WriteLog(level, "CyclicSequence: " + CyclicSequence, nameof(BaseTask));
            SeqLogger.WriteLog(level, "StartDepot: " + StartDepot, nameof(BaseTask));
            SeqLogger.WriteLog(level, "FinishDepot: " + FinishDepot, nameof(BaseTask));
            SeqLogger.WriteLog(level, "TimeLimit: " + TimeLimit, nameof(BaseTask));
            SeqLogger.WriteLog(level, "UseMIPprecedenceSolver: " + UseMIPprecedenceSolver, nameof(BaseTask));
            SeqLogger.WriteLog(level, "LocalSearchStrategy: " + LocalSearchStrategy, nameof(BaseTask));
            PositionMatrix.ToLog(level);
        }
    }
}