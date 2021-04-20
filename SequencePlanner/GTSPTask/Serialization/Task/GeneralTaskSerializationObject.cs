using System;
using System.Collections.Generic;
using SequencePlanner.Model;
using SequencePlanner.Helper;
using SequencePlanner.GTSPTask.Task.General;
using SequencePlanner.GTSPTask.Serialization.SerializationObject;
using SequencePlanner.GTSPTask.Serialization.SerializationObject.Token;
using SequencePlanner.OR_Tools;

namespace SequencePlanner.GTSPTask.Serialization.Task
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


        public GeneralTaskSerializationObject(){ }

        public GeneralTaskSerializationObject(List<string> seqString) : base()
        {
            var tokenizer = new SEQTokenizer();
            tokenizer.Tokenize(seqString);
            FillBySEQTokens(tokenizer);
        }

        public GeneralTaskSerializationObject(GeneralTask task)
        {
            //Cyclic = task.Cyclic;
            //BidirectionMotionDefault = Line.BIDIRECTIONAL_DEFAULT;
            //if (task.StartDepot != null)
            //    StartDepot = task.StartDepot.UserID;
            //else
            //    StartDepot = -1;
            //if (task.FinishDepot != null)
            //    FinishDepot = task.FinishDepot.UserID;
            //else
            //    FinishDepot = -1;
            //TimeLimit = task.TimeLimit;
            //ConfigList = new List<PositionSerializationObject>();
            //foreach (var item in task.PositionMatrix.Positions)
            //{
            //    ConfigList.Add(new PositionSerializationObject(item.Out)); ///TODO: Rapid FIX change it!
            //}
            //DistanceFunction = new DistanceFunctionSerializationObject(task.PositionMatrix);
            //ResourceFunction = new ResourceFunctionSerializationObject(task.PositionMatrix);
            //OverrideCost = new StrictEdgeWeightSetSerializationObject(task.PositionMatrix.StrictUserEdgeWeights);
            //AddInMotionChangeoverToCost = false;
            //AddMotionLengthToCost = false;
            //LocalSearchStrategy = task.LocalSearchStrategy.ToString();
            //Validate = task.Validate;

            //TaskType = "General";
            //UseShortcutInAlternatives = task.UseShortcutInAlternatives;
            //ProcessHierarchy = new List<ProcessHierarchySerializationObject>();
            //MotionPrecedence = new List<OrderConstraintSerializationObject>();
            //ProcessPrecedences = new List<OrderConstraintSerializationObject>();
            //MotionList = new List<HybridLineSerializationObject>();
            //foreach (var proc in task.Processes)
            //{
            //    foreach (var alternative in proc.Alternatives)
            //    {
            //        foreach (var lineTask in alternative.Tasks)
            //        {
            //            foreach (var position in lineTask.Positions)
            //            {
            //                ProcessHierarchy.Add(new ProcessHierarchySerializationObject()
            //                {
            //                    ProcessID = proc.UserID,
            //                    AlternativeID = alternative.UserID,
            //                    TaskID = lineTask.UserID,
            //                    PositionID = position.Node.UserID
            //                });
            //            }
            //        }
            //    }
            //}

            //foreach (var posPrec in task.MotionPrecedence)
            //{
            //    ProcessPrecedences.Add(new OrderConstraintSerializationObject()
            //    {
            //        BeforeID = posPrec.Before.UserID,
            //        AfterID = posPrec.After.UserID
            //    });
            //}

            //foreach (var procPrec in task.ProcessPrecedence)
            //{
            //    MotionPrecedence.Add(new OrderConstraintSerializationObject()
            //    {
            //        BeforeID = procPrec.Before.UserID,
            //        AfterID = procPrec.After.UserID
            //    });
            //}
        }


        public GeneralTask ToGeneralTask()
        {
            var task = new GeneralTask();
            //TaskType
            //task.Validate = Validate;
            //task.Cyclic = Cyclic;
            //task.PositionMatrix = new PositionMatrix();
            //Line.BIDIRECTIONAL_DEFAULT = BidirectionMotionDefault;
            //task.PositionMatrix.UseResourceInLineLength = AddInMotionChangeoverToCost;
            //task.PositionMatrix.UseLineLengthInWeight = AddMotionLengthToCost;
            //task.IdlePenalty = IdlePenalty;
            //foreach (var pos in ConfigList)
            //{
            //    var newPosition = pos.ToConfig();
            //    task.PositionMatrix.Positions.Add(new GTSPNode(newPosition));
            //    if (newPosition.UserID == StartDepot)
            //        task.StartDepot = newPosition;
            //    if (newPosition.UserID == FinishDepot)
            //        task.FinishDepot = newPosition;
            //}
            //if (StartDepot != -1 && task.StartDepot == null)
            //    SeqLogger.Error("StartDepot not exist position!", nameof(GeneralTaskSerializationObject));
            //if (FinishDepot != -1 && task.FinishDepot == null)
            //    SeqLogger.Error("FinishDepot not exist as position!", nameof(GeneralTaskSerializationObject));
            //task.TimeLimit = TimeLimit;
            //task.PositionMatrix.StrictUserEdgeWeights = OverrideCost.ToStrictEdgeWeightSet(task.PositionMatrix.Positions);

            //task.PositionMatrix.DistanceFunction = DistanceFunction.ToDistanceFunction();
            //task.PositionMatrix.ResourceFunction = ResourceFunction.ToResourceFunction();
            //task.UseMIPprecedenceSolver = UseMIPprecedenceSolver;
            //task.LocalSearchStrategy = LocalSearchStrategyEnum.ResolveEnum(LocalSearchStrategy);
            //AddLinesToConfigList(task);
            //CreateProcessHierarchy(task);
            //CreatePrecedences(task);
            //task.UseShortcutInAlternatives = UseShortcutInAlternatives;
            return task;
        }
        private void AddLinesToConfigList(GeneralTask task)
        {
            foreach (var line in MotionList)
            {
                var l = new Line()
                {
                    NodeA = FindPosition(line.PositionIDA, task),
                    NodeB = FindPosition(line.PositionIDB, task),
                    UserID = line.LineID,
                    Name = line.Name,
                    Bidirectional = line.Bidirectional
                };
                task.PositionMatrix.Positions.Add(new GTSPNode(l));
            }
        }
        private void CreatePrecedences(GeneralTask pointLikeTask)
        {
            foreach (var posPrec in MotionPrecedence)
            {
                var before = FindNode(posPrec.BeforeID, pointLikeTask);
                var after = FindNode(posPrec.AfterID, pointLikeTask);
                if (before == null || after == null)
                    throw new SeqException("Phrase error line precedence user id not found!");
                pointLikeTask.MotionPrecedence.Add(new GTSPPrecedenceConstraint()
                {
                    Before = before.Node,
                    After = after.Node
                });
            }

            foreach (var processPrec in ProcessPrecedences)
            {
                var before = FindProcess(processPrec.BeforeID, pointLikeTask);
                var after = FindProcess(processPrec.AfterID, pointLikeTask);
                if (before == null || after == null)
                    throw new SeqException("Phrase error process precedence user id not found!");
                pointLikeTask.ProcessPrecedence.Add(new GTSPPrecedenceConstraint()
                {
                    Before = before,
                    After = after
                });
            }
        }
        private void CreateProcessHierarchy(GeneralTask pointLikeTask)
        {
            foreach (var item in ProcessHierarchy)
            {
                Process proc = FindProcess(item.ProcessID, pointLikeTask);

                if (proc == null)
                {
                    proc = new Process()
                    {
                        UserID = item.ProcessID
                    };
                    pointLikeTask.Processes.Add(proc);
                }

                Alternative alter = FindAlternative(item.AlternativeID, proc);
                
                if (alter == null)
                {
                    alter = new Alternative()
                    {
                        UserID = item.AlternativeID
                    };
                    pointLikeTask.Alternatives.Add(alter);
                    proc.Alternatives.Add(alter);
                }

                Model.Task task = FindTask(item.TaskID, alter);
                
                if (task == null)
                {
                    task = new Model.Task
                    {
                        UserID = item.TaskID
                    };
                    pointLikeTask.Tasks.Add(task);
                    alter.Tasks.Add(task);
                }
           
                var position = FindNode(item.MotionID, pointLikeTask);

                if (position == null)
                {
                    //position = new Position()
                    //{
                    //    UserID = item.PositionID
                    //};
                    //pointLikeTask.PositionMatrix.Positions.Add(new GTSPNode(position));
                    Console.WriteLine("PointLike GTSP builder process hierarchy ID error, this error sholud be caught by validation!");
                }
                task.Positions.Add(position);
            }
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
            BidirectionMotionDefault = tokenizer.GetBoolByHeader("BidirectionLineDefault");
            AddInMotionChangeoverToCost = tokenizer.GetBoolByHeader("AddInMotionChangeoverToCost");
            AddMotionLengthToCost = tokenizer.GetBoolByHeader("AddMotionLengthToCost");
            IdlePenalty = tokenizer.GetDoubleByHeader("IdlePenalty");
            Line.BIDIRECTIONAL_DEFAULT = BidirectionMotionDefault;
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

        public static Process FindProcess(int userID, GeneralTask task)
        {
            foreach (var item in task.Processes)
            {
                if (item.UserID == userID)
                {
                    return item;
                }
            }
            return null;
        }
        public static Alternative FindAlternative(int userID, Process process)
        {
            if (process != null)
            {
                foreach (var item in process.Alternatives)
                {
                    if (item.UserID == userID)
                        return item;
                }
            }
            return null;
        }
        public static Model.Task FindTask(int userID, Alternative alternative)
        {
            if (alternative != null)
            {
                foreach (var item in alternative.Tasks)
                {
                    if (item.UserID == userID)
                        return item;
                }
            }
            return null;
        }
        public static Position FindPosition(int userID, GeneralTask task)
        {
            foreach (var item in task.PositionMatrix.Positions)
            {
                if (item.In.UserID == userID)
                {
                    return item.In;
                }
                if (item.Out.UserID == userID)
                {
                    return item.Out;
                }
            }
            return null;
        }
        public static GTSPNode FindNode(int userID, GeneralTask task)
        {
            foreach (var item in task.PositionMatrix.Positions)
            {
                if (item.Node.UserID == userID)
                {
                    return item;
                }
            }
            return null;
        }
    }
}