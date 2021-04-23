using System;
using System.Collections.Generic;
using SequencePlanner.Helper;
using SequencePlanner.GTSPTask.Serialization.SerializationObject;
using SequencePlanner.GTSPTask.Serialization.SerializationObject.Token;
using SequencePlanner.OR_Tools;
using SequencePlanner.GeneralModels;

namespace SequencePlanner.GTSPTask.Serialization.Task
{
    public class NewGeneralTaskSerializationObject
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


        public NewGeneralTaskSerializationObject(){ }

        public NewGeneralTaskSerializationObject(List<string> seqString) : base()
        {
            var tokenizer = new SEQTokenizer();
            tokenizer.Tokenize(seqString);
            FillBySEQTokens(tokenizer);
        }

        public NewGeneralTaskSerializationObject(NewGeneralTask task)
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
            AddInMotionChangeoverToCost = false;
            AddMotionLengthToCost = false;
            LocalSearchStrategy = task.SolverSettings.Metaheuristics.ToString();
            Validate = task.Validate;

            TaskType = "General";
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
                    MotionID = record.Motion.ID
                });
            }

            foreach (var posPrec in task.MotionPrecedences)
            {
                ProcessPrecedences.Add(new OrderConstraintSerializationObject()
                {
                    BeforeID = posPrec.Before.ID,
                    AfterID = posPrec.After.ID
                });
            }

            foreach (var procPrec in task.ProcessPrecedences)
            {
                MotionPrecedence.Add(new OrderConstraintSerializationObject()
                {
                    BeforeID = procPrec.Before.ID,
                    AfterID = procPrec.After.ID
                });
            }
        }


        public NewGeneralTask ToGeneralTask()
        {
            var task = new NewGeneralTask();
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
                SeqLogger.Error("StartDepot not exist position!", nameof(NewGeneralTaskSerializationObject));
            if (FinishDepot != -1 && task.FinishDepotConfig == null)
                SeqLogger.Error("FinishDepot not exist as position!", nameof(NewGeneralTaskSerializationObject));
            //task.PositionMatrix.StrictUserEdgeWeights = OverrideCost.ToStrictEdgeWeightSet(task.PositionMatrix.Positions);
            return task;
        }
        private void AddLinesToConfigList(NewGeneralTask task)
        {
            foreach (var motion in MotionList)
            {
                var m = new Motion()
                {
                    ID = motion.LineID,
                    Name = motion.Name,
                    Configs = new List<Config>() { task.Hierarchy.GetConfigByID(motion.PositionIDA), task.Hierarchy.GetConfigByID(motion.PositionIDB) },
                    Bidirectional = motion.Bidirectional
                };
            }
        }
        private void CreatePrecedences(NewGeneralTask task)
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
        private void CreateProcessHierarchy(NewGeneralTask task)
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

                Alternative alter = task.Hierarchy.GetAlternativeByID(item.AlternativeID);
                if (alter == null)
                {
                    alter = new Alternative()
                    {
                        ID = item.AlternativeID
                    };
                }

                GeneralModels.Task t = task.Hierarchy.GetTaskByID(item.TaskID);
                if (t == null)
                {
                    t = new GeneralModels.Task
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
                        Bidirectional = item.Bidirectional,
                        Configs = FindConfigsForMotion(task, item.ConfigIDs),
                        Name = item.Name
                    };
                }

                var record = new HierarchyRecord(){
                    Process =proc,
                    Alternative = alter,
                    Task =t,
                    Motion = motion,
                };
                task.Hierarchy.HierarchyRecords.Add(record);
                task.Hierarchy.Motions.Add(motion);
            }
        }

        private List<Config> FindConfigsForMotion(NewGeneralTask task, List<int> configIDs)
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
            TaskType = tokenizer.GetStringByHeader("TaskType");
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
            UseShortcutInAlternatives = tokenizer.GetBoolByHeader("UseShortcutInAlternatives" );
            ProcessHierarchy = tokenizer.GetProcessHierarchyByHeader("ProcessHierarchy" );
            MotionPrecedence = tokenizer.GetPrecedenceListByHeader("MotionPrecedence" );
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
            seq += "TaskType: " + TaskType + newline;
            seq += "Validate: " + Validate + newline;
            seq += "Cyclic: " + Cyclic + newline;
            if (StartDepot != -1)
                seq += "StartDepot: " + StartDepot + newline;
            if (FinishDepot != -1)
                seq += "FinishDepot: " + FinishDepot + newline;
            seq += "TimeLimit: " + TimeLimit + newline;
            seq += "LocalSearchStrategy: " + LocalSearchStrategy + newline;
            seq += "ContourPenalty: " + IdlePenalty + newline;
            seq += "BidirectionLineDefault: " + BidirectionMotionDefault + newline;
            seq += "UseResourceInLineLength: " + AddInMotionChangeoverToCost + newline;
            seq += "UseLineLengthInWeight: " + AddMotionLengthToCost + newline;
            seq += DistanceFunction.ToSEQShort();
            seq += ResourceFunction.ToSEQ();
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