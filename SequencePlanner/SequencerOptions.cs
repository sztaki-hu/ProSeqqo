using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Options
{
    public class SequencerOptions
    {
        public List<Option> Options { get; private set; }
        public TaskType TaskType { get; private set; }
        public EdgeWeightSource EdgeWeightSource { get; private set; }
        public DistanceFunction DistanceFunction { get; private set; }

        public SequencerOptions()
        {
            TaskType = new TaskType();
            EdgeWeightSource = new EdgeWeightSource();
            DistanceFunction = new DistanceFunction();

            Options.Add(TaskType);
            Options.Add(EdgeWeightSource);
            Options.Add(DistanceFunction);

        }

        public bool isAllOptionSatied()
        {
            return false;
        }

        public List<Option> MissingOptions()
        {
            return Options;
        }

        public bool ValidateOption(string name, string param)
        {

            return true;
        }
    }
}
