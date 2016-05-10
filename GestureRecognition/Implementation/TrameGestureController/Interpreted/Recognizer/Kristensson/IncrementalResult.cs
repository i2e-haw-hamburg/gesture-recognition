using System;
using System.Collections.Generic;
using TrameSkeleton.Math;

namespace GestureRecognition.Implementation.Pipeline.Interpreted.Kristensson
{
    class IncrementalResult
    {
        public IncrementalResult(Pattern pattern, double prob, int indexOfMostLikelySegment)
        {
            this.Pattern = pattern;
            this.Prob = prob;
            this.IndexOfMostLikelySegment = indexOfMostLikelySegment;
        }

        public Pattern Pattern { get; set; }

        public double Prob { get; set; }

        public int IndexOfMostLikelySegment { get; set; }

        public static IncrementalResult BaselineResult(IList<Vector3> unkPts, Pattern pattern, double beta,
            double lambda, double e_sigma)
        {
            var segments = pattern.Segments;

            var pts = segments[segments.Count - 1];
            var samplingPtCount = pts.Count;
            var unkResampledPts = Helper.Resample(unkPts, samplingPtCount);
            var prob = Geometry.LikelihoodOfMatch(unkResampledPts, pts, e_sigma, e_sigma / beta, lambda);

            return new IncrementalResult(pattern, prob, segments.Count - 1);
        }

        public static void MarginalizeIncrementalResults(IList<IncrementalResult> results)
        {
            var totalMass = 0.0d;
            foreach (IncrementalResult r in results)
            {
                totalMass += r.Prob;
            }
            foreach (IncrementalResult r in results)
            {
                r.Prob /= totalMass;
            }
        }

        public static IncrementalResult IncResult(IList<Vector3> unkPts, Pattern pattern, double beta,
            double lambda, double e_sigma)
        {
            var segments = pattern.Segments;
            double maxProb = 0.0d;
            int maxIndex = -1;
            for (int i = 0, n = segments.Count; i < n; i++)
            {
                var pts = segments[i];
                var samplingPtCount = pts.Count;
                var unkResampledPts = Helper.Resample(unkPts, samplingPtCount);
                var prob = Geometry.LikelihoodOfMatch(unkResampledPts, pts, e_sigma, e_sigma / beta, lambda);
                if (prob > maxProb)
                {
                    maxProb = prob;
                    maxIndex = i;
                }
            }
            return new IncrementalResult(pattern, maxProb, maxIndex);
        }

    }
}