using SequencePlanner.Phraser.Options.Values;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class GTSPRepresentation
    {
        protected static double PlusInfity { get; set; }
        protected static double MinusInfity { get; set; }

        public List<Position> Positions { get; set; }
        public GraphRepresentation Graph { get; set; }

        public List<ConstraintDisjoint> ConstraintsDisjoints { get; set; }
        public List<ConstraintOrder> ConstraintsOrder { get; set; }
        public EdgeWeightCalculator EdgeWeightCalculator { get; set; }
        public int WeightMultiplier { get; set; }
        public double[,] PositionMatrix { get; set; }
        public PositionMatrixOptionValue PositionMatrixOriginal { get; set; }

        public GTSPRepresentation()
        {
            Positions = new List<Position>();
            PlusInfity = int.MaxValue;
            MinusInfity = int.MinValue;
            ConstraintsDisjoints = new List<ConstraintDisjoint>();
            ConstraintsOrder = new List<ConstraintOrder>();
            Graph = new GraphRepresentation() {
                WeightMultiplier = this.WeightMultiplier
            };
        }

        public virtual void Build() { }
        public virtual void GenerateDisjunctSets() { }
        
        public Position FindPositionByPID(int PID)
        {
            foreach (var item in Positions)
            {
                if (item.ID == PID)
                {
                    return item;
                }
            }
            return null;
        }
        public Position FindPositionByID(int ID)
        {
            foreach (var item in Positions)
            {
                if (item.GID == ID)
                {
                    return item;
                }
            }
            return null;
        }
    }
}