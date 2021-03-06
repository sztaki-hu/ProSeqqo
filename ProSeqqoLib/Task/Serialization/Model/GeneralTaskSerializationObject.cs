using ProSeqqoLib.Helper;
using ProSeqqoLib.Model;
using ProSeqqoLib.Model.Hierarchy;
using ProSeqqoLib.Task;
using ProSeqqoLib.Task.Serialization.Model;
using ProSeqqoLib.Task.Serialization.Token;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProSeqqoLib.GTSPTask.Serialization.Task
{
    public class GeneralTaskSerializationObject
    {
        public string TaskType { get; set; }
        public bool Validate { get; set; }
        public bool Cyclic { get; set; }
        public int StartDepot { get; set; }
        public int FinishDepot { get; set; }

        //Solver
        public string LocalSearchStrategy { get; set; }
        public int TimeLimit { get; set; }
        public bool UseMIPprecedenceSolver { get; set; }
        public bool UseShortcutInAlternatives { get; set; }

        //Cost
        public DistanceFunctionSerializationObject DistanceFunction { get; set; }
        public double IdlePenalty { get; set; }
        public bool BidirectionMotionDefault { get; set; }
        public bool AddMotionLengthToCost { get; set; }
        public bool AddInMotionChangeoverToCost { get; set; }

        //Resource
        public ResourceFunctionSerializationObject ResourceFunction { get; set; }

        //Hierarchy
        public List<ConfigSerializationObject> ConfigList { get; set; }
        public List<HybridLineSerializationObject> MotionList { get; set; }
        public List<ProcessHierarchySerializationObject> ProcessHierarchy { get; set; }
        public StrictEdgeWeightSetSerializationObject OverrideCost { get; set; }
        public List<OrderConstraintSerializationObject> ProcessPrecedences { get; set; }
        public List<OrderConstraintSerializationObject> MotionPrecedence { get; set; }


        public GeneralTaskSerializationObject() { }

        public GeneralTaskSerializationObject(List<string> seqString) : base()
        {
            var tokenizer = new SEQTokenizer();
            tokenizer.Tokenize(seqString);
            FillBySEQTokens(tokenizer);
        }

        public GeneralTaskSerializationObject(GeneralTask task)
        {
            Cyclic = task.Cyclic;
            BidirectionMotionDefault = task.Hierarchy.BidirectionalMotionDefault;
            if (task.StartDepotConfig != null)
                StartDepot = task.StartDepotConfig.ID;
            else
                StartDepot = -1;
            if (task.FinishDepotConfig != null)
                FinishDepot = task.FinishDepotConfig.ID;
            else
                FinishDepot = -1;
            TimeLimit = task.SolverSettings.TimeLimit;
            ConfigList = new List<ConfigSerializationObject>();
            foreach (var item in task.Hierarchy.Configs)
            {
                ConfigList.Add(new ConfigSerializationObject(item)); ///TODO: Rapid FIX change it!
            }
            DistanceFunction = new DistanceFunctionSerializationObject(task);
            ResourceFunction = new ResourceFunctionSerializationObject(task.CostManager.ResourceFunction);
            OverrideCost = new StrictEdgeWeightSetSerializationObject(task.CostManager.OverrideCost);
            AddInMotionChangeoverToCost = task.CostManager.AddInMotionChangeoverToCost;
            AddMotionLengthToCost = task.CostManager.AddMotionLengthToCost;
            LocalSearchStrategy = task.SolverSettings.Metaheuristics.ToString();
            Validate = task.Validate;
            IdlePenalty = task.CostManager.IdlePenalty;
            TaskType = "General";
            UseMIPprecedenceSolver = task.SolverSettings.UseMIPprecedenceSolver;
            UseShortcutInAlternatives = task.SolverSettings.UseShortcutInAlternatives;
            ProcessHierarchy = new List<ProcessHierarchySerializationObject>();
            MotionPrecedence = new List<OrderConstraintSerializationObject>();
            ProcessPrecedences = new List<OrderConstraintSerializationObject>();
            MotionList = new List<HybridLineSerializationObject>();
            foreach (var record in task.Hierarchy.HierarchyRecords)
            {
                ProcessHierarchy.Add(new ProcessHierarchySerializationObject()
                {
                    ProcessID = record.Process.ID,
                    AlternativeID = record.Alternative.ID,
                    TaskID = record.Task.ID,
                    MotionID = record.Motion.ID,
                    ConfigIDs = record.Motion.Configs.Select(c => c.ID).ToList()
                });
            }

            foreach (var motPrec in task.MotionPrecedences)
            {
                MotionPrecedence.Add(new OrderConstraintSerializationObject()
                {
                    BeforeID = motPrec.Before.ID,
                    AfterID = motPrec.After.ID
                });
            }

            foreach (var procPrec in task.ProcessPrecedences)
            {
                ProcessPrecedences.Add(new OrderConstraintSerializationObject()
                {
                    BeforeID = procPrec.Before.ID,
                    AfterID = procPrec.After.ID
                });
            }
        }


        public GeneralTask ToGeneralTask()
        {
            var task = new GeneralTask();
            //TaskType
            task.Validate = Validate;
            task.Cyclic = Cyclic;
            task.Hierarchy.BidirectionalMotionDefault = BidirectionMotionDefault;
            task.CostManager.AddInMotionChangeoverToCost = AddInMotionChangeoverToCost;
            task.CostManager.AddMotionLengthToCost = AddMotionLengthToCost;
            task.CostManager.IdlePenalty = IdlePenalty;
            task.CostManager.DistanceFunction = DistanceFunction.ToDistanceFunction();
            task.CostManager.ResourceFunction = ResourceFunction.ToResourceFunction();

            task.SolverSettings.UseMIPprecedenceSolver = UseMIPprecedenceSolver;
            task.SolverSettings.Metaheuristics = LocalSearchStrategyEnum.ResolveEnum(LocalSearchStrategy);
            task.SolverSettings.UseShortcutInAlternatives = UseShortcutInAlternatives;
            task.SolverSettings.TimeLimit = TimeLimit;

            CreateProcessHierarchy(task);
            CreateOverrideCosts(task);
            CreatePrecedences(task);
            foreach (var config in task.Hierarchy.Configs)
            {
                if (config.ID == StartDepot)
                    task.StartDepotConfig = config;
                if (config.ID == FinishDepot)
                    task.FinishDepotConfig = config;
            }
            //AddLinesToConfigList(task);
            if (StartDepot != -1 && task.StartDepotConfig == null)
                SeqLogger.Error("StartDepot not exist configuration!", nameof(GeneralTaskSerializationObject));
            if (FinishDepot != -1 && task.FinishDepotConfig == null)
                SeqLogger.Error("FinishDepot not exist as configuration!", nameof(GeneralTaskSerializationObject));
            //task.PositionMatrix.StrictUserEdgeWeights = OverrideCost.ToStrictEdgeWeightSet(task.PositionMatrix.Positions);
            return task;
        }

        private void CreateOverrideCosts(GeneralTask task)
        {
            foreach (var item in OverrideCost.Weights)
            {
                Config A = null;
                Config B = null;
                foreach (var c in task.Hierarchy.Configs)
                {
                    if (c.ID == item.A)
                        A = c;
                    if (c.ID == item.B)
                        B = c;
                }

                if (A is null || B is null)
                    throw new SeqException("Config not found for override cost: " + item.A + " " + item.B);

                task.CostManager.OverrideCost.Add(new DetailedConfigCost()
                {
                    A = A,
                    B = B,
                    OverrideCost = item.Weight,
                    Bidirectional = item.Bidirectional
                });
            }
        }

        private void AddLinesToConfigList(GeneralTask task)
        {
            foreach (var motion in MotionList)
            {
                var m = new Motion()
                {
                    ID = motion.LineID,
                    Name = motion.Name,
                    Configs = new List<Config>() { task.Hierarchy.GetConfigByID(motion.ConfigIDA), task.Hierarchy.GetConfigByID(motion.ConfigIDB) },
                    Bidirectional = motion.Bidirectional
                };
            }
        }

        private void CreatePrecedences(GeneralTask task)
        {
            foreach (var posPrec in MotionPrecedence)
            {
                var before = task.Hierarchy.GetMotionByID(posPrec.BeforeID);
                var after = task.Hierarchy.GetMotionByID(posPrec.AfterID);
                if (before == null || after == null)
                    throw new SeqException("Phrase error line precedence user id not found!");
                task.MotionPrecedences.Add(new MotionPrecedence(before, after));
            }

            foreach (var processPrec in ProcessPrecedences)
            {
                var before = task.Hierarchy.GetProcessByID(processPrec.BeforeID);
                var after = task.Hierarchy.GetProcessByID(processPrec.AfterID);
                if (before == null || after == null)
                    throw new SeqException("Phrase error process precedence user id not found!");
                task.ProcessPrecedences.Add(new ProcessPrecedence(before, after));
            }
        }
        private void CreateProcessHierarchy(GeneralTask task)
        {
            foreach (var pos in ConfigList)
            {
                task.Hierarchy.Configs.Add(pos.ToConfig());
            }

            foreach (var item in ProcessHierarchy)
            {
                Process proc = task.Hierarchy.GetProcessByID(item.ProcessID);
                if (proc == null)
                {
                    proc = new Process()
                    {
                        ID = item.ProcessID
                    };
                }

                Alternative alter = task.Hierarchy.GetAlternativeInProcess(item.AlternativeID, proc);
                //Alternative alter = task.Hierarchy.GetAlternativeByID(item.AlternativeID);
                if (alter == null)
                {
                    alter = new Alternative()
                    {
                        ID = item.AlternativeID
                    };
                }


                Model.Hierarchy.Task t = task.Hierarchy.GetTaskInProcessAlternative(item.TaskID,proc,alter);
                //Model.Hierarchy.Task t = task.Hierarchy.GetTaskByID(item.TaskID);
                if (t == null)
                {
                    t = new Model.Hierarchy.Task
                    {
                        ID = item.TaskID
                    };
                }

                var motion = task.Hierarchy.GetMotionByID(item.MotionID);
                if (motion == null)
                {
                    motion = new Motion()
                    {
                        ID = item.MotionID,
                        Configs = FindConfigsForMotion(task, item.ConfigIDs),
                        Name = item.Name
                    };
                    if (item.Bidirectional is null)
                    {
                        motion.Bidirectional = BidirectionMotionDefault;
                    }
                    else
                    {
                        motion.Bidirectional = (bool)item.Bidirectional;
                    }
                }

                var record = new HierarchyRecord()
                {
                    Process = proc,
                    Alternative = alter,
                    Task = t,
                    Motion = motion,
                };
                task.Hierarchy.HierarchyRecords.Add(record);
                task.Hierarchy.Motions.Add(motion);
            }
        }

        private List<Config> FindConfigsForMotion(GeneralTask task, List<int> configIDs)
        {
            var configs = new List<Config>();
            foreach (var item in configIDs)
            {
                configs.Add(task.Hierarchy.GetConfigByID(item));
            }
            return configs;
        }

        public new void FillBySEQTokens(SEQTokenizer tokenizer)
        {
            TaskType = tokenizer.GetStringByHeader("Task");
            Validate = tokenizer.GetBoolByHeader("Validate");
            Cyclic = tokenizer.GetBoolByHeader("Cyclic");
            StartDepot = tokenizer.GetIntByHeader("StartDepot");
            FinishDepot = tokenizer.GetIntByHeader("FinishDepot");
            TimeLimit = tokenizer.GetIntByHeader("TimeLimit");
            ConfigList = tokenizer.GetConfigListByHeader("ConfigList");
            UseMIPprecedenceSolver = tokenizer.GetBoolByHeader("UseMIPprecedenceSolver");
            BidirectionMotionDefault = tokenizer.GetBoolByHeader("BidirectionMotionDefault");
            AddInMotionChangeoverToCost = tokenizer.GetBoolByHeader("AddInMotionChangeoverToCost");
            AddMotionLengthToCost = tokenizer.GetBoolByHeader("AddMotionLengthToCost");
            IdlePenalty = tokenizer.GetDoubleByHeader("IdlePenalty");
            BidirectionMotionDefault = tokenizer.GetBoolByHeader("BidirectionMotionDefault");
            DistanceFunction = new DistanceFunctionSerializationObject();
            DistanceFunction.FillBySEQTokens(tokenizer);
            if ((ConfigList == null || ConfigList.Count == 0) & DistanceFunction.Function == "Matrix")
                ConfigList = DistanceFunction.DistanceMatrix.ConfigList;
            ResourceFunction = new ResourceFunctionSerializationObject();
            ResourceFunction.FillBySEQTokens(tokenizer);
            LocalSearchStrategy = tokenizer.GetStringByHeader("LocalSearchStrategy");
            OverrideCost = tokenizer.GetStrictEdgeWeightSet("OverrideCost");
            UseShortcutInAlternatives = tokenizer.GetBoolByHeader("UseShortcutInAlternatives");
            ProcessHierarchy = tokenizer.GetProcessHierarchyByHeader("ProcessHierarchy");
            MotionPrecedence = tokenizer.GetPrecedenceListByHeader("MotionPrecedence");
            ProcessPrecedences = tokenizer.GetPrecedenceListByHeader("ProcessPrecedence");
            MotionList = tokenizer.GetHybridMotionListByHeader("MotionList");
            tokenizer.CheckNotPhrased();
        }
        public string ToSEQ()
        {
            string seq = "";
            string newline = "\n";
            //string separator = ";";
            seq += "#Export @ " + DateTime.UtcNow + newline;
            seq += "Task: " + TaskType + newline;
            seq += "Validate: " + Validate + newline;
            seq += "Cyclic: " + Cyclic + newline;
            if (StartDepot != -1)
                seq += "StartDepot: " + StartDepot + newline;
            if (FinishDepot != -1)
                seq += "FinishDepot: " + FinishDepot + newline;
            seq += "TimeLimit: " + TimeLimit + newline;
            seq += "LocalSearchStrategy: " + LocalSearchStrategy + newline;
            seq += "IdlePenalty: " + IdlePenalty + newline;
            seq += "BidirectionMotionDefault: " + BidirectionMotionDefault + newline;
            seq += "AddInMotionChangeoverToCost: " + AddInMotionChangeoverToCost + newline;
            seq += "AddMotionLengthToCost: " + AddMotionLengthToCost + newline;
            seq += DistanceFunction.ToSEQShort();
            seq += ResourceFunction.ToSEQ();
            seq += "UseMIPprecedenceSolver: " + UseMIPprecedenceSolver + newline;
            seq += "UseShortcutInAlternatives: " + UseShortcutInAlternatives + newline;
            seq += DistanceFunction.ToSEQLong();
            if (DistanceFunction.Function != "Matrix")
            {
                seq += "ConfigList:" + newline;
                foreach (var item in ConfigList)
                {
                    seq += item.ToSEQ();
                }
            }
            if (OverrideCost != null && OverrideCost.Weights.Count > 0)
            {
                seq += "StrictEdgeWeights: " + newline;
                seq += OverrideCost.ToSEQ();
            }
            seq += "MotionList:" + newline;
            foreach (var line in MotionList)
            {
                seq += line.ToSEQ();
            }
            seq += "ProcessHierarchy:" + newline;
            foreach (var line in ProcessHierarchy)
            {
                seq += line.ToSEQ();
            }
            seq += "MotionPrecedence:" + newline;
            foreach (var prec in MotionPrecedence)
            {
                seq += prec.ToSEQ();
            }

            seq += "ProcessPrecedence:" + newline;
            foreach (var prec in ProcessPrecedences)
            {
                seq += prec.ToSEQ();
            }
            return seq;
        }
    }
}