using System;
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
        public List<ConstraintDisjoint> ConstraintsDisjoints { get; set; }
        public List<ConstraintOrder> ConstraintsOrder { get; set; }
        public EdgeWeightFunctions.EdgeWeightFunction EdgeWeightCalculator { get; set; }

        public GraphRepresentation()
        {
            PlusInfity = int.MaxValue;
            MinusInfity = int.MinValue;
            PositionMatrix = new double[1,1];
            ConstraintsDisjoints = new List<ConstraintDisjoint>();
            ConstraintsOrder = new List<ConstraintOrder>();
            EdgeWeightCalculator = EdgeWeightFunctions.Euclidian_Distance;
        }
        public void Build(GTSPRepresentation gtsp)
        {
            CreateEdges(gtsp);
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
        public void CreateEdges(GTSPRepresentation gtsp)
        {
            Edges = new List<Edge>();
            CreateEdgesProcess(gtsp);
            CreateEdgesTask(gtsp);
        }
        public void CreateEdgesProcess(GTSPRepresentation gtsp)
        {
            foreach (var proc in gtsp.Processes)
            {
                foreach (var proc2 in gtsp.Processes)
                {
                    if (proc.ID != proc2.ID)
                    {
                        foreach (var alternative in proc.Alternatives)
                        {
                            if (alternative.Tasks.Count > 0)
                            {
                                foreach (var alternative2 in proc2.Alternatives)
                                {
                                    if (alternative.ID!=alternative2.ID && alternative2.Tasks.Count > 0)
                                    {
                                        ConnectTasks(alternative.Tasks[alternative.Tasks.Count-1], alternative2.Tasks[0]);
                                        //connectTasks(alternative.Tasks[0], alternative2.Tasks[alternative2.Tasks.Count-1]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void CreateEdgesTask(GTSPRepresentation gtsp)
        {
            foreach (var alternative in gtsp.Alternatives)
            {
                for (int i = 0; i < alternative.Tasks.Count - 1; i++)
                {
                    ConnectTasks(alternative.Tasks[i], alternative.Tasks[i + 1]);
                }
            }
        }
        private void ConnectTasks(Task a, Task b)
        {
            foreach (var posA in a.Positions)
            {
                foreach (var posB in b.Positions)
                {
                    Edges.Add(new Edge()
                    {
                        NodeA = posA,
                        NodeB = posB,
                        Weight = EdgeWeightCalculator(posA.Configuration, posB.Configuration),
                        Directed = true
                    });
                }
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

        public void WriteGraph()
        {
            Console.WriteLine("Edges:");
            foreach (var item in Edges)
            {
                Console.WriteLine( item.ToString());
            }
           
        }
    }
}