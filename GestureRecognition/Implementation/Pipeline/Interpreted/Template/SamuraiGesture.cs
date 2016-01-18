using System;
using System.Collections.Generic;
using System.Linq;
using Trame;
using TrameSkeleton.Math;

namespace GestureRecognition.Implementation.Pipeline.Interpreted.Template
{
    /// <summary>
    /// Gesture class for two handed gestures.
    /// </summary>
    public class SamuraiGesture : ABasisTemplate
    {

        public SamuraiGesture() : base("implode", "Implode")
        {}

        /// <summary>
        /// Add a new entry to the sequences dictionary. In two hand gestures the count of entries should be two.
        /// One for the left and one for the right hand.
        /// </summary>
        /// <param name="jointType">The joint type for the sequence.</param>
        /// <param name="sequence">The enumeration of two dimensional vectors of a gesture.</param>
        public void Add(JointType jointType, IEnumerable<Vector3> sequence)
        {
            Sequences.Add(jointType, new Sequence<Vector3>(jointType, sequence.ToList()));
        }

        public override double EndCondition(IDictionary<JointType, InputVector> input)
        {
            var left = input[JointType.HIP_LEFT];
            var right = input[JointType.HAND_RIGHT];
            if ((left.First - right.First).Length() > 0.2)
            {
                return 0;
            }
            // fast, linear movement 

            var difference = right.Last - right.First;
            var distance = left.Stream.Zip(right.Stream, (l, r) => (l - r).Length()).Min();
            return Math.Min(1, 1 - 1 / (12.5 * distance * distance)) * (Math.Abs(difference.Y) > 0.4 ? 1 : 0);
        }

        public override double StartCondition(IDictionary<JointType, InputVector> input)
        {
            return 1.0;
        }

        public override double MotionCharacteristic(IDictionary<JointType, InputVector> input)
        {
            return 1.0;
        }
    }
}
