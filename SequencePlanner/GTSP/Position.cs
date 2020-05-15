using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class Position: NodeBase
    {
        public static int Dimension { get; set; }
        public Task Task { get; set; }
        public Alternative Alternative { get; set; }
        public Process Process { get; set; }
        public List<double> Configuration { get; set; }
        
        public Position() : base()
        {
            Name = "Position_" + ID;
            Configuration = new List<double>();
            Configuration.Add(0);
            Configuration.Add(0);
            Configuration.Add(0);
        }

        public Position(String name, int id, List<double> config = null):base(name, id)
        {

            if (config == null)
            {
                Configuration = new List<double>();
                for (int i = 0; i < Dimension; i++)
                {
                    Configuration.Add(0);
                }
            }
            else
            {
                Configuration = config;
            }

        }

        public Position(String name, int id, List<double> config = null, Alternative alternative = null, Process process=null) : this(name, id, config)
        {
            Alternative = alternative;
            Process = process;
        }

        public override string ToString()
        {
            String tmp = "";
            foreach (var item in Configuration)
            {
                tmp += item + ", ";
            }
            if (!Virtual)
                return "[" + ID + "]" + Name + " Proc: " + Process.Name + " Alter: " + Alternative.Name + " Config: "+tmp;
            else
                return "[" + ID + "]" + Name + " Virtual!";
        }
    }
}
