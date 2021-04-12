using SequencePlanner.GTSPTask.Serialization.SerializationObject;
using SequencePlanner.GTSPTask.Serialization.SerializationObject.Token;
using SequencePlanner.GTSPTask.Task.Base;
using SequencePlanner.GTSPTask.Task.General;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Serialization.Task
{
    public class GeneralTaskSerializationObject : BaseTaskSerializationObject
    {
        public List<ProcessHierarchySerializationObject> ProcessHierarchy { get; set; }
        public List<OrderConstraintSerializationObject> PositionPrecedences { get; set; }
        public List<OrderConstraintSerializationObject> ProcessPrecedences { get; set; }
        public List<HybridLineSerializationObject> LineList { get; set; }
        public bool UseShortcutInAlternatives { get; set; }

        public GeneralTaskSerializationObject() : base()
        {
        }

        public GeneralTaskSerializationObject(List<string> seqString):base(seqString)
        {
            var tokenizer = new SEQTokenizer();
            tokenizer.Tokenize(seqString);
            FillBySEQTokens(tokenizer);
        }

        public GeneralTaskSerializationObject(GeneralTask task):base(task)
        {
            TaskType = "General";
            UseShortcutInAlternatives = task.UseShortcutInAlternatives;
            ProcessHierarchy = new List<ProcessHierarchySerializationObject>();
            PositionPrecedences = new List<OrderConstraintSerializationObject>();
            ProcessPrecedences = new List<OrderConstraintSerializationObject>();
            LineList = new List<HybridLineSerializationObject>();
            foreach (var proc in task.Processes)
            {
                foreach (var alternative in proc.Alternatives)
                {
                    foreach (var lineTask in alternative.Tasks)
                    {
                        foreach (var position in lineTask.Positions)
                        {
                            ProcessHierarchy.Add(new ProcessHierarchySerializationObject()
                            {
                                ProcessID = proc.UserID,
                                AlternativeID = alternative.UserID,
                                TaskID = lineTask.UserID,
                                PositionID = position.Node.UserID
                            });
                        }
                    }
                }
            }

            foreach (var posPrec in task.PositionPrecedence)
            {
                ProcessPrecedences.Add(new OrderConstraintSerializationObject()
                {
                    BeforeID = posPrec.Before.UserID,
                    AfterID = posPrec.After.UserID
                });
            }

            foreach (var procPrec in task.ProcessPrecedence)
            {
                PositionPrecedences.Add(new OrderConstraintSerializationObject()
                {
                    BeforeID = procPrec.Before.UserID,
                    AfterID = procPrec.After.UserID
                });
            }
        }

        public GeneralTask ToGeneralTask()
        {
            var PointLikeTask = new GeneralTask();
            base.ToBaseTask((BaseTask)PointLikeTask);
            AddLinesToPositionList(PointLikeTask);
            CreateProcessHierarchy(PointLikeTask);
            CreatePrecedences(PointLikeTask);
            PointLikeTask.UseShortcutInAlternatives = UseShortcutInAlternatives;
            return PointLikeTask;
        }

        private void AddLinesToPositionList(GeneralTask task)
        {
            foreach (var line in LineList)
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
            foreach (var posPrec in PositionPrecedences)
            {
                var before = FindNode(posPrec.BeforeID, pointLikeTask);
                var after = FindNode(posPrec.AfterID, pointLikeTask);
                if (before == null || after == null)
                    throw new SeqException("Phrase error line precedence user id not found!");
                pointLikeTask.PositionPrecedence.Add(new GTSPPrecedenceConstraint()
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

                Alternative alter = FindAlternative(item.AlternativeID, pointLikeTask, proc);
                
                if (alter == null)
                {
                    alter = new Alternative()
                    {
                        UserID = item.AlternativeID
                    };
                    pointLikeTask.Alternatives.Add(alter);
                    proc.Alternatives.Add(alter);
                }

                Model.Task task = FindTask(item.TaskID, pointLikeTask, alter);
                
                if (task == null)
                {
                    task = new Model.Task
                    {
                        UserID = item.TaskID
                    };
                    pointLikeTask.Tasks.Add(task);
                    alter.Tasks.Add(task);
                }
           
                var position = FindNode(item.PositionID, pointLikeTask);

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

        public void FillBySEQTokens(SEQTokenizer tokenizer)
        {
            base.FillBySEQTokens(tokenizer);
            UseShortcutInAlternatives = tokenizer.GetBoolByHeader("UseShortcutInAlternatives" );
            ProcessHierarchy = tokenizer.GetProcessHierarchyByHeader("ProcessHierarchy" );
            PositionPrecedences = tokenizer.GetPrecedenceListByHeader("PositionPrecedence" );
            ProcessPrecedences = tokenizer.GetPrecedenceListByHeader("ProcessPrecedence");
            LineList = tokenizer.GetHybridLineListByHeader("LineList");
        }

        public string ToSEQ()
        {
            string seq = "";
            string newline = "\n";
            seq+=base.ToSEQShort();
            seq += "UseShortcutInAlternatives: " + UseShortcutInAlternatives + newline;
            seq +=base.ToSEQLong();
            seq += "LineList:" + newline;
            foreach (var line in LineList)
            {
                seq += line.ToSEQ();
            }
            seq += "ProcessHierarchy:" + newline;
            foreach (var line in ProcessHierarchy)
            {
                seq += line.ToSEQ();
            }
            seq += "PositionPrecedence:" + newline;
            foreach (var prec in PositionPrecedences)
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

        public Process FindProcess(int userID, GeneralTask task)
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

        public Alternative FindAlternative(int userID, GeneralTask task, Process process)
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

        public Model.Task FindTask(int userID, GeneralTask task, Alternative alternative)
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

        public Position FindPosition(int userID, GeneralTask task)
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

        public GTSPNode FindNode(int userID, GeneralTask task)
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