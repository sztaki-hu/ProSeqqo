using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options;
using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public static class SeqTemplateCompiler
    {
        private static List<Position> Positions;

        public static SeqGTSPTask Compile(SeqTemplate template)
        {
            Positions = new List<Position>();
            SeqGTSPTask sequencerTask = new SeqGTSPTask();
            PositionList(template);
            PositionMatrix(template);
            sequencerTask.GTSP = template.GTSP;
            ProcessHierarchy(sequencerTask, template);
            AddOrderConstraints(sequencerTask, template);
            return sequencerTask;
        }

        private static void FindStartAndFinishDepot(SeqGTSPTask sequencerTask, SeqTemplate template)
        {
            //Call after GTSP filled
            if (template.StartDepotID != -1)
                sequencerTask.StartDepot = template.GTSP.FindPositionByID(template.StartDepotID);

            if (template.FinishDepotID != -1)
                sequencerTask.FinishDepot = template.GTSP.FindPositionByID(template.FinishDepotID);
        }

        private static void ProcessHierarchy(SeqGTSPTask sequencerTask, SeqTemplate template)
        {
            var gtst = template.GTSP;
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
                        template.GTSP.ConstraintsOrder.Add(new ConstraintOrder(before, after));
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
                        template.GTSP.ConstraintsOrder.AddRange(CreateOrderConstraintsBetweenProc(sequencerTask, before, after));
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

        private static void PositionList(SeqTemplate template)
        {
            foreach (var item in template.PositionList)
            {
                Positions.Add(new Position( item.ID, item.Name, item.Position));
            }
        }

        private static void PositionMatrix(SeqTemplate template)
        {
            //string name = "";
            //for (int i = 0; i < template.PositionMatrix.ID.Count; i++)
            //{
            //    if (template.PositionMatrix.Name.Count > 0)
            //    {
            //        name = template.PositionMatrix.Name[i];
            //    }
            //    else
            //    { 
            //         name = "Position_" +i;
            //    }
            //    Positions.Add(new Position(template.PositionMatrix.ID[i],name, new List<double>()));
            //}
        }

        private static bool InList(List<int> list, int ID)
        {
            foreach (var item in list)
            {
                if (item == ID)
                    return true;
            }
            return false;
        }
    }
}