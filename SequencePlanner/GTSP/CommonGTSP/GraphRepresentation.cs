using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class GraphRepresentation
    {
        private static double PlusInfity { get; set; }
        //private static double MinusInfity { get; set; }
 
        public List<Edge> Edges { get; set; }
        public double[,] PositionMatrix {get;set;}
        public int[,] PositionMatrixRound { get; set; }
        public int WeightMultiplier { get; set; }
        private int PositionNumber { get; set; }

        public GraphRepresentation()
        {
            PlusInfity = int.MaxValue;
            //MinusInfity = int.MinValue;
            PositionNumber = -1;
            PositionMatrix = new double[1,1];
            WeightMultiplier = -1;
            Edges = new List<Edge>();
        }
        public void Build()
        {
            InitMatrices();
            InitEdgeWeightMultiplier();
            foreach (var edge in Edges)
            {
                CalculateEdgeWeight(edge);
                PositionMatrix[edge.NodeA.ID, edge.NodeB.ID] = edge.Weight;
                PositionMatrixRound[edge.NodeA.ID, edge.NodeB.ID] = Convert.ToInt32(WeightMultiplier*edge.Weight);
            }
        }

        public void CalculateEdgeWeight(Edge edge)
        {
            if(edge.NodeA.Virtual || edge.NodeB.Virtual)
            {
                edge.Weight = 0;
            }
            else
            {
                //edge.Weight = EdgeWeightCalculator(edge.NodeA.Configuration,edge.NodeB.Configuration);
                edge.Weight = edge.Weight;
            }
        }


        private void InitMatrices()
        {
            var maxPID = FindMaxID() + 1;
            PositionMatrix = new double[maxPID, maxPID];
            PositionMatrixRound = new int[maxPID, maxPID];
            for (int i = 0; i < maxPID; i++)
            {
                for (int j = 0; j < maxPID; j++)
                {
                    PositionMatrix[i, j] = PlusInfity;
                    PositionMatrixRound[i, j] = (int)PlusInfity;
                }
            }
            PositionNumber = maxPID;
        }

        private void InitEdgeWeightMultiplier()
        {
            //This function set Weight Mulitplier automatically.
            //The values of edges need to be scaled up, becauese Google-OR-Tools uses round numbers (int, long)
            //
            //---|--------|-*--*-*-**-*--*|---------------------------> PlusInfinity
            //   0    minWeight       maxWeight
            //
            //
            //---|------------|-*--**-*-**-*--*-*-**-*-**-*|----------> PlusInfinity
            //   0        minWeight                     maxWeight
            if (WeightMultiplier == -1)
            {
                var minWeight = PlusInfity;
                var maxWeight = 0.0;
                foreach (var item in Edges)
                {
                    if (item.Weight < minWeight)
                        minWeight = item.Weight;
                    if (item.Weight > maxWeight)
                        maxWeight = item.Weight;
                }

                var maxAvgWeight = PlusInfity / PositionNumber;
                maxAvgWeight /= 10;
                if (maxAvgWeight > maxWeight && maxWeight != 0)
                    WeightMultiplier = Convert.ToInt32(maxAvgWeight / maxWeight);
                else
                    WeightMultiplier = 1;

            }
        }

        private int FindMaxID()
        {
            int maxPID=0;
            foreach (var edge in Edges)
            {
                if (edge.NodeA.ID > maxPID)
                {
                    maxPID = edge.NodeA.ID;
                }

                if (edge.NodeB.ID > maxPID)
                {
                    maxPID = edge.NodeB.ID;
                }
            }
            return maxPID;
        }

        public void WriteGraph()
        {
            Console.WriteLine("Edges:");
            foreach (var item in Edges)
            {
                Console.WriteLine( item.ToString());
            }
        }

        public void WriteMatrces()
        {
            for (int i = 0; i < PositionMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < PositionMatrix.GetLength(0);j++)
                {
                    if(PositionMatrix[i, j]==int.MaxValue)
                        Console.Write("+inf" + ";");
                    else
                        Console.Write(PositionMatrix[i,j]+";");
                }
                Console.Write("\n");
            }
        }
    }
}