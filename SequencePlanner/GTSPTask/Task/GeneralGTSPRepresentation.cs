using System.Collections.Generic;
using SequencePlanner.Model;

namespace SequencePlanner.GTSPTask.Task.General
{
    public class GeneralGTSPRepresentation
    {
        public int StartDepot  { get;  set; }
        public int FinishDepot  { get;  set; }
        public long[][] InitialRoutes { get; set; }
        public double[,] Matrix  { get;  set; }
        public int[,] RoundedMatrix  { get;  set; }
        public List<GTSPDisjointConstraint> DisjointConstraints  { get;  set; }
        public List<GTSPPrecedenceConstraint> PrecedenceConstraints  { get;  set; }
    }
}