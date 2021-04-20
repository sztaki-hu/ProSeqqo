using System;
using System.Collections.Generic;

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

        public PCGTSPRepresentation()
        {
            CostMultiplier = 1000;
            DisjointSets = new List<MotionDisjointSet>();
            MotionPrecedences = new List<MotionPrecedence>();
            InitialSolution = new List<Motion>();
        }

        public void Build(Hierarchy hierarchy, CostManager costManager)
        {
            CostMatrix = new double[hierarchy.Motions.Count, hierarchy.Motions.Count];
            RoundedCostMatrix = new int[hierarchy.Motions.Count, hierarchy.Motions.Count];

            for (int i = 0; i < hierarchy.Motions.Count; i++)
            {
                hierarchy.Motions[i].SequenceMatrixID = i;
            }

            for (int i = 0; i < hierarchy.Motions.Count; i++)
            {
                for (int j = 0; j < hierarchy.Motions.Count; j++)
                {
                    CostMatrix[i, j] = Int32.MaxValue / 10000;
                    RoundedCostMatrix[i, j] = Convert.ToInt32(CostMatrix[i, j] * CostMultiplier);
                }
            }
            ConnectProcesses(hierarchy, costManager);
            ConnectInAlternatives(hierarchy, costManager);
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

        private void ConnectProcesses(Hierarchy hierarchy, CostManager costManager)
        {
            foreach (var proc in hierarchy.GetProcesses())
            {
                foreach (var proc2 in hierarchy.GetProcesses())
                {
                    if (proc.GlobalID != proc2.GlobalID)
                    {
                        foreach (var alternative in hierarchy.GetAlternativesOf(proc))
                        {
                            if (hierarchy.GetTasksOf(alternative).Count > 0)
                            {
                                foreach (var alternative2 in hierarchy.GetAlternativesOf(proc2))
                                {
                                    if (alternative.GlobalID != alternative2.GlobalID && hierarchy.GetTasksOf(alternative2).Count > 0)
                                    {
                                        ConnectTasks(hierarchy.GetTasksOf(alternative)[^1], hierarchy.GetTasksOf(alternative2)[0], hierarchy, costManager);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ConnectInAlternatives(Hierarchy hierarchy, CostManager costManager)
        {
            foreach (var alternative in hierarchy.GetAlternatives())
            {
                for (int i = 0; i < hierarchy.GetTasksOf(alternative).Count - 1; i++)
                {
                    ConnectTasks(hierarchy.GetTasksOf(alternative)[i], hierarchy.GetTasksOf(alternative)[i + 1], hierarchy, costManager);
                }
            }
        }

        private void ConnectTasks(Task a, Task b, Hierarchy hierarchy, CostManager costManager)
        {
            foreach (var from in hierarchy.GetMotionsOf(a))
            {
                foreach (var to in hierarchy.GetMotionsOf(b))
                {
                    CostMatrix[from.SequenceMatrixID, to.SequenceMatrixID] = costManager.ComputeCost(from, to);
                    RoundedCostMatrix[from.SequenceMatrixID, to.SequenceMatrixID] = Convert.ToInt32(CostMatrix[from.SequenceMatrixID, to.SequenceMatrixID] * CostMultiplier);
                }
            }
        }
    }
}