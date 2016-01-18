using System;
using System.Collections.Generic;
using System.Linq;
using GestureRecognition.Implementation.Pipeline.Interpreted.Template;
using Trame;

namespace GestureRecognition.Implementation.Pipeline.Interpreted.Blank
{
    public class ThreeDGestureRecognizer : IRecognizer
    {
        private IEnumerable<ITemplate> _templates;

        public void Initialize(IEnumerable<ITemplate> templates)
        {
            _templates = templates;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input">the current input as dictionary of all relevant joints and their path</param>
        /// <returns>list of results</returns>
        public IEnumerable<Result> Recognize(IDictionary<JointType, InputVector> input)
        {
            return _templates.Select(t => new Result(t, ProbabilityCalculation(t, input), input));
        }

        private double ProbabilityCalculation(ITemplate template, IDictionary<JointType, InputVector> input)
        {
            var end = template.EndCondition(input);
            var start = template.StartCondition(input);
            var motion = template.MotionCharacteristic(input);
            return end*start*motion;
        }
        
    }
}
