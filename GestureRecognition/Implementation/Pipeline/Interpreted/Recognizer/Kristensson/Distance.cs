using System;
using System.Collections.Generic;
using TrameSkeleton.Math;

namespace GestureRecognition.Implementation.Pipeline.Interpreted.Kristensson
{
    public static class Distance
    {
        public static double Between(Vector3 p1, Vector3 p2)
        {
            return Between((int)p1.X, (int)p1.Y, (int)p2.X, (int)p2.Y);
        }

        public static int Between(int x1, int y1, int x2, int y2)
        {
            if ((x2 -= x1) < 0)
            {
                x2 = -x2;
            }
            if ((y2 -= y1) < 0)
            {
                y2 = -y2;
            }
            return (x2 + y2 - (((x2 > y2) ? y2 : x2) >> 1));
        }
        public static double Euclidean(IList<Vector3> pts1, IList<Vector3> pts2)
        {
            if (pts1.Count != pts2.Count)
            {
                throw new ArgumentException("lists must be of equal lengths, cf. " + pts1.Count + " with " + pts2.Count);
            }
            int n = pts1.Count;
            double td = 0;
            for (int i = 0; i < n; i++)
            {
                td += Euclidean(pts1[i], pts2[i]);
            }
            return td / n;
        }

        public static double TurningAngle(IList<Vector3> pts1, IList<Vector3> pts2)
        {
            if (pts1.Count != pts2.Count)
            {
                throw new ArgumentException("lists must be of equal lengths, cf. " + pts1.Count + " with " + pts2.Count);
            }
            int n = pts1.Count;
            double td = 0;
            for (int i = 0; i < n - 1; i++)
            {
                td += Math.Abs(TurningAngle(pts1[i], pts1[i + 1], pts2[i], pts2[i + 1]));
            }
            if (double.IsNaN(td))
            {
                return 0.0;
            }
            return td / (n - 1);
        }

        public static double Euclidean(Vector3 pt1, Vector3 pt2)
        {
            return Math.Sqrt(SquaredEuclidean(pt1, pt2));
        }

        public static double SquaredEuclidean(Vector3 pt1, Vector3 pt2)
        {
            return (pt1.X - pt2.X) * (pt1.X - pt2.X) + (pt1.Y - pt2.Y) * (pt1.Y - pt2.Y);
        }

        public static double TurningAngle(Vector3 ptA1, Vector3 ptA2, Vector3 ptB1, Vector3 ptB2)
        {
            var len_a = Euclidean(ptA1, ptA2);
            var len_b = Euclidean(ptB1, ptB2);
            if (len_a == 0 || len_b == 0)
            {
                return 0.0;
            }
            var cos =
                (float)(((ptA1.X - ptA2.X) * (ptB1.X - ptB2.X) + (ptA1.Y - ptA2.Y) * (ptB1.Y - ptB2.Y)) / (len_a * len_b));
            if (Math.Abs(cos) > 1.0)
            {
                return 0.0;
            }
            return Math.Acos(cos);
        }
    }
}