using System;
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
            Options.Add(new TaskType()
            {
                Name = "TaskType",
                IncludeableNames = new List<string> { "ProcessHierarchy", "ProcessPrecedence", "PositionPrecedence", "Line", "Line Precedence", "Contour Precedence", "ContourPenalty" },
                Need = true
            });

            Options.Add(new EdgeWeightSource()
            {
                Name = "EdgeWeightSource",
                IncludeableNames = new List<string> { "PositionList", "DistanceFunction", "PositionNumber" },
                Need = true
            });

            Options.Add(new Dimension()
            {
                Name = "Dimensions",
                IncludeableNames = new List<string> { },
                Need = true
            });

            Options.Add(new TimeLimit()
            {
                Name = "TimeLimit",
                IncludeableNames = new List<string> { },
                Need = true
            });

            Options.Add(new CyclicSequence()
            {
                Name = "CyclicSequence",
                IncludeableNames = new List<string> { },
                Need = true
            });

            Options.Add(new WeightMultiplier()
            {
                Name = "WeightMultiplier",
                IncludeableNames = new List<string> { },
                Need = true
            });

            Options.Add(new DistanceFunction()
            {
                Name = "DistanceFunction",
                IncludeableNames = new List<string> { "TrapezoidParams/Acceleration", "TrapezoidParams/Speed" },
                Need = false
            });

            Options.Add(new TrapezoidParamsAcceleration() { Name = "TrapezoidParams/Acceleration" });
            Options.Add(new TrapezoidParamsSpeed() { Name = "TrapezoidParams/Speed" });
            Options.Add(new StartDepot() { Name = "StartDepot" });
            Options.Add(new FinishDepot() { Name = "FinishDepot" });
            Options.Add(new ProcessHierarchy() { Name = "ProcessHierarchy" });
            Options.Add(new ProcessPrecedence() { Name = "ProcessPrecedence" });
            Options.Add(new PositionPrecedence() { Name = "PositionPrecedence" });
            Options.Add(new LineList() { Name = "LineList" });
            Options.Add(new LinePrecedence() { Name = "LinePrecedence" });
            Options.Add(new ContourPrecedence() { Name = "ContourPrecedence" });
            Options.Add(new PositionList() { Name = "PositionList" });
            Options.Add(new PositionNumber() { Name = "PositionNumber" });
            Options.Add(new PositionMatrix() { Name = "PositionMatrix" });
            Options.Add(new ContourPenalty() { Name = "ContourPenalty" });

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

            bool find = false;
            for (int i = 0; i < tmp.Count; i++)
            {
                if (tmp[i].Contains("**NewParam**"))
                {
                    i++;
                    var opt = findOption(tmp[i]);
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
                        addIncluded(result.NewInclude);
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
                                included += addIncluded(result.NewInclude);
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

        public Option findOption(string Name)
        {
            foreach (var item in Options)
            {
                if (item.Name.Contains(Name))
                    return item;
            }
            return null;
        }

        private List<Option> getOptionsByNames(List<string> options)
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

        private int addIncluded(List<string> optionNames)
        {
            List<Option> tmp = getOptionsByNames(optionNames);
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