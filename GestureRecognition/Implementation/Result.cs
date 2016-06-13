using System;
using System.Collections.Generic;
using GestureRecognition.Implementation.Pipeline.Interpreted.Template;
using GestureRecognition.Interface.Commands;
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
        public double Probability;
        
        /// <summary>
        /// The template associated with this recognition result.
        /// </summary>
        public AInterpretedCommand Command;

        /// <summary>
        /// Instantiates a new result.
        /// </summary>
        /// <param name="template">The Template associated with this result.</param>
        /// <param name="probability">The probability that this result is correct.</param>
        public Result(ITemplate template, double probability) : this(template.Command, probability)
        {}

        public Result(AInterpretedCommand cmd, double probability)
        {
            Command = cmd;
            this.Probability = probability;
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
            if (Probability.Equals(r.Probability))
            {
                return 0;
            }
            if (Probability < r.Probability)
            {
                return 1;
            }
            return -1;
        }

        public override string ToString()
        {
            return Command.Id + ": " + Probability;
        }
    }
}
