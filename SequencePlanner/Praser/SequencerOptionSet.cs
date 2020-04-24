using SequencePlanner.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner
{
    public class SequencerOptionSet
    {
        public List<OptionProcess> processList = new List<OptionProcess>();
        public SequencerOptionSet()
        {
            LoadOptions();
        }
        
        
        public void LoadOptions(){
            processList.Add(new OptionProcess(new TaskType(), true));
            processList.Add(new OptionProcess(new EdgeWeightSource(), true));
            processList.Add(new OptionProcess(new Dimension(), true));
            processList.Add(new OptionProcess(new TimeLimit(), true));
            processList.Add(new OptionProcess(new CyclicSequence(), true));
            processList.Add(new OptionProcess(new CyclicSequenceStartDepot(), true));
            processList.Add(new OptionProcess(new CyclicSequenceFinishDepot(), true));
            processList.Add(new OptionProcess(new WeightMultiplier(), true));
            processList.Add(new OptionProcess(new DistanceFunction(), false));
            processList.Add(new OptionProcess(new TrapezoidParamsAcceleration(), false));
            processList.Add(new OptionProcess(new TrapezoidParamsSpeed(), false));

            processList.Add(new OptionProcess(new ProcessHierarchy(), false));
            processList.Add(new OptionProcess(new ProcessPrecedence(), false));
            processList.Add(new OptionProcess(new PositionPrecedence(), false));
            
            processList.Add(new OptionProcess(new Line(), false));
            processList.Add(new OptionProcess(new LinePrecedence(), false));
            processList.Add(new OptionProcess(new ContourPrecedence(), false));
            
            processList.Add(new OptionProcess(new PositionList(), false));
            processList.Add(new OptionProcess(new PositionNumber(), false));
            processList.Add(new OptionProcess(new PositionMatrix(), false));
        }

        public bool CheckValidationComplete()
        {
            foreach (var option in processList)
            {
                //If an option required or included need to be validated
                if (option.required || option.included)
                    if (!option.validated)
                        return false;
            }
            return true;
        }

        public void IncludeOption(string name)
        {
            for (int i = 0; i < processList.Count; i++)
            {
                if (processList[i].option.Name.Equals(name))
                    processList[i].Include();
            }
        }

        public void ValidateOption(string name, List<string> parameters)
        {
            for (int i = 0; i < processList.Count; i++)
            {
                if (processList[i].option.Name.Equals(name) && processList[i].option.Validate(parameters))
                    processList[i].Validate();
            }
        }
        public void Validate(List<string> file)
        {
            foreach (var process in processList)
            {

            }
        }
    }

    public struct OptionProcess
    {
        public IOption option;
        public bool required;
        public bool validated;
        public bool included;

        public OptionProcess(IOption option, bool required = false)
        {
            this.option = option;
            this.required = required;
            this.validated = false;
            this.included = false;
        }

        public void Include()
        {
            included = true;
        }

        public void Validate()
        {
            validated = true;
        }
    }
}
