﻿using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options;
using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Template
{
    public static class TemplateCompiler
    {
        private static List<Position> Positions;

        public static SequencerTask Compile(Template template)
        {
            Positions = new List<Position>();
            SequencerTask sequencerTask = new SequencerTask();
            PositionList(template);
            ProcessHierarchy(sequencerTask, template);
            sequencerTask.GTSP = template.GTSP;
            return sequencerTask;
        }

        public static void ProcessHierarchy(SequencerTask sequencerTask, Template template)
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

                Position position = gtst.FindPosition(item.PositionID);
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

        public static void PositionList(Template template)
        {
            foreach (var item in template.PositionList)
            {
                Positions.Add(new Position( item.ID, item.Name, item.Position));
            }
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