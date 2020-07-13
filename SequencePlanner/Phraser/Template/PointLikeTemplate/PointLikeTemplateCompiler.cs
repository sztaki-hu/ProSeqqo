using SequencePlanner.GTSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public class PointLikeTemplateCompiler: TemplateCompiler
    {
        private PointLikeTask Task { get; set; }
        private PointLikeTemplate Template { get; set; }

        public PointLikeTask Compile(PointLikeTemplate template, CommonTask common)
        {
            Template = template;
            Task = new PointLikeTask(common);
            BuildGTSP();
            AddOrderConstraints();
            return Task;
        }

        private void BuildGTSP()
        {
            Task.GTSP.WeightMultiplier = Task.WeightMultiplier;
            Task.GTSP.EdgeWeightCalculator = Task.DistanceFunction;
            Task.GTSP.PositionMatrixOriginal = Task.PositionMatrix;
            if(Task.PositionMatrix!=null)
                Task.GTSP.PositionMatrix = Task.PositionMatrix.Matrix;
            ProcessHierarchy();
        }

        private void ProcessHierarchy()
        {
            var gtst = Task.GTSP;
            foreach (var item in Template.ProcessHierarchy)
            {
                Process proc = gtst.FindProcess(item.ProcessID);
                if (proc == null)
                {
                    proc = new Process(item.ProcessID);
                    Task.GTSP.AddProcess(proc);
                }

                Alternative alter = gtst.FindAlternative(item.AlternativeID);
                if (alter == null)
                {
                    alter = new Alternative(item.AlternativeID);
                    Task.GTSP.AddAlternative(proc, alter);
                }

                Task task = gtst.FindTask(item.TaskID);
                if (task == null)
                {
                    task = new Task(item.TaskID);
                    Task.GTSP.AddTask(alter, task);
                }

                Position position = gtst.FindPositionByID(item.PositionID);
                if (position == null)
                {
                    foreach (var pos in Task.PositionList)
                    {
                        if (pos.GID == item.PositionID)
                            Task.GTSP.AddPosition(task, pos);
                    }
                }
            }
        }

        private void AddOrderConstraints()
        {
            if (Template.PositionPrecedence != null)
            {
                foreach (var item in Template.PositionPrecedence)
                {
                    var before = Task.GTSP.FindPositionByID(item.BeforeID);
                    var after = Task.GTSP.FindPositionByID(item.AfterID);
                    if (before != null && after != null)
                        Task.GTSP.ConstraintsOrder.Add(new ConstraintOrder(before, after));
                    else
                        if (before == null)
                        Console.WriteLine("Compile error: PositionPrecedence BeforeID [" + item.BeforeID + "] not found!");
                    else
                        Console.WriteLine("Compile error: PositionPrecedence AfterID [" + item.AfterID + "] not found!");
                }
            }

            if (Template.ProcessPrecedence != null)
            {
                foreach (var precedence in Template.ProcessPrecedence)
                {
                    Process before = null;
                    Process after = null;
                    foreach (var process in Task.GTSP.Processes)
                    {
                        if (process.GID == precedence.BeforeID)
                            before = process;
                        if (process.GID == precedence.AfterID)
                            after = process;
                    }
                    if (before != null && after != null)
                        Task.GTSP.ConstraintsOrder.AddRange(CreateOrderConstraintsBetweenProc(Task, before, after));
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
        private List<ConstraintOrder> CreateOrderConstraintsBetweenProc(PointLikeTask Task, Process before, Process after)
        {
            var tmp = new List<ConstraintOrder>();
            foreach (var posBefore in Task.GTSP.Positions)
            {
                foreach (var posAfter in Task.GTSP.Positions)
                {
                    if (posBefore.Process.GID == before.GID && posAfter.Process.GID == after.GID)
                    {
                        tmp.Add(new ConstraintOrder(posBefore, posAfter));
                    }
                }
            }
            return tmp;
        }
    }
}