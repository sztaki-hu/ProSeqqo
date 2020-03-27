using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SequencePlanner.Options
{
    public class SequencerOptions
    {
        public List<IOption> Options { get; private set; }
        public TaskType TaskType { get; private set; }
        public EdgeWeightSource EdgeWeightSource { get; private set; }
        public DistanceFunction DistanceFunction { get; private set; }

        public SequencerOptions()
        {
            TaskType = new TaskType();
            EdgeWeightSource = new EdgeWeightSource();
            DistanceFunction = new DistanceFunction();

            Options = new List<IOption>();
            Options.Add((IOption)TaskType);
            Options.Add((IOption)EdgeWeightSource);
            Options.Add((IOption)DistanceFunction);

        }

        public void ReadFile(string file)
        {

            string[] lines = File.ReadAllLines(@file);
            foreach (string line in lines)
            {
                Console.WriteLine("\t" + line);
            }
            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }

        public bool isAllOptionSatied()
        {
            return false;
        }

        public List<IOption> MissingOptions()
        {
            return Options;
        }

        public bool ValidateOption(string name, string param)
        {

            return true;
        }
    }
}
