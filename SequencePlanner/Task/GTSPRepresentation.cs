using SequencePlanner.Helper;
using SequencePlanner.Model;
using SequencePlanner.Model.Hierarchy;
using System;
using System.Collections.Generic;

namespace SequencePlanner.Task
{
    public class GTSPRepresentation
    {
        public Motion StartDepot { get; set; }
        public Motion FinishDepot { get; set; }
        public double[,] CostMatrix { get; set; }
        public int[,] RoundedCostMatrix { get; set; }
        public int CostMultiplier { get; set; }
        public List<MotionPrecedence> MotionPrecedences { get; set; }
        public List<MotionDisjointSet> DisjointSets { get; set; }
        public List<Motion> InitialSolution { get; set; }
        public long[][] InitialRoutes { get { return ToInitialRoute(); } }

        private GeneralTask Task { get; set; }

        public GTSPRepresentation()
        {
            CostMultiplier = 1000;
            DisjointSets = new List<MotionDisjointSet>();
            MotionPrecedences = new List<MotionPrecedence>();
            InitialSolution = new List<Motion>();
        }

        public GTSPRepresentation(GeneralTask task)
        {
            Task = task;
            CostMultiplier = 1000;
            DisjointSets = new List<MotionDisjointSet>();
            MotionPrecedences = new List<MotionPrecedence>();
            InitialSolution = new List<Motion>();
        }

        public void Build()
        {
            CostMatrix = new double[Task.Hierarchy.Motions.Count, Task.Hierarchy.Motions.Count];
            RoundedCostMatrix = new int[Task.Hierarchy.Motions.Count, Task.Hierarchy.Motions.Count];

            for (int i = 0; i < Task.Hierarchy.Motions.Count; i++)
            {
                Task.Hierarchy.Motions[i].SequenceMatrixID = i;
                Task.CostManager.ComputeCost(Task.Hierarchy.Motions[i]);
            }

            for (int i = 0; i < Task.Hierarchy.Motions.Count; i++)
            {
                for (int j = 0; j < Task.Hierarchy.Motions.Count; j++)
                {
                    CostMatrix[i, j] = Int32.MaxValue / 10000;
                    RoundedCostMatrix[i, j] = Convert.ToInt32(CostMatrix[i, j] * CostMultiplier);
                }
            }

            CreateDisjointConstraints();
            ConnectProcesses();
            ConnectInAlternatives();
            CreatePrecedenceConstraints();
            StartDepot = Task.StartDepot;
            FinishDepot = Task.FinishDepot;
        }

        public void ORToolsFixFinishDepot()
        {
            if(Task.FinishDepot is not null)
            {
                var finishSeqID = Task.FinishDepot.SequenceMatrixID;
                var maxSeqID = 0;
                Motion maxSeqMotion = null;
                foreach (var item in Task.Hierarchy.GetMotions())
                {
                    if (item.SequenceMatrixID > maxSeqID)
                    {
                        maxSeqID = item.SequenceMatrixID;
                        maxSeqMotion = item;
                    }
                }
                if(maxSeqMotion!=null && Task.FinishDepot.GlobalID != maxSeqMotion.GlobalID)
                {
                    maxSeqMotion.SequenceMatrixID = finishSeqID;
                    Task.FinishDepot.SequenceMatrixID = maxSeqID;
                }
                SwapNodes(finishSeqID, maxSeqID);
            }
        }

        private void CreatePrecedenceConstraints(bool fullProcessPrecedence = false)
        {
            if (ProcessPrecedence.IsCyclic(Task.ProcessPrecedences))
                throw new SeqException("Process precedences are cyclic.");

            if (Task.MotionPrecedences != null)
            {
                MotionPrecedences.AddRange(Task.MotionPrecedences);
            }

            List<MotionPrecedence> bidirPrec = new List<MotionPrecedence>();
            for (int i = 0; i < MotionPrecedences.Count; i++)
            {
                if(MotionPrecedences[i].Before.Bidirectional)
                {
                    bidirPrec.Add(new MotionPrecedence(Task.Hierarchy.GetMotionByID(-MotionPrecedences[i].Before.ID), MotionPrecedences[i].After));
                }

                if (MotionPrecedences[i].After.Bidirectional)
                {
                    bidirPrec.Add(new MotionPrecedence(MotionPrecedences[i].Before, Task.Hierarchy.GetMotionByID(-MotionPrecedences[i].After.ID)));
                }

                if (MotionPrecedences[i].Before.Bidirectional && MotionPrecedences[i].After.Bidirectional)
                {
                    bidirPrec.Add(new MotionPrecedence(Task.Hierarchy.GetMotionByID(-MotionPrecedences[i].Before.ID), Task.Hierarchy.GetMotionByID(-MotionPrecedences[i].After.ID)));
                }
            }

            MotionPrecedences.AddRange(bidirPrec);

            if (Task.ProcessPrecedences != null)
            {
                foreach (var precedence in Task.ProcessPrecedences)
                {
                    if (fullProcessPrecedence)
                        MotionPrecedences.AddRange(CreateProcessPrecedenceFull(precedence));
                    else
                        MotionPrecedences.AddRange(CreateProcessPrecedence(precedence));
                }
            }
        }

        private List<MotionPrecedence> CreateProcessPrecedence(ProcessPrecedence precedence)
        {
            List<MotionPrecedence> motionPrecedences = new List<MotionPrecedence>();
            var bef = Task.Hierarchy.GetMotionsOfFirstTasksOfAlternativesInProcesses(precedence.Before);
            var after = Task.Hierarchy.GetMotionsOfLastTasksOfAlternativesInProcesses(precedence.After);
            foreach (var b in bef)
            {
                foreach (var a in after)
                {
                    motionPrecedences.Add(new MotionPrecedence(b, a));
                }
            }
            return motionPrecedences;
        }

        public List<MotionPrecedence> CreateProcessPrecedenceFull(ProcessPrecedence precedence)
        {
            List<MotionPrecedence> motionPrecedences = new List<MotionPrecedence>();
            List<Motion> BeforeMotions = Task.Hierarchy.GetMotionsOf(precedence.Before);
            List<Motion> AfterMotions = Task.Hierarchy.GetMotionsOf(precedence.Before);
            foreach (var before in BeforeMotions)
            {
                foreach (var after in AfterMotions)
                {
                    motionPrecedences.Add(new MotionPrecedence(before, after));
                }
            }
            return motionPrecedences;
        }

        public  List<MotionPrecedenceList> CreatePrecedenceHierarchiesForInitialSolution()
        {
            var prec = new List<MotionPrecedenceList>();
            foreach (var alternative in Task.Hierarchy.GetAlternatives())
            {
                var listOfTasks = Task.Hierarchy.GetTasksOf(alternative);
                if (listOfTasks.Count >= 2)
                {
                    for (int i = 0; i < listOfTasks.Count - 1; i++)
                    {
                        MotionPrecedenceList tmp = new MotionPrecedenceList();
                        foreach (var item in Task.Hierarchy.GetMotionsOf(listOfTasks[i]))
                        {
                            tmp.Before.Add(item);
                        }
                        foreach (var item in Task.Hierarchy.GetMotionsOf(listOfTasks[i+1]))
                        {
                            tmp.After.Add(item);
                        }
                        prec.Add(tmp);
                    }
                }
            }
            return prec;
        }

        private List<MotionDisjointSet> CreateDisjointConstraints()
        {
            DisjointSets = new List<MotionDisjointSet>();
            foreach (var process in Task.Hierarchy.GetProcesses())
            {
                var alternatives = Task.Hierarchy.GetAlternativesOf(process);
                if (alternatives.Count > 0)//!
                {
                    int[] taskNumberOfAlternatives = new int[alternatives.Count];
                    int maxTaskNumber = 0;
                    for (int i = 0; i < alternatives.Count; i++)
                    {
                        taskNumberOfAlternatives[i] = Task.Hierarchy.GetTasksOf(alternatives[i]).Count;
                        if (taskNumberOfAlternatives[i] > maxTaskNumber)
                            maxTaskNumber = taskNumberOfAlternatives[i];
                    }

                    //if (process.Alternatives.Count < 0)
                    //{
                    //    maxTaskNumber = process.Alternatives[0].Tasks.Count;
                    //}

                    for (int i = 0; i < maxTaskNumber; i++)
                    {
                        var constraint = new MotionDisjointSet();
                        for (int j = 0; j < alternatives.Count; j++)
                        {
                            if (taskNumberOfAlternatives[j] <= i)
                            {
                                //Add positions of positions of j. alternative last layer
                                var tasks = Task.Hierarchy.GetTasksOf(alternatives[j]);
                                var positions = Task.Hierarchy.GetMotionsOf(tasks[taskNumberOfAlternatives[j] - 1]);
                                foreach (var position in positions)
                                    constraint.Elements.Add(position);
                            }
                            else
                            {
                                var tasks = Task.Hierarchy.GetTasksOf(alternatives[j]);
                                var positions = Task.Hierarchy.GetMotionsOf(tasks[i]);
                                //Add positions of positions of j. alternative i.layer
                                foreach (var position in positions)
                                    constraint.Elements.Add(position);
                            }
                        }
                        DisjointSets.Add(constraint);
                    }
                }
            }
            return DisjointSets;
        }

        private void ConnectProcesses()
        {
            foreach (var proc in Task.Hierarchy.GetProcesses())
            {
                foreach (var proc2 in Task.Hierarchy.GetProcesses())
                {
                    if (proc.GlobalID != proc2.GlobalID)
                    {
                        foreach (var alternative in Task.Hierarchy.GetAlternativesOf(proc))
                        {
                            if (Task.Hierarchy.GetTasksOf(alternative).Count > 0)
                            {
                                foreach (var alternative2 in Task.Hierarchy.GetAlternativesOf(proc2))
                                {
                                    if (alternative.GlobalID != alternative2.GlobalID && Task.Hierarchy.GetTasksOf(alternative2).Count > 0)
                                    {
                                        ConnectTasks(Task.Hierarchy.GetTasksOf(alternative)[^1], Task.Hierarchy.GetTasksOf(alternative2)[0]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ConnectInAlternatives()
        {
            foreach (var alternative in Task.Hierarchy.GetAlternatives())
            {
                for (int i = 0; i < Task.Hierarchy.GetTasksOf(alternative).Count - 1; i++)
                {
                    ConnectTasks(Task.Hierarchy.GetTasksOf(alternative)[i], Task.Hierarchy.GetTasksOf(alternative)[i + 1]);
                }
            }
        }

        private void ConnectTasks(Model.Hierarchy.Task a, Model.Hierarchy.Task b)
        {
            foreach (var from in Task.Hierarchy.GetMotionsOf(a))
            {
                foreach (var to in Task.Hierarchy.GetMotionsOf(b))
                {
                    CostMatrix[from.SequenceMatrixID, to.SequenceMatrixID] = Task.CostManager.ComputeCost(from, to).FinalCost;
                    RoundedCostMatrix[from.SequenceMatrixID, to.SequenceMatrixID] = Convert.ToInt32(CostMatrix[from.SequenceMatrixID, to.SequenceMatrixID] * CostMultiplier);
                }
            }
        }

        private long[][] ToInitialRoute()
        {
            long[][] rout = new long[1][];
            rout[0] = new long[InitialSolution.Count];
            for (int i = 0; i < InitialSolution.Count; i++)
            {
                rout[0][i] = InitialSolution[i].SequenceMatrixID;
            }
            return rout;
        }

        private void SwapNodes(int from, int to)
        {
            int n = CostMatrix.GetLength(0);
            double[] tmp = new double[n];
            int[] tmpRound = new int[n];
            for (int i = 0; i < n; i++)
            {
                tmp[i] = CostMatrix[i, to];
                tmpRound[i] = RoundedCostMatrix[i, to];

                CostMatrix[i,to] = CostMatrix[i,from];
                RoundedCostMatrix[i,to] = RoundedCostMatrix[i,from];

                CostMatrix[i, from] = tmp[i];
                RoundedCostMatrix[i, from] = tmpRound[i];
            }
            for (int i = 0; i < n; i++)
            {
                tmp[i] = CostMatrix[to, i];
                tmpRound[i] = RoundedCostMatrix[to,i];

                CostMatrix[to, i] = CostMatrix[from,i];
                RoundedCostMatrix[to,i] = RoundedCostMatrix[from,i];

                CostMatrix[from,i] = tmp[i];
                RoundedCostMatrix[from,i] = tmpRound[i];
            }
        }
    }
}