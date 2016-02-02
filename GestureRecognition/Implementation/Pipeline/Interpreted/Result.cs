using System;
using System.Collections.Generic;
using GestureRecognition.Implementation.Pipeline.Interpreted.Template;
using Trame;

namespace GestureRecognition.Implementation.Pipeline.Interpreted
{
    /// <summary>
    ///     Holds a recognition result.
    /// </summary>
    public class Result : IComparable<Result>
    {
        /// <summary>
        /// The probability associated with this recognition result.
        /// </summary>
        public double prob;

        /// <summary>
        /// The point sequences associated with this recognition result.
        /// </summary>
        public IDictionary<JointType, InputVector> pts;

        /// <summary>
        /// The template associated with this recognition result.
        /// </summary>
        public ITemplate template;

        /// <summary>
        /// Instantiates a new result.
        /// </summary>
        /// <param name="template">The Template associated with this result.</param>
        /// <param name="prob">The probability that this result is correct.</param>
        /// <param name="pts">The directory of lists of points associated with this result.</param>
        public Result(ITemplate template, double prob, IDictionary<JointType, InputVector> pts)
        {
            this.template = template;
            this.prob = prob;
            this.pts = pts;
        }

        /// <summary>
        ///     Compares two results based on their probabilities.
        /// </summary>
        /// <param name="r">The result this instnace will be compared to</param>
        /// <returns>
        ///     Returns negative number if r is more likely than this instance, 0 if likelyhood is the same, positive number
        ///     otherwise
        /// </returns>
        public int CompareTo(Result r)
        {
            if (prob.Equals(r.prob))
            {
                return 0;
            }
            if (prob < r.prob)
            {
                return 1;
            }
            return -1;
        }

        public override string ToString()
        {
            return template.Id + ": " + prob;
        }
    }
}
