using System;
using System.Collections.Generic;
using SequencePlanner.Helper;

namespace SequencePlanner.GeneralModels
{
    public class PCGTSPRepresentation
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

        private NewGeneralTask Task { get; set; }


        public PCGTSPRepresentation()
        {
            CostMultiplier = 1000;
            DisjointSets = new List<MotionDisjointSet>();
            MotionPrecedences = new List<MotionPrecedence>();
            InitialSolution = new List<Motion>();
        }

        public PCGTSPRepresentation(NewGeneralTask task)
        {
            Task = task;
            CostMultiplier = 1000;
            DisjointSets = new List<MotionDisjointSet>();
            MotionPrecedences = new List<MotionPrecedence>();
            InitialSolution = new List<Motion>();
        }

        public void Build()
        {
            CostMatrix =        new double[Task.Hierarchy.Motions.Count, Task.Hierarchy.Motions.Count];
            RoundedCostMatrix = new int[Task.Hierarchy.Motions.Count   , Task.Hierarchy.Motions.Count];

            for (int i = 0; i < Task.Hierarchy.Motions.Count; i++)
            {
                Task.Hierarchy.Motions[i].SequenceMatrixID = i;
                Task.CostManager.ComputeLength(Task.Hierarchy.Motions[i]);
            }

            for (int i = 0; i < Task.Hierarchy.Motions.Count; i++)
            {
                for (int j = 0; j < Task.Hierarchy.Motions.Count; j++)
                {
                    CostMatrix[i, j] = Int32.MaxValue / 10000;
                    RoundedCostMatrix[i, j] = Convert.ToInt32(CostMatrix[i, j] * CostMultiplier);
                }
            }

            ConnectProcesses();
            ConnectInAlternatives();
            CreatePrecedenceConstraints();
            StartDepot = Task.StartDepot;
            FinishDepot = Task.FinishDepot;
        }

        private void CreatePrecedenceConstraints(bool fullProcessPrecedence = false)
        {
            if (ProcessPrecedence.IsCyclic(Task.ProcessPrecedences))
                throw new SeqException("Process precedences are cyclic.");

            if (Task.MotionPrecedences != null)
            {
                MotionPrecedences.AddRange(Task.MotionPrecedences);
            }

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

        private List<MotionPrecedence> CreateProcessPrecedenceFull(ProcessPrecedence precedence)
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

        private void ConnectTasks(Task a, Task b)
        {
            foreach (var from in Task.Hierarchy.GetMotionsOf(a))
            {
                foreach (var to in Task.Hierarchy.GetMotionsOf(b))
                {
                    CostMatrix[from.SequenceMatrixID, to.SequenceMatrixID] = Task.CostManager.ComputeCost(from, to);
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
    }
}