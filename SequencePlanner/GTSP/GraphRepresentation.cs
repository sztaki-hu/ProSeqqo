using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class GraphRepresentation
    {
        public static double Infity {get;set;}
        public List<Process> Processes { get; set; }
       
        public List<Alternative> Alternatives { get; set; }
        public List<Task> Tasks { get; set; }
        public List<Position> Positions { get; set; }
        public List<Edge> Edges { get; set; }
        public List<ConstraintDisjoint> ConstraintsDisjoints { get; set; }
        public List<ConstraintOrder> ConstraintsOrder { get; set; }


        public GraphRepresentation()
        {
            Processes = new List<Process>();
            Alternatives = new List<Alternative>();
            Tasks = new List<Task>();
            Positions = new List<Position>();
            Infity = int.MaxValue;
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

        public void createEdgesVirtual()
        {
            Edges = new List<Edge>();
            createEdgesProcessLevel();
            createEdgesAlternativeLevel();
        }

        public void createEdgesProcessLevel()
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

            foreach (var edge in Edges)
            {
                Console.WriteLine(edge.ToString()); ;
            }
        }
    }
}
