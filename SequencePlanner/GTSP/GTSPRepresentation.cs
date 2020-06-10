using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class GTSPRepresentation
    {

        private static double PlusInfity { get; set; }
        private static double MinusInfity { get; set; }

        public List<Process> Processes { get; set; }
        public List<Alternative> Alternatives { get; set; }
        public List<Task> Tasks { get; set; }
        public List<Position> Positions { get; set; }
        public GraphRepresentation Graph { get; set; }

        public List<ConstraintDisjoint> ConstraintsDisjoints { get; set; }
        public List<ConstraintOrder> ConstraintsOrder { get; set; }
        public EdgeWeightFunctions.EdgeWeightFunction EdgeWeightCalculator { get; set; }

        public GTSPRepresentation()
        {
            Processes = new List<Process>();
            Alternatives = new List<Alternative>();
            Tasks = new List<Task>();
            Positions = new List<Position>();
            PlusInfity = int.MaxValue;
            MinusInfity = int.MinValue;
            ConstraintsDisjoints = new List<ConstraintDisjoint>();
            ConstraintsOrder = new List<ConstraintOrder>();
            EdgeWeightCalculator = EdgeWeightFunctions.Euclidian_Distance;
        }
        internal void Build()
        {
            Graph.createEdges(this);
            Graph.Build(this);
            //createEdges();
        }

        public void AddProcess(Process process)
        {
            Processes.Add(process);
            Positions.Add(process.Start);
            Positions.Add(process.Finish);
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
            Positions.Add(alternative.Start);
            Positions.Add(alternative.Finish);
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


        public void ApplyConstraints() { }
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

            }
        }

        public Process FindProcess(int ID)
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
        public Alternative FindAlternative(int ID)
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
        public Task FindTask(int ID)
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
        public Position FindPosition(int ID)
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
        }

    }
}
