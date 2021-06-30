using System;
using System.Collections.Generic;

namespace ProSeqqoLib.Model.ShortestPath
{
    public class Dijkstra
    {
        private static readonly int NO_PARENT = -1;
        private double[] shortestDistances;
        private int[] parents;

        public void Run(double[,] adjacencyMatrix, int startVertex)
        {
            int nVertices = adjacencyMatrix.GetLength(0);
            shortestDistances = new double[nVertices];
            bool[] added = new bool[nVertices];

            for (int vertexIndex = 0; vertexIndex < nVertices; vertexIndex++)
            {
                shortestDistances[vertexIndex] = int.MaxValue;
                added[vertexIndex] = false;
            }

            shortestDistances[startVertex] = 0;
            parents = new int[nVertices];
            parents[startVertex] = NO_PARENT;

            for (int i = 1; i < nVertices; i++)
            {
                int nearestVertex = -1;
                double shortestDistance = double.MaxValue;
                for (int vertexIndex = 0;
                        vertexIndex < nVertices;
                        vertexIndex++)
                {
                    if (!added[vertexIndex] &&
                        shortestDistances[vertexIndex] <
                        shortestDistance)
                    {
                        nearestVertex = vertexIndex;
                        shortestDistance = shortestDistances[vertexIndex];
                    }
                }
                added[nearestVertex] = true;
                
                for (int vertexIndex = 0;
                        vertexIndex < nVertices;
                        vertexIndex++)
                {
                    double edgeDistance = adjacencyMatrix[nearestVertex, vertexIndex];

                    if (edgeDistance > 0
                        && ((shortestDistance + edgeDistance) < shortestDistances[vertexIndex]))
                    {
                        parents[vertexIndex] = nearestVertex;
                        shortestDistances[vertexIndex] = shortestDistance + edgeDistance;
                    }
                }
            }
        }

        public List<int> getSolution(int finishVertex)
        {
            var tmp = new List<int>();
            getPath(finishVertex, tmp);
            return tmp;
        }

        public double getDistance(int finishVertex)
        {
            return shortestDistances[finishVertex];
        }

        private void printPath(int currentVertex, int[] parents)
        {
            if (currentVertex == NO_PARENT)
            {
                return;
            }
            printPath(parents[currentVertex], parents);
            Console.Write(currentVertex + " ");
        }

        private void getPath(int currentVertex, List<int> tmp)
        {
            if (currentVertex == NO_PARENT)
            {
                return;
            }
            getPath(parents[currentVertex], tmp);
            if (parents[currentVertex] != NO_PARENT)
                tmp.Add(parents[currentVertex]);
        }
    }
}
