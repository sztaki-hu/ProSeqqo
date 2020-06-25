using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options;
using SequencePlanner.Phraser.Options.Values;
using SequencePlanner.Phraser.Template;
using SequencePlanner.Task;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner
{
    public class Test
    {
        public void ReadFileTest()
        {
            SeqOptionSet optionSet = new SeqOptionSet();
            Template template = new Template();
            optionSet.ReadFile("test/test10.txt");
            optionSet.Validate();
            template.OptionSet = optionSet;
            template.Validate();
            SequencerTask task = template.Compile();
            task.Build();
            task.Run();
            GraphViz.CreateGraphViz(task.GTSP, "test/FromFile.dot");
            task.GTSP.Graph.WriteGraph();
        }
        public void RepresentationTest()
        {
            Position.initMaxID();
            GTSPRepresentation gtsp = new GTSPRepresentation();
            GraphRepresentation graph = new GraphRepresentation();
            Process p = new Process();
            Alternative pa = new Alternative();
            Alternative pb = new Alternative();
            Alternative pc = new Alternative();

            Process p2 = new Process();
            Alternative p2a = new Alternative();
            Alternative p2b = new Alternative();
            Alternative p2c = new Alternative();

            Process p3 = new Process();
            Alternative p3a = new Alternative();
            Alternative p3b = new Alternative();
            Alternative p3c = new Alternative();

            Process p4 = new Process();
            Alternative p4a = new Alternative();
            Alternative p4b = new Alternative();
            Alternative p4c = new Alternative();

            List<GTSP.Task> t = new List<GTSP.Task>();
            for (int i = 0; i < 80; i++)
            {
                t.Add(new GTSP.Task());
            }
            List<Position> pos = new List<Position>();
            for (int i = 0; i < 80; i++)
            {
                pos.Add(new Position());
            }

            gtsp.AddProcess(p);
            gtsp.AddAlternative(p, new Alternative[] { pa, pb, pc });
            gtsp.AddTask(pa, new GTSP.Task[] { t[0], t[1] });
            gtsp.AddTask(pb, new GTSP.Task[] { t[2], t[3] });
            gtsp.AddTask(pc, new GTSP.Task[] { t[4], t[5] });
            gtsp.AddPosition(t[0], new Position[] { pos[0], pos[1] });
            gtsp.AddPosition(t[1], new Position[] { pos[2], pos[3] });
            gtsp.AddPosition(t[2], new Position[] { pos[4], pos[5] });
            gtsp.AddPosition(t[3], new Position[] { pos[6], pos[7] });
            gtsp.AddPosition(t[4], new Position[] { pos[8], pos[9] });
            gtsp.AddPosition(t[5], new Position[] { pos[10], pos[11], pos[12] });
                 
            gtsp.AddProcess(p2);
            gtsp.AddAlternative(p2, new Alternative[] { p2a, p2b, p2c });
            gtsp.AddTask(p2a, new GTSP.Task[] { t[6], t[7] , t[8], t[9] });
            gtsp.AddTask(p2b, new GTSP.Task[] { });
            gtsp.AddTask(p2c, new GTSP.Task[] { t[10], t[11] });
            gtsp.AddPosition(t[6], new Position[] { pos[13], pos[14] });
            gtsp.AddPosition(t[7], new Position[] { pos[15], pos[16], pos[17], pos[18] });
            gtsp.AddPosition(t[8], new Position[] { pos[19], pos[20] });
            gtsp.AddPosition(t[9], new Position[] { pos[21], pos[22] });
            gtsp.AddPosition(t[10], new Position[]{ pos[23], pos[24] });
            gtsp.AddPosition(t[11], new Position[]{ pos[25], pos[26], pos[27] });
                 
                 
            gtsp.AddProcess(p3);
            gtsp.AddAlternative(p3, new Alternative[] { p3a, p3b, p3c });
            gtsp.AddTask(p3a, new GTSP.Task[] { t[12], t[13] });
            gtsp.AddTask(p3b, new GTSP.Task[] { t[14], t[15], t[16] });
            gtsp.AddTask(p3c, new GTSP.Task[] { t[17] });
            gtsp.AddPosition(t[12], new Position[] { pos[28], pos[29] });
            gtsp.AddPosition(t[13], new Position[] { pos[30], pos[31] });
            gtsp.AddPosition(t[14], new Position[] { pos[32], pos[33] });
            gtsp.AddPosition(t[15], new Position[] { pos[34], pos[35], pos[36], pos[37] });
            gtsp.AddPosition(t[16], new Position[] { pos[38] });
            gtsp.AddPosition(t[17], new Position[] { pos[39], pos[40], pos[41] });
                 
            gtsp.AddProcess(p4);
            gtsp.AddAlternative(p4, new Alternative[] { p4a, p4b, p4c });
            gtsp.AddTask(p4a, new GTSP.Task[] { t[18], t[19] });
            gtsp.AddTask(p4b, new GTSP.Task[] { t[20], t[21], t[22] });
            gtsp.AddTask(p4c, new GTSP.Task[] { t[23] });
            gtsp.AddPosition(t[18], new Position[] { pos[42], pos[43] });
            gtsp.AddPosition(t[19], new Position[] { pos[44], pos[45], pos[46] });
            gtsp.AddPosition(t[20], new Position[] { pos[47] });
            gtsp.AddPosition(t[21], new Position[] { pos[48], pos[49], pos[50], pos[51] });
            gtsp.AddPosition(t[22], new Position[] { pos[52] });
            gtsp.AddPosition(t[23], new Position[] { pos[53], pos[54], pos[55] });
            
            gtsp.Build();
            GraphViz.CreateGraphViz(gtsp, "GTSP.dot");
            Console.WriteLine("\nProcess Num: " + gtsp.Processes.Count);
            Console.WriteLine("Alternative Num: " + gtsp.Alternatives.Count);
            Console.WriteLine("Task Num: " + gtsp.Tasks.Count);
            Console.WriteLine("Position/Node Num:" + gtsp.Positions.Count);
            int virtualNode = 0;
            foreach (var item in gtsp.Positions)
            {
                if (item.Virtual)
                    virtualNode++;
            }
        }
        public void SequencerTaskTest()
        {
            GraphRepresentation graph = new GraphRepresentation();
            SequencerTask sTask = new SequencerTask
            {
                TaskType = TaskTypeEnum.Point_Like,
                EdgeWeightSource = EdgeWeightSourceEnum.FullMatrix,
                Dimension = 3,
                TimeLimit = 300,
                CyclicSequence = true,
                StartDepotID = 99,
                FinishDepotID = 100,
                WeightMultiplierAuto = true,
                WeightMultiplier = 100
            };
            sTask.Build();
            sTask.Run();
        }
    }
}