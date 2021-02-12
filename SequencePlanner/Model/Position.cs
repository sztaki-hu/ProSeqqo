namespace SequencePlanner.Model
{
    public class Position: BaseNode
    {
        public double[] Vector { get; set; }
        public int Dimension { get { return Vector.Length; } }
        public Position():base()
        {
            Name = UserID + "_Position_" + GlobalID;
            Vector = new double[]{ };
        }
    }
}
