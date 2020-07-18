using SequencePlanner.Phraser.Helper;
using SequencePlanner.Phraser.Template;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Schema;

namespace SequencePlanner.Phraser.Options
{
    public abstract class OptionSet
    {
        public List<Option> Options;
        public List<Option> Need;
        public List<Option> Included;

        public OptionSet()
        {
            Options = new List<Option>();
            Need = new List<Option>();
            Included = new List<Option>();
            Init();
        }

        public abstract void Init();
        public abstract void EtcValidationVertify();

        public void ReadFile(string file, bool validate = true)
        {
            string[] lines = File.ReadAllLines(@file);
            OptionSetPhraser phraser = new OptionSetPhraser();
            List<string> linesList = phraser.ReadFile(lines);
            FillValues(linesList);
            if (validate)
                Validate();
        }

        public void Validate()
        {
            ValidateNeeded();
            ValidateIncluded();
            ValidateOthers();
            VerifyValidations();
        }

        public void ValidateNeeded()
        {
            //Validate Needed List
            foreach (var option in Need)
            {
                var result = option.Validate();
                if (result != null)
                {
                    if (result.Validated)
                    {
                        AddNeededIncludes(result.NewIncludeNeed);
                        AddOptionalIncludes(result.NewIncludeOptional);
                    }
                }
            }
        }
        public void ValidateIncluded()
        {
            //Validate Included List
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
                            if (result.Validated)
                            {
                                Included[i].Included = true;
                                included += AddNeededIncludes(result.NewIncludeNeed);
                                included += AddOptionalIncludes(result.NewIncludeOptional);
                            }
                        }
                    }
                }
                if (included == -1)
                    included = 0;
            }
        }
        public void ValidateOthers()
        {
            //Validate Others List
            foreach (var item in Options)
            {
                if (!item.Validated)
                {
                    item.Validate();
                }
            }
        }
        public void VerifyValidations()
        {
            if (TemplateManager.DEBUG)
            {
                Console.WriteLine("Validate:");
                Console.WriteLine("\tNeeded:");
                foreach (var option in Options)
                {
                    if (option.Need && !option.Included)
                        if (option.Validated)
                            Console.WriteLine("\t\tOK: " + option.Name);
                        else
                            Console.WriteLine("\t\tFAIL: " + option.Name);
                }

                Console.WriteLine();
                Console.WriteLine("\tIncluded and not Optional:");
                foreach (var option in Options)
                {
                    if (option.Included && !option.Optional)
                        if (option.Validated)
                            Console.WriteLine("\t\tOK: " + option.Name);
                        else
                            Console.WriteLine("\t\tFAIL: " + option.Name);
                }

                Console.WriteLine();
                Console.WriteLine("\tIncluded and Optional:");
                foreach (var option in Options)
                {
                    if (option.Included && option.Optional)
                        if (option.Validated)
                            Console.WriteLine("\t\tOK: " + option.Name);
                        else
                            Console.WriteLine("\t\tFAIL: " + option.Name);
                }

                Console.WriteLine();
                Console.WriteLine("\tEtc (Not needed and not included):");
                foreach (var option in Options)
                {
                    if (!option.Included && !option.Need)
                        if (option.Validated)
                            Console.WriteLine("\t\tOK: " + option.Name);
                        else
                            Console.WriteLine("\t\tFAIL: " + option.Name);
                }
            }

            foreach (var option in Options)
            {
                if (option.Need || option.Included)
                {
                    if (!option.Validated && !option.Optional)
                    {
                        throw new SequencerException(option.Name + " option not validated (missing or format not accepted) but needed because default parameter or included by an other.", "Add option to input file or overview syntax.");
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

        private void FillValues(List<string> lines)
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

        private int AddNeededIncludes(List<string> optionNames)
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
                    item.Need = true;
                    item.Optional = false;
                    included++;
                }
            }
            return included;
        }

        private int AddOptionalIncludes(List<string> optionNames)
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
                    item.Need = false;
                    item.Optional = true;
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