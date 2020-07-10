using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options;
using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public static class SeqPointTemplateCompiler
    {
        public static SeqGTSPTask Compile(SeqTemplate template)
        {
            SeqGTSPTask sequencerTask = new SeqGTSPTask();
            PositionMatrix(sequencerTask, template);
            BuildGTSP(sequencerTask, template);
            AddOrderConstraints(sequencerTask, template);
            return sequencerTask;
        }

        private static void BuildGTSP(SeqGTSPTask sequencerTask, SeqTemplate template)
        {
            sequencerTask.GTSP.WeightMultiplier = template.WeightMultiplier;
            sequencerTask.GTSP.EdgeWeightCalculator = EdgeWeightFunctions.toFunction(template.DistanceFunction);
            sequencerTask.GTSP.PositionMatrix = template.PositionMatrix;
            ProcessHierarchy(sequencerTask, template);
        }

        private static void FindStartAndFinishDepot(SeqGTSPTask sequencerTask, SeqTemplate template)
        {
            //Call after GTSP filled
            if (template.StartDepotID != -1)
                sequencerTask.StartDepot = sequencerTask.GTSP.FindPositionByID(template.StartDepotID);

            if (template.FinishDepotID != -1)
                sequencerTask.FinishDepot = sequencerTask.GTSP.FindPositionByID(template.FinishDepotID);
        }

        private static void ProcessHierarchy(SeqGTSPTask sequencerTask, SeqTemplate template)
        {
            var gtst = sequencerTask.GTSP;
            List<Position> Positions = new List<Position>();
            foreach (var item in template.PositionList)
            {
                Positions.Add(new Position(item.ID, item.Name, item.Position));
            }

            foreach (var item in template.ProcessHierarchy)
            {
                Process proc = gtst.FindProcess(item.ProcessID);
                if (proc == null)
                {
                    proc = new Process(item.ProcessID);
                    sequencerTask.GTSP.AddProcess(proc);
                }

                Alternative alter = gtst.FindAlternative(item.AlternativeID);
                if (alter == null)
                {
                    alter = new Alternative(item.AlternativeID);
                    sequencerTask.GTSP.AddAlternative(proc, alter);
                }

                Task task = gtst.FindTask(item.TaskID);
                if (task == null)
                {
                    task = new Task(item.TaskID);
                    sequencerTask.GTSP.AddTask(alter, task);
                }

                Position position = gtst.FindPositionByID(item.PositionID);
                if (position == null)
                {
                    foreach (var pos in Positions)
                    {
                        if (pos.ID == item.PositionID)
                            sequencerTask.GTSP.AddPosition(task, pos);
                    }
                }
            }
        }

        private static void AddOrderConstraints(SeqGTSPTask sequencerTask, SeqTemplate template)
        {
            if (template.PositionPrecedence != null)
            {
                foreach (var item in template.PositionPrecedence)
                {
                    var before = sequencerTask.GTSP.FindPositionByID(item.BeforeID);
                    var after = sequencerTask.GTSP.FindPositionByID(item.AfterID);
                    if (before != null && after != null)
                        sequencerTask.GTSP.ConstraintsOrder.Add(new ConstraintOrder(before, after));
                    else
                        if (before == null)
                        Console.WriteLine("Compile error: PositionPrecedence BeforeID [" + item.BeforeID + "] not found!");
                    else
                        Console.WriteLine("Compile error: PositionPrecedence AfterID [" + item.AfterID + "] not found!");
                }
            }

            if (template.ProcessPrecedence != null)
            {
                foreach (var precedence in template.ProcessPrecedence)
                {
                    Process before =null;
                    Process after = null;
                    foreach (var process in sequencerTask.GTSP.Processes)
                    {
                        if (process.ID == precedence.BeforeID)
                            before = process;
                        if (process.ID == precedence.AfterID)
                            after = process;
                    }
                    if (before != null && after != null)
                        sequencerTask.GTSP.ConstraintsOrder.AddRange(CreateOrderConstraintsBetweenProc(sequencerTask, before, after));
                    else
                        if (before == null)
                        Console.WriteLine("Compile error: ProcessPrecedence BeforeID [" + precedence.BeforeID + "] not found!");
                    else
                        Console.WriteLine("Compile error: ProcessPrecedence AfterID [" + precedence.AfterID + "] not found!");
                    //item.AfterID();
                    //template.GTSP.ConstraintsOrder.Add(new ConstraintOrder(item.BeforeID, item.AfterID));
                }

            }
        }

        private static List<ConstraintOrder> CreateOrderConstraintsBetweenProc(SeqGTSPTask sequencerTask, Process before, Process after)
        {
            var tmp = new List<ConstraintOrder>();
            foreach (var posBefore in sequencerTask.GTSP.Positions)
            {
                foreach (var posAfter in sequencerTask.GTSP.Positions)
                {
                    if(posBefore.Process.ID == before.ID && posAfter.Process.ID == after.ID)
                    {
                        tmp.Add(new ConstraintOrder(posBefore, posAfter));
                    }
                }
            }
            return tmp;
        }

        private static void PositionMatrix(SeqGTSPTask sequencerTask, SeqTemplate template)
        {
            if (template.PositionList == null && template.PositionMatrix != null)
            {
                template.PositionList = new List<Options.Values.PositionOptionValue>();
                string name = "";
                for (int i = 0; i < template.PositionMatrix.ID.Count; i++)
                {
                    if (template.PositionMatrix.Name.Count > 0)
                    {
                        name = template.PositionMatrix.Name[i];
                    }
                    else
                    {
                        name = "Position_" + i;
                    }
                    template.PositionList.Add(new Options.Values.PositionOptionValue() { ID = template.PositionMatrix.ID[i], Name = name, Dim = 0, Position = new List<double>() });
                }
            }
        }
    }
}