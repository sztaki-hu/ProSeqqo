using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class Position : NodeBase
    {
        private static int PIDmax = 0;
        public int PID { get; set; }
        public static int Dimension { get; set; }
        public Task Task { get; set; }
        public Process Process { get; set; }
        public Alternative Alternative { get; set; }
        public List<double> Configuration { get; set; }

        public Position() : base()
        {
            PID = PIDmax++;
            Name = "Position_" + ID;
            Configuration = new List<double>();
            Configuration.Add(0);
            Configuration.Add(0);
            Configuration.Add(0);
        }

        public Position( int id, String name = null, List<double> config = null) : base(name, id)
        {
            PID = PIDmax++;
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

        public Position(int id, String name = null, List<double> config = null, Alternative alternative = null, Process process = null) : this( id,name, config)
        {
            PID = PIDmax++;
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
                return "[" + ID + "]" + "[PID:" + ID + "]" + Name + " Proc: " + Process.Name + " Alter: " + Alternative.Name + " Task: " + Task.Name + " Config: [" + tmp + "]";
            else
                return "[" + ID + "]" + "[PID:" + ID + "]" + Name + " Virtual!";
        }

        public string ConfigString()
        {
            String tmp = "[";
            for (int i = 0; i < Configuration.Count-1; i++)
            {
                tmp += Configuration[i].ToString("F4") + ", ";
            }
            tmp += Configuration[Configuration.Count - 1].ToString("F4");
            tmp += "]";
            return tmp;
        }

        public static void initMaxID()
        {
            PIDmax = 0;
        }
    }
}
