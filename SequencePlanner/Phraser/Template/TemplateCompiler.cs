using SequencePlanner.GTSP;
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
            positionList(sequencerTask, template);
            ProcessHierarchy(sequencerTask, template);
            sequencerTask.Graph = template.Graph;
            return sequencerTask;
        }

        public static void ProcessHierarchy(SequencerTask sequencerTask, Template template)
        {
            var graph = template.Graph;
            foreach (var item in template.ProcessHierarchy)
            {
                Process proc = graph.findProcess(item.ProcessID);
                if (proc == null)
                {
                    proc = new Process(item.ProcessID);
                    graph.addProcess(proc);
                }

                Alternative alter = graph.findAlternative(item.AlternativeID);
                if (alter == null)
                {
                    alter = new Alternative(item.AlternativeID);
                    graph.addAlternative(proc, alter);
                }

                Task task = graph.findTask(item.TaskID);
                if (task == null)
                {
                    task = new Task(item.TaskID);
                    graph.addTask(alter, task);
                }

                Position position = graph.findPosition(item.PositionID);
                if (position == null)
                {
                    foreach (var pos in Positions)
                    {
                        if (pos.ID == item.PositionID)
                            graph.addPosition(task, pos);
                    }
                }
            }

            graph.createEdges();
        }

        public static void positionList(SequencerTask sequencerTask, Template template)
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