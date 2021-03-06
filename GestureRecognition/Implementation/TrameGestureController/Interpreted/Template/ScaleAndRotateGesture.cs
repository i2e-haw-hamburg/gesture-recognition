﻿using System;
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
        private const int CountOfFrames = 12;
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
            var frameSize = 3;
            var leftHand = input[JointType.HAND_LEFT];
            var rightHand = input[JointType.HAND_RIGHT];
            var center = input[JointType.CENTER];
            var isOld = leftHand.Stream.Count() >= (CountOfFrames / frameSize);
            var box = BoundaryBox.Create(center.First - new Vector3(0.6, -0.1, 0.2), 0.8, 0.5, 0.5);
            // check boundary box
            return 1;
            // return isOld && box.IsIn(leftHand.Last) && box.IsIn(rightHand.Last) ? 1 : 0.1;
        }

        public override double StartCondition(IDictionary<JointType, InputVector> input)
        {
            var leftHand = input[JointType.HAND_LEFT];
            var rightHand = input[JointType.HAND_RIGHT];
            var center = input[JointType.CENTER];
            var box = BoundaryBox.Create(center.First - new Vector3(0.6, -0.1, 0.2), 0.8, 0.5, 0.5);
            // check boundary box
            return 1;
            // return box.IsIn(leftHand.First) && box.IsIn(rightHand.First) ? 1 : 0.1;
        }

        public override double MotionCharacteristic(IDictionary<JointType, InputVector> input)
        {
            var leftHand = input[JointType.HAND_LEFT];
            var rightHand = input[JointType.HAND_RIGHT];
            var leftHandMovement = leftHand.Last - leftHand.First;
            var rightHandMovement = rightHand.Last - rightHand.First;
            var turningAngle = Vector3.Angle(leftHandMovement, rightHandMovement) * 360 /(2 * Math.PI);
            var sameDirection = turningAngle < 30 || turningAngle > 330;
            return !sameDirection ? 1 : 0;
        }
    }
}
