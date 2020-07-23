using System;
using System.Collections.Generic;
using System.Text;

namespace SequencePlanner.GTSP
{
    public class Position : NodeBase
    {
        private static int maxPositionID = 0;
        public static int Dimension { get; set; }
        public Task Task { get; set; }
        public Process Process { get; set; }
        public Alternative Alternative { get; set; }
        public List<double> Configuration { get; set; }

        public Position(int UserID = -1) : base()
        {
            ID = maxPositionID++;
            UID = UserID;
            Name = "Position_" + UID;
            Configuration = new List<double>();
            Dimension = 0;
        }

        public Position( int UserID, List<double> config = null, string name = null) : this(UserID)
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
            if (name != null)
                Name = name;
        }

        public Position(int id, string name, List<double> config = null, Alternative alternative = null, Process process = null) : this(id, config, name)
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
                return "[" + UID + "]" + "[ID:" + ID + "]" + Name + " Proc: " + Process.Name + " Alter: " + Alternative.Name + " Task: " + Task.Name + " Config: [" + tmp + "]";
            else
                return "[" + UID + "]" + "[ID:" + ID + "]" + Name + " Virtual!";
        }

        public string ConfigString()
        {
            String tmp = "[";
            for (int i = 0; i < Configuration.Count-1; i++)
            {
                tmp += Configuration[i].ToString("F4") + "; ";
            }
            if (Configuration.Count>0)
                tmp += Configuration[Configuration.Count - 1].ToString("F4");
            tmp += "]";
            return tmp;
        }

        public static void InitMaxID()
        {
            maxPositionID = 0;
        }
    }
}
