using SequencePlanner.Model;
using SequencePlanner.OR_Tools;
using System;

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

        public event IBaseTask.TaskCompleted SequencingTaskCompleted;

        public BaseTask()
        {
            PositionMatrix = new PositionMatrix();
        }


        protected int[,] ScaleUpWeights(double[,] matrix)
        {
            int[,] roundedMatrix = new int[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    roundedMatrix[i, j] = Convert.ToInt32(IGTSPRepresentation.WEIGHT_MULTIPLIER * matrix[i, j]);
                }
            }
            return roundedMatrix;
        }

        //public abstract ITaskResult RunModel();

        //public abstract Task<ITaskResult> RunModelAsync(int taskID, CancellationToken cancellationToken);

        public abstract void ValidateModel();

    }
}