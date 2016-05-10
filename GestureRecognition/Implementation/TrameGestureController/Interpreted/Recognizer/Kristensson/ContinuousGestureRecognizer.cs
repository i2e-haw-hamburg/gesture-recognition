using System;
using System.Collections.Generic;
using System.Linq;
using GestureRecognition.Implementation.Pipeline.Interpreted.Template;
using Trame;
using TrameSkeleton.Math;

namespace GestureRecognition.Implementation.Pipeline.Interpreted.Kristensson
{
    /**
    * A continuous gesture recognizer. Outputs a probability distribution over
    * a set of template gestures as a function of received sampling points.
    * 
    * History:
    * Version 1.0 (August 12, 2011)   - Initial public release
    * Version 2.0 (September 6, 2011) - Simplified the public interface, simplified
    *                                   internal implementation.
    * 
    * For details of its operation, see the paper referenced below.
    * 
    * Documentation is here: http://pokristensson.com/increc.html
    * 
    * Copyright (C) 2011 Per Ola Kristensson, University of St Andrews, UK.
    * 
    * If you use this code for your research then please remember to cite our paper:
    * 
    * Kristensson, P.O. and Denby, L.C. 2011. Continuous recognition and visualization
    * of pen strokes and touch-screen gestures. In Procceedings of the 8th Eurographics
    * Symposium on Sketch-Based Interfaces and Modeling (SBIM 2011). ACM Press: 95-102.
    * 
    * @author Per Ola Kristensson
    * @author Leif Denby
    *
    */
    class ContinuousGestureRecognizer : IRecognizer
    {
        /* Beginning of public interface */

        /// <summary>
        ///     Default value for sigma.
        /// </summary>
        public const double DefaultESigma = 200.0;

        /// <summary>
        ///     default value for beta.
        /// </summary>
        public const double DefaultBeta = 400.0;

        /// <summary>
        ///     Default value for lambda.
        ///     mixture weight that controls the relative contribution for both similarity measures (euclid. dist. and turning
        ///     angle) in distance calculation.
        /// </summary>
        public const double DefaultLambda = 0.4;

        /// <summary>
        ///     end point biased parameter
        /// </summary>
        public const double DefaultKappa = 1.0;

        private readonly IList<Pattern> _patterns = new List<Pattern>();
        /// <summary>
        /// the distance between sampling points in the normalized space(1000 x 1000 units)
        /// </summary>
        private int _samplePointDistance;


        /// <summary>
        /// Initializes an instance of a continuous gesture recognizer.
        /// </summary>
        /// <param name="templates">the set of templates the recognizer will recognize</param>
        public void Initialize(IEnumerable<ITemplate> templates)
        {
            SetTemplateSet(templates);
        }

        public IEnumerable<Result> Recognize(IDictionary<JointType, InputVector> input)
        {
            return Recognize(input[JointType.HAND_RIGHT].Stream.ToList());
        }
        
        /// <summary>
        ///     Sets the set of templates this recognizer will recognize.
        /// </summary>
        /// <param name="templates">the set of templates this recognizer will recognize</param>
        public void SetTemplateSet(IEnumerable<ITemplate> templates)
        {
            _patterns.Clear();
            foreach (var t in templates)
            {
                var points = t.GetSequence(JointType.HAND_RIGHT).Points;
                Geometry.Normalize(points);
                _patterns.Add(new Pattern(t, Helper.EqudisistantProgressiveSubSequences(t.GetSequence(JointType.HAND_RIGHT).Points, 200)));
            }
           
        }
        
        /// <summary>
        ///     Outputs a list of templates and their associated probabilities for the given input.
        ///     REFERENCE here
        /// </summary>
        /// <param name="input">a list of input points.</param>
        /// <param name="beta">a parameter, see the paper for details.</param>
        /// <param name="lambda">a parameter, see the paper for details.</param>
        /// <param name="kappa">a parameter, see the paper for details.</param>
        /// <param name="e_sigma">a parameter, see the paper for details.</param>
        /// <returns>a list of templates and their associated probabilities.</returns>
        public IList<Result> Recognize(IList<Vector3> input, double beta = DefaultBeta, double lambda = DefaultLambda, double kappa = DefaultKappa, double e_sigma = DefaultESigma)
        {
            var uniquePoints = RemoveContiguousEqualPoints(input);

            if (input.Count < 2)
            {
                return new List<Result>();
            }

            var incResults = IncrementalResults(uniquePoints, beta, lambda, kappa, e_sigma);
            return GetResults(incResults);
        }

        private IList<Vector3> RemoveContiguousEqualPoints(IList<Vector3> input)
        {
            var newPoints = new List<Vector3>(input.Count);
            foreach (Vector3 p in input)
            {
                if (newPoints.Count < 1 || !p.Equals(newPoints.Last()))
                {
                    newPoints.Add(p);
                }
            }
            return newPoints;
        }

        private IList<Result> GetResults(IList<IncrementalResult> incrementalResults)
        {
            return incrementalResults
                .Where(ir => ir.IndexOfMostLikelySegment >= 0)
                .Select(ir => new Result(ir.Pattern.Template, ir.Prob, new Dictionary<JointType, InputVector>
                {
                    {JointType.HAND_RIGHT, new InputVector(ir.Pattern.Segments[ir.IndexOfMostLikelySegment])}
                }))
                .ToList();
        }

        public List<IncrementalResult> IncrementalResults(IList<Vector3> input, double beta, double lambda, double kappa, double e_sigma)
        {
            var results = new List<IncrementalResult>();
            var unkPts = Helper.DeepCopyPoints(input);
            Geometry.Normalize(unkPts);
            foreach (var pattern in _patterns)
            {
                var result = IncrementalResult.IncResult(unkPts, pattern, beta, lambda, e_sigma);
                var lastSegmentPts = pattern.Segments[pattern.Segments.Count - 1];
                var completeProb = Geometry.LikelihoodOfMatch(Helper.Resample(unkPts, lastSegmentPts.Count), lastSegmentPts, e_sigma, e_sigma / beta, lambda);
                var x = 1 - completeProb;
                result.Prob *= (1 + kappa * Math.Exp(-x * x));
                results.Add(result);
            }
            return results;
        }
    }
}