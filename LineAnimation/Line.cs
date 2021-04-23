namespace LineAnimation
{
    public class Line
    {
        public int ID { get; set; }
        public int ContourID { get; set; }
        public string Name { get; set; }
        public Point A { get; set; }
        public Point B { get; set; }
        public string Tag { get; set; }
        public DrawType DrawType { get; set; }
        public bool Visibility { get; set; }
        public double Length { get; set; }
    }

    public enum DrawType
    {
        Travel,
        Work
    }
}