using System.Collections.Generic;
using System.Linq;
using TrameSkeleton.Math;

namespace GestureRecognition.Implementation.Pipeline.Interpreted.Kristensson
{
    public static class Helper
    {
        private const int MaxResamplingPts = 1000;

        public static IList<Vector3> DeepCopyPoints(IList<Vector3> pts)
        {
            return pts.Select(pt => new Vector3(pt.X, pt.Y, 0)).ToList();
        }

        /// <summary>
        /// Generate the equidistant progressive subsequences
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="ptSpacing"></param>
        /// <returns></returns>
        public static IList<IList<Vector3>> EqudisistantProgressiveSubSequences(IList<Vector3> pts, int ptSpacing)
        {
            var sequences = new List<IList<Vector3>>();
            var nSamplePoints = ResamplingPointCount(pts, ptSpacing);
            var resampledPts = Resample(pts, nSamplePoints);
            for (int i = 1, n = resampledPts.Count; i < n; i++)
            {
                var seq = DeepCopyPoints(resampledPts.GetRange(0, i + 1));
                sequences.Add(seq);
            }
            return sequences;
        }
        
        public static int ResamplingPointCount(IList<Vector3> pts, int samplePointDistance)
        {
            var len = Geometry.SpatialLength(pts);
            return (int)(len / samplePointDistance) + 1;
        }

        public static void Resample(int[] template, int[] buffer, int n, int numTargetPoints)
        {
            var segment_buf = new int[MaxResamplingPts];

            double l, segmentLen, horizRest, verticRest, dx, dy;
            int x1, y1, x2, y2;
            int i, m, a, segmentPoints, j, maxOutputs, end;

            m = n * 2;
            l = Geometry.SpatialLength(template, n);
            segmentLen = l / (numTargetPoints - 1);
            SegmentPoints(template, n, segmentLen, segment_buf);
            horizRest = 0.0f;
            verticRest = 0.0f;
            x1 = template[0];
            y1 = template[1];
            a = 0;
            maxOutputs = numTargetPoints * 2;
            for (i = 2; i < m; i += 2)
            {
                x2 = template[i];
                y2 = template[i + 1];
                segmentPoints = segment_buf[(i / 2) - 1];
                dx = -1.0f;
                dy = -1.0f;
                if (segmentPoints - 1 <= 0)
                {
                    dx = 0.0f;
                    dy = 0.0f;
                }
                else
                {
                    dx = (x2 - x1) / (double)(segmentPoints);
                    dy = (y2 - y1) / (double)(segmentPoints);
                }
                if (segmentPoints > 0)
                {
                    for (j = 0; j < segmentPoints; j++)
                    {
                        if (j == 0)
                        {
                            if (a < maxOutputs)
                            {
                                buffer[a] = (int)(x1 + horizRest);
                                buffer[a + 1] = (int)(y1 + verticRest);
                                horizRest = 0.0;
                                verticRest = 0.0;
                                a += 2;
                            }
                        }
                        else
                        {
                            if (a < maxOutputs)
                            {
                                buffer[a] = (int)(x1 + j * dx);
                                buffer[a + 1] = (int)(y1 + j * dy);
                                a += 2;
                            }
                        }
                    }
                }
                x1 = x2;
                y1 = y2;
            }
            end = (numTargetPoints * 2) - 2;
            if (a < end)
            {
                for (i = a; i < end; i += 2)
                {
                    if (a > 0 && m > 0)
                    {
                        buffer[i + 1] = (buffer[i - 1] + template[m - 1]) / 2;
                    }
                }
            }
            buffer[maxOutputs - 2] = template[m - 2];
            buffer[maxOutputs - 1] = template[m - 1];
        }


        public static double SegmentPoints(int[] pts, int n, double length, int[] buffer)
        {
            int i, m;
            int x1, y1, x2, y2, ps;
            double rest, currentLen;

            m = n * 2;
            rest = 0.0f;
            x1 = pts[0];
            y1 = pts[1];
            for (i = 2; i < m; i += 2)
            {
                x2 = pts[i];
                y2 = pts[i + 1];
                currentLen = Distance.Between(x1, y1, x2, y2);
                currentLen += rest;
                rest = 0.0f;
                ps = (int)((currentLen / length));
                if (ps == 0)
                {
                    rest += currentLen;
                }
                else
                {
                    rest += currentLen - (ps * length);
                }
                if (i == 2 && ps == 0)
                {
                    ps = 1;
                }
                buffer[(i / 2) - 1] = ps;
                x1 = x2;
                y1 = y2;
            }
            return rest;
        }


        public static List<Vector3> Resample(IList<Vector3> points, int numTargetPoints)
        {
            var r = new List<Vector3>();
            var inArray = ToArray(points);
            var outArray = new int[numTargetPoints * 2];

            Resample(inArray, outArray, points.Count, numTargetPoints);
            for (int i = 0, n = outArray.Length; i < n; i += 2)
            {
                r.Add(new Vector3(outArray[i], outArray[i + 1], 0));
            }
            return r;
        }
        
        public static int[] ToArray(IList<Vector3> points)
        {
            var @out = new int[points.Count * 2];
            for (int i = 0, n = points.Count * 2; i < n; i += 2)
            {
                @out[i] = (int)points[i / 2].X;
                @out[i + 1] = (int)points[i / 2].Y;
            }
            return @out;
        }
    }
}