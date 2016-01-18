namespace GestureRecognition.Implementation.Pipeline.Interpreted.Kristensson
{
    internal class Centroid
    {
        public Centroid(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double X { get; set; }

        public double Y { get; set; }
    }
}