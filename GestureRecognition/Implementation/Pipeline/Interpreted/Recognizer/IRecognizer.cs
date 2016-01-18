using System.Collections.Generic;
using GestureRecognition.Implementation.Pipeline.Interpreted.Template;
using Trame;

namespace GestureRecognition.Implementation.Pipeline.Interpreted
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRecognizer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templates"></param>
        void Initialize(IEnumerable<ITemplate> templates);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        IEnumerable<Result> Recognize(IDictionary<JointType, InputVector> input);
    }
}
