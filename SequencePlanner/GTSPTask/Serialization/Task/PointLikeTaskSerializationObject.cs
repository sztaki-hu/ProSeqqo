using Newtonsoft.Json;
using SequencePlanner.GTSPTask.Serialization.SerializationObject;
using SequencePlanner.GTSPTask.Serialization.SerializationObject.Token;
using SequencePlanner.GTSPTask.Task.Base;
using SequencePlanner.GTSPTask.Task.PointLike;
using SequencePlanner.Helper;
using SequencePlanner.Model;
using System;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Serialization.Task
{
    public class PointLikeTaskSerializationObject : BaseTaskSerializationObject
    {
        [JsonProperty(Order = 11)]
        public List<ProcessHierarchySerializationObject> ProcessHierarchy { get; set; }
        [JsonProperty(Order = 12)]
        public List<OrderConstraintSerializationObject> PositionPrecedences { get; set; }
        [JsonProperty(Order = 13)]
        public List<OrderConstraintSerializationObject> ProcessPrecedences { get; set; }
        public bool UseShortcutInAlternatives { get; set; }

        public PointLikeTaskSerializationObject() : base()
        {
        }
        public PointLikeTaskSerializationObject(List<string> seqString):base(seqString)
        {
            var tokenizer = new SEQTokenizer();
            tokenizer.Tokenize(seqString);
            FillBySEQTokens(tokenizer);
        }
        public PointLikeTaskSerializationObject(PointLikeTask task):base(task)
        {
            TaskType = "PointLike";
            UseShortcutInAlternatives = task.UseShortcutInAlternatives;
            ProcessHierarchy = new List<ProcessHierarchySerializationObject>();
            PositionPrecedences = new List<OrderConstraintSerializationObject>();
            ProcessPrecedences = new List<OrderConstraintSerializationObject>();
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

        public PointLikeTask ToPointLikeTask()
        {
            var PointLikeTask = new PointLikeTask();
            base.ToBaseTask((BaseTask)PointLikeTask);
            CreateProcessHierarchy(PointLikeTask);
            CreatePrecedences(PointLikeTask);
            PointLikeTask.UseShortcutInAlternatives = UseShortcutInAlternatives;
            return PointLikeTask;
        }

        private void CreatePrecedences(PointLikeTask pointLikeTask)
        {
            foreach (var posPrec in PositionPrecedences)
            {
                var before = FindPosition(posPrec.BeforeID, pointLikeTask);
                var after = FindPosition(posPrec.AfterID, pointLikeTask);
                if (before == null || after == null)
                    throw new SeqException("Phrase error line precedence user id not found!");
                pointLikeTask.PositionPrecedence.Add(new GTSP.GTSPPrecedenceConstraint()
                {
                    Before = before,
                    After = after
                });
            }

            foreach (var processPrec in ProcessPrecedences)
            {
                var before = FindProcess(processPrec.BeforeID, pointLikeTask);
                var after = FindProcess(processPrec.AfterID, pointLikeTask);
                if (before == null || after == null)
                    throw new SeqException("Phrase error contour precedence user id not found!");
                pointLikeTask.ProcessPrecedence.Add(new GTSP.GTSPPrecedenceConstraint()
                {
                    Before = before,
                    After = after
                });
            }
        }

        private void CreateProcessHierarchy(PointLikeTask pointLikeTask)
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
           
                Position position = FindPosition(item.PositionID, pointLikeTask);

                if (position == null)
                {
                    position = new Position()
                    {
                        UserID = item.PositionID
                    };
                    pointLikeTask.PositionMatrix.Positions.Add(new GTSPNode(position));
                    Console.WriteLine("PointLike GTSP builder process hierarchy ID error, this error sholud be caught by validation!");
                }
                task.Positions.Add(new GTSPNode(position));
            }
        }

        public void FillBySEQTokens(SEQTokenizer tokenizer)
        {
            base.FillBySEQTokens(tokenizer);
            UseShortcutInAlternatives = tokenizer.GetBoolByHeader("UseShortcutInAlternatives" );
            ProcessHierarchy = tokenizer.GetProcessHierarchyByHeader("ProcessHierarchy" );
            PositionPrecedences = tokenizer.GetPrecedenceListByHeader("PositionPrecedence" );
            ProcessPrecedences = tokenizer.GetPrecedenceListByHeader("ProcessPrecedence");
        }
        public string ToSEQ()
        {
            string seq = "";
            string newline = "\n";
            seq+=base.ToSEQShort();
            seq += "UseShortcutInAlternatives: " + UseShortcutInAlternatives + newline;
            seq +=base.ToSEQLong();
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

        public Process FindProcess(int userID, PointLikeTask task)
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

        public Alternative FindAlternative(int userID, PointLikeTask task, Process process)
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

        public Model.Task FindTask(int userID, PointLikeTask task, Alternative alternative)
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

        public Position FindPosition(int userID, PointLikeTask task)
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
    }
}