using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options
{
    public class FullOptionSet: OptionSet
    {
        public override void Init()
        {
            Options.Add(new TaskType());
            Options.Add(new EdgeWeightSource());
            Options.Add(new Dimension());
            Options.Add(new TimeLimit());
            Options.Add(new CyclicSequence());
            Options.Add(new WeightMultiplier());
            Options.Add(new DistanceFunction());
            Options.Add(new TrapezoidParamsAcceleration());
            Options.Add(new TrapezoidParamsSpeed());
            Options.Add(new StartDepot());
            Options.Add(new FinishDepot());
            Options.Add(new ProcessHierarchy());
            Options.Add(new ProcessPrecedence());
            Options.Add(new PositionPrecedence());
            Options.Add(new LineList());
            Options.Add(new LinePrecedence());
            Options.Add(new ContourPrecedence());
            Options.Add(new PositionList());
            Options.Add(new PositionMatrix());
            Options.Add(new ContourPenalty());

            foreach (var item in Options)
            {
                if (item.Need)
                    Need.Add(item);
            }
        }
    }
}
