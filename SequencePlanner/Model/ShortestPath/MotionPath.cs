using SequencePlanner.Model.Hierarchy;
using System.Collections.Generic;

namespace SequencePlanner.Model.ShortestPath
{
    public class MotionPath
    {
        private static int ProxyMotionMaxID = 1000000;
        public Motion Start { get; private set; }
        public Motion Finish { get; private set; }
        public List<Motion> Path { get; set; }
        public double Cost { get; set; }
        public Motion ProxyMotion { get; set; }
    
        public MotionPath(Motion start, Motion finish)
        {
            Start = start;
            Finish = finish;
            Path = new List<Motion>();
            Path.Add(start);
            Path.Add(finish);
        }

        public void AddMotion(Motion m)
        {
            if (m.GlobalID != Start.GlobalID && m.GlobalID != Finish.GlobalID)
                Path.Insert(Path.Count - 1, m);
            //else
                //throw new Exception("The path already contain start and finish.");
        }

        public void CreateProxyMotion()
        {
            ProxyMotion = new Motion(ProxyMotionMaxID++, Start.FirstConfig, Finish.LastConfig);
            ProxyMotion.Name = "ShortcutProxyMotion " + Start.ID+Start.Name + "-" + Finish.ID+Finish.Name;
            //ProxyMotion.OverrideCostIn = new OverrideCost() { Cost = Cost };
            ProxyMotion.ShortcutCost = Cost;
            ProxyMotion.isShortcut = true;
        }

        public override string ToString()
        {
            string path = " Path: ";
            foreach (var item in Path)
            {
                path += "[" + item.ID + "] " + item.Name;
            }
            return "From: [" + Start.ID + "] " + Start.Name + " To: [" + Finish.ID + "] " + Finish.Name + path + "Cost: " + Cost;
        }
    }
}