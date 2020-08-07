using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace LineLikeAnimation
{
    public class Point
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Point3D Config { get; set; }
        public string Tag { get; set; }

        public Point(string id, string name, string config)
        {
            ID = int.Parse(id);
            Name = name;
            Config = PraseConfig(config);
        }

        private Point3D PraseConfig(string config)
        {
            List<double> conf = new List<double>();
            string[] c = config.Split(';');
            foreach (var item in c)
            {
                conf.Add(double.Parse(item));
            }
            if (conf.Count == 2)
                conf.Add(0);
            return new Point3D(conf[0], conf[1], conf[2]);
        }
    }
}
