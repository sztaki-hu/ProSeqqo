using SequencePlanner.Phraser.Options.Values;
using SequencePlanner.Phraser.Template;
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
        public void Build(List<Position> positions, List<ProcessHierarchyOptionValue> processHierarchies, List<PrecedenceOptionValue> procPrec, List<PrecedenceOptionValue> posPrec)
        {
            Positions = positions;
            CreateProcessHierarchy(processHierarchies);
            EqualizeTaskNumber();
            GenerateDisjunctSets();
            AddOrderConstraints(posPrec, procPrec);
            CreateEdges();
            Graph.Build();
            if(TemplateManager.DEBUG)
                WriteGTSP();
        }

        private void CreateProcessHierarchy(List<ProcessHierarchyOptionValue> processHierarchy)
        {
            foreach (var item in processHierarchy)
            {
                Process proc = FindProcess(item.ProcessID);
                Alternative alter = FindAlternative(item.AlternativeID);
                Task task = FindTask(item.TaskID);
                Position position = FindPositionByUID(item.PositionID);

                if (proc == null)
                {
                    proc = new Process(item.ProcessID);
                    AddProcess(proc);
                }
                if (alter == null)
                {
                    alter = new Alternative(item.AlternativeID);
                    AddAlternative(proc, alter);
                }
                if (task == null)
                {
                    task = new Task(item.TaskID);
                    AddTask(alter, task);
                }
                if (position == null)
                {
                    AddPosition(task, position);
                    Console.WriteLine("PointLike GTSP builder process hierarchy ID error, this error sholud be caught by validation!");
                }
                else
                {
                    task.Positions.Add(position);
                    position.Task = task;
                    position.Alternative = task.Alternative;
                    position.Process = task.Process;
                }
            }
        }
        private void AddOrderConstraints(List<PrecedenceOptionValue> positionPrecedence, List<PrecedenceOptionValue> procPrecedence)
        {
            if (positionPrecedence != null)
            {
                foreach (var item in positionPrecedence)
                {
                    var before = FindPositionByUID(item.BeforeID);
                    var after = FindPositionByUID(item.AfterID);
                    if (before != null && after != null)
                        ConstraintsOrder.Add(new ConstraintOrder(before, after));
                    else
                    {
                        if (before == null)
                            Console.WriteLine("Compile error: PositionPrecedence BeforeID [" + item.BeforeID + "] not found!");
                        if (after == null)
                            Console.WriteLine("Compile error: PositionPrecedence AfterID [" + item.AfterID + "] not found!");
                    }
                }
            }

            if (procPrecedence != null)
            {
                foreach (var precedence in procPrecedence)
                {
                    Process before = null;
                    Process after = null;
                    foreach (var process in Processes)
                    {
                        if (process.UID == precedence.BeforeID)
                            before = process;
                        if (process.UID == precedence.AfterID)
                            after = process;
                    }
                    if (before != null && after != null)
                        ConstraintsOrder.AddRange(CreateOrderConstraintsBetweenProc(before, after));
                    else
                        if (before == null)
                        Console.WriteLine("Compile error: ProcessPrecedence BeforeID [" + precedence.BeforeID + "] not found!");
                    else
                        Console.WriteLine("Compile error: ProcessPrecedence AfterID [" + precedence.AfterID + "] not found!");
                    //item.AfterID();
                    //Template.GTSP.ConstraintsOrder.Add(new ConstraintOrder(item.BeforeID, item.AfterID));
                }

            }
        }
        public new void GenerateDisjunctSets()
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
                    if (constraint.DisjointSet.Length > 1)
                        ConstraintsDisjoints.Add(constraint);
                }
            }
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

        public void CreateEdges()
        {
            Graph = new GraphRepresentation()
            {
                WeightMultiplier = this.WeightMultiplier
            };
            Graph.Edges = new List<Edge>();
            CreateEdgesProcess();
            CreateEdgesTask();
        }
        public void CreateEdgesProcess()
        {
            foreach (var proc in Processes)
            {
                foreach (var proc2 in Processes)
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
        public void CreateEdgesTask()
        {
            foreach (var alternative in Alternatives)
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
                    Graph.Edges.Add(new Edge()
                    {
                        NodeA = posA,
                        NodeB = posB,
                        Weight = EdgeWeightCalculator.Calculate(posA, posB),
                        Directed = true
                    });
                }
            }
        }

        public Process FindProcess(int ID)
        {
            foreach (var item in Processes)
            {
                if (item.UID == ID)
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
                if (item.UID == ID)
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
                if (item.UID == ID)
                {
                    return item;
                }
            }
            return null;
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
                            AddPlaceholderPosition(alterTaskNum - alternative.Tasks.Count, alternative);
                        if (alternative.Tasks.Count > alterTaskNum)
                            AddPlaceholderPosition(alternative.Tasks.Count - alterTaskNum, maxAlter);
                    }
                }
            }
        }
        private void AddPlaceholderPosition(int numberOfPlaceholderTasks, Alternative alternative)
        {
            for (int i = 0; i < numberOfPlaceholderTasks; i++)
            {
                var task = new Task() { Name = "Placeholer_Alt_" + i };
                AddTask(alternative, task);
                AddPosition(task, new Position() { Name = "Placeholder_Pos_" + i, Virtual = true });
            }
        }

        private List<ConstraintOrder> CreateOrderConstraintsBetweenProc(Process before, Process after)
        {
            var tmp = new List<ConstraintOrder>();
            foreach (var posBefore in Positions)
            {
                foreach (var posAfter in Positions)
                {
                    if (posBefore.Process.GID == before.GID && posAfter.Process.GID == after.GID)
                    {
                        tmp.Add(new ConstraintOrder(posBefore, posAfter));
                    }
                }
            }
            return tmp;
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

            Console.WriteLine("ConstraintsDisjoints: ");
            foreach (var item in ConstraintsDisjoints)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("ConstraintsOrders: ");
            foreach (var item in ConstraintsOrder)
            {
                Console.WriteLine(item);
            }

            Graph.WriteGraph();
            Graph.WriteMatrces();
        }
    }
}
