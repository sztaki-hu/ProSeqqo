﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.Phraser.Options
{
    public class OptionSet
    {
        public List<Option> Options;
        public List<Option> Need;
        public List<Option> Included;
        public List<Option> Validated;

        public OptionSet()
        {
            Options = new List<Option>();
            Need = new List<Option>();
            Included = new List<Option>();
            Validated = new List<Option>();
            Init();
        }

        public void Init()
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
            Options.Add(new PositionNumber());
            Options.Add(new PositionMatrix());
            Options.Add(new ContourPenalty());

            foreach (var item in Options)
            {
                if (item.Need)
                    Need.Add(item);
            }
        }

        public void FillValues(List<string> lines)
        {
            List<string> tmp = new List<string>();

            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < Options.Count; j++)
                {
                    if (lines[i].Contains(Options[j].Name))
                    {
                        tmp.Add("**NewParam**");
                    }
                }
                tmp.Add(lines[i]);
            }

            for (int i = 0; i < tmp.Count; i++)
            {
                if (tmp[i].Contains("**NewParam**"))
                {
                    i++;
                    var opt = FindOption(tmp[i]);
                    if (opt != null && i < tmp.Count)
                    {
                        while ((tmp.Count > i) && !tmp[i].Contains("**NewParam**"))
                        {
                            opt.ValueString.Add(tmp[i]);
                            i++;
                        }
                        i--;
                    }
                }
            }
        }

        public void Validate()
        {
            if (SequencerTask.DEBUG)
                Console.WriteLine("Need:");
            foreach (var option in Need)
            {
                var result = option.Validate();
                if (result != null)
                {
                    if (!result.Validated) {
                        if (SequencerTask.DEBUG)
                            Console.WriteLine("Error in validation: " + option.Name);
                        return;
                    }else
                    {
                        if (SequencerTask.DEBUG)
                            Console.WriteLine("Validated: " + option.Name + "!");
                        AddIncluded(result.NewInclude);
                    }
                }
            }
            if (SequencerTask.DEBUG)
                Console.WriteLine("Included:");
            int included = -1;
            while (included != 0)
            {
                included = 0;
                for (int i = 0; i < Included.Count; i++)
                {
                    if (!Included[i].Validated)
                    {
                        var result = Included[i].Validate();
                        if (result != null)
                        {
                            if (!result.Validated)
                                if (SequencerTask.DEBUG)
                                    Console.WriteLine("Error in validation: " + Included[i].Name);
                            else
                            {
                                if (SequencerTask.DEBUG)
                                        Console.WriteLine("Validated: " + Included[i].Name + "!");
                                Included[i].Included = true;
                                included += AddIncluded(result.NewInclude);
                            }
                        }
                    }
                    
                }


                if (included == -1)
                    included = 0;
            }
            if (SequencerTask.DEBUG)
                Console.WriteLine("Others:");
            foreach (var item in Options)
            {
                if (!item.Validated)
                {
                    var ret = item.Validate();
                    if (ret != null)
                    {
                        if (SequencerTask.DEBUG)
                        {
                            if (ret.Validated)
                                Console.WriteLine("Validated but not needed: " + item.Name + "!");
                            else
                                Console.WriteLine("Not validated but not needed: " + item.Name + "!");
                        }
                    }
                }
            }

            foreach (var option in Options)
            {
                if(option.Included || option.Need)
                {
                    if (!option.Validated)
                    {
                        Console.WriteLine(option.Name+" is missing or in wrong format!");
                    }
                }
            }

        }

        public Option FindOption(string Name)
        {
            foreach (var item in Options)
            {
                if (item.Name.Contains(Name))
                    return item;
            }
            return null;
        }

        private List<Option> GetOptionsByNames(List<string> options)
        {
            List<Option> ret = new List<Option>();
            foreach (var option in options)
            {
                foreach (var opt in Options)
                {
                    if (option.Equals(opt.Name))
                        ret.Add(opt);
                }
            }
            return ret;
        }

        private int AddIncluded(List<string> optionNames)
        {
            List<Option> tmp = GetOptionsByNames(optionNames);
            int included = 0;
            foreach (var item in tmp)
            {
                bool find = false;
                for (int i = 0; i < Included.Count; i++)
                {
                    if (Included[i].Name == item.Name)
                        find = true;
                }
                if (!find)
                {
                    Included.Add(item);
                    item.Included = true;
                    included++;
                }
            }
            return included;
        }

        public override string ToString()
        {
            string tmp = "";
            foreach (var item in Options)
            {
                tmp += item.ToString() + "\n";
            }
            return tmp;
        }
    }
}