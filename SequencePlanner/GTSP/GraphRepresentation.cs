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

        public List<Process> Processes { get; set; }
        public List<Alternative> Alternatives { get; set; }
        public List<Task> Tasks { get; set; }
        public List<Position> Positions { get; set; }
        public List<Edge> Edges { get; set; }
        public List<Edge> ManualEdges { get; set; }
        public double[,] PositionMatrix {get;set;}
        public int[,] PositionMatrixRound { get; set; }
        public List<ConstraintDisjoint> ConstraintsDisjoints { get; set; }
        public List<ConstraintOrder> ConstraintsOrder { get; set; }
        public EdgeWeightFunctions.EdgeWeightFunction EdgeWeightCalculator { get; set; }

        public GraphRepresentation()
        {
            Processes = new List<Process>();
            Alternatives = new List<Alternative>();
            Tasks = new List<Task>();
            Positions = new List<Position>();
            PlusInfity = int.MaxValue;
            MinusInfity = int.MinValue;
            PositionMatrix = new double[1,1];
            ConstraintsDisjoints = new List<ConstraintDisjoint>();
            ConstraintsOrder = new List<ConstraintOrder>();
            EdgeWeightCalculator = EdgeWeightFunctions.Euclidian_Distance;
        }

        internal void Build()
        {
            createEdges();
        }

        public void addProcess(Process process)
        {
            Processes.Add(process);
            Positions.Add(process.Start);
            Positions.Add(process.Finish);
        }
        public void addProcess(Process[] processes)
        {
            foreach (var item in processes)
            {
                Processes.Add(item);
            }
        }
        public void addAlternative(Process process, Alternative alternative)
        {
            process.Alternatives.Add(alternative);
            Alternatives.Add(alternative);
            alternative.Process = process;
            Positions.Add(alternative.Start);
            Positions.Add(alternative.Finish);
        }
        public void addAlternative(Process process, Alternative[] alternatives)
        {
            foreach (var item in alternatives)
            {
                addAlternative(process, item);
            }
        }
        public void addTask(Alternative alternative, Task task) {
            alternative.Tasks.Add(task);
            Tasks.Add(task);
            task.Alternative = alternative;
            task.Process = alternative.Process;
        }
        public void addTask(Alternative alternative, Task[] tasks) {
            foreach (var item in tasks)
            {
                addTask(alternative, item);
            }
        }
        public void addPosition(Task task, Position position)
        {
            task.Positions.Add(position);
            Positions.Add(position);
            position.Task = task;
            position.Alternative = task.Alternative;
            position.Process = task.Process;
        }
        public void addPosition(Task task, Position[] positions)
        {
            foreach (var item in positions)
            {
                addPosition(task, item);
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
                edge.Weight = EdgeWeightCalculator(edge.NodeA.Configuration,edge.NodeB.Configuration);
            }
            
        }
        public void createEdges()
        {
            Edges = new List<Edge>();
            createEdgesProcessLevel();
            createEdgesTask();

        }
        public void createEdgesFull()
        {

        }
        public void createEdgesVirtual()
        {
            Edges = new List<Edge>();
            createEdgesProcessLevelVirtual();
            createEdgesAlternativeLevel();
            createEdgesTaskVirtual();
        }
        public void createEdgesVirtualFull()
        {
            createEdgesVirtual();
            makeFull();
        }
        public void createEdgesProcessLevelVirtual()
        {
            foreach (var proc in Processes)
            {
                foreach (var proc2 in Processes)
                {
                    if (proc.ID != proc2.ID)
                    {
                        Edges.Add(new Edge() { 
                            NodeA = proc.Finish, 
                            NodeB = proc2.Start, 
                            Directed = true
                        });
                    }
                }
            }
        }

        public void createEdgesProcessLevel()
        {
            foreach (var proc in Processes)
            {
                foreach (var proc2 in Processes)
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
                                        connectTasks(alternative.Tasks[alternative.Tasks.Count-1], alternative2.Tasks[0]);
                                        //connectTasks(alternative.Tasks[0], alternative2.Tasks[alternative2.Tasks.Count-1]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void createEdgesAlternativeLevel()
        {
            foreach (var alter in Alternatives)
            {
               
                Edges.Add(new Edge()
                {
                    NodeA = alter.Process.Start,
                    NodeB = alter.Start,
                    Directed = true
                });

                Edges.Add(new Edge()
                {
                    NodeA = alter.Finish,
                    NodeB = alter.Process.Finish,
                    Directed = true
                });

            }
        }
        public void createEdgesTaskVirtual()
        {
            foreach (var alternative in Alternatives)
            {
                var firstNotEmpty = -1;
                var lastNotEmpty = -1;
                for (int i = 0; i < alternative.Tasks.Count; i++)
                {
                    if (alternative.Tasks[i].Positions.Count > 0 && firstNotEmpty == -1)
                        firstNotEmpty = i;
                    if (alternative.Tasks[i].Positions.Count > 0)
                        lastNotEmpty = i;
                }
                //No task/all empty
                if (firstNotEmpty == -1 && lastNotEmpty ==-1)
                {
                    Edges.Add(new Edge()
                    {
                        NodeA = alternative.Start,
                        NodeB = alternative.Finish,
                        Directed = true
                    });
                }
                else
                {
                    foreach (var position in alternative.Tasks[firstNotEmpty].Positions)
                    {
                        Edges.Add(new Edge()
                        {
                            NodeA = alternative.Start,
                            NodeB = position,
                            Directed = true
                        });
                    }

                    foreach (var position in alternative.Tasks[lastNotEmpty].Positions)
                    {
                        Edges.Add(new Edge()
                        {
                            NodeA = position,
                            NodeB = alternative.Finish,
                            Directed = true
                        });
                    }
               
                    for (int i = 0; i < alternative.Tasks.Count; i++)
                    {
                        if (alternative.Tasks[i].Positions.Count > 0)
                        {
                            firstNotEmpty = i;
                            for (int j = i + 1; j < alternative.Tasks.Count; j++)
                            {
                                if (alternative.Tasks[j].Positions.Count > 0)
                                {
                                    lastNotEmpty = j;
                                    i = j-1;

                                    break;
                                }
                            }
                            if (firstNotEmpty != lastNotEmpty)
                                connectTasks(alternative.Tasks[firstNotEmpty], alternative.Tasks[lastNotEmpty]);
                        }
                    }
                }
            }
        }
        public void createEdgesTask()
        {
            foreach (var alternative in Alternatives)
            {
                //if (alternative.Tasks.Count == 0)
                //{
                //    //Edges.Add(new Edge()
                //    //{
                //    //    NodeA = alternative.Process.Start,
                //    //    NodeB = alternative.Process.Finish,
                //    //    Directed = true
                //    //});
                //}
                //else
                //{
                //    foreach (var position in alternative.Tasks[0].Positions)
                //    {
                //        Edges.Add(new Edge()
                //        {
                //            NodeA = alternative.Process.Start,
                //            NodeB = position,
                //            Directed = true
                //        });
                //    }
                //    foreach (var position in alternative.Tasks[alternative.Tasks.Count - 1].Positions)
                //    {
                //        Edges.Add(new Edge()
                //        {
                //            NodeA = position,
                //            NodeB = alternative.Process.Finish,
                //            Directed = true
                //        });
                //    }
                //}

                for (int i = 0; i < alternative.Tasks.Count - 1; i++)
                {
                    connectTasks(alternative.Tasks[i], alternative.Tasks[i + 1]);
                }
            }
        }

        private void connectTasks(Task a, Task b)
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
        private void makeFull()
        {
            PositionMatrix = new double[Positions.Count, Positions.Count];
            for (int i = 0; i < Positions.Count; i++)
            {
                for (int j = 0; j < Positions.Count; j++)
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

            for (int i = 0; i < Positions.Count; i++)
            {
                for (int j = 0; j < Positions.Count; j++)
                {
                    if (PositionMatrix[i, j] == PlusInfity)
                    {
                        Edges.Add(new Edge()
                        {
                            NodeA = Positions[i],
                            NodeB = Positions[j],
                            Directed = true,
                            Weight = PlusInfity
                        }) ;
                    }
                }
            }
        }
        public List<Edge> deleteVirtualNodes()
        {
            PositionMatrix = new double[Positions.Count, Positions.Count];
            List<Edge> edges = new List<Edge>();
            foreach (var edge in Edges)
            {
                if (edge.Directed)
                {
                    PositionMatrix[edge.NodeA.PID, edge.NodeB.PID] = 1;
                }
                else
                {
                    PositionMatrix[edge.NodeA.PID, edge.NodeB.PID] = 1;
                    PositionMatrix[edge.NodeB.PID, edge.NodeA.PID] = 1;
                }
            }

            for (int i = 0; i < Positions.Count; i++)
            {
                Console.WriteLine("\n");
                for (int j = 0; j < Positions.Count; j++)
                {
                    Console.Write(PositionMatrix[i, j]+" ");
                }
            }

            return edges;
        }
        public void applyConstraints() { }
        public void generateDisjunctSets() {
            ConstraintsDisjoints = new List<ConstraintDisjoint>();
            foreach (var process in Processes)
            {
                int[] taskNumberOfAlternatives = new int[process.Alternatives.Count];
                int maxTaskNumber = 0;
                for (int i = 0; i < process.Alternatives.Count; i++)
                {
                    taskNumberOfAlternatives[i] = process.Alternatives[i].Tasks.Count;
                    if (taskNumberOfAlternatives[i] > maxTaskNumber)
                        maxTaskNumber = taskNumberOfAlternatives[i];
                }

            }
        }

        public Process findProcess(int ID)
        {
            foreach (var item in Processes)
            {
                if (item.ID == ID)
                {
                    return item;
                }
            }
            return null;
        }

        public Alternative findAlternative(int ID)
        {
            foreach (var item in Alternatives)
            {
                if (item.ID == ID)
                {
                    return item;
                }
            }
            return null;
        }

        public Task findTask(int ID)
        {
            foreach (var item in Tasks)
            {
                if (item.ID == ID)
                {
                    return item;
                }
            }
            return null;
        }

        public Position findPosition(int ID)
        {
            foreach (var item in Positions)
            {
                if (item.ID == ID)
                {
                    return item;
                }
            }
            return null;
        }

        public void WriteGraph()
        {
            Console.WriteLine("Processes:");
            foreach (var item in Processes)
            {
                Console.WriteLine( item.ToString());
            }
            Console.WriteLine("Alternatives:");
            foreach (var item in Alternatives)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine("Tasks:");
            foreach (var item in Tasks)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine("Positions:");
            foreach (var item in Positions)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine("Edges:");
            foreach (var edge in Edges)
            {
                Console.WriteLine(edge.ToString()); ;
            }
        }
        public string CreateGraphViz( string file = null, bool virtualNodes = true)
        {
            string viz = "digraph G {\n";
            viz += "node[style = filled];\n";
            viz += "\n\t" + "style = filled;";
            viz += "\n\t" + "color = salmon2;";
            foreach (var item in Edges)
            {
                viz += "\t" + item.NodeA.Name + " -> " + item.NodeB.Name + "[label = " + item.Weight.ToString("0,0.00", new CultureInfo("en-US", false)) + "];\n";
            }

            foreach (var proc in Processes)
            {
                string subgraph = "";
                if (virtualNodes)
                {
                    foreach (var pos in Positions)
                    {
                        if (proc.ID == pos.Process.ID && inEdges(pos.ID))
                        {
                            subgraph += pos.Name + "; ";
                        }
                    }
                }
                else
                {
                    foreach (var pos in Positions)
                    {
                        if (proc.ID == pos.Process.ID)
                        {
                            var find = false;
                            foreach (var alt in Alternatives)
                            {
                                if (pos.ID == alt.Start.ID || pos.ID == alt.Finish.ID)
                                {
                                    find = true;
                                }
                            }
                            if(!find && inEdges(pos.ID))
                                subgraph += pos.Name + "; ";
                        }
                    }
                }
                viz += addSubgraph(subgraph, proc.Name, "", "thistle3");

                if (virtualNodes)
                {
                    foreach (var alt in proc.Alternatives)
                    {
                        string subgraphAlt = "\t";
                        foreach (var pos in Positions)
                        {
                            if (proc.ID == pos.Process.ID && pos.Alternative?.ID == alt.ID)
                            {
                                if(inEdges(pos.ID))
                                    subgraphAlt += pos.Name + "; ";
                            }
                        }
                        viz += addSubgraph(subgraphAlt, alt.Name, "\t\t", "lightblue");
                        viz += addTasksSubgraph(alt);
                        viz += "\n\t\t}\n";
                    }
                }
                viz += "\n\t}\n";
            }
            viz += "}";
            //Console.WriteLine(viz);
            //if (file!=null)
                System.IO.File.WriteAllText(file, viz);
            return viz;
        }
        private string addSubgraph(string content, string label, string prefix = "\t", string color = "white")
        {
            string viz = "";
            viz += "\n" + prefix + "subgraph cluster_" + label + "{";
            viz += "\n\t" + prefix + "style = filled;";
            viz += "\n\t" + prefix + "color = "+color+";";
            viz += "\n\t" + prefix + "label = " + label + ";\n\t\t";
            viz += content;
            return viz;
        }
        private string addTasksSubgraph(Alternative a)
        {
            string tmp = "";
            foreach (var task in a.Tasks)
            {
                tmp += "subgraph cluster_" + task.Name + "{";
                tmp+="\nstyle = filled;";
                tmp += "\ncolor = peachpuff1 ;";
                tmp += "\nlabel = " + task.Name+";";
                foreach (var pos in task.Positions)
                {
                    tmp += pos.Name+ ";";
                }
                tmp += "}\n";
            }
            return tmp;
        }

        private bool inEdges(int ID)
        {
            foreach (var edge in Edges)
            {
                if(edge.NodeA.ID == ID || edge.NodeB.ID == ID)
                    return true;
                         
            }
            return false;
        }
    
    }
}