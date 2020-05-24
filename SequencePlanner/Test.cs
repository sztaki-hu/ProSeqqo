using SequencePlanner.GTSP;
using SequencePlanner.Phraser.Options.Values;
using SequencePlanner.Phraser.Template;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner
{
    public class Test
    {
        public void ReadFileTest()
        {
            Template template = new Template();
            template.Read("test.txt");
            SequencerTask task = template.Compile();
            task.Build();
            task.Run();
            task.Graph.CreateGraphViz(@"C:\Users\Zahorán László\Desktop\FromFile.dot");
            task.Graph.createEdgesVirtual();
            task.Graph.CreateGraphViz(@"C:\Users\Zahorán László\Desktop\FromFileVirtual.dot");
            task.Graph.WriteGraph();
            
            //Console.WriteLine(template.OptionSet.ToString());
            
        }
        public void RepresentationTest()
        {
            Position.initMaxID();
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

            List<Task> t = new List<Task>();
            for (int i = 0; i < 80; i++)
            {
                t.Add(new Task());
            }
            List<Position> pos = new List<Position>();
            for (int i = 0; i < 80; i++)
            {
                pos.Add(new Position());
            }

            graph.addProcess(p);
            graph.addAlternative(p, new Alternative[] { pa, pb, pc });
            graph.addTask(pa, new Task[] { t[0], t[1] });
            graph.addTask(pb, new Task[] { t[2], t[3] });
            graph.addTask(pc, new Task[] { t[4], t[5] });
            graph.addPosition(t[0], new Position[] { pos[0], pos[1] });
            graph.addPosition(t[1], new Position[] { pos[2], pos[3] });
            graph.addPosition(t[2], new Position[] { pos[4], pos[5] });
            graph.addPosition(t[3], new Position[] { pos[6], pos[7] });
            graph.addPosition(t[4], new Position[] { pos[8], pos[9] });
            graph.addPosition(t[5], new Position[] { pos[10], pos[11], pos[12] });

            graph.addProcess(p2);
            graph.addAlternative(p2, new Alternative[] { p2a, p2b, p2c });
            graph.addTask(p2a, new Task[] { t[6], t[7] , t[8], t[9] });
            graph.addTask(p2b, new Task[] { });
            graph.addTask(p2c, new Task[] { t[10], t[11] });
            graph.addPosition(t[6], new Position[] { pos[13], pos[14] });
            graph.addPosition(t[7], new Position[] { pos[15], pos[16], pos[17], pos[18] });
            graph.addPosition(t[8], new Position[] { pos[19], pos[20] });
            graph.addPosition(t[9], new Position[] { pos[21], pos[22] });
            graph.addPosition(t[10], new Position[]{ pos[23], pos[24] });
            graph.addPosition(t[11], new Position[]{ pos[25], pos[26], pos[27] });


            graph.addProcess(p3);
            graph.addAlternative(p3, new Alternative[] { p3a, p3b, p3c });
            graph.addTask(p3a, new Task[] { t[12], t[13] });
            graph.addTask(p3b, new Task[] { t[14], t[15], t[16] });
            graph.addTask(p3c, new Task[] { t[17] });
            graph.addPosition(t[12], new Position[] { pos[28], pos[29] });
            graph.addPosition(t[13], new Position[] { pos[30], pos[31] });
            graph.addPosition(t[14], new Position[] { pos[32], pos[33] });
            graph.addPosition(t[15], new Position[] { pos[34], pos[35], pos[36], pos[37] });
            graph.addPosition(t[16], new Position[] { pos[38] });
            graph.addPosition(t[17], new Position[] { pos[39], pos[40], pos[41] });

            graph.addProcess(p4);
            graph.addAlternative(p4, new Alternative[] { p4a, p4b, p4c });
            graph.addTask(p4a, new Task[] { t[18], t[19] });
            graph.addTask(p4b, new Task[] { t[20], t[21], t[22] });
            graph.addTask(p4c, new Task[] { t[23] });
            graph.addPosition(t[18], new Position[] { pos[42], pos[43] });
            graph.addPosition(t[19], new Position[] { pos[44], pos[45], pos[46] });
            graph.addPosition(t[20], new Position[] { pos[47] });
            graph.addPosition(t[21], new Position[] { pos[48], pos[49], pos[50], pos[51] });
            graph.addPosition(t[22], new Position[] { pos[52] });
            graph.addPosition(t[23], new Position[] { pos[53], pos[54], pos[55] });

            graph.createEdgesVirtual();
            graph.CreateGraphViz(@"C:\Users\Zahorán László\Desktop\GTSP.dot");
            Console.WriteLine("\nProcess Num: " + graph.Processes.Count);
            Console.WriteLine("Alternative Num: " + graph.Alternatives.Count);
            Console.WriteLine("Task Num: " + graph.Tasks.Count);
            Console.WriteLine("Position/Node Num:" + graph.Positions.Count);
            int virtualNode = 0;
            foreach (var item in graph.Positions)
            {
                if (item.Virtual)
                    virtualNode++;
            }
            Console.WriteLine("Virtual Position/Node: " + virtualNode);
            Console.WriteLine("Edge Num: " + graph.Edges.Count);


            graph.createEdges();
            graph.CreateGraphViz(@"C:\Users\Zahorán László\Desktop\GTSPsimple.dot", false);
            Console.WriteLine("\nPosition/Node Num: "+ (graph.Positions.Count-virtualNode+(graph.Processes.Count*2)));
            Console.WriteLine("Edge Num only process virtual: " + graph.Edges.Count);

            //graph.WriteGraph();
            //System.Diagnostics.Process.Start(@"C:\Users\Zahorán László\Desktop\GTSP.dot");
            //graph.deleteVirtualNodes();

            graph.createEdgesVirtualFull();
            Console.WriteLine("\nEdge Full Virtual: " + graph.Edges.Count);
            //graph.CreateGraphViz(@"C:\Users\Zahorán László\Desktop\GTSPfull.dot");
            //graph.WriteGraph();
        }
        public void SequencerTaskTest()
        {
            GraphRepresentation graph = new GraphRepresentation();
            SequencerTask sTask = new SequencerTask();
            sTask.Graph = graph;
            sTask.TaskType = TaskTypeEnum.Point_Like;
            sTask.TaskType = TaskTypeEnum.Line_Like;
            sTask.EdgeWeightSource = EdgeWeightSourceEnum.FullMatrix;
            sTask.EdgeWeightSource = EdgeWeightSourceEnum.CalculateFromPositions;
            sTask.DistanceFunction = DistanceFunctionEnum.Euclidian_Distance;
            sTask.DistanceFunction = DistanceFunctionEnum.Max_Distance;
            sTask.DistanceFunction = DistanceFunctionEnum.Trapezoid_Time;
            sTask.DistanceFunction = DistanceFunctionEnum.Manhattan_Distance;
            sTask.Dimension = 3;
            sTask.TimeLimit = 300;
            sTask.CyclicSequence = true;
            sTask.StartDepotID = 99;
            sTask.FinishDepotID = 100;
            sTask.WeightMultiplierAuto = true;
            sTask.WeightMultiplier = 100;
            sTask.Build();
            sTask.Run();
        }
    }
}

//Bemeneti fájl feldolgozása templatebe
//Template ellenőrzése
//SequencerTask, GraphRepresentation feltöltése
//Futtatása Googl-OR-Tools-al