﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class GraphRepresentation
    {
        private static double PlusInfity { get; set; }
        private static double MinusInfity { get; set; }
 
        public List<Edge> Edges { get; set; }
        public List<Edge> ManualEdges { get; set; }
        public double[,] PositionMatrix {get;set;}
        public int[,] PositionMatrixRound { get; set; }
        public EdgeWeightFunctions.EdgeWeightFunction EdgeWeightCalculator { get; set; }

        public GraphRepresentation()
        {
            PlusInfity = int.MaxValue;
            MinusInfity = int.MinValue;
            PositionMatrix = new double[1,1];
            EdgeWeightCalculator = EdgeWeightFunctions.Euclidian_Distance;
        }
        public void Build(GTSPRepresentation gtsp)
        {
            initMatrices();
            foreach (var edge in Edges)
            {
                CalculateEdgeWeight(edge);
                PositionMatrix[edge.NodeA.PID, edge.NodeB.PID] = edge.Weight;
                PositionMatrixRound[edge.NodeA.PID, edge.NodeB.PID] = Convert.ToInt32(edge.Weight);
            }
            //WriteGraph();
            //WriteMatrces();
        }

        public void CalculateEdgeWeight(Edge edge)
        {
            if(edge.NodeA.Virtual || edge.NodeB.Virtual)
            {
                edge.Weight = 0;
            }
            else
            {
                edge.Weight = EdgeWeightCalculator(edge.NodeA.Configuration,edge.NodeB.Configuration);
            }
        }

        private void MakeFull(GTSPRepresentation gtsp)
        {
            PositionMatrix = new double[gtsp.Positions.Count, gtsp.Positions.Count];
            for (int i = 0; i < gtsp.Positions.Count; i++)
            {
                for (int j = 0; j < gtsp.Positions.Count; j++)
                {
                    if (i == j)
                    {
                        PositionMatrix[i, j] = MinusInfity;
                    }
                    else
                    {
                        PositionMatrix[i, j] = PlusInfity;
                    }
                }
            }

            foreach (var edge in Edges)
            {
                PositionMatrix[edge.NodeA.PID, edge.NodeB.PID] = edge.Weight;
            }

            for (int i = 0; i < gtsp.Positions.Count; i++)
            {
                for (int j = 0; j < gtsp.Positions.Count; j++)
                {
                    if (PositionMatrix[i, j] == PlusInfity)
                    {
                        Edges.Add(new Edge()
                        {
                            NodeA = gtsp.Positions[i],
                            NodeB = gtsp.Positions[j],
                            Directed = true,
                            Weight = PlusInfity
                        }) ;
                    }
                }
            }
        }

        private void initMatrices()
        {
            var maxPID = FindMaxPID() + 1;
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
        }

        private int FindMaxPID()
        {
            int maxPID=0;
            foreach (var edge in Edges)
            {
                if (edge.NodeA.PID > maxPID)
                {
                    maxPID = edge.NodeA.PID;
                }

                if (edge.NodeB.PID > maxPID)
                {
                    maxPID = edge.NodeB.PID;
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
                    Console.Write(PositionMatrix[i,j]+";");
                }
                Console.Write("\n");
            }
        }
    }
}