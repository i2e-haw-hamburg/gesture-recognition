using System.Collections.Generic;
using System.Linq;
using Trame;
using TrameSkeleton.Math;

namespace GestureRecognition.Implementation.Pipeline.Interpreted.Template
{
    /// <summary>
    /// Gesture class for two handed gestures.
    /// </summary>
    public class ScaleAndRotateGesture : ABasisTemplate
    {

        /// <summary> 
        /// The basic constructor needs the gesture id and the gesture name. Both should be unique.
        /// </summary>
        /// <param name="gestureId">An arbitrary id for the defined gesture.</param>
        /// <param name="gestureName">A name that can represents the gesture in UIs.</param>
        public ScaleAndRotateGesture() : base("scaleRotate", "Scale and Rotate")
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
            return input[JointType.HAND_LEFT].Stream.Count() >= 12 ? 1.0 : 0;
        }

        public override double StartCondition(IDictionary<JointType, InputVector> input)
        {
            var leftHand = input[JointType.HAND_LEFT];
            var rightHand = input[JointType.HAND_RIGHT];
            var center = input[JointType.CENTER];
            var box = BoundaryBox.Create(center.First - new Vector3(600, 0, 300), 1200, 500, 600);
            // check boundary box
            return box.IsIn(leftHand.First) && box.IsIn(rightHand.First) ? 1 : 0.1;
        }

        public override double MotionCharacteristic(IDictionary<JointType, InputVector> input)
        {
            var leftHand = input[JointType.HAND_LEFT];
            var rightHand = input[JointType.HAND_RIGHT];
            var center = input[JointType.CENTER];
            var box = BoundaryBox.Create(center.Last - new Vector3(600, 0, 300), 1200, 500, 600);
            // check boundary box
            return box.IsIn(leftHand.Last) && box.IsIn(rightHand.Last) ? 1 : 0.1;
        }
    }
}
