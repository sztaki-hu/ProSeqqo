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


        //DEFAULT
        public Position GetCopy()
        {
            var copy = (Position)base.GetCopy(new Position());
            copy.Vector = (double[]) Vector.Clone();
            return copy;
        }
        public override string ToString()
        {
            return base.ToString();
        }
        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Position node = (Position)obj;
                base.Equals((BaseNode)node);
                if (!node.Vector.Equals(Vector))
                    return false;
                if (node.Dimension != Dimension)
                    return false;
                return true;
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() & Vector.GetHashCode() & Dimension.GetHashCode();
        }
    }
}
