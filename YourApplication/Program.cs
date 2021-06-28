using SequencePlanner.GTSPTask.Serialization.Task;
using SequencePlanner.Helper;
using SequencePlanner.Model.Hierarchy;
using SequencePlanner.Task;
using System;
using System.Collections.Generic;

namespace YourApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //UseFileInterface();
            UseProgramInterface();
        }

        static void UseFileInterface()
        {
            var ser = new GeneralTaskSerializer();
            var task = ser.ImportSEQ("HelloWorld.seq");
            var result = task.Run();
            
            task = ser.ImportJSON("HelloWorld.json");
            task.Run();

            task = ser.ImportXML("HelloWorld.xml");
            task.Run();
        }

        static void UseProgramInterface()
        {
            GeneralTask t = new GeneralTask();
            SeqLogger.LogLevel = LogLevel.Info;
            t.Cyclic = false;
            t.Validate = true;
            t.StartDepotConfig  = new Config() { ID = 100, Name = "Start",  Configuration = new List<double>() {0, 0} }; 
            t.FinishDepotConfig = new Config() { ID = 200, Name = "Finish", Configuration = new List<double>() {10, 10} };
            t.SolverSettings.Metaheuristics = SequencePlanner.Helper.Metaheuristics.GuidedLocalSearch;
            t.SolverSettings.TimeLimit = 1000;
            t.SolverSettings.UseMIPprecedenceSolver = true;
            t.SolverSettings.UseShortcutInAlternatives = true;
            t.Validate = true;
            t.Hierarchy.HierarchyRecords.AddRange(CreateHierarchy(t));
            var result =  t.Run();
            result.ToLog(SequencePlanner.Helper.LogLevel.Info);
        }

        private static List<HierarchyRecord> CreateHierarchy(GeneralTask t)
        {
            var tmp = new List<HierarchyRecord>();

            var PA = new Process() { ID = 1, Name = "Process A" };
            var PB = new Process() { ID = 2, Name = "Process B" };
            var PC = new Process() { ID = 3, Name = "Process B" };

            var AA  = new Alternative() { ID = 1, Name = "Alternative A" };
            var AB  = new Alternative() { ID = 2, Name = "Alternative B" };
            var AC  = new Alternative() { ID = 3, Name = "Alternative B" };

            var TA  = new Task() { ID = 1, Name = "Task A" };
            var TB  = new Task() { ID = 2, Name = "Task B" };
            var TC  = new Task() { ID = 3, Name = "Task B" };

            var CA = new Config() { ID = 1, Configuration = new List<double>() { 5, 5 },     Name = "Config A" };
            var CB = new Config() { ID = 2, Configuration = new List<double>() { 2, 2 },     Name = "Config B" };
            var CC = new Config() { ID = 3, Configuration = new List<double>() { 7.5, 7.5 }, Name = "Config C" };

            var MA = new Motion() { ID = 1, Name = "Motion A", Configs = new List<Config>() { CA } };
            var MB = new Motion() { ID = 2, Name = "Motion B", Configs = new List<Config>() { CB } };
            var MC = new Motion() { ID = 3, Name = "Motion C", Configs = new List<Config>() { CC } };

            tmp.Add(new HierarchyRecord()
            {
                Process = PA,
                Alternative = AA,
                Task = TA,
                Motion = MA
            });

            tmp.Add(new HierarchyRecord()
            {
                Process = PB,
                Alternative = AB,
                Task = TB,
                Motion = MB,
            });


            tmp.Add(new HierarchyRecord()
            {
                Process = PC,
                Alternative = AC,
                Task = TC,
                Motion = MC,
            });

            return tmp;
        }
    }
}
