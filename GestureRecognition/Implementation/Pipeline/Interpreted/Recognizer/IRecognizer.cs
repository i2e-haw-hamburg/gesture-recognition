using System.Collections.Generic;
using GestureRecognition.Implementation.Pipeline.Interpreted.Template;
using Trame;

namespace GestureRecognition.Implementation.Pipeline.Interpreted
{
    /// <summary>
    /// I recognizer.
    /// </summary>
    public interface IRecognizer
    {
        /// <summary>
        /// Initialize the specified templates.
        /// </summary>
        /// <param name="templates">Templates.</param>
        void Initialize(IEnumerable<ITemplate> templates);

        /// <summary>
        /// Recognize the specified input.
        /// </summary>
        /// <param name="input">Input.</param>
        IEnumerable<Result> Recognize(IDictionary<JointType, InputVector> input);
    }
}
