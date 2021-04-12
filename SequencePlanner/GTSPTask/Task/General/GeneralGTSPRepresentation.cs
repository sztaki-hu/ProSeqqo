using SequencePlanner.GTSPTask.Task.Base;
using SequencePlanner.Model;
using System.Collections.Generic;

namespace SequencePlanner.GTSPTask.Task.General
{
    public class GeneralGTSPRepresentation : IGTSPRepresentation
    {
        public List<GTSPPrecedenceConstraint> PrecedenceConstraints  { get;  set; }
        public List<GTSPDisjointConstraint> DisjointConstraints  { get;  set; }
        public int[,] RoundedMatrix  { get;  set; }
        public double[,] Matrix  { get;  set; }
        public int StartDepot  { get;  set; }
        public int FinishDepot  { get;  set; }
        public long[][] InitialRoutes { get; set; }
    }
}