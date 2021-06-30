using ProSeqqoLib.Model.Hierarchy;
using ProSeqqoLib.Task;
using System;
using System.Collections.Generic;

namespace ProSeqqoLib.Model.ShortestPath
{
    public class Graph
    {
        public List<Node> Nodes { get; set; }
        //public List<int> IDs { get; set; }
        public double[,] Costs { get; set; }
        public List<MotionPath> MotionPaths {get;set;}
        public List<Hierarchy.HierarchyRecord> Records {get;set;}
        public List<Node> Starts { get; set; }
        public List<Node> Finishs { get; set; }
    
        public Graph(List<HierarchyRecord> records, CostManager costManager)
        {
            Starts = new List<Node>();
            Finishs = new List<Node>();
            Nodes = new List<Node>();
            Records = records;
            MotionPaths = new List<MotionPath>();
            initCosts(records.Count);
            var tasks = GetTasks();
            if (tasks.Count == 0)
                throw new IndexOutOfRangeException("Empty shortcut!");
            for (int i = 0; i < Records.Count; i++)
            {
                Nodes.Add(new Node(i, Records[i].Motion));
                if (Records[i].FirstTaskOfAlternative)
                    Starts.Add(Nodes[i]);
                if (Records[i].LastTaskOfAlternative)
                    Finishs.Add(Nodes[i]);
            }
            for (int i = 0; i < tasks.Count-1; i++)
            {
                var aktTask = tasks[i];
                var nextTask = tasks[i+1];
                for (int j = 0; j < records.Count; j++)
                {
                    var a = records[j];
                    for (int k = 0; k < records.Count; k++)
                    {
                        var b = records[k];
                        if(a.Task.GlobalID == aktTask.GlobalID && b.Task.GlobalID == nextTask.GlobalID)
                        {
                            Costs[j, k] = costManager.ComputeCost(a.Motion, b.Motion).FinalCost;
                        }
                    }
                }
            }
        }

        public void ComputeShortestPaths()
        {
            MotionPaths = new List<MotionPath>();
            foreach (var s in Starts)
            {
                Dijkstra dijkstra = new Dijkstra();
                dijkstra.Run(Costs, s.ID);
                foreach (var f in Finishs)
                {
                    var solution = new MotionPath(s.Motion, f.Motion);
                    solution.Cost = dijkstra.getDistance(f.ID);
                    foreach (var item in dijkstra.getSolution(f.ID))
                    {
                        solution.AddMotion(Nodes[item].Motion);
                    }
                    solution.CreateProxyMotion();
                    MotionPaths.Add(solution);
                }
            }
            //foreach (var item in MotionPaths)
            //{
            //    Console.WriteLine(item);
            //}
        }

        public List<MotionPath> GetShortestPaths()
        {
            return MotionPaths;
        }

        public List<Hierarchy.Task> GetTasks()
        {
            var tasks = new List<Hierarchy.Task>();
            Hierarchy.Task lastTask = null;

            for (int i = 0; i < Records.Count; i++)
            {
                if(lastTask is not null)
                {
                    if (lastTask.GlobalID != Records[i].Task.GlobalID)
                    {
                        lastTask = Records[i].Task;
                        tasks.Add(lastTask);
                    }
                }
                else
                {
                    lastTask = Records[i].Task;
                    tasks.Add(lastTask);
                }
            }
            return tasks;
        }

        private void initCosts(int size)
        {
            Costs = new double[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Costs[i, j] = double.MaxValue;
                    //Costs[i, j] = 0;
                }
            }
        }
    }
}