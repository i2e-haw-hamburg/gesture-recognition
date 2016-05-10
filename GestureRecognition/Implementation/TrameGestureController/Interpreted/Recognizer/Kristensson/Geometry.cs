using System;
using System.Collections.Generic;
using GestureRecognition.Implementation.Pipeline.Interpreted.Template;
using TrameSkeleton.Math;

namespace GestureRecognition.Implementation.Pipeline.Interpreted.Kristensson
{
    static class Geometry
    {
        private static readonly Rect NormalizedSpace = new Rect(0, 0, 1000, 1000);
        public static int SpatialLength(int[] pat, int n)
        {
            int l;
            int i, m;
            int x1, y1, x2, y2;

            l = 0;
            m = 2 * n;
            if (m > 2)
            {
                x1 = pat[0];
                y1 = pat[1];
                for (i = 2; i < m; i += 2)
                {
                    x2 = pat[i];
                    y2 = pat[i + 1];
                    l += Distance.Between(x1, y1, x2, y2);
                    x1 = x2;
                    y1 = y2;
                }
                return l;
            }
            return 0;
        }


        public static double SpatialLength(IList<Vector3> pts)
        {
            var len = 0.0d;
            var i = pts.GetEnumerator();

            if (i.MoveNext())
            {
                var p0 = i.Current;
                while (i.MoveNext())
                {
                    var p1 = i.Current;
                    len += Distance.Between(p0, p1);
                    p0 = p1;
                }
            }
            return len;
        }


        public static void Normalize(IList<Vector3> pts)
        {
            ScaleTo(pts, NormalizedSpace);
            var c = Centroid(pts);
            Translate(pts, -c.X, -c.Y);
        }

        public static void ScaleTo(IList<Vector3> pts, Rect targetBounds)
        {
            var bounds = BoundingBox(pts);
            double a1 = targetBounds.Width;
            double a2 = targetBounds.Height;
            double b1 = bounds.Width;
            double b2 = bounds.Height;
            double scale = Math.Sqrt(a1 * a1 + a2 * a2) / Math.Sqrt(b1 * b1 + b2 * b2);
            if (double.IsInfinity(scale))
            {
                scale = 0;
            }
            Scale(pts, scale, scale, bounds.X, bounds.Y);
        }

        public static void Scale(IList<Vector3> pts, double sx, double sy, double originX, double originY)
        {
            Translate(pts, -originX, -originY);
            Scale(pts, sx, sy);
            Translate(pts, originX, originY);
        }

        public static void Scale(IList<Vector3> pts, double sx, double sy)
        {
            for (int i = 0; i < pts.Count; i++)
            {
                pts[i] = new Vector3((float)(pts[i].X * sx), (float)(pts[i].Y * sy), 0);
            }
        }
        
        /// <summary>
        ///     Normalizes a point sequence so that it is scaled and centred within a defined box.
        /// </summary>
        /// <param name="pts">an input point sequence.</param>
        /// <param name="x">the horizontal component of the upper-left corner of the defined box.</param>
        /// <param name="y">the vertical component of the upper-left corner of the defined box.</param>
        /// <param name="width">the width of the defined box.</param>
        /// <param name="height">the height of the defined box</param>
        /// <returns>a newly created point sequence that is centred and fits within the defined box.</returns>
        public static IList<Vector3> Normalize(IList<Vector3> pts, int x, int y, int width, int height)
        {
            var outPts = Helper.DeepCopyPoints(pts);
            Geometry.ScaleTo(outPts, new Rect(0, 0, width - x, height - y));
            var c = Centroid(outPts);
            Translate(outPts, -c.X, -c.Y);
            Translate(outPts, width - x, height - y);
            return outPts;
        }
        
        public static void Translate(IList<Vector3> pts, double dx, double dy)
        {
            for (int i = 0; i < pts.Count; i++)
            {
                pts[i] = new Vector3(pts[i].X + (int)Math.Floor(dx), pts[i].Y + (int)Math.Floor(dy), 0);
            }
        }

        public static Rect BoundingBox(IList<Vector3> pts)
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;
            foreach (var pt in pts)
            {
                var x = (int)pt.X;
                var y = (int)pt.Y;
                if (x < minX)
                {
                    minX = x;
                }
                if (x > maxX)
                {
                    maxX = x;
                }
                if (y < minY)
                {
                    minY = y;
                }
                if (y > maxY)
                {
                    maxY = y;
                }
            }
            return new Rect(minX, minY, (maxX - minX), (maxY - minY));
        }

        public static Centroid Centroid(IList<Vector3> pts)
        {
            var totalMass = pts.Count;
            var xIntegral = 0.0;
            var yIntegral = 0.0;
            foreach (var pt in pts)
            {
                xIntegral += pt.X;
                yIntegral += pt.Y;
            }
            return new Centroid(xIntegral / totalMass, yIntegral / totalMass);
        }

        public static double LikelihoodOfMatch(IList<Vector3> pts1, IList<Vector3> pts2, double eSigma, double aSigma, double lambda)
        {
            if (eSigma == 0 || eSigma < 0)
            {
                throw new ArgumentException("eSigma must be positive");
            }
            if (aSigma == 0 || eSigma < 0)
            {
                throw new ArgumentException("aSigma must be positive");
            }
            if (lambda < 0 || lambda > 1)
            {
                throw new ArgumentException("lambda must be in the range between zero and one");
            }
            double x_e = Distance.Euclidean(pts1, pts2);
            double x_a = Distance.TurningAngle(pts1, pts2);
            return Math.Exp(-(x_e * x_e / (eSigma * eSigma) * lambda + x_a * x_a / (aSigma * aSigma) * (1 - lambda)));
        }
    }
}