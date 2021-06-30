using ProSeqqoLib.Model.Hierarchy;
using System.Windows.Media.Media3D;

namespace LineAnimation
{
    public class Point
    {
        public string Tag { get; set; }        
        public Point3D Config { get; set; }

        public Point(Point3D point, string tag = "")
        {
            Tag = tag;
            Config = point;
        }

        public Point(Config config)
        {
            Point3D point = new Point3D();
            if (config.Configuration.Count > 0)
                point.X = config.Configuration[0];
            if (config.Configuration.Count > 1)
                point.Y = config.Configuration[1];
            if (config.Configuration.Count > 2)
                point.Z = config.Configuration[2];
            Config = point;
            Tag = config.ToString(); 
        }
    }
}
