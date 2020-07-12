using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class GTSPPointRepresentation : GTSPRepresentation
    {
        public List<Process> Processes { get; set; }
        public List<Alternative> Alternatives { get; set; }
        public List<Task> Tasks { get; set; }


        public GTSPPointRepresentation()
        {
            Processes = new List<Process>();
            Alternatives = new List<Alternative>();
            Tasks = new List<Task>();
            Positions = new List<Position>();
            PlusInfity = int.MaxValue;
            MinusInfity = int.MinValue;
            ConstraintsDisjoints = new List<ConstraintDisjoint>();
            ConstraintsOrder = new List<ConstraintOrder>();
            Graph = new GraphRepresentation() {
                WeightMultiplier = this.WeightMultiplier
            };
        }
        public void Build()
        {
            EqualizeTaskNumber();
            GenerateDisjunctSets();
            CreateEdges(this);
            Graph.Build();
            //WriteGTSP();
        }

        public void AddProcess(Process process)
        {
            Processes.Add(process);
//            Positions.Add(process.Start);
//            Positions.Add(process.Finish);
        }
        public void AddProcess(Process[] processes)
        {
            foreach (var item in processes)
            {
                Processes.Add(item);
            }
        }
        public void AddAlternative(Process process, Alternative alternative)
        {
            process.Alternatives.Add(alternative);
            Alternatives.Add(alternative);
            alternative.Process = process;
            //Positions.Add(alternative.Start);
            //Positions.Add(alternative.Finish);
        }
        public void AddAlternative(Process process, Alternative[] alternatives)
        {
            foreach (var item in alternatives)
            {
                AddAlternative(process, item);
            }
        }
        public void AddTask(Alternative alternative, Task task)
        {
            alternative.Tasks.Add(task);
            Tasks.Add(task);
            task.Alternative = alternative;
            task.Process = alternative.Process;
        }
        public void AddTask(Alternative alternative, Task[] tasks)
        {
            foreach (var item in tasks)
            {
                AddTask(alternative, item);
            }
        }
        public void AddPosition(Task task, Position position)
        {
            task.Positions.Add(position);
            Positions.Add(position);
            position.Task = task;
            position.Alternative = task.Alternative;
            position.Process = task.Process;
        }
        public void AddPosition(Task task, Position[] positions)
        {
            foreach (var item in positions)
            {
                AddPosition(task, item);
            }
        }

        public void GenerateDisjunctSets()
        {
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

                if (process.Alternatives.Count < 0)
                {
                    maxTaskNumber = process.Alternatives[0].Tasks.Count;
                }
                for (int i = 0; i < maxTaskNumber; i++)
                {
                    var constraint = new ConstraintDisjoint();
                    //constraint.addConstraint();
                    foreach (var alternative in process.Alternatives)
                    {
                        foreach (var position in alternative.Tasks[i].Positions)
                        {
                            constraint.Add(position);
                        }
                    }
                    ConstraintsDisjoints.Add(constraint);
                }
            }
        }
        
        private void EqualizeTaskNumber()
        {
            foreach (var process in Processes)
            {
                if (process.Alternatives.Count > 0)
                {
                    var alterTaskNum = process.Alternatives[0].Tasks.Count;
                    Alternative maxAlter = process.Alternatives[0];
                    foreach (var alternative in process.Alternatives)
                    {
                        if (alternative.Tasks.Count < alterTaskNum)
                            addPlaceholderPosition(alterTaskNum - alternative.Tasks.Count, alternative);
                        if (alternative.Tasks.Count > alterTaskNum)
                            addPlaceholderPosition(alternative.Tasks.Count - alterTaskNum, maxAlter);
                    }
                }
            }
        }
        private void addPlaceholderPosition(int numberOfPlaceholderTasks, Alternative alternative)
        {
            for (int i = 0; i < numberOfPlaceholderTasks; i++)
            {
                var task = new Task() { Name = "Placeholer_Alt_" + i };
                AddTask(alternative, task);
                AddPosition(task, new Position() { Name = "Placeholder_Pos_" + i, Virtual = true });
            }
        }


        public void CreateEdges(GTSPPointRepresentation gtsp)
        {
            Graph = new GraphRepresentation() {
                WeightMultiplier = this.WeightMultiplier
            };
            Graph.Edges = new List<Edge>();
            CreateEdgesProcess(gtsp);
            CreateEdgesTask(gtsp);
        }
        public void CreateEdgesProcess(GTSPPointRepresentation gtsp)
        {
            foreach (var proc in gtsp.Processes)
            {
                foreach (var proc2 in gtsp.Processes)
                {
                    if (proc.GID != proc2.GID)
                    {
                        foreach (var alternative in proc.Alternatives)
                        {
                            if (alternative.Tasks.Count > 0)
                            {
                                foreach (var alternative2 in proc2.Alternatives)
                                {
                                    if (alternative.GID != alternative2.GID && alternative2.Tasks.Count > 0)
                                    {
                                        ConnectTasks(alternative.Tasks[alternative.Tasks.Count - 1], alternative2.Tasks[0]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void CreateEdgesTask(GTSPPointRepresentation gtsp)
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
                    Graph.Edges.Add(createEdge(posA, posB));
                }
            }
        }
       private Edge createEdge(Position a, Position b)
        {
            if (PositionMatrix == null)
            {
                return new Edge()
                {
                    NodeA = a,
                    NodeB = b,
                    Weight = EdgeWeightCalculator.Calculate(a.Configuration, b.Configuration),
                    Directed = true
                };
            }
            else
            {
                return new Edge()
                {
                    NodeA = a,
                    NodeB = b,
                    Weight = findEdgeWeight(a,b),
                    Directed = true
                };
            }

        }

        private double findEdgeWeight(Position a, Position b)
        {
            int aIDindex = -1, bIDindex = -1;
            for (int i = 0; i < PositionMatrixOriginal.ID.Count; i++)
            {
                if (PositionMatrixOriginal.ID[i] == a.GID)
                {
                    aIDindex = i;
                }
                if (PositionMatrixOriginal.ID[i] == b.GID)
                {
                    bIDindex = i;
                }
            }
            if(aIDindex!=-1 && bIDindex != -1)
            {
                return PositionMatrix[aIDindex, bIDindex];
            }
            else
            {
                return 0;
            }

        }

        public Process FindProcess(int ID)
        {
            foreach (var item in Processes)
            {
                if (item.GID == ID)
                {
                    return item;
                }
            }
            return null;
        }
        public Alternative FindAlternative(int ID)
        {
            foreach (var item in Alternatives)
            {
                if (item.GID == ID)
                {
                    return item;
                }
            }
            return null;
        }
        public Task FindTask(int ID)
        {
            foreach (var item in Tasks)
            {
                if (item.GID == ID)
                {
                    return item;
                }
            }
            return null;
        }
        public Position FindPositionByPID(int PID)
        {
            foreach (var item in Positions)
            {
                if (item.ID == PID)
                {
                    return item;
                }
            }
            return null;
        }
        public Position FindPositionByID(int ID)
        {
            foreach (var item in Positions)
            {
                if (item.GID == ID)
                {
                    return item;
                }
            }
            return null;
        }

        public void WriteGTSP()
        {
            Console.WriteLine("Processes:");
            foreach (var item in Processes)
            {
                Console.WriteLine(item.ToString());
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
            //Console.WriteLine("Edges:");
            //foreach (var edge in Edges)
            //{
            //    Console.WriteLine(edge.ToString()); ;
            //}

            Console.WriteLine("ConstraintsDisjoints: ");
            foreach (var item in ConstraintsDisjoints)
            {
                Console.WriteLine(item);
            }
        }
    }
}
