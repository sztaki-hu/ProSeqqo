using ProSeqqoLib.Model.Hierarchy;

namespace LineAnimation
{
    public class Line
    {
        public Point A { get; set; }
        public Point B { get; set; }
        public string Tag { get; set; }
        public DrawType DrawType { get; set; }
        public bool Visibility { get; set; }

        public Line(Motion m)
        {
            A = new Point(m.FirstConfig);
            B = new Point(m.LastConfig);
            Tag = m.ID+" "+m.Name;
            DrawType = DrawType.Work;
        }

        public Line(Motion from, Motion to)
        {
            A = new Point(from.LastConfig);
            B = new Point(to.FirstConfig);
            //Tag = m.ID + " " + m.Name;
            DrawType = DrawType.Travel;
        }
    }

    public enum DrawType
    {
        Travel,
        Work
    }
}